using System;
using System.ComponentModel.DataAnnotations;

namespace SPG.Venus.Tierheim.Domain.Dtos
{
    public class NewTierheimDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Bitte geben Sie den Namen an.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Der Name muss zwischen 2 und 100 Zeichen lang sein.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bitte geben Sie die Straße an.")]
        public string Street { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bitte geben Sie die Hausnummer an.")]
        public string Number { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bitte geben Sie die Stadt an.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bitte geben Sie das Land an.")]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bitte geben Sie das Startdatum an.")]
        public TimeSpan StartDate { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie das Enddatum an.")]
        public TimeSpan EndDate { get; set; }
    }
}
