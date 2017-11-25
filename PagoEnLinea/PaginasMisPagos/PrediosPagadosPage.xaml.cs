using System;
using System.Collections.Generic;
using PagoEnLinea.PaginasPago;
using Xamarin.Forms;

namespace PagoEnLinea.PaginasMisPagos
{
    public partial class PrediosPagadosPage : ContentPage
    {
        public List<string> list = new List<string>();
        public PrediosPagadosPage()
        {
            InitializeComponent();

            list.Add("");
            listView.ItemsSource = list;
           
        }

        void Handle_SearchButtonPressed(object sender, System.EventArgs e)
        {
            
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new TabPredios());
        }
    }
}
