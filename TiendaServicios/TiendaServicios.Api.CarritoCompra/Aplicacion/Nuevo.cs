using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Modelo;
using TiendaServicios.Api.CarritoCompra.Persistencia;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {

            public DateTime FechaCreacionSesion { get; set; }
            public List<string> ProductoLista { get; set; }
        }
        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CarritoContexto _contexto;
            public Manejador(CarritoContexto contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carrito = new CarritoSesion
                {
                    FechaCreacion = request.FechaCreacionSesion
                };

                _contexto.CarritoSesion.Add(carrito);
                var valor = await _contexto.SaveChangesAsync();
                if (valor == 0)
                {
                    throw new Exception("No se guardó el carrito");
                }

                int id = carrito.CarritoSesionId;
                foreach (var obj in request.ProductoLista)
                {
                    var carritoDetalle = new CarritoSesionDetalle
                    {
                        CarritoSesionId = id,
                        FechaCreacion = DateTime.Now,
                        ProductoSeleccionado = obj
                    };

                    _contexto.CarritoSesionDetalle.Add(carritoDetalle);
                };
                valor = await _contexto.SaveChangesAsync();
                if (valor > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se guardó el detalle del carrito");
            }
        }
    }

}
