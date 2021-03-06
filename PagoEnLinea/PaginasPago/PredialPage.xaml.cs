﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using CarritoBD;
using Newtonsoft.Json;
using PagoEnLinea.Modelos;
using Xamarin.Forms;
using System.Linq;
using System.Text.RegularExpressions;

namespace PagoEnLinea.PaginasPago
{
    /// <summary>
    /// Esta clase muestra una pantalla donde el usuario puede buscar mediante una clave castral
    /// un predio a pagar para ser añadido al carrito
    /// </summary>
    public partial class PredialPage : ContentPage
    {
        public static List<Carrito> list;
        public static List<DesglosePredio> despre;
        public static List<CheckItem> items;
        public static Predio pred;
        public static Datos liq;
        public static double TOTAL;
        public static int BIMIN, BIFIN;
        public static List<int> ordenlista;

        /// <summary>
        /// Inicializa los componentes visuales de su XAML
        /// dependiente si el dispositivo es android o iOS muestra
        /// una barra de busqueda diferente debido a que no poseen las mismas propiedades
        /// según sea el sistema operativo donde se esta corriendo la aplicación
        /// añade eventos a las barras de busqueda y a la lista que mostrará el predio correspondiente
          /// </summary>
        public PredialPage()
        {
            list = new List<Carrito>();
            InitializeComponent();

       
            listView.ItemTapped += OnitemTapped;
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
        /// evento que corresponde a la barra de busqueda en dispositivos android
        /// una vez que el usuario presina la tecla de retorno en su teclado virtual
        /// se consume el servicio para mostrar los predios asociadas a una clave catastral
        /// y se llena una lista que la contiene
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async private void onSearchComp(object sender, EventArgs e)
        {
            HttpResponseMessage response;

            string ContentType = "application/json"; // or application/xml

            Clave clv = new Clave();

            clv.cveCatastral = enBuscar.Text;


            var jsonstring = JsonConvert.SerializeObject(clv);

            try
            {


                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);
                response = await cliente.PostAsync(Constantes.URL_CAJA+"/liquidacion-predials/adeudos", new StringContent(jsonstring, Encoding.UTF8, ContentType));
                var y = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine(y);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {


                    pred = new Predio();
                    pred = JsonConvert.DeserializeObject<Predio>(y);

                    despre = new List<DesglosePredio>();
                    TOTAL = 0;
                    foreach (var total in pred.adeudos)
                    {
                        TOTAL += total.total;
                        despre.Add(new DesglosePredio
                        {
                            bimIni = total.bimInicial,
                            bimFin = total.bimFinal,
                            pago = total.total,
                            Detalles = (total.detalle.Count()>0)?
                                         total.detalle[0].conceptoDescripcion +"\n"+ "$ " + total.detalle[0].importe + "\n"+"\n"+
                                              total.detalle[1].conceptoDescripcion + "\n" + "$ " + total.detalle[1].importe + "\n" + "\n" +
                                              total.detalle[2].conceptoDescripcion + "\n" + "$ " + total.detalle[2].importe + "\n" + "\n" +
                                              total.detalle[3].conceptoDescripcion + "\n" + "$ " + total.detalle[3].importe :"No hay detalles que mostrar"

                        });
                    }


                    despre = Ordenar(despre);


                    list = new List<Carrito> { new Carrito{
                                Name = "Predio: "+pred.cveCatastral,
                               owner= pred.propietario,
                                Description = pred.colonia+" "+pred.calle + " "+ pred.numeroExt,
                                price = TOTAL}};

                    BindingContext = list;
                    listView.ItemsSource = list;


                    BIFIN = despre.Last().SinOrdenBiFin;
                    BIMIN = despre.First().bimIni;
                    items = new List<CheckItem>();

                    string ordIn, ordFin;

                    foreach (var item in despre)
                    {
                        ordIn = item.bimIni.ToString().Substring(1) + "-" + item.bimIni.ToString().Substring(0, 1);
                        ordFin = item.bimFin.ToString().Substring(0, 4) + "-" + item.bimFin.ToString().Substring(item.bimFin.ToString().Length - 1);
                        items.Add(new CheckItem
                        {
                            Name = ordIn + " / " + ordFin + "  $" + item.pago,
                            Pago = item.pago,
                            binIn = item.bimIni,
                            binfin = item.bimFin,
                            sinOrdenbimFin = item.SinOrdenBiFin,
                            Details = item.Detalles
                           
                        });
                    }

                    //results.Text = "(none)";
                }




                else
                {
                    System.Diagnostics.Debug.WriteLine(y);

                    var resp = JsonConvert.DeserializeObject<Msg>(y);

                    await DisplayAlert("Error", resp.mensaje, "OK");


                }
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);

            }

        }

