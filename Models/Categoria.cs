﻿using System.ComponentModel.DataAnnotations;

namespace MiApi.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public ICollection<Producto> Productos { get; set; }
    }
}