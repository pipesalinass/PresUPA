using System;
using System.Collections.Generic;
using Core.Models;

namespace Core.Controllers
{
    /// <summary>
    /// Operaciones del sistema.
    /// </summary>
    public interface ISistema
    {
        /// <summary>
        /// Operacion de sistema: Almacena una persona en el sistema.
        /// </summary>
        /// <param name="persona">Persona a guardar en el sistema.</param>
        void Save(Persona persona);

        /// <summary>
        /// Obtiene todas las personas del sistema.
        /// </summary>
        /// <returns>The IList of Persona</returns>
        IList<Persona> GetPersonas();

        /// <summary>
        /// Guarda a un usuario en el sistema
        /// </summary>
        /// <param name="persona"></param>
        /// <param name="password"></param>
        void Save(Persona persona, string password);

        /// <summary>
        /// Obtiene el usuario desde la base de datos, verificando su login y password.
        /// </summary>
        /// <param name="rutEmail">RUT o Correo Electronico</param>
        /// <param name="password">Contrasenia de acceso al sistema</param>
        /// <returns></returns>
        Usuario Login(string rutEmail, string password);

        /// <summary>
        /// Busqueda de una persona por rut o correo electronico.
        /// </summary>
        /// <param name="rutEmail">RUT o Correo Electronico</param>
        /// <returns>La persona si existe</returns>
        Persona Find(string rutEmail);
        
        /// <summary>
        /// Guarda una cotizacion en el sistema
        /// </summary>
        void AgregarCotizacion(Cotizacion cotizacion);

        /// <summary>
        /// Elimina una cotizacion del sistema
        /// </summary>
        void EliminarCotizacion(int id);
        
        /// <summary>
        /// Busca cotizacion segun el criterio, la cotizacion debe estar entre dos fechas
        /// </summary>
        /// <param name="criterioDeBusqueda">Criterio de busqueda</param>
        /// <param name="fechaInicio">Fecha inicio cotizacion</param>
        /// <param name="fechaTermino">Fecha termino cotizacion</param>
        /// <returns>Una lista de cotizaciones que cumplan con el criterio de busqueda</returns>
        IList<Cotizacion> BuscarCotizacion(string criterioDeBusqueda);

        IList<Cotizacion> BuscarCotizacionEntreFechas(string criterioDeBusqueda, DateTime fechaInicio,
            DateTime fechaTermino);
        

    }
}