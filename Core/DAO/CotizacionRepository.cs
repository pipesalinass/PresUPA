using System;
using System.Linq;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Core.DAO
{
    /// <summary>
    /// 
    /// </summary>

    public class CotizacionRepository : ModelRepository<Cotizacion>, ICotizacionRepository
    {
        public CotizacionRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public List<Cotizacion> GetByDate(DateTime d1, DateTime d2)
        {
            IList<Cotizacion> cotizaciones = this.GetAll();
            List<Cotizacion> cotizacionesEntreFechas = new List<Cotizacion>();
            foreach (Cotizacion cotizacion in cotizaciones)

            {
                if (cotizacion.FechaCreacion > d1 && cotizacion.FechaCreacion < d2)
                {
                    cotizacionesEntreFechas.Add(cotizacion);
                    
                }
            }

            return cotizacionesEntreFechas;

        }
    }
}