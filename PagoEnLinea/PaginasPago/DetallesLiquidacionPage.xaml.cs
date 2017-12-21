using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PagoEnLinea.PaginasPago
{
    public partial class DetallesLiquidacionPage : ContentPage
    {
        public DetallesLiquidacionPage(IList<Modelos.LiquidacionDesConcepto> liqdesconcep)
        {
            InitializeComponent();


            BindingContext = liqdesconcep;
            listView.ItemsSource = liqdesconcep;

        }
    }
}
