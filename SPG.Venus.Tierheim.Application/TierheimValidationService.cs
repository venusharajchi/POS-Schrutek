using System;
using SPG.Venus.Tierheim.Domain.Exceptions;
using SPG.Venus.Tierheim.Domain.Model;
using SPG.Venus.Tierheim.Repository;

namespace SPG.Venus.Tierheim.Application
{
	public class TierheimValidationService
	{
        private readonly TierheimRepository _tierheimRepository;


        public TierheimValidationService
        (
            TierheimRepository tierheimRepository
        )
        {
            _tierheimRepository = tierheimRepository;
        }


        public Tierheimhaus getTierheimById(int tierheimId)
		{
            Tierheimhaus? tierheim = _tierheimRepository.GetById(tierheimId).SingleOrDefault();

            if (tierheim == null)
            {
                throw new ArgumentException($"Tierheimhaus mit Id {tierheimId} nicht gefunden!");
            }

            return tierheim;
        }
	}
}

