using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using CarritoBD;
using Newtonsoft.Json;
using PagoEnLinea.Modelos;
using Xamarin.Forms;

namespace PagoEnLinea.PaginasPago
{


    /// <summary>
    /// esta clase muestra una pantalla los items añadidos al carrito en forma de liquidación
    /// </summary>
    public partial class CarritoPage : ContentPage
    {

        public static Pago pagos;

        public static List<Carrito> list;

        /// <summary>
        /// inizializa los componenetes visuales de su XAML añade un evento de tipo ItemTapped
        /// a la lista que contiene todos los items del carrito
        /// </summary>
        public CarritoPage()
        {
            InitializeComponent();
            list = new List<Carrito>();
            InitializeComponent();

            listView.ItemTapped += OnitemTapped;


            //listView.ItemsSource = list;


        }


        /// <summary>
        /// llama al metodo total al mostrar la pantalla carrito
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            total();

        }


        /// <summary>
        /// Este método obtiene todos los items de la base de datos y apartir de su propiedad "price"
        /// obtiene el total a apagar ademas de que recupera el id para consumir el servicio de pagar
        /// </summary>
        async void total()
        {




            double tot = 0;


            ((App)Application.Current).ResumeAtTodoId = -1;
            list = await App.Database.GetItemsAsync();
            listView.ItemsSource = list;
            List<int> idlidaciones = new List<int>();
            pagos = new Pago();

            foreach (var item in list)
            {
                tot += item.price;
                idlidaciones.Add(item.idLiqs);
                }


            Total.Text = tot.ToString();
            pagos.idLiqs = idlidaciones;
            pagos.auth = "autorizado"; // emulado

        }

        /// <summary>
        /// Evento que permite seleccionar un item del carrito
        /// y elegir si se desea eliminar.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void OnitemTapped(object sender, ItemTappedEventArgs e)
        {



            var action = await DisplayActionSheet("¿Qué desea hacer?", "Cancelar", "Quitar del carrito");

            if (!string.IsNullOrEmpty(action))
            {
                if (action.Equals("Quitar del carrito"))
                {
                    var respuesta = await DisplayAlert("Quitar", "¿Esta seguro que desea quitar este elemento del carrito?", "Si", "Cancelar");
                    {
                        if (respuesta)
                        {
                            ((App)Application.Current).ResumeAtTodoId = -1;
                            var carrito = await App.Database.GetItemsAsync();
                            var todoItem = (Carrito)e.Item;

                            IEnumerable<Carrito> resultado = carrito.Where(clav => clav.ClaveCastrasl.Contains(todoItem.ClaveCastrasl));

                            if (resultado.Count() > 1 && !(todoItem.NoLiquidacion.Equals(resultado.Last().NoLiquidacion)))
                            {
                                await DisplayAlert("Advertencia", "Se eliminará tambien la liquidación comprendida al año actual debido a que no es posible pagarla sin pagar las liqudaciones anteriores", "OK");
                                await App.Database.DeleteItemAsync(resultado.Last());
                            }


                            await App.Database.DeleteItemAsync(todoItem);
                            total();


                        }
                    }
                }
            }
           ((ListView)sender).SelectedItem = null;






        }



        /// <summary>
        /// evento asignado al boton pagar, consume al servicio que notifica que las liquidaciones se encuentran pagadas
        /// actualmente se encuentra emulado ya que la autorizacion es automática debido a que no hay conexion con el banco
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void Handle_Clicked(object sender, System.EventArgs e)
        {

            HttpResponseMessage response;

            string ContentType = "application/json"; // or application/xml

            var jsonstring = JsonConvert.SerializeObject(pagos);

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);

                response = await client.PostAsync(Constantes.URL_CAJA + "/liquidacions/pago", new StringContent(jsonstring, Encoding.UTF8, ContentType));
                var y = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(y);
                System.Diagnostics.Debug.WriteLine(jsonstring);


                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {


                   
                    ((App)Application.Current).ResumeAtTodoId = -1;
                    var liquidaciones = await App.Database.GetItemsAsync();
                    foreach (var item in liquidaciones)
                    {
                        await App.Database.DeleteItemAsync(item);
                    }

                    await Navigation.PushAsync(new PasarelaPage());
                    await DisplayAlert("Exito", "¡El pago se ha realizado con exito!", "OK");

                }




                else
                {
                    System.Diagnostics.Debug.WriteLine(y);



                    await DisplayAlert("Error", "No fué posible realizar el pago", "OK");


                }
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert("Error", "No fué posible realizar el pago intente mas tarde", "OK");

            }
        }

    }
}

