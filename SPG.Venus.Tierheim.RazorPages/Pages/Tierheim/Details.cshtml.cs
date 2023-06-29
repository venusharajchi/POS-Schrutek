using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SPG.Venus.Tierheim.Application;
using SPG.Venus.Tierheim.Domain.Dtos;
using SPG.Venus.Tierheim.Domain.Exceptions;
using SPG.Venus.Tierheim.Domain.Model;

namespace SPG.Venus.Tierheim.RazorPages.Pages.Tierheim
{
    public class DetailsModel : PageModel
    {
        private readonly TierheimService _tierheimService;

        public Tierheimhaus Tierheim { get; set; }

        public DetailsModel(TierheimService tierheimService)
        {
            _tierheimService = tierheimService;
        }

        public IActionResult OnGet(int id)
        {
            try
            {
                Tierheim = _tierheimService.GetOne(id);
                return Page();
            }
            catch (TierheimServiceException ex)
            {
                return NotFound();
            }
        }
    }
}
