using Microsoft.EntityFrameworkCore;
using SPG.Venus.Tierheim.Domain.Dtos;
using SPG.Venus.Tierheim.Domain.Exceptions;
using SPG.Venus.Tierheim.Domain.Interfaces;
using SPG.Venus.Tierheim.Domain.Model;
using SPG.Venus.Tierheim.Repository;

namespace SPG.Venus.Tierheim.Application
{
    public class KundenService
    {
        private readonly KundeRepository _kundeRepository;
        private readonly KundeValidationService _kundeValidationService;

        private readonly TierheimRepository _tierheimRepository;
        private readonly TierheimValidationService _tierheimhausValidationService;


        public KundenService(
            KundeRepository kundeRepository,
            KundeValidationService kundeValidationService,

            TierheimRepository tierheimRepository,
            TierheimValidationService tierheimhausValidationService
        )
        {
            _tierheimRepository = tierheimRepository;
            _kundeRepository = kundeRepository;

            _tierheimhausValidationService = tierheimhausValidationService;
            _kundeValidationService = kundeValidationService;
        }



        // GET ALL -----------------------------------------------------------------

        public List<Kunde> GetAll(int currentPage, int itemsPerPage, string sort = "asc_sort")
        {
            // Sort Kunde
            var tierheimhausQuery = sort.Equals("asc_sort")
               ? _kundeRepository.GetAll().OrderBy(t => t.Nachname)
               : _kundeRepository.GetAll().OrderByDescending(t => t.Nachname);

            // Page Kunde
            return tierheimhausQuery.Skip((currentPage - 1) * itemsPerPage)
                 .Take(itemsPerPage)
                 .ToList();
        }




        // NEW KUNDE ---------------------------------------------------------------

        public void NewKunde(NewKundeDto dto)
        {
            try
            {
                // -> ArgumentEx
                var addresse = new Addresse(dto.Street, dto.Number, dto.City, dto.Country);
                var kunde = new Kunde(Guid.NewGuid(), dto.Vorname, dto.Nachname, addresse, dto.Geschlecht);

                // -> KundeRepositoryException
                _kundeRepository.Create(kunde);
            }
            catch (ArgumentException ex)
            {
                throw new KundeServiceException(
                    "KundeService#NewKunde: Kunden Validation Error", ex);
            }
            catch (KundeRepositoryException ex)
            {
                throw new KundeServiceException(
                    "KundeService#NewKunde: Kunden DB Error", ex);
            }
        }




        // UPDATE KUNDE ------------------------------------------------------------

        public void UpdateKunde(UpdateKundeDto dto)
        {
            try
            {
                // -> ArgumentEx
                Kunde kunde = _kundeValidationService.GetKundeById(dto.KundeId);

                kunde.Vorname = dto.Vorname;
                kunde.Nachname = dto.Nachname;
                kunde.Adresse = new Addresse(dto.Street, dto.Number, dto.City, dto.Country);
                kunde.Geschlecht = dto.Geschlecht;

                // -> KundeRepositoryException
                _kundeRepository.Update(kunde);
            }
            catch (ArgumentException ex)
            {
                throw new KundeServiceException(
                    "KundeService#UpdateKunde: Kunden Validation Error", ex);
            }
            catch (KundeRepositoryException ex)
            {
                throw new KundeServiceException(
                    "KundeService#UpdateKunde: Kunden DB Error", ex);
            }
        }




        // HOLE HAUSTIER AUS HEIM ------------------------------------------------------

        public void HoleHaustierAusHeim(HaustierAusHeimDto dto)
        {
            // Transaction
            using (var transaction = _kundeRepository.GetContext().Database.BeginTransaction())
            {
                try
                {
                    // -> ArgumentEx
                    Kunde kunde = _kundeValidationService.GetKundeById(dto.KundeId);
                    Tierheimhaus tierheim = _tierheimhausValidationService.getTierheimById(dto.TierheimId);

                    Haustier haustier;
                    if (dto.Tierart == Tierart.Hund)
                        haustier = tierheim.HundMitnehmen(dto.MaxAlter);
                    else if (dto.Tierart == Tierart.Katze)
                        haustier = tierheim.KatzeMitnehmen(dto.MaxAlter);
                    else
                        throw new ArgumentException("Invalid Tierart specified.");

                    kunde.AddHaustier(haustier);

                    _kundeRepository.Update(kunde);
                    _tierheimRepository.Update(tierheim);

                    transaction.Commit();
                }
                catch (ArgumentException ex)
                {
                    transaction.Rollback();
                    throw new KundeServiceException("KundenService#HoleHaustierAusHeim: Kunden Validation Error", ex);
                }
                catch (KundeRepositoryException ex)
                {
                    transaction.Rollback();
                    throw new KundeServiceException("KundenService#HoleHaustierAusHeim: Kunden DB Error", ex);
                }
                catch (TierheimRepositoryException ex)
                {
                    transaction.Rollback();
                    throw new KundeServiceException("KundenService#HoleHaustierAusHeim: Tierheim DB Error", ex);
                }
            }
        }





        // ALLE TIERE ZURÜCKBRINGEN ------------------------------------------------

        public void AlleTiereZurueckBringen(AlleTiereZurueckBringenDto dto)
        {
            // Transaction
            using (var transaction = _kundeRepository.GetContext().Database.BeginTransaction())
            {
                try
                {
                    Kunde kunde = _kundeValidationService.GetKundeById(dto.KundeId);
                    Tierheimhaus tierheim = _tierheimhausValidationService.getTierheimById(dto.TierheimId);

                    kunde.AlleZurueckInsHeimBringen(tierheim);

                    _kundeRepository.Update(kunde);
                    _tierheimRepository.Update(tierheim);
                }
                catch (ArgumentException ex)
                {
                    throw new KundeServiceException(
                        "KundenService#AlleTiereZurueckBringen: Kunden Validation Error", ex);
                }
                catch (KundeRepositoryException ex)
                {
                    throw new KundeServiceException(
                        "KundenService#AlleTiereZurueckBringen: Kunden DB Error", ex);
                }
            }
        }



        // DELETE KUNDE ------------------------------------------------------------

        public void DeleteKunde(int kundeId)
        {
            try
            {
                _kundeRepository.Delete(kundeId);
            }
            catch (KundeRepositoryException ex)
            {
                throw new KundeServiceException(
                    "KundenService#DeleteKunde: Kunden DB Error", ex);
            }
        }
    }
}
