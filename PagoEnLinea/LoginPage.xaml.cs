using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using PagoEnLinea.servicios;
using PagoEnLinea.Paginas;

namespace PagoEnLinea
{
    public partial class LoginPage : ContentPage
    {
       
        MasterDetailPage fpm = new MasterDetailPage();
       
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

            MessagingCenter.Subscribe<PopupCarga>(this, "noAuth", (Sender) => { enUsuario.ErrorText = "Correo electronico"; enContraseña.ErrorText = "contraseña"; });


            olvido.GestureRecognizers.Add(tgr);

        }



        void Registrar_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new RegistroPage());
        }

       async void Handle_Clicked(object sender, System.EventArgs e)
        {
            //Application.Current.MainPage = new Menu();

           // Application.Current.MainPage = new MasterDetailPage { Master = new MenuMaster(), Detail = new NavigationPage(new HomePage()) };
            //await Navigation.PushModalAsync(new NavigationPage(fmp);
            await PopupNavigation.PushAsync(new PopupCarga(enUsuario.Text, enContraseña.Text), true);
            //viewModel.SignIn();
        }
    }
}

