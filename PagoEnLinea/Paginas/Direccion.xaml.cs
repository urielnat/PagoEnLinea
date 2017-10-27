using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    public partial class Direccion : ContentPage
    {
        public static List<string> list;   
        public Direccion()
        {
            list = new List<string>();
            InitializeComponent();
            list.Add("");
            list.Add("");
            BindingContext = list;
            listView.ItemsSource = list;
        }

        async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new ModificarDireccion());
        }
    }
}
