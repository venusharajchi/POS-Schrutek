using Microsoft.EntityFrameworkCore;
using SPG.Venus.Tierheim.Domain.Dtos;
using SPG.Venus.Tierheim.Domain.Exceptions;
using SPG.Venus.Tierheim.Domain.Interfaces;
using SPG.Venus.Tierheim.Domain.Model;
using SPG.Venus.Tierheim.Repository;

namespace SPG.Venus.Tierheim.Application;

public class TierheimService
{
    private readonly TierheimRepository _tierheimRepository;
    private readonly TierheimValidationService _tierheimValidationService;

    public TierheimService
    (
        TierheimRepository tierheimRepository,
        TierheimValidationService tierheimValidationService
    )
    {
        _tierheimRepository = tierheimRepository;
        _tierheimValidationService = tierheimValidationService;
    }



    // GET ALL -----------------------------------------------------------------

    public List<Tierheimhaus> GetAll(int currentPage, int itemsPerPage, string sort = "asc_sort")
    {
        // Sort Tierheime
        var tierheimhausQuery = sort.Equals("asc_sort")
           ? _tierheimRepository.GetAll().OrderBy(t => t.Name)
           : _tierheimRepository.GetAll().OrderByDescending(t => t.Name);

        // Page Tierheime
        return tierheimhausQuery.Skip((currentPage - 1) * itemsPerPage)
             .Take(itemsPerPage)
             .ToList();
    }




    // CREATE TIERHEIM ---------------------------------------------------------

    public void NewTierheim(NewTierheimDto dto)
    {
        try
        {
            // -> ArgumentEx
            var address = new Addresse(dto.Street, dto.Number, dto.City, dto.Country);
            var newTierheim = new Tierheimhaus(Guid.NewGuid(), dto.Name, address, dto.StartDate, dto.EndDate);

            // -> TierheimRepositoryEx
            _tierheimRepository.Create(newTierheim);
        }
        catch(ArgumentException ex)
        {
            throw new TierheimServiceException(
                "TierheimService#NewTierheim: Tierheim Validation Error", ex);
        }
        catch (TierheimRepositoryException ex)
        {
            throw new TierheimServiceException(
                "TierheimService#NewTierheim: Tierheim DB Error", ex);
        }
    }




    // HUND INS HEIM -----------------------------------------------------------

    public void HundInsHeim(HundInsHeimDto dto)
    {

        try
        {
            // -> ArgumentEx
            Tierheimhaus tierheim = _tierheimValidationService.getTierheimById(dto.TierheimId);

            // -> ArgumentEx
            var hund = new Hund(Guid.NewGuid(), dto.IsBissig, dto.Name, dto.Geschlecht, dto.Alter);
            tierheim.TierInsHeimBringen(hund);

            // -> TierheimRepositoryEx
            _tierheimRepository.Update(tierheim);

        }
        catch (ArgumentException ex)
        {
            throw new TierheimServiceException(
                "TierheimService#HundInsHeim: Tierheim Validation Error", ex);
        }
        catch (TierheimRepositoryException ex)
        {
            throw new TierheimServiceException(
                "TierheimService#HundInsHeim: Tierheim DB Error", ex);
        }
    }




    // KATZE INS HEIM -----------------------------------------------------------

    public void KatzeInsHeim(KatzeInsHeimDto dto)
    {
        try
        {
            // -> ArgumentEx
            Tierheimhaus tierheim = _tierheimValidationService.getTierheimById(dto.TierheimId);

            // -> ArgumentEx
            var katze = new Katze(Guid.NewGuid(), dto.IsAnschmiegsam, dto.Name, dto.Geschlecht, dto.Alter);
            tierheim.TierInsHeimBringen(katze);

            // -> TierheimRepositoryEx
            _tierheimRepository.Update(tierheim);

        }
        catch (ArgumentException ex)
        {
            throw new TierheimServiceException(
                "TierheimService#KatzeInsHeim: Tierheim Validation Error", ex);
        }
        catch (TierheimRepositoryException ex)
        {
            throw new TierheimServiceException(
                "TierheimService#KatzeInsHeim: Tierheim DB Error", ex);
        }
    }



    // DELETE TIERHEIM ----------------------------------------------------------

    public void DeleteTierheim(int tierheimId)
    {
        try
        {
            // -> TierheimRepositoryEx
            _tierheimRepository.Delete(tierheimId);
        }
        catch (TierheimRepositoryException ex)
        {
            throw new TierheimServiceException(
                "TierheimService#DeleteTierheim: Tierheim DB Error", ex);
        }
    }
}

