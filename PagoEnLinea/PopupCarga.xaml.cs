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
    public partial class PopupCarga
    {
      
        authenticate auth = new authenticate();
        info inf = new info();
       
        public PopupCarga(string user, string psw)
        {
            auth.username = user;
            auth.password = psw;
            inf.usuario = user;
            Application.Current.Properties.Clear(); 
            InitializeComponent();
            conecta();
        }
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
                    response = await cliente.PostAsync(Constantes.URL+"/authenticate", new StringContent(jsonstring, Encoding.UTF8, ContentType));
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