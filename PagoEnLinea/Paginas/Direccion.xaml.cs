using System;
using System.Collections.Generic;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    public partial class Direccion : ContentPage
    {
        public static List<Modelos.infodir> list;
        public infodir infdir { set; get; }
        public Direccion()
        {
          
            InitializeComponent();
          
        }

        async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new ModificarDireccion());
        }

        async void conectar()
        {
            if (Application.Current.Properties.ContainsKey("user"))
            {


                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["user"] as string);
                list = new List<Modelos.infodir>();
                if (inf != null)
                {
                    foreach (var dato in inf.direcciones){
                        //catdir.cp = dato.catalogoDir.cp;
                        list.Add(new Modelos.infodir
                        {

                            calle = dato.calle,
                            numero = dato.numero,
                            numeroInterior = dato.numeroInterior,


                            cp = dato.catalogoDir.cp,
                            asentamiento = dato.catalogoDir.asentamiento
                                                                        
                            

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
