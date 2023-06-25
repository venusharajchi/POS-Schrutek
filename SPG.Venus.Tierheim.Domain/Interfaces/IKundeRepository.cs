using System;
using SPG.Venus.Tierheim.Domain.Model;


namespace SPG.Venus.Tierheim.Domain.Interfaces
{
	public interface IKundeRepository
	{
        void Create(Kunde newEntity);
    }
}

