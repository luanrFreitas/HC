using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HustleCastle.Models
{
    public class Tutorial
    {
        public int ID { get; set; }
        [Display(Name ="Título")]
        public string Title { get; set; }
        [Display(Name = "Conteúdo")]
        public string Text { get; set; }
        [Display(Name = "Autor")]
        public string Author { get; set; }
        [Display(Name = "Data de criação")]
        public string CreatedAt { get; set; }
    }
}
