using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace VivisolPalletXCM.Models
{
    public class FormModel
    {
        [Required]
        [Range(1, 999999999, ErrorMessage = "ID Documento non valido")]
        [Display(Name = "ID Documento")]
        public string idDocumento { get; set; }

        [Required]
        [Range(1, 999999999, ErrorMessage = "Numero consegna non valido")]
        [Display(Name = "Numero Consegna")]
        public string numeroConsegna { get; set; }
    }
}