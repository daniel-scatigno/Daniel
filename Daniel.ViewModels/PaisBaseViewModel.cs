﻿using System.ComponentModel.DataAnnotations;

namespace Daniel.ViewModels
{
    public class PaisBaseViewModel : DanielViewModel
    {
        [Required]
        [StringLength(2)]
        public string Sigla { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        public int? Codigo { get; set; }

        public int? CodigoSiscomex { get; set; }

        [Required]
        [StringLength(10)]
        public string SiglaLinguagem { get; set; }

        [Required]
        [StringLength(10)]
        public string SiglaMoeda { get; set; }

        [Required]
        [StringLength(100)]
        public string IdentificadorRegistro { get; set; }
    }
}
