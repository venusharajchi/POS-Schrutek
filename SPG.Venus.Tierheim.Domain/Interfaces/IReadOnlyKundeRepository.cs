using System;
using SPG.Venus.Tierheim.Domain.Model;


namespace SPG.Venus.Tierheim.Domain.Interfaces
{
	public interface IReadOnlyKundeRepository
	{
        IQueryable<Kunde> GetAll();
    }
}

