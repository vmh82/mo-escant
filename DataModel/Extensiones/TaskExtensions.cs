using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataModel.Infraestructura.Offline.DB;
using Xamarin.Forms;

namespace DataModel.Extensions
{
    public static class TaskExtensions
    {
        public static async void SafeFireAndForget(this Task task,
            bool returnToCallingContext,
            Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(returnToCallingContext);
            }

            // if the provided action is not null, catch and
            // pass the thrown exception
            catch (Exception ex) when (onException != null)
            {
                onException(ex);
            }
        }
        
    }

    public static class TaskFunciones
    {
        static string path = DependencyService.Get<IDBLite>().DatabasePath();        

        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(path, DB3.Flags, true);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        public static async Task<bool> InitializeAsync<T>(this bool initialized)
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(T).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(T)).ConfigureAwait(false);
                    initialized = true;                                 
                }
            }
            return initialized;
        }

        public static async Task<bool> CreateDatabase(this bool initialized, Type[] tableTypes)
        {
            if (!initialized)
            {
                //Create the tables
                await Database.CreateTablesAsync(CreateFlags.None, tableTypes);
                
                initialized = true;
                
            };
            return initialized;
        }

    }

}
