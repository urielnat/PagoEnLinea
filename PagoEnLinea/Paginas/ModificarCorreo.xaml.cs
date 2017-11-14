using System;
using System.Collections.Generic;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    public partial class ModificarCorreo : ContentPage
    {
        string ID;
        public ModificarCorreo(string id)
        {
            ID = id;
            
            InitializeComponent();
           
        }
        void conectar()
        {
            ClienteRest cliente = new ClienteRest();
            Email email = new Email();
            email.id = ID;
            email.correoe = enCorreo.Text;
            email.tipo = enTipo.Text;

            cliente.PUT("http://192.168.0.18:8080/api/email/actualizar-email",email);
            MessagingCenter.Subscribe<ClienteRest>(this, "putcorreo", (sender) =>
              {
                  DisplayAlert("Guardado", "¡Correo Modificado con Exito!", "OK");
                  Navigation.PopAsync();
              });

            MessagingCenter.Subscribe<ClienteRest>(this, "errorCorreo", (Sender) => {
                DisplayAlert("Error", "¡No fue posible modifcar el correo!", "OK");

            });
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            conectar();

        }
    }
}
