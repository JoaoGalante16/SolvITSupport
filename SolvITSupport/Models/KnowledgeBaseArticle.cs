using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolvITSupport.Models
{
    [Table("BaseConhecimento")] // Diz ao EF que esta classe corresponde à tabela 'BaseConhecimento'
    public class KnowledgeBaseArticle
    {
        [Key]
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Categoria { get; set; }
        public string Descricao { get; set; }
        public string Conteudo { get; set; }
        public string Tags { get; set; }
        public int Visualizacoes { get; set; }

        [Column("Util")] // Mapeia para a coluna 'Util'
        public int Util { get; set; }

        [Column("NaoUtil")] // Mapeia para a coluna 'NaoUtil'
        public int NaoUtil { get; set; }
        public string IdAutor { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
        public bool Publicado { get; set; }
        public bool Destaque { get; set; }
    }
}
