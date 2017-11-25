using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PagoEnLinea.PaginasPago
{
    public partial class CarritoPage : ContentPage
    {
        public static List<string> list;
        public CarritoPage()
        {
            InitializeComponent();
            list = new List<string>();
            InitializeComponent();
            list.Add("");
            list.Add("");
           

            listView.ItemsSource = list;


        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            DisplayAlert("Quitar","¿Desea quitar ese elemento del carrito?","Si","No");
        }
    }
}
