using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01WebApi.Models;

namespace P01WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {

        private readonly equiposContext _equiposContext;

        public ReservasController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }


        [HttpGet]
        [Route("getall_Reservas")]
        public IActionResult ObtenerReserva()
        {
            var listadoReserva = (from r in _equiposContext.reservas
                                      //select r).ToList();

                                  join e in _equiposContext.equipos
                                  on r.equipo_id equals e.id_equipos

                                  join u in _equiposContext.usuarios
                                  on r.usuario_id equals u.usuario_id

                                  join es in _equiposContext.estados_reserva
                                  on r.estado_reserva_id equals es.estado_res_id

                                  select new
                                  {

                                      r.reserva_id,
                                      r.equipo_id,
                                      r.usuario_id,
                                      r.fecha_salida,
                                      r.hora_salida,
                                      r.tiempo_reserva,
                                      estado = es.estado_res_id,
                                      r.fecha_retorno,
                                      r.hora_retorno

                                  }).ToList();

            if (listadoReserva.Count == 0) { return NotFound(); }

            return Ok(listadoReserva);
        }

        [HttpPost]
        [Route("add_Reserva")]
        public IActionResult CrearReserva([FromBody] reservas ReservaNuevo)
        {
            try
            {

                _equiposContext.reservas.Add(ReservaNuevo);
                _equiposContext.SaveChanges();

                return Ok(ReservaNuevo);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("actualizar_Reserva/{id}")]

        public IActionResult actualizarReserva(int id, [FromBody] reservas ReservaModificar)
        {
            reservas? ReservaExiste = (from r in _equiposContext.reservas
                                       where r.reserva_id == id
                                       select r).FirstOrDefault();

            if (ReservaExiste == null)
                return NotFound();

            ReservaExiste.equipo_id = ReservaModificar.equipo_id;
            ReservaExiste.usuario_id = ReservaModificar.usuario_id;
            ReservaExiste.fecha_salida = ReservaModificar.fecha_salida;
            ReservaExiste.hora_salida = ReservaModificar.hora_salida;
            ReservaExiste.tiempo_reserva = ReservaModificar.tiempo_reserva;
            ReservaExiste.estado_reserva_id = ReservaModificar.estado_reserva_id;
            ReservaExiste.fecha_retorno = ReservaModificar.fecha_retorno;
            ReservaExiste.hora_retorno = ReservaModificar.hora_retorno;


            _equiposContext.Entry(ReservaExiste).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(ReservaExiste);

        }

        [HttpDelete]
        [Route("delete_Reserva/{id}")]

        public IActionResult eliminarReserva(int id)
        {
            reservas? ReservaExiste = (from r in _equiposContext.reservas
                                       where r.reserva_id == id
                                       select r).FirstOrDefault();

            if (ReservaExiste == null)
                return NotFound();

            _equiposContext.reservas.Attach(ReservaExiste);
            _equiposContext.reservas.Remove(ReservaExiste);
            _equiposContext.SaveChanges();

            return Ok();
        }
    }
}
