using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01WebApi.Models;

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
    }
}
