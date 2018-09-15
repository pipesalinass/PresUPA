using System;
using Core.Models;

namespace Core.DAO
{
    /// <summary>
    /// Operaciones de un repositorio de personas.
    /// </summary>
    public interface IItemRepository : IRepository<Persona>
    {
        /// <inheritdoc cref="ModelRepository{T}"/>

        /// <summary>
        /// Busca una cotizacion por id
        /// </summary>
        /// <param name="id">RUT</param>
        /// <returns>The Personas</returns>
        Item GetById(int id);
    }
}