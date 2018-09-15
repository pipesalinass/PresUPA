using System;

namespace Core.Models
{
    public class Item : BaseEntity
    {
        /// <summary>
        /// Descripcion de la cotizacion 
        /// </summary>
       public int CotizacionId { get; set; }
       public string descripcion { get; set; }
        
        /// <summary>
        /// Rut cliente que está asociado a la cotizacion
        /// </summary>
       public int precio { get; set; }
       public Cotizacion cotizacion { get; set; }

       
    }
}