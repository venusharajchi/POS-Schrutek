using System.ComponentModel.DataAnnotations;
using SPG.Venus.Tierheim.Domain.Model;

namespace SPG.Venus.Tierheim.Domain.Dtos
{
    public class UpdateKundeDto
    {
        public int KundeId { get; set; }

        [Required(ErrorMessage = "Vorname ist erforderlich.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Vorname muss zwischen 2 und 50 Zeichen lang sein.")]
        public string Vorname { get; set; }

        [Required(ErrorMessage = "Nachname ist erforderlich.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Nachname muss zwischen 2 und 50 Zeichen lang sein.")]
        public string Nachname { get; set; }

        [Required(ErrorMessage = "Straße ist erforderlich.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Straße muss zwischen 2 und 100 Zeichen lang sein.")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Hausnummer ist erforderlich.")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Hausnummer muss zwischen 1 und 20 Zeichen lang sein.")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Stadt ist erforderlich.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Stadt muss zwischen 2 und 50 Zeichen lang sein.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Land ist erforderlich.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Land muss zwischen 2 und 50 Zeichen lang sein.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Geschlecht ist erforderlich.")]
        public Geschlecht Geschlecht { get; set; }
    }
}
