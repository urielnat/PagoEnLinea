using System;
using System.Collections.Generic;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea
{
    public partial class HomePage : ContentPage
    {
        
        public HomePage()
        {
         
            InitializeComponent();
           
           
        }
         async void conectar(){
            if (Application.Current.Properties.ContainsKey("token"))
            {

                //System.Diagnostics.Debug.WriteLine("propiedad guardada"+Application.Current.Properties["token"] as string);
                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["token"] as string);
                if (inf != null)
                {
                    nombre.Text = "Bienvenido: "+inf.persona.nombre;
                }

            }
            }
        protected override void OnAppearing()
        {
            conectar();
        }

    }
}
