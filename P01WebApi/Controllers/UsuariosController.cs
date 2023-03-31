using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01WebApi.Models;

namespace P01WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {

        private readonly equiposContext _equiposContext;

        public UsuariosController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }


        [HttpGet]
        [Route("getall_Usuarios")]
        public IActionResult ObtenerUsuario()
        {
            var listadoUsuarios = (from u in _equiposContext.usuarios

                                   join c in _equiposContext.carreras
                                   on u.carrera_id equals c.carrera_id

                                        select new{ 
                                              
                                            u.usuario_id,
                                            u.nombre,
                                            u.documento,
                                            u.tipo,
                                            u.carnet,
                                            id_carrera = c.carrera_id
                                            
                                           }).ToList();

            if (listadoUsuarios.Count == 0) { return NotFound(); }

            return Ok(listadoUsuarios);
        }

        [HttpPost]
        [Route("add_Usuarios")]
        public IActionResult CrearUsuario([FromBody] usuarios UsarioNuevo)
        {
            try
            {

                _equiposContext.usuarios.Add(UsarioNuevo);
                _equiposContext.SaveChanges();

                return Ok(UsarioNuevo);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("actualizar_Usuarios/{id}")]

        public IActionResult actualizarUsuario(int id, [FromBody] usuarios UsuarioModificar)
        {
            usuarios? UsuarioExiste = (from u in _equiposContext.usuarios
                                          where u.usuario_id == id
                                          select u).FirstOrDefault();

            if (UsuarioExiste == null)
                return NotFound();

            UsuarioExiste.nombre = UsuarioModificar.nombre;
            UsuarioExiste.documento = UsuarioModificar.documento;
            UsuarioExiste.tipo = UsuarioModificar.tipo;
            UsuarioExiste.carnet = UsuarioModificar.carnet;
            UsuarioExiste.carrera_id = UsuarioModificar.carrera_id;


            _equiposContext.Entry(UsuarioExiste).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(UsuarioExiste);

        }

        [HttpDelete]
        [Route("delete_Usuario/{id}")]

        public IActionResult eliminarUsuario(int id)
        {
            usuarios? UsuarioExiste = (from u in _equiposContext.usuarios
                                          where u.usuario_id == id
                                          select u).FirstOrDefault();

            if (UsuarioExiste == null)
                return NotFound();

            _equiposContext.usuarios.Attach(UsuarioExiste);
            _equiposContext.usuarios.Remove(UsuarioExiste);
            _equiposContext.SaveChanges();

            return Ok();
        }
    }
}
