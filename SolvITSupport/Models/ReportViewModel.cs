using System.Collections.Generic;

namespace SolvITSupport.Models
{
    // --- Modelos Auxiliares ---
    public class ChartData
    {
        public List<string> Labels { get; set; } = new List<string>();
        public List<int> Data { get; set; } = new List<int>();
    }

    public class MonthlyChartData
    {
        public List<string> Labels { get; set; } = new List<string>();
        public List<int> ResolvidosData { get; set; } = new List<int>();
        public List<int> EmAbertoData { get; set; } = new List<int>();
    }

    public class ProblemaComumViewModel
    {
        public string Titulo { get; set; }
        public int Total { get; set; }
        public int Resolvidos { get; set; }
    }

    public class CategoryDetailViewModel
    {
        public string CategoryName { get; set; }
        public int Count { get; set; }
        public string ColorHex { get; set; }
    }

    // --- Modelo Principal (Versão Limpa) ---
    public class ReportViewModel
    {
        // --- Aba 1: Visão Geral ---
        public int KpiChamadosTotais { get; set; }
        public double KpiTaxaResolucao { get; set; }
        public double KpiTempoMedioRespostaHoras { get; set; }
        public double KpiSatisfacaoCliente { get; set; }
        public MonthlyChartData ChamadosPorMes { get; set; }
        public ChartData DistribuicaoPorCategoria { get; set; }
        public List<ProblemaComumViewModel> ProblemasMaisComuns { get; set; }

        // --- Aba 2: Por Categoria ---
        public ChartData ChamadosPorPrioridade { get; set; }
        public List<CategoryDetailViewModel> DetalhamentoPorCategoria { get; set; }
    }
}