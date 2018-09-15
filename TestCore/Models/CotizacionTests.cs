using System;
using System.Collections.Generic;
using Core;
using Core.Models;
using Xunit;

namespace TestCore.Models
{
    /// <summary>
    /// Testing clase Cotizacion.
    /// </summary>
    public class CotizacionTests
    {
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void TestConstructor()
        {
            Console.WriteLine("Creating Cotizacion ..");
            Cotizacion cotizacion = new Cotizacion()
            {
                Id = 1,
                RutCliente = "197116730",
                RutUsuarioCreador = "181690321",
                FechaCreacion = DateTime.Now,
                Items = new List<Item>()

            };
            Console.WriteLine(cotizacion);

        }

        [Fact]
        public void ValidateTest()
        {
            Cotizacion cotizacion = new Cotizacion()
            {
                Id = 1,
                RutCliente = "197116730",
                RutUsuarioCreador = "181690321",
                FechaCreacion = DateTime.MaxValue,
                Items = new List<Item>()

            };
        }
    }
}