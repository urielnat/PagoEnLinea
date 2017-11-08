using System;
using System.Collections.Generic;
using System.Diagnostics;
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
       async void Handle_Clicked(object sender, System.EventArgs e)
        {
            var action = await DisplayActionSheet("Modificar datos", "Cancelar", "Eliminar", "Modificar", "Agregar");
            Debug.WriteLine("Action: " + action);
        }

        async void contraseña_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ModificarContraseña());       
        }



    }
}
