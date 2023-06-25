using System;
using SPG.Venus.Tierheim.Domain.Model;


namespace SPG.Venus.Tierheim.Domain.Interfaces
{
	public interface ITierheimRepository
	{
        void Create(Tierheimhaus newEntity);
    }
}

