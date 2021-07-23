using PatientEHR.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PatientEHR.APISDK.FileManager
{
    public class FileManager
    {
        private DbManager _dbManager;
        private System.Timers.Timer _fileTimer = null; // new System.Timers.Timer();
        private System.Collections.Concurrent.ConcurrentDictionary<string, LocalFile> _files = new System.Collections.Concurrent.ConcurrentDictionary<string, LocalFile>();
        private bool _isDBOperating = false;
        private FileManager()
        {
            var dbName = $"localfile_{APISDK.ApiSDK.Instance.CurrentUserInfo.PatientNo}.db3";
            _dbManager = DbManager.GetInstance(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dbName));

            if (_fileTimer != null)
            {
                _fileTimer.Dispose();
                _fileTimer = null;
            }
            _fileTimer = new System.Timers.Timer(100);
            _fileTimer.Elapsed -= _fileTimer_Elapsed;
            _fileTimer.Elapsed += _fileTimer_Elapsed;
            _fileTimer.Stop();
        }

        private void _fileTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _fileTimer.Stop();
            try
            {
                if (!_isDBOperating && !_files.IsEmpty)
                {
                    var lf = _files.ElementAt(0);
                    SaveToLocalDb(lf.Value);
                    _files.TryRemove(lf.Key, out _);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (!_files.IsEmpty)
                {
                    _fileTimer.Start();
                }
            }
        }

        public void Close()
        {
            _fileTimer.Stop();
            _dbManager.CloseDB();
        }

        public static FileManager Instance { get; private set; } = new FileManager();


        public async Task DownloadFileAsync(string downloadUrl, string fileName, Action<LocalFile> afterDownload, Action<Exception> onError)
        {
            try
            {
                DelLocalFile(fileName);
                if (string.IsNullOrEmpty(downloadUrl)) return; 
                WebClient client = new WebClient();
                try
                {
                    var tmpFileName = System.IO.Path.Combine(Common.ImageFolder, ApiSDK.Instance.CurrentUserInfo.PatientNo.ToString(), fileName);
                    System.IO.FileInfo fi = new System.IO.FileInfo(tmpFileName);
                    if (!System.IO.Directory.Exists(fi.Directory.FullName))
                    {
                        System.IO.Directory.CreateDirectory(fi.Directory.FullName);
                    }
                    var stream = client.OpenRead(downloadUrl);
                    using (FileStream fs = new FileStream(tmpFileName, FileMode.Create))
                    {
                        stream.CopyTo(fs);
                    }
                    //client.DownloadFile(downloadUrl, $"temp_{fileName}");
                    //System.IO.File.Copy($"temp_{fileName}", tmpFileName, true);
                    afterDownload?.Invoke(new LocalFile { FileName = fileName, FilePath = tmpFileName });
                }
                catch (Exception ex)
                {
                    onError?.Invoke(ex);
                }

            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
            }

        }

        public void DownloadFile(string url, string displayName, string fileId, Action<LocalFile> afterDownload, Action<Exception> onError)
        {
            try
            {
                var lf = GetLocalFile(url);
                if (lf == null || !System.IO.File.Exists(lf.FilePath))
                {
                    Task.Factory.StartNew(() =>
                    {
                        lf = _DownloadFile(url, displayName);
                        lf.FileId = fileId;
                        _files.AddOrUpdate(url, lf, (key, f) => f);
                        _fileTimer.Start();
                        afterDownload?.Invoke(lf);
                    });
                }
                else
                {
                    afterDownload?.Invoke(lf);
                }
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
            }
        }
        private void SaveToLocalDb(LocalFile lf)
        {
            _dbManager.Add(lf).GetAwaiter();
        }

        public List<LocalFile> GetLocalFiles() => _files.Values.ToList();

        public bool ContainsKey(string fileName) => GetLocalFile(fileName) != null;

        public void AddLocalFile(LocalFile lf) => SaveToLocalDb(lf);

        public LocalFile GetLocalFile(string fileName)
        {
            if (_files.ContainsKey(fileName))
            {
                return _files[fileName];
            }
            return _dbManager.GetLocalFile(fileName).GetAwaiter().GetResult();
        }


        public void DelLocalFile(string fileName)
        {
            var lf = GetLocalFile(fileName);
            if (lf != null)
            {
                if (!string.IsNullOrEmpty(lf.FilePath)) _DeleteFile(lf.FilePath);
                if (!string.IsNullOrEmpty(lf.PreviewFilePath)) _DeleteFile(lf.PreviewFilePath);
            }
            _dbManager.DeleteLocalFile(lf).GetAwaiter();
        }

        private void _DeleteFile(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    System.IO.File.Delete(filePath);
                }
                catch (Exception) { }
            }
        }

        private LocalFile _DownloadFile(string downLoadUrl, string fileName)
        {
            string url = downLoadUrl;
            var newFileName = Guid.NewGuid().ToString("D");
            string imagepostfix = System.IO.Path.GetExtension(fileName);// fileItem.FileName.Substring(fileItem.FileName.IndexOf("."));
            var dt = DateTime.Now;
            string localFullFileName = GetFileName(dt, string.Format("{0}{1}", newFileName, imagepostfix));
            string localFullFileThumbName = GetFileName(dt, string.Format("{0}_thumb.png", newFileName, imagepostfix));

            if (ImageUtility.IsImage(fileName))
            {
                DownloadManager.DownLoadIfNotExist(url, localFullFileName);
            }
            else if (ImageUtility.IsVideo(fileName))
            {
                DownloadManager.DownLoadIfNotExist(url, localFullFileName);
            }
            else
            {
                DownloadManager.DownLoadIfNotExist(url, localFullFileName);
                localFullFileThumbName = "other.png";
            }

            return new LocalFile()
            {
                DisplayName = fileName,
                FileName = downLoadUrl,
                FilePath = localFullFileName,
                PreviewFilePath = localFullFileThumbName
            };
        }

        public static string CurrUserID => ApiSDK.Instance.CurrentUserInfo.PatientNo.ToString();
        public static string LoaclFileRootPath => System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), "LocalFiles", CurrUserID);

        public static string GetFileName(string fileName) => System.IO.Path.Combine(LoaclFileRootPath, $"{DateTime.Today:yyyyMMdd}", fileName);
        public static string GetFileName(DateTime dt, string fileName) => System.IO.Path.Combine(LoaclFileRootPath, $"{dt:yyyyMMdd}", fileName);

    }
}
