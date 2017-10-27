using System;
using System.Collections.Generic;
using System.Net.Http;
using PagoEnLinea.servicios;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace PagoEnLinea
{
    public partial class PopupCarga
    {
        string u;
        public PopupCarga(string user, string psw)
        {
            u = user;
            InitializeComponent();
            conecta();
        }
        void conecta()
        {
            try
            { //1002000001
                
                ClienteRest client = new ClienteRest();
                client.PUT("http://192.168.0.29/api/cajeros/asignarCaja",u);




               
                MessagingCenter.Subscribe<ClienteRest>(this, "error",async (Sender) => await PopupNavigation.PopAsync());
                MessagingCenter.Subscribe<ClienteRest>(this, "ok", async (Sender) => await PopupNavigation.PopAsync());



                
            }

            catch { }
        }
    }
}