        DesglosePredios<CheckItem> multiPage;

        /// <summary>
        /// evento que corresponde a la lista que contiene el predial buscado
        /// muestra un cuadro de diálogo en donde se le presenta al usuario la opción
        /// de ver detalles del predial seleccionado o añadirla al carrito
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
       async void OnitemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            // do something with e.SelectedItem
            var resp = await DisplayAlert("Predio", "¿Qué desea hacer?", "Agregar al carrito", "ver detalles");
            if (!resp)
            {



               

               
                    multiPage = new DesglosePredios<CheckItem>(items) { Title = "Seleccione el pago" };

                await Navigation.PushModalAsync(multiPage);
             
            }
            else
            {
                HttpResponseMessage response;

                string ContentType = "application/json"; // or application/xml

                GenerarLiquidacion genliq = new GenerarLiquidacion();

                genliq.cveCatastral = pred.cveCatastral;
                genliq.bimIni = BIMIN;


                genliq.bimFin = BIFIN;

                var jsonstring = JsonConvert.SerializeObject(genliq);

                try
                {


                    HttpClient cliente = new HttpClient();
                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);
                    response = await cliente.PostAsync(Constantes.URL_CAJA+"/liquidacion-predials/genera", new StringContent(jsonstring, Encoding.UTF8, ContentType));
                    var y = await response.Content.ReadAsStringAsync();

                    var x = "{\"data\":" + y + "}";
                   

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        System.Diagnostics.Debug.WriteLine(x);
                        liq = new Datos();
                        liq = JsonConvert.DeserializeObject<Datos>(x);

                        ((App)Application.Current).ResumeAtTodoId = -1;
                        var carrito = await App.Database.GetItemsAsync();
                        foreach (var car in carrito)
                        {
                            if (!string.IsNullOrEmpty(car.ClaveCastrasl)) { 
                            if (car.ClaveCastrasl.Equals(liq.data.First().liquidacionPredial.clavecatastral))
                            {
                                await App.Database.DeleteItemAsync(car);
                                await DisplayAlert("Advertencia", "Se eliminó un elemento del carrito debido a que la liquidación fué actualizada", "OK");
                            }
                        }
                        }

                        foreach(var dato in liq.data){
                            var todoItem = new Carrito
                            {
                                Name = "Liquidacion: " + dato.numeroLiquidacion,
                                Description = dato.concepto,
                                price = dato.total,
                                ClaveCastrasl = dato.liquidacionPredial.clavecatastral,
                                NoLiquidacion = dato.numeroLiquidacion,
                                idLiqs = dato.id
                                                    
                            };
                          

                            await App.Database.SaveItemAsync(todoItem);
                           
                        }
                        await DisplayAlert("Añadido", "Se añadio al carrito", "OK");
                        listView.ItemsSource = null;



                        //results.Text = "(none)";
                    }




