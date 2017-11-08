using System;
using System.Collections.Generic;

namespace PagoEnLinea.Modelos
{
    public class CatalogoDir
    {
        public string asentamiento { get; set; }
        public string ciudad { get; set; }
        public string cp { get; set; }
        public string estado { get; set; }
        public string id { get; set; }
        public string municipio { get; set; }
        public string pais { get; set; }
        public string tipoasentamiento { get; set; }
    }

    public class Direccion
    {
        public string calle { get; set; }
        public CatalogoDir catalogoDir { get; set; }
        public string id { get; set; }
        public string numero { get; set; }
        public string numeroInterior { get; set; }
        public string tipo { get; set; }
    }

    public class Email
    {
        public string correoe { get; set; }
        public string id { get; set; }
        public string tipo { get; set; }
    }

    public class Persona
    {
        public string amaterno { get; set; }
        public string apaterno { get; set; }
        public string curp { get; set; }
        public string edoCivil { get; set; }
        public string fechanac { get; set; }
        public string id { get; set; }
        public string nombre { get; set; }
        public string sexo { get; set; }
    }

    public class Telefono
    {
        public string id { get; set; }
        public string lada { get; set; }
        public string telefono { get; set; }
        public string tipo { get; set; }
    }

    public class Usuarios
    {
        public string archivo { get; set; }
        public string certificado { get; set; }
        public string contrasena { get; set; }
        public string contrasenaSAT { get; set; }
        public Direccion direccion { get; set; }
        public Email email { get; set; }
        public string llave { get; set; }
        public Persona persona { get; set; }
        public Telefono telefono { get; set; }
        public string tipousuario { get; set; }
        public object rolsistema { get; set; }
        public IList<object> datosFacturacion { get; set; }

    }

    public class authenticate
    {
        public string password { get; set; }
        public bool rememberMe { get; set; }
        public string username { get; set; }
    }

    public class info{
        public string rol { get; set; }
        public string usuario { get; set;}

    }
    public class InfoUsuario
    {
        public Persona persona { get; set; }
        public IList<Email> email { get; set; }
        public IList<Telefono> telefonos { get; set; }
        public IList<Direccion> direcciones { get; set; }
        public object usuario { get; set; }
        public object rolsistema { get; set; }
        public IList<object> datosFacturacion { get; set; }
    }

    public class infodir
    {
        public string asentamiento { get; set; }
        public string ciudad { get; set; }
        public string cp { get; set; }
        public string estado { get; set; }
        public string id { get; set; }
        public string municipio { get; set; }
        public string pais { get; set; }
        public string tipoasentamiento { get; set; }
        public string calle { get; set; }
        public string numero { get; set; }
        public string numeroInterior { get; set; }
        public string tipo { get; set; }
    }


}

