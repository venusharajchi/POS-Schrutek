using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using SPG.Venus.Tierheim.Domain.Dtos;
using SPG.Venus.Tierheim.Domain.Exceptions;
using SPG.Venus.Tierheim.Domain.Interfaces;
using SPG.Venus.Tierheim.Domain.Model;
using SPG.Venus.Tierheim.Repository;
using SPG.Venus.Tierheim.Repository.Tierheim;

namespace SPG.Venus.Tierheim.Application;

public class KundeService
{
    // REPOS -------------------------------------------------------------------
    private readonly KundeRepository _kundeRepository;
    private readonly TierheimRepository _tierheimRepository;

    private readonly IReadOnlyRepositoryBase<Kunde> _readOnlyKundeRepository;
    private readonly IReadOnlyRepositoryBase<Tierheimhaus> _readOnlyTierheimRepository;
    // -------------------------------------------------------------------------


    public KundeService
    (
        KundeRepository kundeRepository,
        TierheimRepository tierheimRepository,

        IReadOnlyRepositoryBase<Kunde> readOnlyKundeRepository,
        IReadOnlyRepositoryBase<Tierheimhaus> readOnlyTierheimRepository
    )
    {
        _kundeRepository = kundeRepository;
        _tierheimRepository = tierheimRepository;

        _readOnlyKundeRepository = readOnlyKundeRepository;
        _readOnlyTierheimRepository = readOnlyTierheimRepository;
    }



    // CREATE KUNDE ------------------------------------------------------------

    public void NewKunde(NewKundeDto dto)
    {
        try
        {
            // Map Dto to Address Domain
            var address = new Addresse(dto.Street, dto.Number, dto.City, dto.Country);

            // Map Dto to Kunde Domain
            var newKunde = new Kunde(Guid.NewGuid(), dto.Vorname, dto.Nachname, address, dto.Geschlecht);

            // Save Kunde in DB
            _kundeRepository.Create(newKunde);
        }
        // Exception Translation
        catch (RepositoryException ex)
        {
            throw new ServiceException("Create Kunde ist fehlgeschlagen!", ex);
        }
    }




    // GET ALL -----------------------------------------------------------------

    public List<Kunde> GetAll(int currentPage, int itemsPerPage, string sort = "asc_sort")
    {
        try
        {
            // Sort Kunden
            var kundeQuery = sort.Equals("asc_sort")
                ? _kundeRepository.GetAll().OrderBy(k => k.Nachname)
                : _kundeRepository.GetAll().OrderByDescending(k => k.Nachname);

            // Page Kunden
            return kundeQuery.Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();
        }
        // Exception Translation
        catch (RepositoryException ex)
        {
            throw new ServiceException("GetAll Kunden ist fehlgeschlagen!", ex);
        }
    }




    // HOLE HUND AUS HEIM ------------------------------------------------------

    public void HoleHundAusHeim(Guid kundenGuid, string tierheimName, int maxAlter)
    {
        try
        {
            Kunde? kunde = _readOnlyKundeRepository.GetByGuid<Kunde>(kundenGuid);
            if (kunde == null)
                throw new RepositoryException($"Kunde mit Id {kundenGuid} nicht gefunden!");


            Tierheimhaus? tierheimhaus = _readOnlyTierheimRepository.GetByPK<string, Personal>(tierheimName);
            if (tierheimhaus == null)
                throw new RepositoryException($"Tierheimhaus mit Name {tierheimhaus} nicht gefunden!");

            // Update DB
            kunde.HoleHundAusHeim(tierheimhaus, maxAlter);
            _kundeRepository.Update(kunde);
            _tierheimRepository.Update(tierheimhaus);
        }
        // Exception Translation
        catch (RepositoryException ex)
        {
            throw new ServiceException("GetAll Kunden ist fehlgeschlagen!", ex);
        }
    }
}
