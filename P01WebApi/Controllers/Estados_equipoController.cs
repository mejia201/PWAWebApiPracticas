using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace P01WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Estados_equipoController : ControllerBase
    {
        private readonly equiposContext _equiposContext;

        public Estados_equipoController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }


        [HttpGet]
        [Route("getall_EstadoEquipos")]
        public IActionResult ObtenerEstadoEquipos()
        {
            List<estados_equipo> listadoEstadoEquipo = (from s in _equiposContext.estados_equipo
                                           select s).ToList();

            if (listadoEstadoEquipo.Count == 0) { return NotFound(); }

            return Ok(listadoEstadoEquipo);
        }

        [HttpPost]
        [Route("add_EstadoEquipo")]
        public IActionResult CrearEstadoEquipo([FromBody] estados_equipo EstadoequipoNuevo)
        {
            try
            {

                _equiposContext.estados_equipo.Add(EstadoequipoNuevo);
                _equiposContext.SaveChanges();

                return Ok(EstadoequipoNuevo);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("actualizar_EstadoEquipo/{id}")]

        public IActionResult actualizarEstadoEquipo(int id, [FromBody] estados_equipo EstadoEquipoModificar)
        {
            estados_equipo? EstadoEquipoExiste = (from s in _equiposContext.estados_equipo
                                             where s.id_estados_equipo == id
                                             select s).FirstOrDefault();

            if (EstadoEquipoExiste == null)
                return NotFound();

            EstadoEquipoExiste.descripcion = EstadoEquipoModificar.descripcion;
            EstadoEquipoExiste.estado = EstadoEquipoModificar.estado;

            _equiposContext.Entry(EstadoEquipoExiste).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(EstadoEquipoExiste);

        }

        [HttpDelete]
        [Route("delete_EstadoEquipo/{id}")]

        public IActionResult eliminarEstadoEquipo(int id)
        {
            estados_equipo? EstadoEquipoExiste = (from s in _equiposContext.estados_equipo
                                             where s.id_estados_equipo == id
                                             select s).FirstOrDefault();

            if (EstadoEquipoExiste == null)
                return NotFound();

            _equiposContext.estados_equipo.Attach(EstadoEquipoExiste);
            _equiposContext.estados_equipo.Remove(EstadoEquipoExiste);
            _equiposContext.SaveChanges();

            return Ok();
        }
    }
}
