using System;

using Xamarin.Forms;

namespace PagoEnLinea
{
    public class PerfilPage : ContentPage
    {
        public PerfilPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

