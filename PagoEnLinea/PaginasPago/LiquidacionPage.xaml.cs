using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using CarritoBD;
using Newtonsoft.Json;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea.PaginasPago
{
    /// <summary>
    /// Esta clase muestra una pantalla donde el usuario puede buscar mediante un número
    /// una liquidación para ser añadida al carrito
    /// </summary>
    public partial class LiquidacionPage : ContentPage
    {
        public static List<Carrito> list;
        public static List<Carrito> lista;
        public static Liquidacion liq;
        public static List<Liquidacion> deslip;
        public static IList<LiquidacionDesConcepto> liqdesconcep;
      
        /// <summary>
        /// Inicializa los componentes visuales de su XAML
        /// dependiente si el dispositivo es android o iOS muestra
        /// una barra de busqueda diferente debido a que no poseen las mismas propiedades
        /// según sea el sistema operativo donde se esta corriendo la aplicación
        /// añade eventos a las barras de busqueda y a la lista que mostrará la liquidacion correspondiente
        /// </summary>
        public LiquidacionPage()
        {

            list = new List<Carrito>();
            InitializeComponent();

            listView.ItemTapped += OnitemTapped;
            buscar.TextChanged += onSearchBarChanged;
            enBuscar.Completed += onSearchComp;
            enBuscar.TextChanged += onSearchChanged;
            buscar.IsVisible = true;
            if (Device.RuntimePlatform == Device.iOS){
                buscar.IsVisible = true;
                enBuscar.IsVisible = false;
                imgBuscar.IsVisible = false;


                
            }else{
                buscar.IsVisible = false;
                enBuscar.IsVisible = true;
                imgBuscar.IsVisible = true;
            }
       
        }

        /// <summary>
        /// evento que corresponde a la barra de busqueda en dispositivos android
        /// una vez que el usuario presina la tecla de retorno en su teclado virtual
        /// se consume el servicio para mostrar las liquidaciones asociadas a un número
        /// y se llena una lista que la contiene
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async private void onSearchComp(object sender, EventArgs e)
        {
            HttpResponseMessage response;

            string numero = enBuscar.Text;


            try
            {


                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);
                 response = await cliente.GetAsync(Constantes.URL_CAJA+"/liquidacions/numero/" + numero);
                var y = await response.Content.ReadAsStringAsync();



                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    System.Diagnostics.Debug.WriteLine(y);
                    liq = new Liquidacion();
                    liq = JsonConvert.DeserializeObject<Liquidacion>(y);



                    liqdesconcep = liq.liquidacionDesConcepto;


                    lista = new List<Carrito> { new Carrito{
                            Name = "Liquidacion: "+liq.numeroLiquidacion,
                            owner = liq.liquidacionPredial.propietarios,
                            Description =liq.concepto,
                            NoLiquidacion = liq.numeroLiquidacion,
                            price = liq.total,
                            idLiqs = liq.id
                        }};

                    BindingContext = lista;
                    listView.ItemsSource = lista;





                    //results.Text = "(none)";
                }




                else
                {
                    System.Diagnostics.Debug.WriteLine(y);



                    await DisplayAlert("Error", "No encontrado", "OK");


                }
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);

            }
        }


        /// <summary>
        /// valida dinámicamente que solo se puedan introducir números en las barra de busqueda de android
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void onSearchChanged(object sender, TextChangedEventArgs e)
        {
            if (!Regex.IsMatch(e.NewTextValue, "^[0-9]+$", RegexOptions.CultureInvariant))
                (sender as Entry).Text = Regex.Replace(e.NewTextValue, "[^0-9]", string.Empty);
            Entry entry = sender as Entry;
             String val = entry.Text;

            if (val.Length > 13)
            {
                val = val.Remove(val.Length - 1);
                entry.Text = val;
            }
        }

        /// <summary>
        /// valida dinámicamente que solo se puedan introducir números en las barra de busqueda de iOS
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void onSearchBarChanged(object sender, TextChangedEventArgs e)
        {
            if (!Regex.IsMatch(e.NewTextValue, "^[0-9]+$", RegexOptions.CultureInvariant))
                (sender as SearchBar).Text = Regex.Replace(e.NewTextValue, "[^0-9]", string.Empty);
            SearchBar entry = sender as SearchBar;
            String val = entry.Text;

            if (val.Length > 13)
            {
                val = val.Remove(val.Length - 1);
                entry.Text = val;
            }
        }


        /// <summary>
        /// evento que corresponde a la barra de busqueda en dispositivos iOS
        /// una vez que el usuario presina la tecla de "Buscar" en su teclado virtual
        /// se consume el servicio para mostrar las liquidaciones asociadas a un número
        /// y se llena una lista que la contendrá
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void Handle_SearchButtonPressed(object sender, System.EventArgs e)
        {
            HttpResponseMessage response;

            string numero = buscar.Text;
              

            try
            {


                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);
                response = await cliente.GetAsync(Constantes.URL_CAJA+"/liquidacions/numero/" + numero);
                var y = await response.Content.ReadAsStringAsync();



                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    System.Diagnostics.Debug.WriteLine(y);
                    liq = new Liquidacion();
                    liq = JsonConvert.DeserializeObject<Liquidacion>(y);



                    liqdesconcep = liq.liquidacionDesConcepto;


                    lista = new List<Carrito> { new Carrito{
                            Name = "Liquidacion: "+liq.numeroLiquidacion,
                            owner = liq.liquidacionPredial.propietarios,
                            Description =liq.concepto,
                            NoLiquidacion = liq.numeroLiquidacion,
                            price = liq.total,
                            idLiqs = liq.id
                        }};

                    BindingContext = lista;
                    listView.ItemsSource = lista;



                   

                    //results.Text = "(none)";
                }




                else
                {
                    System.Diagnostics.Debug.WriteLine(y);



                    await DisplayAlert("Error", "No encontrado", "OK");


                }
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);

            }
            
           
        }


        /// <summary>
        /// evento que corresponde a la lista que contiene la liquidación buscada
        /// muestra un cuadro de diálogo en donde se le presenta al usuario la opción
        /// de ver detalles de la liquidación seleccionada o añadirla al carrito
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void OnitemTapped(object sender, ItemTappedEventArgs e)
        {
            var existe = false;

            ((App)Application.Current).ResumeAtTodoId = -1;
            list = await App.Database.GetItemsAsync();
            if (e.Item == null) return;
            // do something with e.SelectedItem
            var resp= await DisplayAlert("Liquidacion", "¿Qué desea hacer?", "Agregar al carrito", "ver detalles");
            if(!resp){
                await Navigation.PushAsync(new DetallesLiquidacionPage(liqdesconcep));
            }else{



                foreach(var item in list){
                    if(item.NoLiquidacion.Equals((e.Item as Carrito).NoLiquidacion))
                        existe = true;
                    }

                if(existe){
                    await DisplayAlert("Advertencia", "La liquidación ya ha sido añadida con anterioridad","OK");
                }else{
                    var todoItem = (Carrito)e.Item;
                    await App.Database.SaveItemAsync(todoItem);
                    await DisplayAlert("Añadido", "Se añadio al carrito", "OK");
                    listView.ItemsSource = null;
                }

               
               
            }

           ((ListView)sender).SelectedItem = null; // de-select
        }
       


        protected override void OnAppearing()
        {
           // conectar();
        }
    }
}
