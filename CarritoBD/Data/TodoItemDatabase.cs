using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace CarritoBD.Data
{
    public class TodoItemDatabase
    {
        readonly SQLiteAsyncConnection database;

        public TodoItemDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Carrito>().Wait();
        }

        public Task<List<Carrito>> GetItemsAsync()
        {
            return database.Table<Carrito>().ToListAsync();
        }

        public Task<List<Carrito>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<Carrito>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

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

        public Task<int> DeleteItemAsync(Carrito item)
        {
            return database.DeleteAsync(item);
        }
    }
}
