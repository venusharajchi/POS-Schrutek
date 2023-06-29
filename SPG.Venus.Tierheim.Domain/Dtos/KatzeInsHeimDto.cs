using System;
using SPG.Venus.Tierheim.Domain.Model;
using System.ComponentModel.DataAnnotations;

namespace SPG.Venus.Tierheim.Domain.Dtos
{
    public class KatzeInsHeimDto
    {
        [Required(ErrorMessage = "Bitte geben Sie den Namen der Katze an.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Der Katzenname muss zwischen 2 und 50 Zeichen lang sein.")]
        public string Name { get; set; } = String.Empty;

        [Required(ErrorMessage = "Bitte geben Sie das Geschlecht der Katze an.")]
        public Geschlecht Geschlecht { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie das Alter der Katze an.")]
        [Range(0, 20, ErrorMessage = "Das Alter der Katze muss zwischen 0 und 20 liegen.")]
        public int Alter { get; set; }

        public bool IsAnschmiegsam { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie die Tierheim-ID an.")]
        public int TierheimId { get; set; } // PK
    }
}
