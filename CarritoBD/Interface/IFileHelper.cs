using System;
namespace CarritoBD.Interface
{

    /// <summary>
    ///interfaz que permite obtener el archivo base de datos en el almacenamiento interno del dispositivo apartir de su nombre
    /// es necesaria debido a que el archivo difiere de su localizacion en dispositivos android y iOS
    /// la clase que hacen uso de esta interfaz tienen el nombre de FileHelper y se encuentran en los modulos .Droid y .iOS respectivamente
    /// </summary>
    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
    }
}
