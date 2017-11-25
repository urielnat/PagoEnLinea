﻿using System;
using System.Collections.Generic;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    public partial class ModificarTelefono : ContentPage
    {
        string ID;
        public ModificarTelefono(string id, string tipoTel,int tipo)
        {
            InitializeComponent();
            ID = id;
            for (var i = 0; i < pkTipo.Items.Count; i++)
            {
                if (pkTipo.Items[i].Equals(tipoTel))
                {
                    pkTipo.SelectedIndex = i;
                }
            }

            if (tipo == 0)
            {
                btnModificar.IsVisible = true;
                btnAgregar.IsVisible = false;
            }
            if (tipo == 1)
            {
                btnAgregar.IsVisible = true;
                btnModificar.IsVisible = false;
            }
        }

        async void conectar()
        {
            if (Application.Current.Properties.ContainsKey("token"))
            {


                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["token"] as string);
                if (inf != null)
                {
                    Telefono modtel;

                   
                   // modtel.persona = inf.persona;
                    bool auth = false, auth2 = false;


                    modtel = new Telefono
                    {
                        id = ID,
                        tipo = pkTipo.Items[pkTipo.SelectedIndex],
                        telefono = enTelefono.Text,
                        lada = enLada.Text
                    };
                         
                    ClienteRest client = new ClienteRest();
                    if (!String.IsNullOrEmpty(enTelefono.Text) && !(enTelefono.Text.Length < 7))
                    {

                        auth = true;

                    }
                    else
                    {
                        auth = false;
                        await DisplayAlert("Error", "Teléfono Invalido", "ok");
                    }
                  

                    if (!String.IsNullOrEmpty(enLada.Text) && !(enTelefono.Text.Length < 3))
                    {

                        auth2 = true;

                    }
                    else
                    {
                        auth2 = false;
                        await DisplayAlert("Error", "LADA incorrecta", "ok");
                    }

                    if(auth&&auth2){

                        client.PUT(Constantes.URL+"/telefonos/modificar", modtel);
                        MessagingCenter.Subscribe<ClienteRest>(this, "puttelefono", (Sender) => {
                            DisplayAlert("Guardado", "¡Teléfono Modificado con Exito!", "OK");
                            MessagingCenter.Unsubscribe<ClienteRest>(this,"puttelefono");
                            Navigation.PopAsync();
                        });

                        MessagingCenter.Subscribe<ClienteRest>(this, "errorTelefono", (Sender) => {
                            DisplayAlert("Error", "¡No fué posible Modificar el Teléfono!", "OK");
                            MessagingCenter.Unsubscribe<ClienteRest>(this, "errorTelefono");

                        });
                    }
                    
                }

            }

        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            conectar();
        }

        async void Agregar_Clicked(object sender, System.EventArgs e)
        {
            ClienteRest cliente = new ClienteRest();
            var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["token"] as string);
            if (inf != null)
            {
                Modelos.ModificarTelefono modtel = new Modelos.ModificarTelefono();


                modtel.persona = inf.persona;
                bool auth = false, auth2 = false, auth3 = false;


                modtel.telefono = new Telefono
                {
                   
                    tipo = pkTipo.Items[pkTipo.SelectedIndex],
                    telefono = enTelefono.Text,
                    lada = enLada.Text
                };

                ClienteRest client = new ClienteRest();
                if (!String.IsNullOrEmpty(enTelefono.Text) && !(enTelefono.Text.Length < 7))
                {

                    auth = true;

                }
                else
                {
                    auth = false;
                    await DisplayAlert("Error", "Teléfono Invalido", "ok");
                }


                if (!String.IsNullOrEmpty(enLada.Text) && !(enTelefono.Text.Length < 3))
                {

                    auth2 = true;

                }
                else
                {
                    auth2 = false;
                    await DisplayAlert("Error", "LADA incorrecta", "ok");
                }


                if (pkTipo.SelectedIndex>-1)
                {

                    auth3 = true;

                }
                else
                {
                    auth3 = false;
                    await DisplayAlert("Error", "Seleccione el tipo de teléfono", "ok");
                }

                if (auth && auth2&&auth3)
                {

                    client.POST(Constantes.URL + "/telefono/agregar", modtel,1);
                    MessagingCenter.Subscribe<ClienteRest>(this, "OK", (Sender) => {
                        DisplayAlert("Guardado", "¡Teléfono Añadido con Exito!", "OK");
                        MessagingCenter.Unsubscribe<ClienteRest>(this, "OK");
                        Navigation.PopAsync();
                    });

                    MessagingCenter.Subscribe<ClienteRest>(this, "error", (Sender) => {
                        DisplayAlert("Error", "¡No fué posible añadir el Teléfono!", "OK");
                        MessagingCenter.Unsubscribe<ClienteRest>(this, "error");

                    });
                }

            }

        }
        }
         
    }

    //http://192.168.0.18:8080/api/telefonos/modificar





