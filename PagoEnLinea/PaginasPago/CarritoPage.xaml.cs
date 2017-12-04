using System;
using System.Collections.Generic;
using CarritoBD;
using Xamarin.Forms;

namespace PagoEnLinea.PaginasPago
{
    public partial class CarritoPage : ContentPage
    {
        public static List<Carrito> list;
        public CarritoPage()
        {
            InitializeComponent();
            list = new List<Carrito>();
            InitializeComponent();

            listView.ItemTapped += OnitemTapped;
           

            //listView.ItemsSource = list;


        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            total();

        }

        async void total(){
            int total = 0;
            // Reset the 'resume' id, since we just want to re-start here
            ((App)Application.Current).ResumeAtTodoId = -1;
            list = await App.Database.GetItemsAsync();
            listView.ItemsSource = list;
            foreach (var pago in list)
            {

                total += pago.price;
            }
            Total.Text = total.ToString();
        }


        async void OnitemTapped(object sender, ItemTappedEventArgs e)
        {







            var action = await DisplayActionSheet("¿Qué desea hacer?", "Cancelar", "Quitar del carrito");

            if (action.Equals("Quitar del carrito"))
            {
                var respuesta = await DisplayAlert("Quitar", "¿Esta seguro que desea quitar este elémento del carrito?", "Si", "Cancelar");
                {
                    if (respuesta)
                    {

                        var todoItem = (Carrito)e.Item;
                        await App.Database.DeleteItemAsync(todoItem);
                        total();


                    }
                }
            }
           ((ListView)sender).SelectedItem = null;






        }

        async void OnItemAdded(object sender, EventArgs e)
        {
            
        }


    }
}
