using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01WebApi.Models;

namespace P01WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarrerasController : ControllerBase
    {
        private readonly equiposContext _equiposContext;

        public CarrerasController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }

        [HttpGet]
        [Route("getall_Carreras")]
        public IActionResult ObtenerCarreras()
        {
            var listadoCarreras = (from c in _equiposContext.carreras

                                   join f in _equiposContext.facultades
                                   on c.facultad_id equals f.facultad_id

                                   select c).ToList();

            if (listadoCarreras.Count == 0) { return NotFound(); }

            return Ok(listadoCarreras);
        }

        [HttpPost]
        [Route("add_Carrera")]
        public IActionResult CrearCarrera([FromBody] carreras CarreraNueva)
        {
            try
            {

                _equiposContext.carreras.Add(CarreraNueva);
                _equiposContext.SaveChanges();

                return Ok(CarreraNueva);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("actualizar_Carrera/{id}")]

        public IActionResult actualizarCarrera(int id, [FromBody] carreras CarreraModificar)
        {
            carreras? CarreraExiste = (from c in _equiposContext.carreras
                                   where c.carrera_id == id
                                   select c).FirstOrDefault();

            if (CarreraExiste == null)
                return NotFound();

            CarreraExiste.nombre_carrera = CarreraModificar.nombre_carrera;
            CarreraExiste.facultad_id = CarreraModificar.facultad_id;

            _equiposContext.Entry(CarreraExiste).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(CarreraExiste);

        }

        [HttpDelete]
        [Route("delete_Carrera/{id}")]

        public IActionResult eliminarCarrera(int id)
        {
            carreras? CarreraExiste = (from c in _equiposContext.carreras
                                       where c.carrera_id == id
                                       select c).FirstOrDefault();

            if (CarreraExiste == null)
                return NotFound();

            _equiposContext.carreras.Attach(CarreraExiste);
            _equiposContext.carreras.Remove(CarreraExiste);
            _equiposContext.SaveChanges();

            return Ok();
        }
    }
}
