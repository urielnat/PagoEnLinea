using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using PagoEnLinea.servicios;

namespace PagoEnLinea
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            var tgr = new TapGestureRecognizer();
            tgr.Tapped += (sender, e) =>
            {
                olvido.Text = "Favor de comunicarse con Soporte Técnico \nTelefono: \nCorreo Electronico:";
                olvido.HorizontalTextAlignment = TextAlignment.Start;
                olvido.TextColor = Color.Black;
            };
            //algo asj

            MessagingCenter.Subscribe<ClienteRest>(this, "error", (Sender) => { enUsuario.ErrorText = "Correo electronico"; enContraseña.ErrorText = "algo"; });


            olvido.GestureRecognizers.Add(tgr);

        }



        void Registrar_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new RegistroPage());
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await PopupNavigation.PushAsync(new PopupCarga(enUsuario.Text, enContraseña.Text), true);
        }
    }
}

