using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    public partial class CorreosElectronicos : ContentPage
    {
        public static List<string> list;   
        public CorreosElectronicos()
        {
            list = new List<string>(); 
            InitializeComponent();
            list.Add("Correo electrónico");
            list.Add("Correo electrónico");
            BindingContext = list;
            listView.ItemsSource = list;
        }

        async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new ModificarCorreo());
        }
    }
}
