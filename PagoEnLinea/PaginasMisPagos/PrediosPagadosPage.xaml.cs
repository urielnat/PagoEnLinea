using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PagoEnLinea.Modelos;
using PagoEnLinea.PaginasPago;
using Xamarin.Forms;

namespace PagoEnLinea.PaginasMisPagos
{
    public partial class PrediosPagadosPage : ContentPage
    {
        public static Pagos pag;
        public static List<MostrarHistorialPagos> list;
        public PrediosPagadosPage()
        {
            InitializeComponent();

            enBuscar.Completed += onSearchComp;
            enBuscar.TextChanged += onSearchChanged;
            buscar.TextChanged += onSearchBarChanged;
            if (Device.RuntimePlatform == Device.iOS)
            {
                buscar.IsVisible = true;
                enBuscar.IsVisible = false;
                imgBuscar.IsVisible = false;



            }
            else
            {
                buscar.IsVisible = false;
                enBuscar.IsVisible = true;
                imgBuscar.IsVisible = true;
            }
           
        }

       async private void onSearchComp(object sender, EventArgs e)
        {
            HttpResponseMessage response;

            string clave = enBuscar.Text;


            try
            {


                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);

                response = await cliente.GetAsync("http://192.168.0.100:8081/api/historial-pagos/" + clave);
                var y = await response.Content.ReadAsStringAsync();



                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {


                    pag = new Pagos();
                    var x = "{\"pagos\":" + y + "}";
                    pag = JsonConvert.DeserializeObject<Pagos>(x);


                    list = new List<MostrarHistorialPagos>();
                    if (pag.pagos.Count == 0)
                    {
                        await DisplayAlert("Error", "Clave catastral no encontrada o sin pagos aun", "OK");
                    }

                    foreach (var pa in pag.pagos)
                    {
                        list.Add(new MostrarHistorialPagos
                        {
                            clave = "Clave: " + pa.clave,
                            fechaPago = "Fecha de pago: " + pa.fechaPago,
                            liquidacion = "No de Liquidación: " + pa.liquidacion,
                            liquidacionDesc = "Descripción: " + pa.liquidacionDesc,
                            total = "Total: " + pa.total

                        });

                    }





                    BindingContext = list;
                    listView.ItemsSource = list;





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

            string clave = enBuscar.Text;


            try
            {


                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);

                response = await cliente.GetAsync("http://192.168.0.100:8081/api/historial-pagos/"+clave);
                var y = await response.Content.ReadAsStringAsync();



                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                  
                    pag = new Pagos();
                    var x = "{\"pagos\":" + y + "}";
                    pag = JsonConvert.DeserializeObject<Pagos>(x);


                    list = new List<MostrarHistorialPagos>();
                    if(pag.pagos.Count==0){
                        await DisplayAlert("Error","Clave catastral no encontrada o sin pagos aun","OK");
                    }

                    foreach(var pa in pag.pagos){
                        list.Add(new MostrarHistorialPagos{
                            clave = "Clave: "+ pa.clave,
                            fechaPago = "Fecha de pago: "+ pa.fechaPago,
                            liquidacion = "No de Liquidación: " + pa.liquidacion,
                            liquidacionDesc ="Descripción: "+pa.liquidacionDesc,
                            total = "Total: "+pa.total

                        });

                    }
                  




                    BindingContext = list;
                    listView.ItemsSource = list;





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

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            // do something with e.SelectedItem
           // Navigation.PushAsync(new TabPredios());

            ((ListView)sender).SelectedItem = null; // de-select
        }
    }
}
