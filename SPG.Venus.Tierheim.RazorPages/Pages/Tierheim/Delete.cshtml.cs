using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SPG.Venus.Tierheim.Application;
using SPG.Venus.Tierheim.Domain.Exceptions;
using SPG.Venus.Tierheim.Domain.Model;

namespace SPG.Venus.Tierheim.RazorPages.Pages.Tierheim
{
    public class DeleteModel : PageModel
    {
        private TierheimService _tierheimService { get; }

        

        public DeleteModel(TierheimService tierheimService)
        {
            _tierheimService = tierheimService;
        }

        public IActionResult OnGet(int id)
        {
            try
            {
                _tierheimService.DeleteTierheim(id);
                return RedirectToPage("./Index");
            }
            catch (TierheimServiceException ex)
            {
                return NotFound();
            }
        }
    }
}
