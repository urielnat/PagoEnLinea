using System;
using System.IO;
using CarritoBD.Interface;
using PagoEnLinea.Droid;
using Xamarin.Forms;



[assembly: Dependency(typeof(FileHelper))]
namespace PagoEnLinea.Droid
{
 
        public class FileHelper : IFileHelper
        {
            public string GetLocalFilePath(string filename)
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                return Path.Combine(path, filename);
            }
        }
    
}
