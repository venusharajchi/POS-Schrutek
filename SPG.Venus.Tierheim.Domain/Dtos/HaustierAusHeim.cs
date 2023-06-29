using System.ComponentModel.DataAnnotations;

public class HaustierAusHeimDto
{
    [Required(ErrorMessage = "KundeId ist erforderlich.")]
    public int KundeId { get; set; }

    [Required(ErrorMessage = "TierheimId ist erforderlich.")]
    public int TierheimId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "MaxAlter muss eine nicht-negative Ganzzahl sein.")]
    public int MaxAlter { get; set; }

    [Required(ErrorMessage = "Tierart ist erforderlich.")]
    public Tierart Tierart { get; set; }
}

public enum Tierart
{
    Hund,
    Katze
}
