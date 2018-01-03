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
    public partial class LiquidacionPage : ContentPage
    {
        public static List<Carrito> list;
        public static List<Carrito> lista;
        public static Liquidacion liq;
        public static List<Liquidacion> deslip;
        public static IList<LiquidacionDesConcepto> liqdesconcep;
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

        async private void onSearchComp(object sender, EventArgs e)
        {
            HttpResponseMessage response;

            string numero = enBuscar.Text;


            try
            {


                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);
                //response = await cliente.PostAsync("http://192.168.0.18:8080/management/audits/logout", new StringContent("", Encoding.UTF8, ContentType));
                // response = await cliente.PostAsync("http://192.168.0.18:8081/api/liquidacion-predials/adeudos", new StringContent(jsonstring, Encoding.UTF8, ContentType));
                response = await cliente.GetAsync("http://192.168.0.100:8081/api/liquidacions/numero/" + numero);
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
                            price = liq.total}};

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


        async void Handle_SearchButtonPressed(object sender, System.EventArgs e)
        {
            HttpResponseMessage response;

            string numero = buscar.Text;
              

            try
            {


                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);
                //response = await cliente.PostAsync("http://192.168.0.18:8080/management/audits/logout", new StringContent("", Encoding.UTF8, ContentType));
                // response = await cliente.PostAsync("http://192.168.0.18:8081/api/liquidacion-predials/adeudos", new StringContent(jsonstring, Encoding.UTF8, ContentType));
                response = await cliente.GetAsync("http://192.168.0.100:8081/api/liquidacions/numero/" + numero);
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
                            price = liq.total}};

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
