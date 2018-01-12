using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FullCameraPage
{
    /// <summary>
    /// clase que permite obtenr una imagen apartir de una fotografia
    /// esta clase se renderiza diferente según el sistema operativo del dispositivo
    /// pero sus metodos son usados como si fuera una interfaz, la clase que hace uso de sus metodos y atributos
    /// es CameraPageRenderer.cs que se encuentra en los modulos .Droid y iOS respectivamente
    /// </summary>
    public class CameraPage : ContentPage
    {
        /// <summary>
        /// instancia a la clase que obtiene la imagen capturada
        /// </summary>
        public delegate void PhotoResultEventHandler(PhotoResultEventArgs result);

        /// <summary>
        /// apatir de la imagen obtenida es posible realizar opciones ya sea para cancelar la operacion u obtener
        /// la imagen capturada
        /// </summary>
        public event PhotoResultEventHandler OnPhotoResult;

        /// <summary>
        /// permite setear la imagen capturada
        /// </summary>
        /// <param name="image">bytes de la imagen capturada</param>
        /// <param name="width">Ancho de la imagen</param>
        /// <param name="height">Alto de la imagen</param>
        public void SetPhotoResult(byte[] image, int width = -1, int height = -1)
        {
            OnPhotoResult?.Invoke(new PhotoResultEventArgs(image, width, height));
        }

        /// <summary>
        /// cancela la opracion al no asignarle parametros a la imagen
        /// </summary>
        public void Cancel()
        {
            OnPhotoResult?.Invoke(new PhotoResultEventArgs());
        }

    }

    /// <summary>
    /// Establece resultados de captura apartir de una image
    /// </summary>
    public class PhotoResultEventArgs : EventArgs
    {

        /// <summary>
        /// Si el resultado fue cancelado al priedad succes del modelo cambia a falsa y no establece el resto de propiedades
        /// </summary>
        public PhotoResultEventArgs()
        {
            Success = false;
        }

        /// <summary>
        /// si el resultado de captura es exitoso establece las siguientes proiedades apartir de la imagen capturada
        /// </summary>
        /// <param name="image">bytes de la imagen capturada</param>
        /// <param name="width">Ancho de la imagen</param>
        /// <param name="height">Alto de la imagen</param>
        public PhotoResultEventArgs(byte[] image, int width, int height)
        {
            Success = true;
            Image = image;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// modelo de la imagen con diversas propieddades
        /// </summary>
        /// <value>Imagen</value>
        public byte[] Image { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool Success { get; private set; }
    }
}