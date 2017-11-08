using System;
using System.Collections.Generic;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    public partial class CorreosElectronicos : ContentPage
    {
        public static List<Email> list;   
        public CorreosElectronicos()
        {
            
            InitializeComponent();
           
        }

        async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new ModificarCorreo());
        }
        async void conectar()
        {
            if (Application.Current.Properties.ContainsKey("user"))
            {


                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["user"] as string);
                list = new List<Email>();
                if (inf != null)
                {
                    foreach (var dato in inf.email)
                    {
                        //catdir.cp = dato.catalogoDir.cp;
                        list.Add(new Email
                        {

                            correoe = dato.correoe,
                            tipo = dato.tipo,



                        });
                    }


                    BindingContext = list;
                    listView.ItemsSource = list;

                }

            }
        }
        protected override void OnAppearing()
        {
            conectar();
        }
    }
}
