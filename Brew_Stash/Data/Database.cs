using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SQLite;
using SQLitePCL;

namespace Brew_Stash
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        /// <summary>
        /// Creates a database instance
        /// </summary>
        /// <param name="dbPath"></param>

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<FinalOrder>().Wait();
        }

        public Task<List<FinalOrder>> GetOrdersAsync()
        {
            return _database.Table<FinalOrder>().ToListAsync();
        }

        public Task<FinalOrder> GetOrderAsync(int id)
        {
            return _database.Table<FinalOrder>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveOrderAsync(FinalOrder order)
        {
            if (order.ID != 0)
            {
                return _database.UpdateAsync(order);
            }
            else
            {
                return _database.InsertAsync(order);
            }
        }

        public Task<int> DeleteOrderAsync(FinalOrder order)
        {
            return _database.DeleteAsync(order);
        }
    }
}