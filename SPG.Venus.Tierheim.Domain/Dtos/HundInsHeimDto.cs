using System;
using SPG.Venus.Tierheim.Domain.Model;
using System.ComponentModel.DataAnnotations;

namespace SPG.Venus.Tierheim.Domain.Dtos
{
    public class HundInsHeimDto
    {
        [Required(ErrorMessage = "Bitte geben Sie den Namen des Hundes an.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Der Hundename muss zwischen 2 und 50 Zeichen lang sein.")]
        public string Name { get; set; } = String.Empty;

        [Required(ErrorMessage = "Bitte geben Sie das Geschlecht der Katze an.")]
        public Geschlecht Geschlecht { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie das Alter des Hundes an.")]
        [Range(0, 20, ErrorMessage = "Das Alter des Hundes muss zwischen 0 und 20 liegen.")]
        public int Alter { get; set; }

        public bool IsBissig { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie die Tierheim-ID an.")]
        public int TierheimId { get; set; } // PK
    }
}
