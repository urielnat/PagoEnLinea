using System;
using System.Collections.Generic;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    public partial class Telefonos : ContentPage
    {
        public static List<Telefono> list;
        public Telefonos()
        {
         
            InitializeComponent();
          
        }

        async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new ModificarTelefono());
        }

        async void conectar()
        {
            if (Application.Current.Properties.ContainsKey("user"))
            {


                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["user"] as string);
                list = new List<Telefono>();
                if (inf != null)
                {
                    foreach (var dato in inf.telefonos)
                    {
                        //catdir.cp = dato.catalogoDir.cp;
                        list.Add(new Telefono
                        {


                            telefono = dato.telefono,
                            lada = dato.lada,
                            tipo =dato.tipo



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
