using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PagoEnLinea.PaginasPago
{
    public partial class LiquidacionPage : ContentPage
    {
        public static List<string> list;
        public LiquidacionPage()
        {
           
            list = new List<string>();
            InitializeComponent();
            list.Add("");
            listView.ItemsSource = list;
            listView.ItemTapped += OnitemTapped;
          
        }

        void Handle_SearchButtonPressed(object sender, System.EventArgs e)
        {
           
        }
        async void OnitemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            // do something with e.SelectedItem
            var resp= await DisplayAlert("Liquidacion", "¿Qué desea hacer?", "Agregar al carrito", "ver detalles");
            if(!resp){
                await Navigation.PushAsync(new DetallesLiquidacionPage());
            }

            ((ListView)sender).SelectedItem = null; // de-select
        }
       
        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {

        }
    }
}
