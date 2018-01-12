using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    /// <summary>
    /// Esta clase muestra una pantalla al usuario con entradas para modificar su contraseña consumiendo
    /// el servicio correspondiente. 
    /// </summary>
    public partial class ModificarContraseña : ContentPage
    {
        public static string TOKEN;

        /// <summary>
        /// inicializa los componentes visuales del XAML
        /// añade un evento de tipo click al boton añadir  
        /// </summary>
        public ModificarContraseña()
        {
            InitializeComponent();
          
            btnAñadir.Clicked += BtnAñadir_Clicked;
           // btnCambiar.Clicked += Handle_Clicked;
        }


        /// <summary>
        /// Este método valida que las entradas de texto sean correctas,si no es así notifica al usuario
        /// una vez que sean correctas consume al servicio para modificar la contraseña
        /// </summary>
        async void conectar()
        {
            if (Application.Current.Properties.ContainsKey("token"))
            {

                TOKEN = Application.Current.Properties["token"] as string;
               
                    contraseña psw = new contraseña();

                    bool auth = false, auth2 = false, auth3 = false;
                    psw.contrasenaNueva = enPsw.Text;
                    psw.contrasenaActual = enPsw0.Text;
                    ClienteRest client = new ClienteRest();
                    if (!String.IsNullOrEmpty(enPsw.Text) && !(enPsw.Text.Length < 8))
                    {

                        auth = true;

                    }
                    else
                    {
                        auth = false;
                        await DisplayAlert("Error", "Contraseña incorrecta, deben ser al menos 8 caracteres", "ok");
                    }
                    if (!(string.IsNullOrEmpty(enPsw2.Text)))
                    {
                        if (enPsw2.Text.Equals(enPsw.Text))
                        {
                            auth2 = true;
                        }
                        else
                        {
                            auth2 = false;
                            await DisplayAlert("Error", "Las Contraseñas No Concuerdan", "OK");
                        }

                    }
                   

                    if (!(String.IsNullOrEmpty(enPsw0.Text)))
                    {
                        
                        auth3 = true;
                    }
                    else
                    {   await DisplayAlert("Error", "Introduzca su contraseña actual", "OK");
                        auth3 = false;
                    }
                
                  

                   

                    if(auth&&auth2&&auth3){
                        HttpResponseMessage response;

                        string ContentType = "application/json"; // or application/xml
                        var jsonstring = JsonConvert.SerializeObject(psw);

                        //jsonstring = jsonstring.Substring(1, jsonstring.Length - 2);
                        System.Diagnostics.Debug.WriteLine(jsonstring);

                        try
                        {
                            HttpClient clients = new HttpClient();
                            clients.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);

                            response = await clients.PutAsync(Constantes.URL_USUARIOS+"/usuarios/actualizar-contrasena",new StringContent(jsonstring, Encoding.UTF8, ContentType));
                            var y = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine(y);


                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {

                                await DisplayAlert("Guardado", "¡Contraseña modificada con exito!", "OK");
                                await Navigation.PopAsync();


                            }

                            else
                            {

                                var resp = JsonConvert.DeserializeObject<Respuesta>(y);

                                await DisplayAlert("Error", resp.respuesta, "OK");


                            }
                        }
                        catch (HttpRequestException ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);
                            await DisplayAlert("Error", "No fué posible contectarse al servidor intente mas tarde", "OK");

                        }

                    }
                

            }

        }



        /// <summary>
        /// evento que hace uso del metodo conectar(); para poder modificar la contraseña
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            
            conectar();
        }

        void BtnAñadir_Clicked(object sender, EventArgs e)
        {
            
            conectar();
        }
    }
}
