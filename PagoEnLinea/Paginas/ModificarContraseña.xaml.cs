using System;
using System.Collections.Generic;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    public partial class ModificarContraseña : ContentPage
    {
        public ModificarContraseña()
        {
            InitializeComponent();
        }

        async void conectar()
        {
            if (Application.Current.Properties.ContainsKey("user"))
            {


                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["user"] as string);
                if (inf != null)
                {
                    contraseña psw = new contraseña();
                    psw.persona = inf.persona;
                    bool auth = false, auth2 = false, auth3 = false;
                    psw.contrasena = enPsw.Text;
                    ClienteRest client = new ClienteRest();
                    if(!String.IsNullOrEmpty(enPsw.Text)&&!(enPsw.Text.Length<6)){

                        auth = true;

                    }else{
                        auth = false;
                       await DisplayAlert("Error","Contraseña Invalida","ok");
                    }
                    if(enPsw2.Text.Equals(enPsw.Text)){
                        auth2 = true;
                    }else{
                        auth2 = false;
                        await DisplayAlert("Error", "Las Contraseñas No Concuerdan", "OK");
                    }
                    if (Application.Current.Properties.ContainsKey("psw"))
                    {  if (enPsw0.Text.Equals(Application.Current.Properties["psw"] as string)){
                            auth3 = true;
                    }else{
                            auth3 = false;
                    }
                    }
                   

                    if(auth&&auth2&&auth3){
                        client.PUT("http://192.168.0.18:8080/api/usuarios/actualizar-contrasena", psw);
                        MessagingCenter.Subscribe<ClienteRest>(this, "putcontraseña", (Sender) => {
                            DisplayAlert("Guardado","¡Contraseña Modificada con Exito!","OK");
                            Navigation.PopAsync();
                        });

                        MessagingCenter.Subscribe<ClienteRest>(this, "errorContraseña", (Sender) => {
                            DisplayAlert("Error", "¡No fue posible modifcar la Contraseña", "OK");
                           
                        });
                    }
                }

            }

        }


        void Handle_Clicked(object sender, System.EventArgs e)
        {
            conectar();
        }
    }
}
