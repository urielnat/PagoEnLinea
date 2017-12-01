using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PagoEnLinea.PaginasPago
{
    public partial class PredialPage : ContentPage
    {
        public static List<string> list;
        public PredialPage()
        {
            list = new List<string>();
            InitializeComponent();
            list.Add("");
            listView.ItemsSource = list;
            listView.ItemTapped += OnitemTapped;
        }

        async void OnitemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            // do something with e.SelectedItem
            var resp = await DisplayAlert("Predial", "¿Qué desea hacer?", "Agregar al carrito", "ver detalles");
            if (!resp)
            {
                //await Navigation.PushAsync(new DetallesLiquidacionPage());
            }

           ((ListView)sender).SelectedItem = null; // de-select
        }

        void Handle_SearchButtonPressed(object sender, System.EventArgs e)
        {
            //throw new NotImplementedException();
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
