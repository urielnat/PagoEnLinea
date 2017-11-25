using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PagoEnLinea
{
    public partial class RecuperarContraseñaPage : ContentPage
    {
        public RecuperarContraseñaPage()
        {
            InitializeComponent();
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PopToRootAsync();
        }
    }
}
