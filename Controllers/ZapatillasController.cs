using Microsoft.AspNetCore.Mvc;
using PracticaZapatillas.Models;
using PracticaZapatillas.Repositories;

namespace PracticaZapatillas.Controllers
{
    public class ZapatillasController : Controller
    {
        private RepositoryZapatillas repo;
        public ZapatillasController(RepositoryZapatillas repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> List()
        {
            List<Zapatilla> zapatillas = await this.repo.GetZapatillasAsync();
            return View(zapatillas);
        }        
        public async Task<IActionResult> Details(int idZapatilla)
        {
            Zapatilla zapatilla = await this.repo.FindZapatillaAsync(idZapatilla);
            return View(zapatilla);
        }
    }
}
