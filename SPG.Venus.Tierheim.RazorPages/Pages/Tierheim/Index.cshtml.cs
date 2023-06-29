using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SPG.Venus.Tierheim.Application;
using SPG.Venus.Tierheim.Domain.Model;

namespace SPG.Venus.Tierheim.RazorPages.Pages.Tierheim
{
    public class IndexModel : PageModel
    {
        private readonly TierheimService _tierheimService;

        public IndexModel(TierheimService tierheimService)
        {
            _tierheimService = tierheimService;
        }

        public List<Tierheimhaus> Tierheims { get; set; }
        
        public void OnGet()
        {
            Tierheims = _tierheimService.GetAll();
        }
        
    }

}