                    else
                    {
                        System.Diagnostics.Debug.WriteLine(y);

                        var resp2 = JsonConvert.DeserializeObject<Msg>(y);

                        await DisplayAlert("Error", resp2.mensaje, "OK");


                    }
                }
                catch (HttpRequestException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);

                }






            }

     ((ListView)sender).SelectedItem = null; // de-select
        }



        /// <summary>
        /// obtiene el total apagar a partir las fechas de pago que el usuario seleccionó en la pantalla DesglosePredios
        /// para poderlos añadir al carrito en forma de liquidación
        /// </summary>
        void ObtenerTotal()
        {
            
            if (multiPage != null)
            {
                double valores = 0;

                var answers = multiPage.GetSelection();
                foreach (var a in answers)
                {
                    valores += a.Pago;


                }

               
                if (answers.Count > 0)
                {
                    BIMIN = answers.First().binIn;
                    BIFIN = answers.Last().sinOrdenbimFin;
                    list = new List<Carrito>{new Carrito
                        {
                            Name = "Predio:" +pred.cveCatastral,owner= pred.propietario ,Description =  pred.colonia+" "+pred.calle + " "+ pred.numeroExt, price = valores

                        }};
                    BindingContext = list;
                    listView.ItemsSource = list;

                }
                else
                {
                    DisplayAlert("Información","No es posible dejar todas las fechas sin seleccionar por lo que se tomó el total de las mismas.","OK");
                    list = new List<Carrito> { new Carrito{
                                Name = "Predio: "+pred.cveCatastral,
                            owner= pred.propietario,
                                Description = pred.colonia+" "+pred.calle + " "+ pred.numeroExt,
                            price = TOTAL}};

                    BindingContext = list;
                    listView.ItemsSource = list;
                    
                }

            }
         


        }
        protected override void OnAppearing()
        {
            ObtenerTotal();

        }

        /// <summary>
        /// evento que corresponde a la barra de busqueda en dispositivos iOS
        /// una vez que el usuario presina la tecla de "Buscar" en su teclado virtual
        /// se consume el servicio para mostrar los prediales asociadas a una clave catastral
        /// y se llena una lista que la contendrá
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
         async void Handle_SearchButtonPressed(object sender, System.EventArgs e)
        {
            //throw new NotImplementedException();


            HttpResponseMessage response;

            string ContentType = "application/json"; // or application/xml

            Clave clv = new Clave();

            clv.cveCatastral = buscar.Text;


            var jsonstring = JsonConvert.SerializeObject(clv);

            try
            {


                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);
                response = await cliente.PostAsync(Constantes.URL_CAJA+"/liquidacion-predials/adeudos", new StringContent(jsonstring, Encoding.UTF8, ContentType));
                var y = await response.Content.ReadAsStringAsync();



                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                   
                    pred = new Predio();
                    pred = JsonConvert.DeserializeObject<Predio>(y);

                   despre = new List<DesglosePredio>();
                    TOTAL = 0;
                    foreach(var total in pred.adeudos){
                        TOTAL += total.total;
                        despre.Add( new DesglosePredio{
                            bimIni = total.bimInicial,
                            bimFin = total.bimFinal,
                            pago = total.total
                        });
                    }


                    despre=  Ordenar(despre);


                        list = new List<Carrito> { new Carrito{
                                Name = "Predio: "+pred.cveCatastral,
                               owner= pred.propietario,
                                Description = pred.colonia+" "+pred.calle + " "+ pred.numeroExt,
                                price = TOTAL}};
                    
                        BindingContext = list;
                        listView.ItemsSource = list;


                    BIFIN = despre.Last().SinOrdenBiFin;
                    BIMIN = despre.First().bimIni;
                    items = new List<CheckItem>();

                    string ordIn, ordFin;

                    foreach (var item in despre)
                    {
                        ordIn = item.bimIni.ToString().Substring(1) + "-" + item.bimIni.ToString().Substring(0, 1);
                        ordFin = item.bimFin.ToString().Substring(0,4) + "-" + item.bimFin.ToString().Substring(item.bimFin.ToString().Length-1);
                        items.Add(new CheckItem
                        {
                            Name = ordIn + " / " + ordFin + "  $" + item.pago,
                            Pago = item.pago,
                            binIn = item.bimIni,
                            binfin = item.bimFin,
                            sinOrdenbimFin = item.SinOrdenBiFin
                        });
                    }

                        //results.Text = "(none)";
                    }




                else
                {
                    System.Diagnostics.Debug.WriteLine(y);

                    var resp = JsonConvert.DeserializeObject<Msg>(y);

                    await DisplayAlert("Error", resp.mensaje, "OK");


                }
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);

            }




        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }


        /// <summary>
        /// metodo que ordena porfecha el desgloce de fechas a pagar ya que originalmente
        /// no vienen ordenados y es necesario para las validaciones de pago
        /// </summary>
        /// <returns>la nueva lista ordenada</returns>
        /// <param name="t">una lista de tipo DesglocePredio</param>
        List<DesglosePredio> Ordenar(List<DesglosePredio> t){

            List<DesglosePredio> lista = new List<DesglosePredio>();
            int bim=0, año=0, newBimFin=0;

            foreach(var item in t){
                bim = item.bimFin / 10000;
                año = item.bimFin - (bim * 10000);
                newBimFin = (año * 10) + bim;
                lista.Add(new DesglosePredio
                {
                    bimFin = newBimFin,
                    bimIni = item.bimIni,
                    pago = item.pago,
                    SinOrdenBiFin = item.bimFin,
                    Detalles = item.Detalles

                });
            }

            var newList = lista.OrderBy(x => x.bimFin).ToList();
            return newList;
        }
    }
}
