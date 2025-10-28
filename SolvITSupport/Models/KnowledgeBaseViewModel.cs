﻿using System.Collections.Generic;

namespace SolvITSupport.Models
{
    // Este é o ViewModel principal da página
    public class KnowledgeBaseViewModel
    {
        public List<KnowledgeBaseArticle> Articles { get; set; }
        public List<Category> Categories { get; set; }
        public List<PopularTopicViewModel> PopularTopics { get; set; }

        // Filtros atuais
        public string CurrentSearchString { get; set; }
        public string CurrentCategoryName { get; set; }

        // Estatísticas do Rodapé
        public int TotalArticles { get; set; }
        public int TotalViews { get; set; }
        public double ResolutionRate { get; set; }
    }

    // Este é um sub-modelo, usado para os "Tópicos Populares"
    public class PopularTopicViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int ArticleCount { get; set; }
        public string Icon { get; set; } // Para o ícone do FontAwesome
    }
}