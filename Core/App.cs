using System;
using System.Collections.Generic;
using Core.Controllers;
using Core.Models;

namespace Core
{
    /// <summary>
    /// 
    /// </summary>
    public class App
    {
        /// <summary>
        /// Punto de entrada de la aplicacion.
        /// </summary>
        /// <param name="args"></param>
        /// <exception cref="ModelException"></exception>
        private static void Main(string[] args)
        {
            Console.WriteLine("Building Sistema ..");
            ISistema sistema = Startup.BuildSistema();

            Console.WriteLine("Creating Persona ..");
            {
                Persona persona = new Persona()
                {
                    Rut = "130144918",
                    Nombre = "Diego",
                    Paterno = "Urrutia",
                    Materno = "Astorga",
                    Email = "durrutia@ucn.cl"
                };

                Console.WriteLine(persona);
                Console.WriteLine(Utils.ToJson(persona));

                // Save in the repository
                sistema.Save(persona);
            }

            Console.WriteLine("Finding personas ..");
            {
                IList<Persona> personas = sistema.GetPersonas();
                Console.WriteLine("Size: " + personas.Count);

                foreach (Persona persona in personas)
                {
                    Console.WriteLine("Persona = " + Utils.ToJson(persona));
                }
            }

            Console.WriteLine("Done.");

            Console.WriteLine("Creating Cotizacion ..");
            {
                Cotizacion cotizacion = new Cotizacion();
                {
                    cotizacion.Id = 1;
                    cotizacion.FechaCreacion = DateTime.Now;
                    cotizacion.RutCliente = "197116730";
                    cotizacion.RutUsuarioCreador = "181690321";
                    cotizacion.Items = new List<Item>();

                }
                ;
                Item item1 = new Item();
                {
                    item1.descripcion = "Item de prueba1";
                    item1.precio = 40000;
                }
                ;
                Item item2 = new Item();
                {
                    item2.descripcion = "Item de prueba2";
                    item2.precio = 30000;

                }

                Console.WriteLine(item1.descripcion);
                cotizacion.Items.Add(item1);
                cotizacion.Items.Add(item2);

                Console.WriteLine(cotizacion);
                Console.WriteLine(Utils.ToJson(cotizacion));
                sistema.AgregarCotizacion(cotizacion);

                IList<Cotizacion> cot = sistema.BuscarCotizacion("181690321");

            }

            Console.WriteLine("Creacion cotizacion .. done.");

        }
    }
}