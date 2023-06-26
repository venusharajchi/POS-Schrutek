using System;
using SPG.Venus.Tierheim.Domain.Model;

namespace SPG.Venus.Tierheim.Domain.Dtos
{
	public class NewKundeDto
	{
        public string Vorname { get; set; } = String.Empty;
        public string Nachname { get; set; } = String.Empty;
        public string Street { get; set; } = String.Empty;
        public string Number { get; set; } = String.Empty;
        public string City { get; set; } = String.Empty;
        public string Country { get; set; } = String.Empty;
        public Geschlecht Geschlecht { get; set; } = Geschlecht.Frau;
    }
}

