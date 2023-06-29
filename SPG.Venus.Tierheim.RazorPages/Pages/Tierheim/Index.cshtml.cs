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

        /*
        public void OnGet()
        {
            Tierheims = new List<Tierheimhaus>
            {
                new Tierheimhaus(Guid.NewGuid(),
                    "Test Tierheim 1",
                    new Addresse("Musterstraße 1", "123", "Musterstadt 1", "Musterland 1"),
                    TimeSpan.FromHours(8),
                    TimeSpan.FromHours(17)),

                new Tierheimhaus(Guid.NewGuid(),
                    "Test Tierheim 2",
                    new Addresse("Musterstraße 2", "456", "Musterstadt 2", "Musterland 2"),
                    TimeSpan.FromHours(9),
                    TimeSpan.FromHours(18)),

                new Tierheimhaus(Guid.NewGuid(),
                    "Test Tierheim 3",
                    new Addresse("Musterstraße 3", "789", "Musterstadt 3", "Musterland 3"),
                    TimeSpan.FromHours(10),
                    TimeSpan.FromHours(19))
            };
        }
        */

        
        public void OnGet()
        {
            Tierheims = _tierheimService.GetAll();
        }
        
    }

}
