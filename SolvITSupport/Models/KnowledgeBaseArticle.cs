using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Importe isto!

namespace SolvITSupport.Models
{
    // Mapeia esta classe para a tabela correta do DB
    [Table("BaseConhecimento")]
    public class KnowledgeBaseArticle
    {
        [Key] // Define Id como Chave Primária
        public int Id { get; set; }

        [Column("Titulo")] // Mapeia para a coluna "Titulo"
        [Required]
        public string Titulo { get; set; }

        [Column("Descricao")]
        public string Descricao { get; set; }

        [Column("Conteudo")]
        [Required]
        public string Conteudo { get; set; }

        [Column("Tags")]
        public string Tags { get; set; }

        [Column("Visualizacoes")]
        public int Visualizacoes { get; set; }

        [Column("Util")] // Mapeia "VotosUteis" (C#) para "Util" (DB)
        public int VotosUteis { get; set; }

        [Column("DataCriacao")]
        public DateTime DataCriacao { get; set; }

        [Column("UltimaAtualizacao")] // Mapeia "DataAtualizacao" (C#) para "UltimaAtualizacao" (DB)
        public DateTime DataAtualizacao { get; set; }

        // --- COLUNA DE CATEGORIA (MUDANÇA IMPORTANTE) ---
        // Mapeia para a coluna de TEXTO "Categoria" no banco de dados
        [Column("Categoria")]
        public string Categoria { get; set; }

        // --- Propriedades que existem no DB mas não no seu model original ---
        [Column("NaoUtil")]
        public int NaoUtil { get; set; }

        [Column("IdAutor")]
        public string IdAutor { get; set; }

        [Column("Publicado")]
        public bool Publicado { get; set; }

        [Column("Destaque")]
        public bool Destaque { get; set; }

        // --- REMOVIDO ---
        // Estas propriedades foram removidas porque o DB não tem a coluna CategoriaId
        // public int CategoriaId { get; set; }
        // [ForeignKey("CategoriaId")]
        // public virtual Category Categoria { get; set; }
    }
}