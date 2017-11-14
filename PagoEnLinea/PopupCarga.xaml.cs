using System;
using System.Collections.Generic;
using System.Net.Http;
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
        string contraseña;
        public PopupCarga(string user, string psw)
        {
            auth.username = user;
            auth.password = psw;
            inf.usuario = user;
           
            InitializeComponent();
            conecta();
        }
        void conecta()
        {
            try
            { //1002000001
                
                ClienteRest client = new ClienteRest();
                client.POST("http://192.168.0.18:8080/api/authenticate",auth,0);




               
                MessagingCenter.Subscribe<ClienteRest>(this, "errorp",async (Sender) => {
                    await PopupNavigation.PopAsync();
                    MessagingCenter.Send(this,"noAuth");
                });
                MessagingCenter.Subscribe<ClienteRest>(this, "OKR", async (Sender) => {
                    // client.POST("http://192.168.0.18:8080/api/info-contribuyente",inf);


                    await PopupNavigation.PopAsync();
                                               
                   Application.Current.Properties["user"] = inf.usuario;
                    Application.Current.Properties["psw"] = auth.password;
                    await Application.Current.SavePropertiesAsync();
                    Application.Current.MainPage = new MasterDetailPage { Master = new MenuMaster(), Detail = new NavigationPage(new HomePage("")) };
                });



                
            }

            catch { }
        }
    }
}