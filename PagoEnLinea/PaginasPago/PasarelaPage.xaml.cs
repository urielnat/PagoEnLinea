using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PagoEnLinea.PaginasPago
{
    public partial class PasarelaPage : ContentPage
    {
        public PasarelaPage()
        {
            Title = "BBVABancomer";

            WebView webView = new WebView
            {
                Source = new UrlWebViewSource
                {
                    Url = "https://www.bancomer.com/impuestos/index.jsp",
                },
                VerticalOptions = LayoutOptions.FillAndExpand
            };

      
          
            this.Content = new StackLayout
            {
                Children =
                {
                   
                    webView
                }
            };
        }
    }
}
