using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PatientEHR.Models;
using SQLite;

namespace PatientEHR.Services
{
    public class ServiceInstance
    {

        private static string ConnectionKey
        {
            get
            {
                var user = APISDK.ApiSDK.Instance.CurrentUserInfo;
                if (user.PatientNo.ToString().Length<8) {
                    var prefix = "";
                    for (int i = 0; i < 8-user.PatientNo.ToString().Length; i++)
                    {
                        prefix += "0";
                    }
                    return $"{prefix}{user.PatientNo}";
                }
                return $"{user.PatientNo}";
            }
        }


        readonly SQLiteConnectionString options;
        private ServiceInstance(string dbPath)
        {

            options = new SQLiteConnectionString(databasePath: dbPath,
                storeDateTimeAsTicks: true,
                key: ConnectionKey);
            database = new SQLiteAsyncConnection(options);
            //database = new SQLiteAsyncConnection(dbPath);

            database.CreateTableAsync<Item>().Wait();
            database.CreateTableAsync<MedicalHistoryTableModel>().Wait();
            database.CreateTableAsync<SoapModel>().Wait();
            database.CreateTableAsync<SoapFileModel>().Wait();
            database.CreateTableAsync<BaseInfoModel>().Wait();
            database.CreateTableAsync<OrderModel>().Wait();
            database.CreateTableAsync<NoticeModel>().Wait();
            database.CreateTableAsync<RefreshTimeTable>().Wait();
            //database.CreateTableAsync<BBSItem>().Wait();

        }

        private void _SetRefreshTimeTableBaseData()
        {
            //var allTables = Attribute.GetCustomAttributes(typeof(TableAttribute), true);
            //var allKeys = database.Table<RefreshTimeTable>().ToListAsync().Result;
            //foreach (var item in allTables)
            //{
            //    var tbAttr = item as TableAttribute;
            //    if (allKeys.Find(a => a.TableKey == tbAttr.Name) == null)
            //    {
            //        database.InsertAsync(new RefreshTimeTable { TableKey = tbAttr.Name, LastDataDateTime = new DateTime(year: 1900, month: 1, day: 1) });
            //    }
            //}
        }

        readonly SQLiteAsyncConnection database;

        private static Dictionary<string, ServiceInstance> _instances = new Dictionary<string, ServiceInstance>();
        public static ServiceInstance GetInstance(string dbPath)
        {
            if (!_instances.ContainsKey(ConnectionKey))
            {
                _instances.Add(ConnectionKey, new ServiceInstance(dbPath));
            }
            return _instances[ConnectionKey];
        }

        public static void CloseDB(string dbPath)
        {
            if (_instances.ContainsKey(ConnectionKey))
            {
                _instances[ConnectionKey].database.CloseAsync().Wait();
                _instances.Remove(ConnectionKey);
            }
        }

        #region ItemTable
        public Task<List<Item>> GetItemsAsync()
        {
            //Get all notes.
            return database.Table<Item>().ToListAsync();
        }

        public Task<Item> GetItemAsync(int id)
        {
            // Get a specific note.
            return database.Table<Item>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Item note)
        {
            if (note.Id != 0)
            {
                // Update an existing note.
                return database.UpdateAsync(note);
            }
            else
            {
                // Save a new note.
                return database.InsertAsync(note);
            }
        }

        public Task<int> DeleteNoteAsync(Item note)
        {
            // Delete a note.
            return database.DeleteAsync(note);
        }
        #endregion

        #region RefreshTable  CRUD
        public Task<List<RefreshTimeTable>> GetRefreshTimeTableItemsAsync()
        {
            return database.Table<RefreshTimeTable>().ToListAsync();
        }
        public Task<RefreshTimeTable> GetRefreshTimeTableModelAsync(string tableKey)
        {
            return database.Table<RefreshTimeTable>().Where(s => s.TableKey == tableKey).FirstOrDefaultAsync();
        }
        public Task<int> SaveRefreshTableItemAsync(RefreshTimeTable md)
        {
            return md.Id != 0 ? database.UpdateAsync(md) : database.InsertAsync(md);
        }
        #endregion


        #region NoticeTable CRUD
        public Task<List<NoticeModel>> GetNoticeItemsAsync()
        {
            return database.Table<NoticeModel>().ToListAsync();
        }
        public Task<int> AddNewNoticeItemsAsync(List<NoticeModel> ls)
        {
            return database.InsertAllAsync(ls);
        }
        #endregion

        #region TreatmentTable CRUD
        public Task<List<MedicalHistoryTableModel>> GetMedicalHistoryTableModelsAsync()
        {
            return database.Table<MedicalHistoryTableModel>()
                .OrderByDescending(s => s.ConsultDate)
                .ToListAsync();
        }
        public Task<MedicalHistoryTableModel> GetMedicalHistoryAsync(int medicalHisId)
        {
            return database.Table<MedicalHistoryTableModel>()
                    .Where(s => s.MedicalHistoryID == medicalHisId)
                    .FirstOrDefaultAsync();
        }
        public Task<int> SaveMedicalHisAsync(List<MedicalHistoryTableModel> ls)
        {
            return database.InsertAllAsync(ls);
        }
        #endregion

        #region TreatmentDetail 

        #region SOAP  CRUD
        public Task<SoapModel> GetSoapModelAsync(int medicalHisId)
        {
            return database.Table<SoapModel>()
                .Where(s => s.MedicalHistoryID == medicalHisId)
                .FirstOrDefaultAsync();
        }
        public Task<int> SaveSoapAsync(SoapModel md)
        {
            return database.InsertAsync(md);
        }
        #endregion

        #region OrderData   CRUD
        public Task<List<OrderModel>> GetOrderItemsAsync(int medicalHisId)
        {
            return database.Table<OrderModel>()
                .Where(s => s.MedicalHistoryID == medicalHisId)
                .OrderByDescending(s => s.ConsultDate)
                .ToListAsync();
        }
        public Task<int> SaveOrderItemsAsync(List<OrderModel> ls)
        {
            return database.InsertAllAsync(ls);
        }
        #endregion

        #region SOAPFile 
        public Task<List<SoapFileModel>> GetSoapFilesAsync(int medicalInfoFileID)
        {
            return database.Table<SoapFileModel>()
                .Where(s => s.MedicalInfoFileID == medicalInfoFileID)
                .ToListAsync();
        }
        public Task<int> SaveSoapFileAsync(List<SoapFileModel> files)
        {
            return database.InsertAllAsync(files);
        }

        #endregion
        #endregion

        #region PatientAttribute
        public Task<List<BaseInfoModel>> GetPatientAttributeInfoItemsAsync()
        {
            return database.Table<BaseInfoModel>().ToListAsync();
        }
        public Task<int> SavePatientAttributeInfoAsync(List<BaseInfoModel> ls)
        {
            return database.InsertAllAsync(ls);
        }
        #endregion
    }
}
