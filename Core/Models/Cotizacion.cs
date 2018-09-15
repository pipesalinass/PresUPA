using System;
using System.Collections.Generic;


namespace Core.Models
{
    /// <summary>
    /// Clase que representa una cotizacion en el sistema de presupuesto.
    /// </summary>
    public class Cotizacion : BaseEntity
    {
        /// <summary>
        /// Id unico.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Rut cliente asociado a cotizacion.
        /// </summary>
        public string RutCliente { get; set; }

        /// <summary>
        /// Rut usuario creador de la cotizacion.
        /// </summary>
        public string RutUsuarioCreador { get; set; }

        /// <summary>
        /// Fecha en que se crea la cotizacion.
        /// </summary>
        public DateTime FechaCreacion { get; set; }

        /// <summary>
        /// Items que componen la cotizacion
        /// </summary>
        public IList<Item> Items { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] archivoPDF { get; set; }

        public override void Validate()
        {
            if (String.IsNullOrEmpty(RutCliente))
            {
                throw new ModelException("Rut cliente no puede ser null");

            }

            if (String.IsNullOrEmpty(RutUsuarioCreador))
            {
                throw new ModelException("Rut usuario creador no puede ser null");

            }

            if (DateTime.Compare(DateTime.Now, FechaCreacion) < 0)
            {
                throw new ModelException("La fecha no puede ser en el futuro");
            }

            Models.Validate.ValidarRut(RutCliente);
            Models.Validate.ValidarRut(RutUsuarioCreador);
        }
    }


}