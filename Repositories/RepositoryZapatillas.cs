using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PracticaZapatillas.Data;
using PracticaZapatillas.Models;

namespace PracticaZapatillas.Repositories
{
    public class RepositoryZapatillas
    {
        private ZapatillasContext context;
        public RepositoryZapatillas(ZapatillasContext context)
        {
            this.context = context;
        }

        public async Task<List<Zapatilla>> GetZapatillasAsync()
        {
            return await this.context.Zapatillas.ToListAsync();
        }        
        public async Task<Zapatilla> FindZapatillaAsync(int idZapatilla)
        {
            return await this.context.Zapatillas.
                FirstOrDefaultAsync(z =>z.IdProducto == idZapatilla);
        }        
        public async Task<List<Imagen>> GetImagenesAsync()
        {
            return await this.context.Imagenes.ToListAsync();
        }        
        public async Task<Imagen> FindImagenAsync(int idImagen)
        {
            return await this.context.Imagenes.
                FirstOrDefaultAsync(i =>i.IdImagen == idImagen);
        }

        public async Task<ImagenPaginacion> GetZapatillaImagenAsync
            (int posicion, int idZapatilla)
        {
            string sql = "";
            SqlParameter pamPosicion = new SqlParameter("@POSICION", posicion);
            SqlParameter pamZapatilla = new SqlParameter("@ZAPATILLA", idZapatilla);
            SqlParameter pamRegistros = new SqlParameter("@REGISTROS", -1);
            pamRegistros.Direction = System.Data.ParameterDirection.Output;

            var consulta = this.context.Imagenes.FromSqlRaw
                (sql, pamPosicion, pamZapatilla, pamRegistros);
            var datos = await consulta.ToListAsync();
            Imagen imagen = datos.FirstOrDefault();
            int registros = (int)pamRegistros.Value;
            return new ImagenPaginacion
            {
                Registros = registros,
                Imagen = imagen
            };

        }

    }
}
