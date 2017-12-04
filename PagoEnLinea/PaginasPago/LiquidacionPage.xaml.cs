using System;
using System.Collections.Generic;
using CarritoBD;
using PagoEnLinea.Modelos;
using Xamarin.Forms;

namespace PagoEnLinea.PaginasPago
{
    public partial class LiquidacionPage : ContentPage
    {
        public static List<string> list;
        public static List<Carrito> lista;
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
            }else{
                var todoItem = (Carrito)e.Item;
                await App.Database.SaveItemAsync(todoItem);
                await Navigation.PopAsync();

               
            }

           ((ListView)sender).SelectedItem = null; // de-select
        }
       

        async void conectar()
        {








            lista = new List<Carrito>
            {
                new Carrito
                {
                    Name = "Liquidacion",
                    Notes = "ejemplo",
                    price = 2000




                }
            };
            BindingContext = lista;
                    listView.ItemsSource = lista;
                

        }
        protected override void OnAppearing()
        {
            conectar();
        }
    }
}
