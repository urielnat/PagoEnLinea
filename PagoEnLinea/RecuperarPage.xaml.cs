using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using PagoEnLinea.Modelos;
using Xamarin.Forms;

namespace PagoEnLinea
{
    /// <summary>
    /// esta clase consume al servicio mediante Post para establecer una nueva contraseña en caso
    /// de que el usuario olvidara la anterior
    /// </summary>
    public partial class RecuperarPage : ContentPage
    {
        public RecuperarPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// evento click asignado a un boton, permite obtener el texto de una entrada
        /// y enviarlo como argumento al consumir el servicio para establecer una nueva contraseña
        /// </summary>
        /// <param name="sender">Objeto que hace referencia al evento </param>
        /// <param name="e">propiedades que son accesibles para el objeto apartir del evento </param>
        async void Handle_Clicked(object sender, System.EventArgs e)
        {

            if (!string.IsNullOrEmpty(enCorreo.Text))
            {
                HttpResponseMessage response;

                string ContentType = "application/json"; // or application/xml
                var jsonstring = enCorreo.Text;

                System.Diagnostics.Debug.WriteLine(jsonstring);
                try
                {
                    HttpClient cliente = new HttpClient();
                    response = await cliente.PostAsync(Constantes.URL_USUARIOS + "/account/reset-password?movil=true&tramitta=false", new StringContent(jsonstring, Encoding.UTF8, ContentType));
                    System.Diagnostics.Debug.WriteLine(cliente.DefaultRequestHeaders);
                    var y = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(y);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {


                        await DisplayAlert("Información", "Se ha enviado un enlace a su correo electónico una vez que lo verifique podrá establecer una nueva contraseña", "OK");
                        await Navigation.PopToRootAsync();

                    }

                    else
                    {
                        System.Diagnostics.Debug.WriteLine(response.StatusCode);
                        var resp = JsonConvert.DeserializeObject<Respuesta>(y);

                        await DisplayAlert("Error", resp.respuesta, "OK");


                    }
                }
                catch (HttpRequestException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);

                }

            }
            else { await DisplayAlert("Error", "Ingrese su correo electrónico primero", "OK"); }
        }
    }
}
