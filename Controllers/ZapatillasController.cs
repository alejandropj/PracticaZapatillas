using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Win32;
using PracticaZapatillas.Models;
using PracticaZapatillas.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public async Task<IActionResult> ZapatillaImagen
            (int? posicion, int idZapatilla)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            ImagenPaginacion model = await this.repo.
                GetZapatillaImagenAsync(posicion.Value, idZapatilla);
            Zapatilla zapatilla = await this.repo.FindZapatillaAsync(idZapatilla);
            ViewData["ZAPATILLASELECCIONADA"] = zapatilla;
            ViewData["REGISTROS"] = model.Registros;
            ViewData["ZAPATILLA"] = idZapatilla;

            int siguiente = posicion.Value + 1;
            if(siguiente > model.Registros)
            {
                siguiente = model.Registros;
            }

            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            ViewData["ULTIMO"] = model.Registros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["POSICION"] = posicion;
            return View(model.Imagen);
        }        
        
        public async Task<IActionResult> _ZapatillaPartial
            (int? posicion, int idZapatilla)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            ImagenPaginacion model = await this.repo.
                GetZapatillaImagenAsync(posicion.Value, idZapatilla);
            Zapatilla zapatilla = await this.repo.FindZapatillaAsync(idZapatilla);
            ViewData["ZAPATILLASELECCIONADA"] = zapatilla;
            ViewData["REGISTROS"] = model.Registros;
            ViewData["ZAPATILLA"] = idZapatilla;

            int siguiente = posicion.Value + 1;
            if(siguiente > model.Registros)
            {
                siguiente = model.Registros;
            }

            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            ViewData["ULTIMO"] = model.Registros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["POSICION"] = posicion;
            return PartialView("_ZapatillaPartial", model.Imagen);
        }
    }
}
