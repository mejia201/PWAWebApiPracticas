using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace P01WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class EquiposController : ControllerBase
    {
        private readonly equiposContext _equiposContext;

        public EquiposController(equiposContext equiposContext)
        {
            this._equiposContext = equiposContext;
        }

        [HttpGet]
        [Route("getall")]
        public IActionResult ObtenerEquipos()
        {
            var listadoEquipo = (from e in _equiposContext.equipos
                                            
                                            join t in _equiposContext.tipo_equipo
                                            on e.tipo_equipo_id equals t.id_tipo_equipo

                                            join m in _equiposContext.marcas
                                            on e.marca_id equals m.id_marcas

                                            join es in _equiposContext.estados_equipo
                                            on e.estado_equipo_id equals es.id_estados_equipo

                                            select new
                                            {
                                                e.id_equipos,
                                                e.nombre,
                                                e.descripcion,
                                                e.tipo_equipo_id,
                                                tipo_equipo = t.descripcion,
                                                e.marca_id,
                                                marca = m.nombre_marca,
                                                e.estado_equipo_id,
                                                estados_equipo = es.descripcion,
                                                e.estado
                                            }).ToList();

            //.Take(1) funciona como Top en base de datos
            //.Skip(3) saltar 3 
            //OrderBy(r => e.marca_id). ThenBy(r=> r.tipo_equipo)


            if(listadoEquipo.Count == 0) { return NotFound(); }

            return Ok(listadoEquipo);
        }


        //localhost: 4455/api/equipos/getbyid?=13&nombre=pwa
        //localhost: 4455/api/equipos/getbyid/23
        
        [HttpGet]
        [Route("getbyid/{id}")]
        public IActionResult get(int id)
        {
            //cuando pongo para comparar el estado me da error la petición, no se porque 
            equipos? unEquipo = (from e in _equiposContext.equipos
                          where e.id_equipos == id //&& e.estado =="A"
                          select e).FirstOrDefault();

            if(unEquipo == null)
            { return NotFound(); }

            return Ok(unEquipo);
        }

        [HttpGet]
        [Route("find")]

        public IActionResult buscar(string filtro)
        {
            List<equipos> equiposList = (from e in _equiposContext.equipos
                                         where (e.nombre.Contains(filtro)
                                         || e.descripcion.Contains(filtro))
                                        // && e.estado == "A"
                                         select e).ToList();

            //if(equiposList.Count() == 0) { return NotFound(); }

            if (equiposList.Any())
            {
                return Ok(equiposList);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Crear([FromBody] equipos equipoNuevo)
        {
            try
            {
               // equipoNuevo.estado = "A";
                _equiposContext.equipos.Add(equipoNuevo);
                _equiposContext.SaveChanges();

                return Ok(equipoNuevo);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("actualizar/{id}")]

        public IActionResult actualizarEquipo(int id, [FromBody] equipos equipoModificar)
        {
            equipos? equipoExiste = (from e in _equiposContext.equipos
                                    where e.id_equipos == id
                                    select e).FirstOrDefault();

            if(equipoExiste == null)
            return NotFound();

            equipoExiste.nombre = equipoModificar.nombre;
            equipoExiste.descripcion = equipoModificar.descripcion;

            _equiposContext.Entry(equipoExiste).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(equipoExiste);

        }

        [HttpDelete]
        [Route("delete/{id}")]

        public IActionResult eliminarEquipo(int id)
        {
            equipos? equipoExiste = (from e in _equiposContext.equipos
                                     where e.id_equipos == id
                                     select e).FirstOrDefault();

            if (equipoExiste == null)
                return NotFound();

            _equiposContext.equipos.Attach(equipoExiste);
            _equiposContext.equipos.Remove(equipoExiste);
            _equiposContext.SaveChanges();

            return Ok();
        }

    }

    
}
