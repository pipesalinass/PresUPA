using System;
using System.Collections.Generic;
using System.Linq;
using Core.DAO;
using Core.Models;


namespace Core.Controllers
{
    /// <summary>
    /// Implementacion de la interface ISistema.
    /// </summary>
    public sealed class Sistema : ISistema
    {
        // Patron Repositorio, generalizado via Generics
        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/
        private readonly IRepository<Persona> _repositoryPersona;

        private readonly IRepository<Usuario> _repositoryUsuario;

        private readonly IRepository<Item> _repositoryItem;

        private readonly ICotizacionRepository _repositoryCotizacion;


        /// <summary>
        /// Inicializa los repositorios internos de la clase.
        /// </summary>
        public Sistema(IRepository<Persona> repositoryPersona, IRepository<Usuario> repositoryUsuario)
        {
            // Setter!
            _repositoryPersona = repositoryPersona ??
                                 throw new ArgumentNullException("Se requiere el repositorio de personas");
            _repositoryUsuario = repositoryUsuario ??
                                 throw new ArgumentNullException("Se requiere repositorio de usuarios");
            _repositoryItem = _repositoryItem ??
                              throw new ArgumentNullException("Se requiere repositorio de items");
            _repositoryCotizacion = _repositoryCotizacion ??
                                    throw new ArgumentNullException("Se requiere repositorio de cotizaciones");


            // Inicializacion del repositorio.
            _repositoryPersona.Initialize();
            _repositoryUsuario.Initialize();
            _repositoryCotizacion.Initialize();
            _repositoryItem.Initialize();


        }

        /// <inheritdoc />
        public void Save(Persona persona)
        {
            // Verificacion de nulidad
            if (persona == null)
            {
                throw new ModelException("Persona es null");
            }

            persona.Validate();
            // Saving the Persona en el repositorio.
            // La validacion de los atributos ocurre en el repositorio.
            _repositoryPersona.Add(persona);
        }

        /// <inheritdoc />
        public IList<Persona> GetPersonas()
        {
            return _repositoryPersona.GetAll();
        }

        /// <inheritdoc />
        public void Save(Persona persona, string password)
        {
            // Guardo o actualizo en el backend.
            _repositoryPersona.Add(persona);

            // Busco si el usuario ya existe
            Usuario usuario = _repositoryUsuario.GetAll(u => u.Persona.Equals(persona)).FirstOrDefault();

            // Si no existe, lo creo
            if (usuario == null)
            {
                usuario = new Usuario()

                {
                    Persona = persona
                };
            }

            // Hash del password
            usuario.Password = BCrypt.Net.BCrypt.HashPassword(password);

            // Almaceno en el backend
            _repositoryUsuario.Add(usuario);

        }

        public void EliminarCotizacion(int id)
        {
            //Busco si la cotizacion existe
            Cotizacion cotizacion = _repositoryCotizacion.GetById(id);

            if (cotizacion == null)
            {
                throw new ModelException("La cotizacion no puede ser null");
            }

            _repositoryCotizacion.Remove(cotizacion);
        }

        public void AgregarCotizacion(Cotizacion cotizacion)
        {
            if (cotizacion == null)
            {
                throw new ModelException("La cotizacion no puede ser null");
            }

            cotizacion.Validate();
            _repositoryCotizacion.Add(cotizacion);
        }

        /// <summary>
        /// Retorna tipo de busqueda segun el criterio de busqueda utilizado
        /// Los tipos de busqueda son los siguientes:
        /// Rut: Para buscar cotizaciones segun el rut del Usuario creador o el rut del cliente
        /// Texto: Para coincidencias de texto dentro de los items de las cotizaciones
        /// Fecha: Buscar una cotizacion creada en la fecha ingresada
        /// </summary>
        /// <param name="criterioBusqueda">Criterio de busqueda para una cotizacion dada; rut, fecha o texto</param>
        /// <returns>String con el tipo de busqueda a realizar</returns>
        public String TipoBusqueda(String criterioDeBusqueda)
        {
            if (String.IsNullOrEmpty(criterioDeBusqueda) || String.IsNullOrWhiteSpace(criterioDeBusqueda))
            {
                throw new ArgumentException("El criterio de busqueda no puede ser null o estar vacio");
            }

            try
            {
                Models.Validate.ValidarRut(criterioDeBusqueda);
                return "Rut";

            }
            catch (ModelException e)
            {
                ;
            }

            DateTime t;
            if (DateTime.TryParse(criterioDeBusqueda, out t))
            {
                return "Fecha";
            }

            return "Texto";

        }

        public IList<Cotizacion> BuscarCotizacion(String criterioDeBusqueda)
        {
            
        }

        public IList<Cotizacion> BuscarCotizacionEntreFechas(String criterioDeBusqueda, DateTime fechaInicio,
            DateTime fechaTermino)
        {
            if (fechaInicio == null || fechaTermino == null)
            {
                throw new ArgumentNullException("Las fechas de busqueda no pueden ser null");
            }

            try
            {
                HashSet<Cotizacion> cotizaciones = new HashSet<Cotizacion>();
                IList<Cotizacion> cotizacionesEncontradas = BuscarCotizacion(criterioDeBusqueda);
                foreach (Cotizacion cotizacion in cotizacionesEncontradas)
                {
                    if (cotizacion.FechaCreacion >= fechaInicio && cotizacion.FechaCreacion <= fechaTermino)
                    {
                        cotizaciones.Add(cotizacion);
                    }
                }

                return cotizaciones.ToList();

            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
                throw new ArgumentException("El criterio de busqueda no puede ser null");
            }
        }

        public void EnviarCotizacion(Cotizacion cot, Persona per)
        {
            if (cot == null || per == null)
            {
                cot.Validate();
                per.Validate();

                sendMail(cot, per.Email);

            }
        }

        private void sendMail(Cotizacion cot, String Correo)
        {
            throw new NotImplementedException();
        }
    


    /// <inheritdoc />
        public Usuario Login(string rutEmail, string password)
        {
            Persona persona = Find(rutEmail);
            if (persona == null)
            {
                throw new ModelException("Usuario no encontrado");
            }
            
            IList<Usuario> usuarios = _repositoryUsuario.GetAll(u => u.Persona.Equals(persona));
            if (usuarios.Count == 0)
            {
                throw new ModelException("Existe la Persona pero no tiene credenciales de acceso");
            }

            if (usuarios.Count > 1)
            {
                throw new ModelException("Mas de un usuario encontrado");
            }

            Usuario usuario = usuarios.Single();
            if (!BCrypt.Net.BCrypt.Verify(password, usuario.Password))
            {
                throw new ModelException("Password no coincide");
            }

            return usuario;

        }

        /// <inheritdoc />
        public Persona Find(string rutEmail)
        {
            return _repositoryPersona.GetAll(p => p.Rut.Equals(rutEmail) || p.Email.Equals(rutEmail)).FirstOrDefault();
        }
    }
}