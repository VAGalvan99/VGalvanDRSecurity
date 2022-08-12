using System;
using System.Collections.Generic;

namespace DL
{
    public partial class Estado
    {
        public Estado()
        {
            Personas = new HashSet<Persona>();
        }

        public int IdEstado { get; set; }
        public string? Nombre { get; set; }

        public virtual ICollection<Persona> Personas { get; set; }
    }
}
