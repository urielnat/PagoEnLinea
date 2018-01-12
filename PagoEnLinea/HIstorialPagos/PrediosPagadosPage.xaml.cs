using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PagoEnLinea.Modelos;
using PagoEnLinea.PaginasPago;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea.PaginasMisPagos
{
    /// <summary>
    /// esta clase muestra una pantalla al usuario con un Historial de sus de pagos realizados
    /// </summary>
    public partial class PrediosPagadosPage : ContentPage
    {
        public static Pagos pag;
        public static List<Respuesta> list;

        /// <summary>
        /// Inicializa los compnentes visuales de de su XAML
        /// </summary>
        public PrediosPagadosPage()
        {
            InitializeComponent();




        }


        /// <summary>
        /// evento de asociado la la lista de historial de pagos, muestra detalles del pago al presionar un item de la lista al consumir un servicio.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var info = (Respuesta)e.Item;






            HttpResponseMessage response;

            try
            {


                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);
                response = await cliente.GetAsync(Constantes.URL_CAJA+"/historico-pagos-movil/detalles/" + info.idPago);
                var y = await response.Content.ReadAsStringAsync();



                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    System.Diagnostics.Debug.WriteLine(y);
                    DetallesHIstorial detalles = new DetallesHIstorial();
                    detalles = JsonConvert.DeserializeObject<DetallesHIstorial>(y);

                    string desgloce="";
                    string tipo;

                    foreach(var det in detalles.respuesta){
                        tipo = (det.llave.Length >= 13) ? "Clave catastral: " : "No de liquidacion: ";
                        desgloce += "Concepto: " + det.concepto + "\n" +
                                                    tipo + det.llave + "\n" +
                                                      "Importe: $" + det.importe +"\n"+"\n" +
                                                      "\n";



                    }



                    await DisplayAlert("Detalles", desgloce, "OK");

                    //results.Text = "(none)";
                }




                else
                {
                    System.Diagnostics.Debug.WriteLine(y);



                    await DisplayAlert("Error", "Error del servidor", "OK");


                }
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);
                await DisplayAlert("Error", "No fué posible establecer la conexión intente mas tarde", "OK");

            }










            ((ListView)sender).SelectedItem = null;



        }

           /// <summary>
           /// método que permite consumir servicio que muestra el historial de pagos a manera de una lista
           /// </summary>
            async void conectar()
            {
                if (Application.Current.Properties.ContainsKey("token"))
                {
                HttpResponseMessage response;
                try
                {


                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);
                    response = await cliente.GetAsync(Constantes.URL_CAJA+"/historico-pagos-movil/");
                var y = await response.Content.ReadAsStringAsync();
                    list = new List<Respuesta>();



                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    System.Diagnostics.Debug.WriteLine(y);
                    Historial detalles = new Historial();
                    detalles = JsonConvert.DeserializeObject<Historial>(y);

                   

                    if (detalles.respuesta.Count > 0)
                    {
                        foreach (var dato in detalles.respuesta)
                        {
                           
                            list.Add(new Respuesta
                            {

                                idPago = dato.idPago,
                                fecha = dato.fecha,
                                importe = dato.importe,
                                estatus = dato.estatus


                            });
                        }

                        lblHistorial.IsVisible = false;
                        historial.IsVisible = true;
                        BindingContext = list;
                        listView.ItemsSource = list;

                    }
                    else
                    {
                        lblHistorial.IsVisible = true;
                        historial.IsVisible = false;

                    }
                }else
                {
                    System.Diagnostics.Debug.WriteLine(y);



                    await DisplayAlert("Error", "Error del servidor", "OK");


                }
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);
                await DisplayAlert("Error", "No fué posible establecer la conexión intente mas tarde", "OK");

            }


            }
        }
        
    

            
        
        
        /// <summary>
        /// Lamama al método conectar al mostrar la pantalla Historial de pagos.
        /// </summary>
        protected override void OnAppearing()
        {
            conectar();
        }


    }
}
