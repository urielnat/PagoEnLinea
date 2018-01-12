using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PagoEnLinea.Modelos;
using PagoEnLinea.Paginas;
using Xamarin.Forms;

namespace PagoEnLinea.servicios
{
    public class ClienteRest
    {
        public async Task<T> GET<T>(string url)
        {

            try
            {
                HttpClient client = new HttpClient();


                if (Application.Current.Properties.ContainsKey("token"))
                {

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);
                }
                var response = await client.GetAsync(url);
                System.Diagnostics.Debug.WriteLine(response);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    System.Diagnostics.Debug.WriteLine("SE HIZO LA CONEXION");
                    var jsonString = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(jsonString);
                    var x=JsonConvert.DeserializeObject<T>(jsonString);
                    return x;


                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(response);
                    MessagingCenter.Send(this, "error");
                }
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
            }


            return default(T);
        }

        public async void POST(string url, object obj,int tipo){
            HttpResponseMessage response;
           
            string ContentType = "application/json"; // or application/xml
            var jsonstring = JsonConvert.SerializeObject(obj);

            System.Diagnostics.Debug.WriteLine(jsonstring);
            try{
                HttpClient cliente = new HttpClient();

                if (Application.Current.Properties.ContainsKey("token"))
                {

                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);
                }

                response = await cliente.PostAsync(url, new StringContent(jsonstring, Encoding.UTF8, ContentType));
                var y = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(y);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    System.Diagnostics.Debug.WriteLine("SE GUARDO POR POST");
                    if (tipo == 0)
                    {
                        MessagingCenter.Send(this, "OKR");
                    }

                    else
                    {
                        MessagingCenter.Send(this, "OK");
                    }


                }
                else if(tipo ==0)
                {
                    System.Diagnostics.Debug.WriteLine(response.ToString());
                    MessagingCenter.Send(this, "errorp");
                }else{
                    
                        System.Diagnostics.Debug.WriteLine(response.ToString());
                        MessagingCenter.Send(this, "error");
                    
                }
            }catch(HttpRequestException e){
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
                MessagingCenter.Send(this, "error");
            }
        }

        public async void PUT(string url, object obj)
        {
            HttpResponseMessage response;

            string ContentType = "application/json"; // or application/xml
            var jsonstring = JsonConvert.SerializeObject(obj);

            //jsonstring = jsonstring.Substring(1, jsonstring.Length - 2);
            System.Diagnostics.Debug.WriteLine(jsonstring);
            try
            {
                HttpClient cliente = new HttpClient();


                if(Application.Current.Properties.ContainsKey("token")){

                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",Application.Current.Properties["token"]as string);
                }


                response = await cliente.PutAsync(url, new StringContent(jsonstring, Encoding.UTF8, ContentType));

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    
                    System.Diagnostics.Debug.WriteLine("SE GUARDO POR PUT");

                  
             
                    MessagingCenter.Send(this, "OK");
                    MessagingCenter.Send(this, "puttelefono");
                    MessagingCenter.Send(this, "putfacturacion");

                }else{
                    
                  

                    MessagingCenter.Send(this, "error");
                   
                    MessagingCenter.Send(this, "errorfacturacion");
                    System.Diagnostics.Debug.WriteLine(response);}
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
            }
        }

        public async Task<T> InfoUsuario<T>(string token)
        {
            try
            {
                HttpResponseMessage response;
                string sUrl = Constantes.URL_USUARIOS+"/info-contribuyente";
               
                info inf = new info();
                //inf.usuario = user;




                var jsonstring = JsonConvert.SerializeObject(inf);





                HttpClient Client = new HttpClient();

              
              
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);

                response = await Client.GetAsync(sUrl);
               
                var y = await response.Content.ReadAsStringAsync();
                 
           

                var oTaskPostAsync = await response.Content.ReadAsStringAsync();


                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                  

                    var users = JsonConvert.DeserializeObject<InfoUsuario>(y);
                
                    return JsonConvert.DeserializeObject<T>(y);


                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(response);
                }
            }catch(HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
            }
            return default(T);
        }
    }
}
