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
        public CatalogoDir catdir { set; get; }
        public Direccion()
        {
          
            InitializeComponent();
          
        }

        async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var info = (infodir)e.SelectedItem;
            //var catinfo = (CatalogoDir)e.SelectedItem;
            await Navigation.PushAsync(new ModificarDireccion(info.id,info.idCat){BindingContext = (infodir)e.SelectedItem});
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
                       
                        list.Add(new Modelos.infodir
                        {
                            id = dato.id,
                            calle = dato.calle,
                            numero = dato.numero,
                            numeroInterior = dato.numeroInterior,
                            tipo = dato.tipo,

                            cp = dato.catalogoDir.cp,
                            asentamiento = dato.catalogoDir.asentamiento,
                            municipio = dato.catalogoDir.municipio,
                            estado = dato.catalogoDir.estado,
                            pais = dato.catalogoDir.pais,
                            tipoasentamiento = dato.catalogoDir.tipoasentamiento,
                            ciudad = dato.catalogoDir.ciudad,
                            idCat = dato.catalogoDir.id
                                                                        
                            

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
