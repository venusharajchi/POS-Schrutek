using System;
using SPG.Venus.Tierheim.Domain.Exceptions;
using SPG.Venus.Tierheim.Domain.Model;
using SPG.Venus.Tierheim.Repository;

namespace SPG.Venus.Tierheim.Application
{
    public class KundeValidationService
    {
        private readonly KundeRepository _kundeRepository;

        public KundeValidationService(KundeRepository kundeRepository)
        {
            _kundeRepository = kundeRepository;
        }

        public Kunde GetKundeById(int kundeId)
        {
            Kunde? kunde = _kundeRepository.GetById(kundeId).SingleOrDefault();

            if (kunde == null)
            {
                throw new ArgumentException($"Kunde mit ID {kundeId} nicht gefunden!");
            }

            return kunde;
        }
    }
}
