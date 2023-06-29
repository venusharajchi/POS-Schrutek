using System;
using SPG.Venus.Tierheim.Domain.Model;
using System.ComponentModel.DataAnnotations;

namespace SPG.Venus.Tierheim.Domain.Dtos
{
    public class NewKundeDto
    {
        [Required(ErrorMessage = "Bitte geben Sie den Vornamen an.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Der Vorname muss zwischen 2 und 50 Zeichen lang sein.")]
        public string Vorname { get; set; } = String.Empty;

        [Required(ErrorMessage = "Bitte geben Sie den Nachnamen an.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Der Nachname muss zwischen 2 und 50 Zeichen lang sein.")]
        public string Nachname { get; set; } = String.Empty;

        [Required(ErrorMessage = "Bitte geben Sie die Straße an.")]
        public string Street { get; set; } = String.Empty;

        [Required(ErrorMessage = "Bitte geben Sie die Hausnummer an.")]
        public string Number { get; set; } = String.Empty;

        [Required(ErrorMessage = "Bitte geben Sie die Stadt an.")]
        public string City { get; set; } = String.Empty;

        [Required(ErrorMessage = "Bitte geben Sie das Land an.")]
        public string Country { get; set; } = String.Empty;

        [Required(ErrorMessage = "Bitte geben Sie das Geschlecht an.")]
        public Geschlecht Geschlecht { get; set; } = Geschlecht.Frau;
    }
}
