using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01WebApi.Models;

namespace P01WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcasController : ControllerBase
    {

        private readonly equiposContext _equiposContext;

        public MarcasController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }

        [HttpGet]
        [Route("getall_Marcas")]
        public IActionResult ObtenerMarcas()
        {
            List<marcas> listadoMarcas = (from m in _equiposContext.marcas
                                                        select m).ToList();

            if (listadoMarcas.Count == 0) { return NotFound(); }

            return Ok(listadoMarcas);
        }

        [HttpPost]
        [Route("add_Marca")]
        public IActionResult CrearMarca([FromBody] marcas MarcaNueva)
        {
            try
            {

                _equiposContext.marcas.Add(MarcaNueva);
                _equiposContext.SaveChanges();

                return Ok(MarcaNueva);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("actualizar_Marca/{id}")]

        public IActionResult actualizarMarca(int id, [FromBody] marcas MarcaModificar)
        {
            marcas? MarcaExiste = (from m in _equiposContext.marcas
                                                  where m.id_marcas == id
                                                  select m).FirstOrDefault();

            if (MarcaExiste == null)
                return NotFound();

            MarcaExiste.nombre_marca = MarcaModificar.nombre_marca;
            MarcaExiste.estados = MarcaModificar.estados;

            _equiposContext.Entry(MarcaExiste).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(MarcaExiste);

        }

        [HttpDelete]
        [Route("delete_Marca/{id}")]

        public IActionResult eliminarMarca(int id)
        {
            marcas? MarcaExiste = (from m in _equiposContext.marcas
                                                  where m.id_marcas == id
                                                  select m).FirstOrDefault();

            if (MarcaExiste == null)
                return NotFound();

            _equiposContext.marcas.Attach(MarcaExiste);
            _equiposContext.marcas.Remove(MarcaExiste);
            _equiposContext.SaveChanges();

            return Ok();
        }

    }
}
