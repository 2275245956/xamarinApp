using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Essentials;

namespace PatientEHR.APISDK.FileManager
{
    public class DbManager
    {

        private static Dictionary<string, DbManager> _instances = new Dictionary<string, DbManager>();

        readonly SQLiteConnectionString options;
        readonly SQLiteAsyncConnection database;

        private static string ConnectionKey
        {
            get
            {
                var user = APISDK.ApiSDK.Instance.CurrentUserInfo;
                if (user.PatientNo.ToString().Length < 8)
                {
                    var prefix = "";
                    for (int i = 0; i < 8 - user.PatientNo.ToString().Length; i++)
                    {
                        prefix += "0";
                    }
                    return $"{prefix}{user.PatientNo}";
                }
                return $"{user.PatientNo}_fileDb";
            }
        }
        private DbManager(string dbPath)
        {
            options = new SQLiteConnectionString(databasePath: dbPath,
               storeDateTimeAsTicks: true,
               key: ConnectionKey);
            database = new SQLiteAsyncConnection(options);
            database.CreateTableAsync<LocalFile>().Wait();
        }

        public static DbManager GetInstance(string dbPath)
        {
            if (!_instances.ContainsKey(ConnectionKey))
            {
                _instances.Add(ConnectionKey, new DbManager(dbPath));
            }
            return _instances[ConnectionKey];
        }
        public  void CloseDB()
        {
            if (_instances.ContainsKey(ConnectionKey))
            {
                _instances[ConnectionKey].database.CloseAsync().Wait();
                _instances.Remove(ConnectionKey);
            }
        }


        #region FileTable

        public Task<LocalFile> GetLocalFile(string fileName)
        {
            return database.Table<LocalFile>()
                .Where(f => f.FileName == fileName)
                .FirstOrDefaultAsync();
        }
        public Task<int> DeleteLocalFile(LocalFile f)
        {
            return database.DeleteAsync(f);
        }

        public Task<int> Add(LocalFile file) {
            return database.InsertAsync(file);
        }
        public Task<int> AddAll(List<LocalFile> files) {
            return database.InsertAllAsync(files);
        }
        #endregion
    }
}
