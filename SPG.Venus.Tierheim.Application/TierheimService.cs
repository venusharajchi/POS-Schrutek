using Microsoft.EntityFrameworkCore;
using SPG.Venus.Tierheim.Domain.Dtos;
using SPG.Venus.Tierheim.Domain.Exceptions;
using SPG.Venus.Tierheim.Domain.Interfaces;
using SPG.Venus.Tierheim.Domain.Model;
using SPG.Venus.Tierheim.Repository.Tierheim;

namespace SPG.Venus.Tierheim.Application;

public class TierheimService
{
    private readonly TierheimRepository _tierheimRepository;
    private readonly IReadOnlyRepositoryBase<Tierheimhaus> _readOnlyTierheimRepository;


    public TierheimService
    (
        TierheimRepository tierheimRepository,
        IReadOnlyRepositoryBase<Tierheimhaus> readOnlyProductRepository
    )
    {
        _tierheimRepository = tierheimRepository;
        _readOnlyTierheimRepository = readOnlyProductRepository;
    }



    // CREATE TIERHEIM ---------------------------------------------------------

    // Data Transfer Object
    public void NewTierheim(NewTierheimDto dto)
    {
        try
        {
            // Verify StartDate is in the past
            if (dto.StartDate > DateTime.Now)
                throw new ServiceException("Start date muss in Vergangheit sein!");

            // Verify EndDate is in the future
            if (dto.EndDate < DateTime.Now)
                throw new ServiceException("End date muss in Zukunft sein!");

            // Map Dto to Address Domain
            var address = new Addresse(dto.Street, dto.Number, dto.City, dto.Country);

            // Map Dto to Tierheim Domain
            var newTierheim = new Tierheimhaus(Guid.NewGuid(),
                dto.Name, address,  dto.StartDate, dto.EndDate);

            // Save Tierheim in DB
            _tierheimRepository.Create(newTierheim);
        }
        // Exception Translation
        catch (RepositoryException ex)
        {
            throw new ServiceException("Create Tierheim ist fehlgeschlagen!", ex);
        }
    }




    // GET ALL -----------------------------------------------------------------

    public List<Tierheimhaus> GetAll(int currentPage, int itemsPerPage, string sort = "asc_sort")
    {
        try
        {
            // Sort Tierheime
            var tierheimhausQuery = sort.Equals("asc_sort")
                ? _tierheimRepository.GetAll().OrderBy(t => t.Name)
                : _tierheimRepository.GetAll().OrderByDescending(t => t.Name);

            // Page Tierheime
            // LINQ
            return tierheimhausQuery.Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();
        }
        // Exception Translation
        catch (RepositoryException ex)
        {
            throw new ServiceException("GetAll Tierheime ist fehlgeschlagen!", ex);
        }
    }




    // HUND INS HEIM -----------------------------------------------------------

    public void HundInsHeim(HundInsHeimDto dto)
    {
        try
        {
            // Haus exsists?
            Tierheimhaus? haus = _readOnlyTierheimRepository.GetByPK<string, Personal>(dto.TierhausName);
            if (haus == null)
                throw new RepositoryException($"Tierheimhaus mit Name {dto.TierhausName} nicht gefunden!");

            // Map Dto to Hund Domain
            var hund = new Hund(Guid.NewGuid(), dto.IsBissig, dto.Name, dto.Geschlecht, dto.Alter);

            // Update DB
            haus.TierInsHeimBringen(hund);
            _tierheimRepository.Update(haus);

        }
        // Exception Translation
        catch (RepositoryException ex)
        {
            throw new ServiceException("HundInsHeim ist fehlgeschlagen, der arme Hund!", ex);
        }
    }



    // KATZE INS HEIM -----------------------------------------------------------

    public void KatzeInsHeim(KatzeInsHeimDto dto)
    {
        try
        {
            // Haus exists?
            Tierheimhaus? haus = _readOnlyTierheimRepository.GetByPK<string, Personal>(dto.TierhausName);
            if (haus == null)
                throw new RepositoryException($"Tierheimhaus mit Name {dto.TierhausName} nicht gefunden!");

            // Map Dto to Katze Domain
            var katze = new Katze(Guid.NewGuid(), dto.IsAnschmiegsam, dto.Name, dto.Geschlecht, dto.Alter);

            // Update DB
            haus.TierInsHeimBringen(katze);
            _tierheimRepository.Update(haus);

        }
        // Exception Translation
        catch (RepositoryException ex)
        {
            throw new ServiceException("KatzeInsHeim ist fehlgeschlagen, die arme Katze!", ex);
        }
    }




}

