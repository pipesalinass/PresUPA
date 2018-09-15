using System;
using System.Collections.Generic;
using Core;
using Core.Controllers;
using Core.DAO;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TestCore.Controllers
{
    /// <summary>
    /// Test del sistema
    /// </summary>
    public class SistemaTests
    {
        
        /// <summary>
        /// Logger de la clase
        /// </summary>
        private readonly ITestOutputHelper _output;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="output"></param>
        public SistemaTests(ITestOutputHelper output)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
        }

        /// <summary>
        /// Test principal de la clase.
        /// </summary>
        [Fact]
        public void AllMethodsTest()
        {
            _output.WriteLine("Starting Sistema test ...");
            ISistema sistema = Startup.BuildSistema();
            
            // Insert null
            {
                Assert.Throws<ModelException>(() => sistema.Save(null));
            }
            
            // Insert persona
            {
                _output.WriteLine("Testing insert ..");
                Persona persona = new Persona()
                {
                    Rut = "130144918",
                    Nombre = "Diego",
                    Paterno = "Urrutia",
                    Materno = "Astorga",
                    Email = "durrutia@ucn.cl"
                };

                sistema.Save(persona);
            }
            
            // GetPersonas
            {
                _output.WriteLine("Testing getPersonas ..");
                Assert.NotEmpty(sistema.GetPersonas());
            }
            
            // Buscar persona
            {
                _output.WriteLine("Testing Find ..");
                Assert.NotNull(sistema.Find("durrutia@ucn.cl"));
                Assert.NotNull(sistema.Find("130144918"));
            }
            
            // Busqueda de usuario
            {
                Exception usuarioNoExiste =
                    Assert.Throws<ModelException>(() => sistema.Login("notfound@ucn.cl", "durrutia123"));
                Assert.Equal("Usuario no encontrado", usuarioNoExiste.Message);
                
                Exception usuarioNoExistePersonaSi =
                    Assert.Throws<ModelException>(() => sistema.Login("durrutia@ucn.cl", "durrutia123"));
                Assert.Equal("Existe la Persona pero no tiene credenciales de acceso", usuarioNoExistePersonaSi.Message);                
            }
            
            // Insertar usuario
            {
                Persona persona = sistema.Find("durrutia@ucn.cl");
                Assert.NotNull(persona);
                _output.WriteLine("Persona: {0}", Utils.ToJson(persona));
                
                sistema.Save(persona, "durrutia123");
            }

            // Busqueda de usuario
            {
                Exception usuarioExisteWrongPassword =
                    Assert.Throws<ModelException>(() => sistema.Login("durrutia@ucn.cl", "este no es mi password"));
                Assert.Equal("Password no coincide", usuarioExisteWrongPassword.Message);

                Usuario usuario = sistema.Login("durrutia@ucn.cl", "durrutia123");
                Assert.NotNull(usuario);
                _output.WriteLine("Usuario: {0}", Utils.ToJson(usuario));

            }

        }

        [Fact]
        public void BusquedaCotizacionesTest()
        {
            _output.WriteLine("Starting System Test");
            ISistema sistema = Startup.BuildSistema();
            
            //insert cotizaciones
            {
                _output.WriteLine("Testing insert .. ");
                Cotizacion cotizacion = new Cotizacion();
                {
                    cotizacion.Id = 1;
                    cotizacion.RutCliente = "197116730";
                    cotizacion.RutUsuarioCreador = "181690321";
                    cotizacion.FechaCreacion = DateTime.Now;
                    cotizacion.Items = new List<Item>();
                    
                } ;
                Item item1 = new Item();
                {
                    item1.descripcion = "Item 1 de prueba";
                    item1.precio = 40000;
                    
                }
                ;
                Item item2 = new Item();
                {
                    item2.descripcion = "Item 2 de prueba";
                    item2.precio = 30000;
                    
                }
                
                Console.WriteLine(item1.descripcion);
                cotizacion.Items.Add(item1);
                cotizacion.Items.Add(item2);
                
                Console.WriteLine(cotizacion);
                Console.WriteLine(Utils.ToJson(cotizacion));
                sistema.AgregarCotizacion(cotizacion);
                

            }
            _output.WriteLine("Done .. ");
            _output.WriteLine("Probando criterio de busqueda");
            {
                
                    Assert.Throws<ArgumentException>(() => sistema.TipoBusqueda(" "));
                    Assert.Throws<ArgumentException>(() => sistema.TipoBusqueda(""));
                    Assert.Throws<ArgumentException>(() => sistema.TipoBusqueda(null));
                    Assert.Throws<ArgumentException>(() => sistema.BuscarCotizacion(" "));
                    Assert.Throws<ArgumentException>(() => sistema.BuscarCotizacion(""));
                    Assert.Throws<ArgumentException>(() => sistema.BuscarCotizacion(null));
            }
            _output.WriteLine("Done ..");
            
        }

        [Fact]
        public void EliminarCotizacionTest()
        {
            _output.WriteLine("Starting System Test .. ");
            ISistema sistema = Startup.BuildSistema();
            
            {
                _output.WriteLine("Testing insert ..");
                Cotizacion cotizacion = new Cotizacion();
                {
                    cotizacion.Id = 1;
                    cotizacion.RutCliente = "197116730";
                    cotizacion.RutUsuarioCreador = "181690321";
                    cotizacion.FechaCreacion = DateTime.Now;
                    cotizacion.Items = new List<Item>();
                    
                } ;
                
                Item item1 = new Item();
                {
                    item1.descripcion = "Item de prueba 1";
                    item1.precio = 40000;
                    
                }
                ;
                Item item2 = new Item();
                {
                    item2.descripcion = "Item de prueba 2";
                    item2.precio = 30000;
                    
                }
                Console.WriteLine(item1.descripcion);
                cotizacion.Items.Add(item1);
                cotizacion.Items.Add(item2);

                Console.WriteLine(cotizacion);
                Console.WriteLine(Utils.ToJson(cotizacion));     
                sistema.AgregarCotizacion(cotizacion);

            }
            _output.WriteLine("Done .. ");
            _output.WriteLine("Testing criterio Id eliminacion");

            {
                Assert.Throws<ModelException>(() => sistema.EliminarCotizacion(111111)); //numero inexistente
                Assert.Throws<ModelException>(() => sistema.EliminarCotizacion(0)); // numero 0
                Assert.Throws<ArgumentException>(() => sistema.BuscarCotizacion(" "));
                Assert.Throws<ArgumentException>(() => sistema.BuscarCotizacion(""));
                Assert.Throws<ArgumentException>(() => sistema.BuscarCotizacion(null));
            }
            _output.WriteLine("Done .. ");
            
        }

        [Fact]
        public void AgregarCotizacionTest()
        {
            _output.WriteLine("Starting System Test .. ");
            ISistema sistema = Startup.BuildSistema();
            Cotizacion cotizacionAdecuada = new Cotizacion();
            {
                cotizacionAdecuada.Id = 1111111,
                cotizacionAdecuada.RutCliente = "197116730",
                cotizacionAdecuada.RutUsuarioCreador = "181690321",
                cotizacionAdecuada.FechaCreacion = DateTime.Now,
                cotizacionAdecuada.Items = new List<Item>()
            }
            ;
            Item item1 = new Item();
            {
                item1.descripcion = "Item 1 de prueba";
                item1.precio = 40000;
                
            }
            ;
            Item item2 = new Item();
            {
                item2.descripcion = "Item 2 de prueba";
                item2.precio = 30000;
                
            }
            
            _output.WriteLine("Cotizacion creada correctamente");
            
        }
        


    }
}