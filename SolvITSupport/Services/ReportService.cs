﻿using Microsoft.EntityFrameworkCore;
using SolvITSupport.Data;
using SolvITSupport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                .Where(t => t.DataCriacao >= dataFiltro)
                .ToListAsync();

            // --- 1. CÁLCULO DOS KPIs (Visão Geral) ---
            int kpiTotalTickets = allTickets.Count;
            int totalResolvidos = allTickets.Count(t => t.DataResolucao.HasValue);
            double kpiTaxaResolucao = (kpiTotalTickets > 0) ? ((double)totalResolvidos / kpiTotalTickets) * 100 : 0;
            var ticketsComPrimeiraResposta = allTickets.Where(t => t.DataPrimeiraResposta.HasValue).ToList();
            double kpiTempoMedioHoras = (ticketsComPrimeiraResposta.Any()) ? ticketsComPrimeiraResposta.Average(t => (t.DataPrimeiraResposta.Value - t.DataCriacao).TotalHours) : 0;
            var ticketsAvaliados = allTickets.Where(t => t.AvaliacaoNota.HasValue).ToList();
            double kpiSatisfacao = (ticketsAvaliados.Any()) ? ticketsAvaliados.Average(t => t.AvaliacaoNota.Value) : 0;

            // --- 2. GRÁFICO 1: CHAMADOS POR MÊS (Visão Geral) ---
            var monthlyChart = new MonthlyChartData();
            for (int i = 5; i >= 0; i--)
            {
                var mesAtual = DateTime.Now.AddMonths(-i);
                monthlyChart.Labels.Add(mesAtual.ToString("MMM"));
                var ticketsDoMes = allTickets.Where(t => t.DataCriacao.Month == mesAtual.Month && t.DataCriacao.Year == mesAtual.Year).ToList();
                monthlyChart.EmAbertoData.Add(ticketsDoMes.Count(t => !t.DataResolucao.HasValue));
                monthlyChart.ResolvidosData.Add(ticketsDoMes.Count(t => t.DataResolucao.HasValue));
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
                KpiSatisfacaoCliente = Math.Round(kpiSatisfacao, 1),
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
    }
}