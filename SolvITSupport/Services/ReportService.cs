using Microsoft.EntityFrameworkCore;
using SolvITSupport.Data;
using SolvITSupport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolvITSupport.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;
        private readonly List<string> knownCategories = new List<string> { "Hardware", "Software", "Rede", "Acesso" };

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReportViewModel> GetDashboardReportsAsync()
        {
            var dataFiltro = DateTime.Now.AddDays(-90);

            var allTickets = await _context.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.Prioridade)
                .Include(t => t.Status) // Incluir o Status
                .Where(t => t.DataCriacao >= dataFiltro)
                .ToListAsync();

            // --- 1. CÁLCULO DOS KPIs (Visão Geral) ---
            int kpiTotalTickets = allTickets.Count;

            // Tempo Médio de Resposta (Já funcional com a alteração do TicketService)
            var ticketsComPrimeiraResposta = allTickets.Where(t => t.DataPrimeiraResposta.HasValue).ToList();
            double kpiTempoMedioHoras = (ticketsComPrimeiraResposta.Any()) ? ticketsComPrimeiraResposta.Average(t => (t.DataPrimeiraResposta.Value - t.DataCriacao).TotalHours) : 0;

            // --- INÍCIO DA LÓGICA MODIFICADA ---

            // Busca apenas tickets resolvidos (que têm DataResolucao)
            var ticketsResolvidos = allTickets.Where(t => t.DataResolucao.HasValue).ToList();

            // Calcula o KPI de Taxa de Resolução
            int totalResolvidos = ticketsResolvidos.Count;
            double kpiTaxaResolucao = (kpiTotalTickets > 0) ? ((double)totalResolvidos / kpiTotalTickets) * 100 : 0;

            // Calcula o KPI de Tempo Médio de Resolução
            double kpiTempoMedioResolucaoHoras = 0;
            if (ticketsResolvidos.Any())
            {
                // Média do tempo (em horas) entre a Resolução e a Criação
                kpiTempoMedioResolucaoHoras = ticketsResolvidos.Average(t => (t.DataResolucao.Value - t.DataCriacao).TotalHours);
            }

            // --- FIM DA LÓGICA MODIFICADA ---


            // --- 2. GRÁFICO 1: CHAMADOS POR MÊS (Visão Geral) ---
            var monthlyChart = new MonthlyChartData();
            for (int i = 5; i >= 0; i--)
            {
                var mesAtual = DateTime.Now.AddMonths(-i);
                monthlyChart.Labels.Add(mesAtual.ToString("MMM"));
                var ticketsDoMes = allTickets.Where(t => t.DataCriacao.Month == mesAtual.Month && t.DataCriacao.Year == mesAtual.Year).ToList();

                // Corrigido para usar Status em vez de DataResolucao (para ser consistente com o Dashboard)
                monthlyChart.EmAbertoData.Add(ticketsDoMes.Count(t => t.Status != null && t.Status.IsFinalStatus == false));
                monthlyChart.ResolvidosData.Add(ticketsDoMes.Count(t => t.Status != null && t.Status.IsFinalStatus == true));
            }

            // --- 3. GRÁFICO 2: DISTRIBUIÇÃO POR CATEGORIA (Visão Geral) ---
            var ticketsByCategoryData = allTickets
                .Where(t => t.Categoria != null)
                .Select(t => new { NormalizedCategory = knownCategories.Contains(t.Categoria.Nome) ? t.Categoria.Nome : "Outros" })
                .GroupBy(t => t.NormalizedCategory)
                .Select(g => new { Label = g.Key, Count = g.Count() })
                .ToList();
            var categoryChart = new ChartData { Labels = ticketsByCategoryData.Select(d => d.Label).ToList(), Data = ticketsByCategoryData.Select(d => d.Count).ToList() };

            // --- 4. GRÁFICO 3: PROBLEMAS MAIS COMUNS (Visão Geral) ---
            var problemasComuns = allTickets
                .GroupBy(t => t.Titulo)
                .Select(g => new ProblemaComumViewModel { Titulo = g.Key, Total = g.Count(), Resolvidos = g.Count(t => t.DataResolucao.HasValue) })
                .OrderByDescending(x => x.Total).Take(5).ToList();

            // --- 5. CÁLCULO PARA ABA "POR CATEGORIA" ---
            var ticketsByPriorityData = allTickets.Where(t => t.Prioridade != null).GroupBy(t => t.Prioridade.Nome).Select(g => new { Label = g.Key, Count = g.Count() }).ToList();
            var priorityChart = new ChartData { Labels = ticketsByPriorityData.Select(d => d.Label).ToList(), Data = ticketsByPriorityData.Select(d => d.Count).ToList() };
            var categoryDetailList = ticketsByCategoryData.OrderByDescending(x => x.Count)
                .Select(d => new CategoryDetailViewModel { CategoryName = d.Label, Count = d.Count, ColorHex = GetCategoryColor(d.Label) }).ToList();

            // --- 6. MONTAR O VIEWMODEL FINAL ---
            var viewModel = new ReportViewModel
            {
                KpiChamadosTotais = kpiTotalTickets,
                KpiTaxaResolucao = Math.Round(kpiTaxaResolucao, 1),
                KpiTempoMedioRespostaHoras = Math.Round(kpiTempoMedioHoras, 1),

                // LINHA MODIFICADA:
                KpiTempoMedioResolucaoHoras = Math.Round(kpiTempoMedioResolucaoHoras, 1),

                ChamadosPorMes = monthlyChart,
                DistribuicaoPorCategoria = categoryChart,
                ProblemasMaisComuns = problemasComuns,
                ChamadosPorPrioridade = priorityChart,
                DetalhamentoPorCategoria = categoryDetailList
            };

            return viewModel;
        }

        // Método auxiliar de cor
        private string GetCategoryColor(string categoryName)
        {
            return categoryName?.ToLower() switch
            {
                "software" => "#4e73df",
                "hardware" => "#6f42c1",
                "rede" => "#e83e8c",
                "acesso" => "#fd7e14",
                "outros" => "#198754",
                _ => "#6c757d"
            };
        }

        public async Task<string> GenerateTicketsCsvAsync()
        {
            // 1. Buscar todos os tickets com os dados relacionados
            var tickets = await _context.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.Prioridade)
                .Include(t => t.Status)
                .Include(t => t.Solicitante)
                .Include(t => t.Atendente)
                .OrderByDescending(t => t.DataCriacao)
                .ToListAsync();

            // 2. Usar StringBuilder para construir o ficheiro CSV
            var builder = new StringBuilder();

            // 3. Adicionar o Cabeçalho (Header)
            builder.AppendLine("TicketID;Titulo;Status;Categoria;Prioridade;Solicitante;Atendente;DataCriacao;DataResolucao");

            // 4. Adicionar os dados de cada ticket
            foreach (var t in tickets)
            {
                builder.AppendLine(
                    $"\"TK-{t.Id}\";" +
                    $"\"{t.Titulo.Replace("\"", "\"\"")}\";" + // Trata aspas dentro do texto
                    $"\"{t.Status?.Nome}\";" +
                    $"\"{t.Categoria?.Nome}\";" +
                    $"\"{t.Prioridade?.Nome}\";" +
                    $"\"{t.Solicitante?.NomeCompleto}\";" +
                    $"\"{t.Atendente?.NomeCompleto ?? "N/A"}\";" +
                    $"\"{t.DataCriacao:yyyy-MM-dd HH:mm}\";" +
                    $"\"{t.DataResolucao?.ToString("yyyy-MM-dd HH:mm") ?? "N/A"}\""
                );
            }

            // 5. Retornar a string completa
            return builder.ToString();
        }
    }
}