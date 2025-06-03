using Microsoft.EntityFrameworkCore;

namespace VDVT.Micro.Product.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public async static Task InsertParamPageHeader<T>(this HttpContext httpContext, IQueryable<T> queryable)
        {
            if (httpContext is null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }
            //contamos el total de la consulta que contenga el IQueryable
            //una vez calculado el total de registros consultados se asigna a la variable
            double total = await queryable.CountAsync();
            //asignamos a la cabecera el total de registros obtenidos.
            httpContext.Response.Headers.Append("cantidad-total-registros", total.ToString());
        }
    }
}
