using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    public partial class DatosPersonales : ContentPage
    {
        public DatosPersonales()
        {
            InitializeComponent();
        }

       async void Handle_Clicked(object sender, System.EventArgs e)
        {
            var action = await DisplayActionSheet("Modificar datos", "Cancelar", "Eliminar", "Modificar", "Agregar");
            Debug.WriteLine("Action: " + action);
        }

        async void contraseña_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ModificarContraseña());       
        }
    }
}
