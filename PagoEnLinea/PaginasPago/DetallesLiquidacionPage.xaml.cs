using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PagoEnLinea.PaginasPago
{
    /// <summary>
    /// Clase que muestra al usuario una pantalla con los detalles de una liquidacion seleccionada
    /// </summary>
    public partial class DetallesLiquidacionPage : ContentPage
    {
        /// <summary>
        /// Inicializa los componentes visuales de su XAML
        /// asigna un contexto de bindeo a dichos componentes visuales
        /// se asgina una lista de items al compontente LisView (declarado en el XAML correspondiente a esta clase)
        /// </summary>
        /// <param name="liqdesconcep">Liqdesconcep.</param>
        public DetallesLiquidacionPage(IList<Modelos.LiquidacionDesConcepto> liqdesconcep)
        {
            InitializeComponent();


            BindingContext = liqdesconcep;
            listView.ItemsSource = liqdesconcep;

        }
    }
}
