using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01WebApi.Models;

namespace P01WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultadesController : ControllerBase
    {

        private readonly equiposContext _equiposContext;

        public FacultadesController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }


        [HttpGet]
        [Route("getall_Facultades")]
        public IActionResult ObtenerFacultad()
        {
            List<facultades> listadoFacultades = (from f in _equiposContext.facultades
                                                        select f).ToList();

            if (listadoFacultades.Count == 0) { return NotFound(); }

            return Ok(listadoFacultades);
        }

        [HttpPost]
        [Route("add_Facultades")]
        public IActionResult CrearFacultad([FromBody] facultades FacultadNueva)
        {
            try
            {

                _equiposContext.facultades.Add(FacultadNueva);
                _equiposContext.SaveChanges();

                return Ok(FacultadNueva);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("actualizar_Facultad/{id}")]

        public IActionResult actualizarFacultad(int id, [FromBody] facultades FacultadModificar)
        {
            facultades? FacultadExiste = (from f in _equiposContext.facultades
                                                  where f.facultad_id == id
                                                  select f).FirstOrDefault();

            if (FacultadExiste == null)
                return NotFound();

            FacultadExiste.nombre_facultad = FacultadModificar.nombre_facultad;
            

            _equiposContext.Entry(FacultadExiste).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(FacultadExiste);

        }

        [HttpDelete]
        [Route("delete_Facultad/{id}")]

        public IActionResult eliminarFacultad(int id)
        {
            facultades? FacultadExiste = (from f in _equiposContext.facultades
                                                  where f.facultad_id == id
                                                  select f).FirstOrDefault();

            if (FacultadExiste == null)
                return NotFound();

            _equiposContext.facultades.Attach(FacultadExiste);
            _equiposContext.facultades.Remove(FacultadExiste);
            _equiposContext.SaveChanges();

            return Ok();
        }


    }
}
