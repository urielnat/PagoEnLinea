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

        MasterDetailPage fpm;
       
        public LoginPage()
        {
            InitializeComponent();
           
            var tgr = new TapGestureRecognizer();
            tgr.Tapped += (sender, e) =>
            {
                Navigation.PushAsync(new RecuperarPage());
            };
            //algo asj

            MessagingCenter.Subscribe<PopupCarga>(this, "noAuth", (Sender) => { enUsuario.ErrorText = "Correo electronico"; enContraseña.ErrorText = "contraseña"; });

            MessagingCenter.Subscribe<PopupCarga>(this, "errorServidor", (Sender) => { DisplayAlert("Error","No fué posible conectarse al servidor intente mas tarde","OK"); });

            MessagingCenter.Subscribe<PopupCarga>(this, "Auth", (Sender) => {
                Application.Current.Properties["user"] = enUsuario.Text;
                Application.Current.Properties["psw"] = enContraseña.Text;
                System.Diagnostics.Debug.WriteLine(Application.Current.Properties["user"] as string);

                Application.Current.SavePropertiesAsync();

                fpm = new MasterDetailPage { Master = new MenuMaster(), Detail = new NavigationPage(new HomePage()) };
            
                Navigation.PushModalAsync(fpm);
            });


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
            await PopupNavigation.PushAsync(new PopupCarga(enUsuario.Text, enContraseña.Text), false);
            //viewModel.SignIn();
        }
    }
}

