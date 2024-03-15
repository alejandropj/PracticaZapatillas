using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PracticaZapatillas.Data;
using PracticaZapatillas.Models;

namespace PracticaZapatillas.Repositories
{
    #region PROCEDIMIENTO ALMACENADO
    /*    create procedure SP_IMAGEN_ZAPATILLA
    (@POSICION int, @ZAPATILLA int
    , @REGISTROS int out)
    as
    select @REGISTROS = count(IDIMAGEN)
    from IMAGENESZAPASPRACTICA
    where IDPRODUCTO = @ZAPATILLA


    select IDIMAGEN, IDPRODUCTO, IMAGEN
    from(
        select cast(
            ROW_NUMBER() OVER (ORDER BY IMAGEN) as int) AS POSICION
            , IDIMAGEN, IDPRODUCTO, IMAGEN

            from IMAGENESZAPASPRACTICA

            where IDPRODUCTO = @ZAPATILLA) as QUERY
        where QUERY.POSICION = @POSICION
    go*/
    #endregion
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
            string sql = "SP_IMAGEN_ZAPATILLA @POSICION, @ZAPATILLA, @REGISTROS OUT";
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

        public void AddImagen(int idZapatilla, List<string> imagenes)
        {
            int maxId = this.context.Imagenes.Max(i => i.IdImagen) + 1;

            for (int i = 0; i < imagenes.Count; i++)
            {

                Imagen newImagen = new Imagen();
                newImagen.IdImagen = maxId + i;
                newImagen.IdProducto = idZapatilla;
                newImagen.ImagenZapatilla = imagenes[i];

                this.context.Imagenes.Add(newImagen);
            }
            this.context.SaveChanges();

        }

    }
}
