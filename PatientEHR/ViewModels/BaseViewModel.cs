using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PatientEHR.Services;
using System.IO;
using PatientEHR.APISDK.FileManager;

namespace PatientEHR.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        private string dbName => $"{APISDK.ApiSDK.Instance.CurrentUserInfo.PatientNo}Notes.db3";
        public ServiceInstance DataStore => ServiceInstance.GetInstance(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dbName));
        //public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

        //public DbManager FileDataStore => DbManager.GetInstance(
        //     Path.Combine(
        //         Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        //         $"{APISDK.ApiSDK.Instance.CurrentUserInfo.PatientNo}LocalFile.db3"
        //     )
        //);
        public FileManager FileDbStrore => FileManager.Instance;


        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
