using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SPG.Venus.Tierheim.Application;
using SPG.Venus.Tierheim.Domain.Dtos;
using SPG.Venus.Tierheim.Domain.Exceptions;

namespace SPG.Venus.Tierheim.RazorPages.Pages.Tierheim
{
    public class CreateModel : PageModel
    {
        private readonly TierheimService _tierheimService;

        [BindProperty]
        public NewTierheimDto NewTierheimDto { get; set; }

        public CreateModel(TierheimService tierheimService)
        {
            _tierheimService = tierheimService;
        }

        public IActionResult OnGet()
        {
            NewTierheimDto = new NewTierheimDto();
            return Page();
        }

        public IActionResult OnPost()
        {

            if (NewTierheimDto.StartDate >= NewTierheimDto.EndDate)
            {
                ModelState.AddModelError("NewTierheimDto.StartDate", "Start date must be before end date.");
                ModelState.AddModelError("NewTierheimDto.EndDate", "End date must be after start date.");
            }

            if (!ModelState.IsValid)
                return Page();

            try
            {
                _tierheimService.NewTierheim(NewTierheimDto);
                return RedirectToPage("./Index");
            }
            catch (TierheimServiceException ex)
            {
                ModelState.AddModelError(string.Empty,
                    "An error occurred while creating the new Tierheim. Please try again.");

                return Page();
            }
        }
    }
}
