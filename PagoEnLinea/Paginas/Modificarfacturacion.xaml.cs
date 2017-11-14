using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    public partial class Modificarfacturacion : ContentPage
    {
        public Modificarfacturacion()
        {
            InitializeComponent();
        }

        async void telefono_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ModificarTelefono());
        }

        async void correo_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ModificarCorreo(null));
        }
    }
}
