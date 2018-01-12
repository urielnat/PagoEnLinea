using System;
using System.Collections.Generic;

namespace PagoEnLinea.Modelos
{


    /// <summary>
    /// modelo de usuarios, empleado para manipular los JSON al consumir servicios
    /// relacionados al perfil y creacion de un usuario
    /// </summary>
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
        public string tipo = "DOMICILIO";
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
        public IList<Telefono> telefono { get; set; }
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
        public string rol = "CONTRIBUYENTE";
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
        public IList<DatosFacturacion> datosFacturacion { get; set; }
    }

    public class infodir
    {
        public string NumerodeDireccion { get; set; }
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
        public string idCat { get; set; }
       
    }

    public class contraseña
    {
        public string contrasenaActual { get; set; }
        public string contrasenaNueva { get; set; }
       
       
    }

    public class TipoAsentamiento
    {
        public IList<string> respuesta { get; set; }
    }

    public  class AgregarDireccion{
        public Direccion direccion { get; set; }
        public Persona persona { get; set; }
        
    }

    public class Token
    {
        public string id_token { get; set; }
    }

    public class ModificarTelefono{
        public string id { get; set; }
        public Persona persona { get; set; }
        public Telefono telefono { get; set; }

    }

    public class DatosFacturacion{
        public Direccion direccion { get; set; }
        public Email email { get; set; }
        public string id { get; set; } //o int
        public string nomrazonSocial { get; set; }
        public Persona persona { get; set; }
        public string rfc { get; set; }

    }


    public class MostrarDatosFacturacion
    {
        public string direccion { get; set; }
        public string email { get; set; }
        public string id { get; set; }
        public string nomrazonSocial { get; set; }
        public Persona persona { get; set; }
        public string rfc { get; set; }
        public string idDireccion { get; set; }
        public string idCatDir { get; set; }
        public string calle { get; set; }

    }


    public class AgregarCorreo
    {
        public Email email { get; set; }
        public string id { get; set; }
        public Persona persona { get; set; }
    }


    public static class Constantes{

       public  static string URL_USUARIOS = "http://192.168.0.18:8080/api";
       public static string URL_CAJA = "http://192.168.0.18:8081/api";
    }

    public class Respuesta
    {
        public string respuesta { get; set; }
        public int id { get; set; }
        public string cp { get; set; }
        public string asentamiento { get; set; }
        public string tipoasentamiento { get; set; }
        public string municipio { get; set; }
        public string estado { get; set; }
        public string ciudad { get; set; }
        public string pais { get; set; }
        public string idPago { get; set; }
        public string fecha { get; set; }
        public string estatus { get; set; }
        public string importe { get; set; }



       
        public string concepto { get; set; }
        public string fechaVigencia { get; set; }

        public string llave { get; set; }
        public bool generoUs { get; set; }
        public string carroId { get; set; }
        public string liquidacionId { get; set; }
        
    }







    public class CodigoPostal
    {
        public IList<Respuesta> respuesta { get; set; }
    }

    public class CargaCP
    {
        public string asentamiento { get; set; }
        public string tipo { get; set; }
        public int catID { get; set; }

    }

    public class Historial
    {
        public IList<Respuesta> respuesta { get; set; }
    }

   

    public class DetallesHIstorial
    {
        public IList<Respuesta> respuesta { get; set; }
    }

}

