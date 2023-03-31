using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace P01WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Estados_reservaController : ControllerBase
    {

        private readonly equiposContext _equiposContext;

        public Estados_reservaController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }

        [HttpGet]
        [Route("getall_EstadoReserva")]
        public IActionResult ObtenerEstadoReserva()
        {
            List<estados_reserva> listadoEstadoReserva = (from s in _equiposContext.estados_reserva
                                                        select s).ToList();

            if (listadoEstadoReserva.Count == 0) { return NotFound(); }

            return Ok(listadoEstadoReserva);
        }


        [HttpPost]
        [Route("add_EstadoReserva")]
        public IActionResult CrearEstadoReserva([FromBody] estados_reserva EstadoReservaNuevo)
        {
            try
            {

                _equiposContext.estados_reserva.Add(EstadoReservaNuevo);
                _equiposContext.SaveChanges();

                return Ok(EstadoReservaNuevo);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("actualizar_EstadoReserva/{id}")]

        public IActionResult actualizarEstadoReserva(int id, [FromBody] estados_reserva EstadoReservaModificar)
        {
            estados_reserva? EstadoReservaExiste = (from s in _equiposContext.estados_reserva
                                                  where s.estado_res_id == id
                                                  select s).FirstOrDefault();

            if (EstadoReservaExiste == null)
                return NotFound();

            EstadoReservaExiste.estado = EstadoReservaModificar.estado;

            _equiposContext.Entry(EstadoReservaExiste).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(EstadoReservaExiste);

        }

        [HttpDelete]
        [Route("delete_EstadoReserva/{id}")]

        public IActionResult eliminarEstadoReserva(int id)
        {
            estados_reserva? EstadoReservaExiste = (from s in _equiposContext.estados_reserva
                                                  where s.estado_res_id == id
                                                  select s).FirstOrDefault();

            if (EstadoReservaExiste == null)
                return NotFound();

            _equiposContext.estados_reserva.Attach(EstadoReservaExiste);
            _equiposContext.estados_reserva.Remove(EstadoReservaExiste);
            _equiposContext.SaveChanges();

            return Ok();
        }
    }
}
