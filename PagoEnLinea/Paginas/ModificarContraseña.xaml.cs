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
    public partial class ModificarContraseña : ContentPage
    {
        public static string TOKEN;
        public ModificarContraseña()
        {
            InitializeComponent();
          
            btnAñadir.Clicked += BtnAñadir_Clicked;
           // btnCambiar.Clicked += Handle_Clicked;
        }

        async void conectar()
        {
            if (Application.Current.Properties.ContainsKey("token"))
            {

                TOKEN = Application.Current.Properties["token"] as string;
                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["token"] as string);
                if (inf != null)
                {
                    contraseña psw = new contraseña();
                    psw.persona = inf.persona;
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
                        await DisplayAlert("Error", "Contraseña Invalida", "ok");
                    }
                    if (!(string.IsNullOrEmpty(enPsw2.Text)))
                    {
                        if (enPsw2.Text.Equals(enPsw.Text))
                        {
                            auth2 = true;
                        }

                    }
                    else
                    {
                        auth2 = false;
                        await DisplayAlert("Error", "Las Contraseñas No Concuerdan", "OK");
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

                            response = await clients.PutAsync(Constantes.URL+"/usuarios/actualizar-contrasena",new StringContent(jsonstring, Encoding.UTF8, ContentType));
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

        }




        void Handle_Clicked(object sender, System.EventArgs e)
        {
            
            conectar();
        }

        void BtnAñadir_Clicked(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("algoaaaa");
            conectar();
        }
    }
}
