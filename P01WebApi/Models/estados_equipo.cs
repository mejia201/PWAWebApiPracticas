﻿using System.ComponentModel.DataAnnotations;


namespace P01WebApi.Models
{
    public class estados_equipo
    {

        [Key]
        public int id_estados_equipo { get; set; }
        public string? descripcion { get; set; }
        public string? estado { get; set; }
    }
}
