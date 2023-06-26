using System;
using SPG.Venus.Tierheim.Domain.Model;

namespace SPG.Venus.Tierheim.Domain.Dtos
{
    public class HundInsHeimDto
    {
        public string Name { get; set; } = String.Empty;
        public Geschlecht Geschlecht { get; set; }
        public int Alter { get; set; }
        public bool IsBissig { get; set; }
        public string TierhausName { get; set; } // PK
    }
}

