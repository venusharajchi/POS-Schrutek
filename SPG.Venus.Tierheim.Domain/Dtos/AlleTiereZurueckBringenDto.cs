using System.ComponentModel.DataAnnotations;

namespace SPG.Venus.Tierheim.Domain.Dtos
{
    public class AlleTiereZurueckBringenDto
    {
        [Required(ErrorMessage = "TierheimId is required.")]
        public int TierheimId { get; set; } // PK

        [Required(ErrorMessage = "KundenId is required.")]
        public int KundeId { get; set; } // PK
    }
}
