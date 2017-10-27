using System;
using System.Collections.Generic;

namespace PagoEnLinea.Modelos
{
    public class Usuario
    {
        public string nombre { get; set; }
        public string apellidoMaterno { get; set; }
        public string apellidoPaterno { get; set; }
        public string sexo { get; set; }
        public string correo { get; set; }
        public string contraseña { get; set; }
        public string confirmarContraseña { get; set; }
        public string razonSocial { get; set; }
        public string domicilio { get; set; }
        public string colonia { get; set; }
        public string numero { get; set; }
        public string codigoPostal { get; set; }
        public string estado { get; set; }
        public string ciudad { get; set; }
        public string telefono { get; set; }
    }

    public class Usuarios
    {
        public IList<Usuario> data { get; set; }
    }
}
