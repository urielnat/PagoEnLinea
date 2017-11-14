using System;
using System.Collections.Generic;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    public partial class ModificarDireccion : ContentPage
    {
        
        string ID,catID;
        public ModificarDireccion(string id, string idcat)
        {
            ID = id;
            catID = idcat;
            InitializeComponent();
        }

        void conectar()
        {
            ClienteRest client = new ClienteRest();

            Modelos.Direccion dir = new Modelos.Direccion();

            dir.id = ID;
            dir.calle = enCalle.Text;
            dir.numero = enNumero.Text;
            dir.numeroInterior = enNumeroInterior.Text;
            dir.catalogoDir = new CatalogoDir()
            {
                id = catID,
                asentamiento = enAsentamiento.Text,
             tipoasentamiento = enTipoAsentamiento.Text,
            cp = enCP.Text,
           ciudad = enCiudad.Text,
            municipio = enMunicipio.Text,
            estado = enEstado.Text,
            pais = enpais.Text
            
            };
           

            client.PUT("http://192.168.0.18:8080/api/direccion/actualizar",dir);

            MessagingCenter.Subscribe<ClienteRest>(this,"putdireccion",(Sender) => {
                    DisplayAlert("Guardado", "¡Direccion Modificada con Exito!", "OK");
                    Navigation.PopAsync();
                
            });

        }
        protected override void OnAppearing()
        {
            
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            conectar();   
        }
    }
}
