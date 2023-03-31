using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace P01WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Tipos_equiposController : ControllerBase
    {

        private readonly equiposContext _equiposContext;

        public Tipos_equiposController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }


        [HttpGet]
        [Route("getall_tipoEquipos")]
        public IActionResult ObtenerTiposEquipos()
        {
            List<tipo_equipo> listadoTipoEquipo = (from t in _equiposContext.tipo_equipo
                                                   select t).ToList();


            if (listadoTipoEquipo.Count == 0) { return NotFound(); }

            return Ok(listadoTipoEquipo);
        }

        [HttpPost]
        [Route("add_TipoEquipo")]
        public IActionResult Crear([FromBody] tipo_equipo TipoequipoNuevo)
        {
            try
            {
                // equipoNuevo.estado = "A";
                _equiposContext.tipo_equipo.Add(TipoequipoNuevo);
                _equiposContext.SaveChanges();

                return Ok(TipoequipoNuevo);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("actualizar_TipoEquipo/{id}")]

        public IActionResult actualizarTipoEquipo(int id, [FromBody] tipo_equipo equipoModificar)
        {
            tipo_equipo? TipoequipoExiste = (from t in _equiposContext.tipo_equipo
                                     where t.id_tipo_equipo == id
                                     select t).FirstOrDefault();

            if (TipoequipoExiste == null)
                return NotFound();

            TipoequipoExiste.descripcion = equipoModificar.descripcion;
            TipoequipoExiste.estado = equipoModificar.estado;

            _equiposContext.Entry(TipoequipoExiste).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(TipoequipoExiste);

        }

        [HttpDelete]
        [Route("delete_TipoEquipo/{id}")]

        public IActionResult eliminarTipoEquipo(int id)
        {
            tipo_equipo? TipoEquipoExiste = (from t in _equiposContext.tipo_equipo
                                     where t.id_tipo_equipo == id
                                     select t).FirstOrDefault();

            if (TipoEquipoExiste == null)
                return NotFound();

            _equiposContext.tipo_equipo.Attach(TipoEquipoExiste);
            _equiposContext.tipo_equipo.Remove(TipoEquipoExiste);
            _equiposContext.SaveChanges();

            return Ok();
        }
    }
}
