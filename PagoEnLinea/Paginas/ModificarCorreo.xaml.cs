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
        public ModificarCorreo(string id,int tipo)
        {
            ID = id;
            
            InitializeComponent();
            if (tipo == 0)
            {
                btnModificar.IsVisible = true;
                btnAgregar.IsVisible = false;
            }
            if (tipo == 1)
            {
                enTipo.Text = "PERSONAL";
                btnAgregar.IsVisible = true;
                btnModificar.IsVisible = false;
            }
           
        }
        void conectar()
        {
            ClienteRest cliente = new ClienteRest();
            Email email = new Email();
            email.id = ID;
            email.correoe = enCorreo.Text;
            email.tipo = enTipo.Text;

            cliente.PUT(Constantes.URL+"/email/actualizar-email",email);
            MessagingCenter.Subscribe<ClienteRest>(this, "putcorreo", (sender) =>
              {
                  DisplayAlert("Guardado", "¡Correo Modificado con Exito!", "OK");
                  Navigation.PopAsync();
              });

            MessagingCenter.Subscribe<ClienteRest>(this, "errorCorreo", (Sender) => {
                DisplayAlert("Error", "¡No fué posible modifcar el correo!", "OK");

            });
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            conectar();

        }

       async void Agregar_Clicked(object sender, System.EventArgs e)
        {
            if (Application.Current.Properties.ContainsKey("token"))
            {


                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["token"] as string);
                if (inf != null)
                {
                    AgregarCorreo addemail = new AgregarCorreo();
                    addemail.persona = inf.persona;
                    addemail.email = new Email
                    {
                        correoe = enCorreo.Text,
                        tipo = "PERSONAL"
                    };
                    ClienteRest client = new ClienteRest();
                   


                    client.POST(Constantes.URL + "/email/agregar", addemail,1);
                        MessagingCenter.Subscribe<ClienteRest>(this, "OK", (Sender) => {
                            DisplayAlert("Guardado", "¡Correo añadido con exito!", "OK");
                            Navigation.PopAsync();
                        MessagingCenter.Unsubscribe<ClienteRest>(this, "OK");
                        });

                        MessagingCenter.Subscribe<ClienteRest>(this, "error", (Sender) => {
                            DisplayAlert("Error", "¡No fué posible añadir el correo actual", "OK");
                        MessagingCenter.Unsubscribe<ClienteRest>(this, "error");

                        });
                    
                }

            }
        }
    }
}
