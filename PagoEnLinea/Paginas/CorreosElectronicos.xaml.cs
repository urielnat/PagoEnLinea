﻿using System;
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
            
            await Navigation.PushAsync(new ModificarCorreo((e.SelectedItem as Email).id,0){ BindingContext = (Email)e.SelectedItem });
        }
        async void conectar()
        {
            if (Application.Current.Properties.ContainsKey("token"))
            {


                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["token"] as string);
                list = new List<Email>();
                if (inf != null)
                {
                    foreach (var dato in inf.email)
                    {
                        //catdir.cp = dato.catalogoDir.cp;
                        list.Add(new Email
                        {
                            id=dato.id,
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

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ModificarCorreo(null,1) );
        }
    }
}
