using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace CarritoBD.Data
{
    /// <summary>
    /// esta clase corresponde a los métodos publicos para realizar diversas acciones correspondientes a la
    /// base de datos de interna del dispositivo móvil la cual almacena los elementos añadidos al carrito y permite gestionarlos
    /// con diversos métodos
    /// </summary>
    public class TodoItemDatabase
    {
        readonly SQLiteAsyncConnection database;

        /// <summary>
        /// crea una coneccion con la base de datos y una tabla para manipularla de tipo carrito (modelo)
        /// </summary>
        /// <param name="dbPath">Nombre de la base de datos a trabajar</param>
        public TodoItemDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Carrito>().Wait();
        }

        /// <summary>
        /// obtiene todos los elementos añadios a la base de datos que en este caso serian items añadidos al carrito
        /// </summary>
        /// <returns>The items async.</returns>
        public Task<List<Carrito>> GetItemsAsync()
        {
            return database.Table<Carrito>().ToListAsync();
        }

        /// <summary>
        /// método actualmente no implementado en el proyecto regresa un item en específico cuando se conoce su id
        /// </summary>
        /// <returns>item según id</returns>
        /// <param name="id">identificador o llave del item</param>
        public Task<Carrito> GetItemAsync(int id)
        {
            return database.Table<Carrito>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Carrito item)
        {
            if (item.ID != 0)
            {
               
                return database.UpdateAsync(item);

            }
            else {
                return database.InsertAsync(item);
            }
        }

        /// <summary>
        /// Borra un item específico de carrito
        /// </summary>
        /// <returns>la base de datos sin el item eliminado </returns>
        /// <param name="item">Item a eliminar</param>
        public Task<int> DeleteItemAsync(Carrito item)
        {
            return database.DeleteAsync(item);
        }
    }
}
