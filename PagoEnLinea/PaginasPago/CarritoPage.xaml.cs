using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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


        protected override void OnAppearing()
        {
            base.OnAppearing();
            total();

        }

        async void total(){
            HttpResponseMessage response;



            double tot = 0;
            // Reset the 'resume' id, since we just want to re-start here
          
            /*
            ((App)Application.Current).ResumeAtTodoId = -1;
            list = await App.Database.GetItemsAsync();
            HttpClient cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJhZG1pbiIsImF1dGgiOiJDQUpBX0NBSkVSTyxST0xFX0FETUlOLFJPTEVfVVNFUiIsInBzdG8iOiJbXSIsIm5tIjoiSVNBQUMiLCJhcDEiOiJCQVVUSVNUQSIsImFwMiI6IkNBTUlOTyIsImV4cCI6MTUxMjY1NzA3Mn0.kpf3hvgb7FPj0sHqdOhlYEvpNPOJxd569RVpr8JKchTMqQuLnVMuFujJEqaSXULi58e6kuaLDBANf2bTcDJAtw");


            try
            {
                foreach (var item in list) { 
                    
                    response = await cliente.GetAsync("http://192.168.0.18:8081/api/liquidacions/numero/" + item.Name.Replace("Liquidacion: ",""));
                    var y = await response.Content.ReadAsStringAsync();



                  if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        tot += item.price;
                    }




                  else
                    {
                      
                        await App.Database.DeleteItemAsync(item);
                        await DisplayAlert("Advertencia", "Se eliminó un item del carrito debido a que la liquidación fué actualizada", "OK");
                    }
                
                }
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);

            }**/

            ((App)Application.Current).ResumeAtTodoId = -1;
            list = await App.Database.GetItemsAsync();
            listView.ItemsSource = list;

            foreach(var item in list){
                tot += item.price;
            }


            Total.Text = tot.ToString();


         
        }


        async void OnitemTapped(object sender, ItemTappedEventArgs e)
        {







            var action = await DisplayActionSheet("¿Qué desea hacer?", "Cancelar", "Quitar del carrito");

            if (!string.IsNullOrEmpty(action)) { 
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

                            if(resultado.Count()>1&&!(todoItem.NoLiquidacion.Equals(resultado.Last().NoLiquidacion))){
                                await DisplayAlert("Advertencia","Se eliminará tambien la liquidación comprendida al año actual debido a que no es posible pagarla sin pagar las liqudaciones anteriores","OK");
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

         void OnItemAdded(object sender, EventArgs e)
        {
            
        }


        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new PasarelaPage());
        }
    }
}
