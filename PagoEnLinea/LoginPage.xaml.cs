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
        /// <summary>
        /// constructor de la aplicacion añade un evento de tipo gesturo
        /// para detectar cuando se preciono el texto de olvido de contraseña
        /// una vez precionaddo lleva a la pantalla para ingresar correo electronico correspondiente
        /// el constructor esta subscrito a una subpantalla llamada PopUpcarga la cual muestra un cuadro de dialogo de carga
        /// el cual se muestra hasta que se recibe una respuesta del servidor
        /// esta respuesta puede ser exitosa "Auth" lo que permite llevar al usuario a la pagina principal de la aplicacion
        /// otra respuesta puede ser "noAuth" lo que implica que las credenciales de inicio de sesion no son correctas y se notifica al usuario
        /// la respuesta obtenida "errorservidor" implica que la conexion con el servidor fallo por algun motivo y se notifica al usuario
        /// </summary>
        public LoginPage()
        {
            InitializeComponent();
           
            var tgr = new TapGestureRecognizer();
            tgr.Tapped += (sender, e) =>
            {
                Navigation.PushAsync(new RecuperarPage());
            };
           



            MessagingCenter.Subscribe<PopupCarga,string>(this, "noAuth", async(Sender,value) => { enUsuario.ErrorText = "Correo electronico"; enContraseña.ErrorText = "contraseña";
                MessagingCenter.Unsubscribe<PopupCarga,string>(this, "noAuth");
                await DisplayAlert("Error",value,"ok");
               
            });

            MessagingCenter.Subscribe<PopupCarga>(this, "errorServidor", (Sender) => { DisplayAlert("Error","No fué posible conectarse al servidor intente mas tarde","OK");
                MessagingCenter.Unsubscribe<PopupCarga>(this, "errorServidor");
            });

            MessagingCenter.Subscribe<PopupCarga>(this, "Auth", async(Sender) => {
                MessagingCenter.Unsubscribe<PopupCarga,string>(this, "noAuth");
                Application.Current.Properties["user"] = enUsuario.Text;
                Application.Current.Properties["psw"] = enContraseña.Text;
                System.Diagnostics.Debug.WriteLine(Application.Current.Properties["user"] as string);

                await  Application.Current.SavePropertiesAsync();
                MessagingCenter.Unsubscribe<PopupCarga>(this, "Auth");
                  fpm = new MasterDetailPage { Master = new MenuMaster(), Detail = new NavigationPage(new HomePage()) };
            
                await Navigation.PushModalAsync(fpm);
               
            });


            olvido.GestureRecognizers.Add(tgr);

           

        }


        /// <summary>
        /// evento click al precionar el boton registrar permite llevar al usuario a la pantalla de registro
        /// 
        /// </summary>
        /// <param name="sender">objeto que hace refencia al eveto</param>
        /// <param name="e">argumentos que son posibles de obtener apartir de la llamada del metodo</param>
        void Registrar_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new RegistroPage());
        }


        /// <summary>
        /// evento click al precionar el boton iniciar sesion el cual permite mostrar la subpantalla PopUpcarga al usuario
        /// </summary>
        /// <param name="sender">objeto que hace refencia al eveto</param>
        /// <param name="e">argumentos que son posibles de obtener apartir de la llamada del metodo</param>
       async void Handle_Clicked(object sender, System.EventArgs e)
        {
            
           
            await PopupNavigation.PushAsync(new PopupCarga(enUsuario.Text, enContraseña.Text), false);
           
        }

    }
}

