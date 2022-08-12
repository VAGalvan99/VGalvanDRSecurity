using System;
using System.Collections.Generic;

namespace DL
{
    public partial class Persona
    {
        public int IdPersona { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Sexo { get; set; }
        public int? IdEstado { get; set; }
        public string? Curp { get; set; }
        public string? Imagen { get; set; }
        public string? EstadoNombre { get; set; }
        public virtual Estado? IdEstadoNavigation { get; set; }
    }
}
