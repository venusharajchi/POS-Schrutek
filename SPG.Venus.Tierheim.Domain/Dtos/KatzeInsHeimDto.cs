using System;
using SPG.Venus.Tierheim.Domain.Model;

namespace SPG.Venus.Tierheim.Domain.Dtos
{
    public class KatzeInsHeimDto
    {
        public string Name { get; set; } = String.Empty;
        public Geschlecht Geschlecht { get; set; }
        public int Alter { get; set; }
        public bool IsAnschmiegsam { get; set; }
        public int TierheimId { get; set; } // PK
    }
}

