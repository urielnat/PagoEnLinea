using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    public partial class DatosPersonales : ContentPage
    {
        
        Persona persona = new Persona();
        public DatosPersonales()
        {
            InitializeComponent();
        }

        async void conectar()
        {
            if (Application.Current.Properties.ContainsKey("user"))
            {


                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["user"] as string);
                if (inf != null)
                {
                    
                    enNombre.Text = inf.persona.nombre;
                    enPaterno.Text = inf.persona.apaterno;
                    enMaterno.Text = inf.persona.amaterno;
                    enCurp.Text = inf.persona.curp;
                    persona.id = inf.persona.id;
                    dtFecha.Date = DateTime.ParseExact(inf.persona.fechanac, "yyyy-MM-dd", null);
                    for(var i = 0;i < pkEstCvl.Items.Count;i++)
                    {
                        if (pkEstCvl.Items[i].Equals(inf.persona.edoCivil)){
                            pkEstCvl.SelectedIndex = i; 
                        }
                     }
                    for (var i = 0; i < pkSexo.Items.Count; i++)
                    {
                        if (pkSexo.Items[i].Equals(inf.persona.sexo))
                        {
                            pkSexo.SelectedIndex = i;
                        }
                    }
                }

            }

        }
        protected override void OnAppearing()
        {
            conectar();
        }
       void Handle_Clicked(object sender, System.EventArgs e)
        {
            //var action = await DisplayActionSheet("Modificar datos", "Cancelar", "Eliminar", "Modificar", "Agregar");
            //Debug.WriteLine("Action: " + action);
            enNombre.IsEnabled = true;
            enPaterno.IsEnabled = true;
            enMaterno.IsEnabled = true;
            enCurp.IsEnabled = true;
            dtFecha.IsEnabled = true;
            pkSexo.IsEnabled = true;
            pkEstCvl.IsEnabled = true;

            btnGuardar.IsEnabled = true;
            btnModificar.IsEnabled = false;

        }

        async void contraseña_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ModificarContraseña());       
        }



         void modificar_Clicked(object sender, System.EventArgs e)
        {
            ClienteRest client = new ClienteRest();
         
            persona.nombre = enNombre.Text;
            persona.apaterno = enPaterno.Text;
            persona.amaterno = enMaterno.Text;
            persona.curp = enCurp.Text;
            persona.sexo = pkSexo.Items[pkSexo.SelectedIndex];
            persona.edoCivil = pkEstCvl.Items[pkEstCvl.SelectedIndex];
            persona.fechanac = dtFecha.Date.ToString("yyyy-MM-dd");

            enNombre.IsEnabled = false;
            enPaterno.IsEnabled = false;
            enMaterno.IsEnabled = false;
            enCurp.IsEnabled = false;
            dtFecha.IsEnabled = false;
            pkSexo.IsEnabled = false;
            pkEstCvl.IsEnabled = false;

           

            client.PUT("http://192.168.0.18:8080/api/personas", persona);

            MessagingCenter.Subscribe<ClienteRest>(this, "putpersona", (send) =>
            {
                DisplayAlert("Guardado", "¡Datos Modificados con Exito!", "OK");

            });

            MessagingCenter.Subscribe<ClienteRest>(this, "errorPersona", (Sender) => {
                DisplayAlert("Error", "¡No fue posible modifcar los datos!", "OK");

            });

            btnGuardar.IsEnabled = false;
            btnModificar.IsEnabled = true;
          
        }
    }
}
