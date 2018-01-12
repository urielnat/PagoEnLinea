using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace PagoEnLinea
{
    /// <summary>
    /// esta clase es una subpantalla de permite mostrar un cuadro de dialogo en la aplicación
    /// para visualizar una barra de carga mientras se conecta al servidor
    ///
    /// </summary>
    public partial class PopupCarga
    {
      
        authenticate auth = new authenticate();
        info inf = new info();
        /// <summary>
        /// constructor de la aplicación hace una llamada al método conectar además de que asigna
        /// parámetros a un objeto de tipo Authenticate el cual servira para enviarlo como JSON al
        /// consumir el servicio de autenticar
        /// </summary>
        /// <param name="user">Contiene el nombre de usuario (correo electrónico)</param>
        /// <param name="psw">Contiene la contraseña del usuario</param>
        public PopupCarga(string user, string psw)
        {
            auth.username = user;
            auth.password = psw;
           
            Application.Current.Properties.Clear(); 
            InitializeComponent();
            conecta();
        }

        /// <summary>
        /// consume al servicio de autenticación mediante post, en cas de ser exitoso
        /// manda el mensaje "Auth" la pantalla de login para notificar al usuario
        ///si las credenciales de inicio de sesión son incorrectas manda el mensaje "noAuth"
        ///si no es posible conectarse al servidor o recibe un código de error manda el mensaje "errorServidor"
        /// </summary>
        async void conecta()
        {
            try
            { 




                    HttpResponseMessage response;

                    string ContentType = "application/json"; // or application/xml
                    var jsonstring = JsonConvert.SerializeObject(auth);


                    try
                    {
                        HttpClient cliente = new HttpClient();
                    response = await cliente.PostAsync(Constantes.URL_USUARIOS+"/authenticate", new StringContent(jsonstring, Encoding.UTF8, ContentType));
                        var y = await response.Content.ReadAsStringAsync();
                        //System.Diagnostics.Debug.WriteLine(y);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            
                        var users = JsonConvert.DeserializeObject<Token>(y);
                        System.Diagnostics.Debug.WriteLine(users.id_token);
                        Application.Current.Properties["token"] = users.id_token;
                        MessagingCenter.Send(this, "Auth");  
                        await PopupNavigation.PopAsync();

                    }else{
                        
                        var resp = JsonConvert.DeserializeObject<Respuesta>(y);
                        MessagingCenter.Send(this, "noAuth",resp.respuesta); 
                        await PopupNavigation.PopAsync();

                        System.Diagnostics.Debug.WriteLine(response);

                    }
                       
                        
                    }
                    catch (HttpRequestException e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
                    MessagingCenter.Send(this, "errorServidor");
                    await PopupNavigation.PopAsync();

                    }
                
               
               




                
            }

            catch { }
        }
    }
}