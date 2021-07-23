using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Collections.Specialized;
using System.Linq;

namespace PatientEHR.APISDK.FileManager
{
    public class DownloadManager
    {


        public static string GetDocumentDirectionStr()
        {
            //			System.IO.DirectoryInfo dinfo= new System.IO.DirectoryInfo(	System.Environment.CurrentDirectory);		
            //			string tmpDirection=dinfo.Parent.FullName + "/Documents/";

            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string tmpDirection = path + "/";
            return tmpDirection;
        }

        public static string GetPreferenceDirectionStr()
        {
            //			showSpecialFolders ();
            //			System.IO.DirectoryInfo dinfo= new System.IO.DirectoryInfo(	System.Environment.CurrentDirectory);		
            //			string tmpDirection=dinfo.Parent.FullName + "/Library/Preferences/";

            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            System.IO.DirectoryInfo dinfo = new System.IO.DirectoryInfo(path);
            string tmpDirection = dinfo.Parent.FullName + "/Library/Preferences/";

            if (!System.IO.Directory.Exists(tmpDirection))
            {
                System.IO.Directory.CreateDirectory(tmpDirection);
            }
            return tmpDirection;
        }

        public static string GetTempDirectionStr()
        {
            //			showSpecialFolders ();
            //			showPathTree (dinfo);
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            System.IO.DirectoryInfo dinfo = new System.IO.DirectoryInfo(path);
            string tmpDirection = dinfo.Parent.FullName + "/tmp/";
            //string tmpDirection=path +"/";//dinfo.Parent.FullName + "/tmp/";

            if (!System.IO.Directory.Exists(tmpDirection))
            {
                System.IO.Directory.CreateDirectory(tmpDirection);
            }
            return tmpDirection;
        }
        private static void showPathTree(System.IO.DirectoryInfo di)
        {
            Console.WriteLine(di.FullName.Replace(System.Environment.CurrentDirectory, ""));
            var dis = di.GetDirectories();
            foreach (var d in dis)
            {
                showPathTree(d);
            }
        }

        private static void showSpecialFolders()
        {
            Console.WriteLine(Environment.MachineName);
            Console.WriteLine("Environment.SpecialFolder.AdminTools:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.AdminTools));
            Console.WriteLine("Environment.SpecialFolder.ApplicationData:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            Console.WriteLine("Environment.SpecialFolder.CDBurning:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.CDBurning));
            Console.WriteLine("Environment.SpecialFolder.CommonAdminTools:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.CommonAdminTools));
            Console.WriteLine("Environment.SpecialFolder.CommonApplicationData:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
            Console.WriteLine("Environment.SpecialFolder.CommonDesktopDirectory:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory));
            Console.WriteLine("Environment.SpecialFolder.CommonDocuments:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            Console.WriteLine("Environment.SpecialFolder.CommonMusic:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic));
            Console.WriteLine("Environment.SpecialFolder.CommonOemLinks:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.CommonOemLinks));
            Console.WriteLine("Environment.SpecialFolder.CommonPictures:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures));
            Console.WriteLine("Environment.SpecialFolder.CommonProgramFiles:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles));
            Console.WriteLine("Environment.SpecialFolder.CommonProgramFilesX86:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86));
            Console.WriteLine("Environment.SpecialFolder.CommonPrograms:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms));
            Console.WriteLine("Environment.SpecialFolder.CommonStartMenu:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu));
            Console.WriteLine("Environment.SpecialFolder.CommonStartup:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup));
            Console.WriteLine("Environment.SpecialFolder.CommonTemplates:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.CommonTemplates));
            Console.WriteLine("Environment.SpecialFolder.CommonVideos:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos));
            Console.WriteLine("Environment.SpecialFolder.Cookies:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.Cookies));
            Console.WriteLine("Environment.SpecialFolder.Desktop:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            Console.WriteLine("Environment.SpecialFolder.DesktopDirectory:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
            Console.WriteLine("Environment.SpecialFolder.Favorites:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.Favorites));
            Console.WriteLine("Environment.SpecialFolder.Fonts:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts));
            Console.WriteLine("Environment.SpecialFolder.History:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.History));
            Console.WriteLine("Environment.SpecialFolder.InternetCache:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache));
            Console.WriteLine("Environment.SpecialFolder.LocalApplicationData:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            Console.WriteLine("Environment.SpecialFolder.LocalizedResources:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.LocalizedResources));
            Console.WriteLine("Environment.SpecialFolder.MyComputer:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.MyComputer));
            Console.WriteLine("Environment.SpecialFolder.MyDocuments:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            Console.WriteLine("Environment.SpecialFolder.MyMusic:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            Console.WriteLine("Environment.SpecialFolder.MyPictures:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
            Console.WriteLine("Environment.SpecialFolder.MyVideos:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
            Console.WriteLine("Environment.SpecialFolder.NetworkShortcuts:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.NetworkShortcuts));
            Console.WriteLine("Environment.SpecialFolder.Personal:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            Console.WriteLine("Environment.SpecialFolder.PrinterShortcuts:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.PrinterShortcuts));
            Console.WriteLine("Environment.SpecialFolder.ProgramFiles:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
            Console.WriteLine("Environment.SpecialFolder.ProgramFilesX86:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86));
            Console.WriteLine("Environment.SpecialFolder.Programs:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.Programs));
            Console.WriteLine("Environment.SpecialFolder.Recent:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.Recent));
            Console.WriteLine("Environment.SpecialFolder.Resources:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.Resources));
            Console.WriteLine("Environment.SpecialFolder.SendTo:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.SendTo));
            Console.WriteLine("Environment.SpecialFolder.StartMenu:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu));
            Console.WriteLine("Environment.SpecialFolder.Startup:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.Startup));
            Console.WriteLine("Environment.SpecialFolder.System:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.System));
            Console.WriteLine("Environment.SpecialFolder.SystemX86:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86));
            Console.WriteLine("Environment.SpecialFolder.Templates:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.Templates));
            Console.WriteLine("Environment.SpecialFolder.UserProfile:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            Console.WriteLine("Environment.SpecialFolder.Windows:");
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.Windows));
        }
        public static void DownLoad(string Url)
        {
            DownLoad(Url, "quiz.plist");
        }

        public static bool DownLoad2(string Url, string newFileName)
        {
            if (string.IsNullOrEmpty(newFileName)) return false;
            string fullfilename = newFileName;
            if (System.IO.File.Exists(fullfilename))
                System.IO.File.Delete(fullfilename);

            System.IO.FileInfo fi = new System.IO.FileInfo(fullfilename);
            if (!System.IO.Directory.Exists(fi.Directory.FullName))
            {
                System.IO.Directory.CreateDirectory(fi.Directory.FullName);
            }

            WebClient client = new WebClient();
            try
            {
                client.DownloadFile(Url, newFileName);
                return System.IO.File.Exists(newFileName);
            }
            catch (Exception)
            {

                //Console.WriteLine("Err:{0}-{1}",Url, ex.Message);
                return false;
            }

        }

        public static bool DownLoadIfNotExist(string Url, string newFileName)
        {
            if (string.IsNullOrEmpty(newFileName)) return false;
            string fullfilename = newFileName;
            if (System.IO.File.Exists(fullfilename))
            {
                return true;
            }

            System.IO.FileInfo fi = new System.IO.FileInfo(fullfilename);
            if (!System.IO.Directory.Exists(fi.Directory.FullName))
            {
                System.IO.Directory.CreateDirectory(fi.Directory.FullName);
            }

            var tmpFileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), Guid.NewGuid().ToString());
            WebClient client = new WebClient();
            try
            {

                client.DownloadFile(Url, tmpFileName);
                System.IO.File.Copy(tmpFileName, newFileName);
                return System.IO.File.Exists(newFileName);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (System.IO.File.Exists(tmpFileName))
                {
                    System.IO.File.Delete(tmpFileName);
                }
            }
        }

        public static string ConvertToTmpPath(string fileName)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(fileName);
            string cache = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
            return System.IO.Path.Combine(cache, fi.Name);

            //			System.IO.FileInfo fi= new System.IO.FileInfo(fileName);
            //			return GetTempDirectionStr() + "tmpfile" + fi.Extension;
        }

        public static bool FileExists(string url)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                {
                    var cd = response.StatusCode;
                    response.Close();
                    return cd == HttpStatusCode.OK;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DownLoad3(string Url, string newFileName)
        {
            if (string.IsNullOrEmpty(newFileName)) return false;

            string fullfilename = ConvertToTmpPath(newFileName);
            if (System.IO.File.Exists(fullfilename))
                System.IO.File.Delete(fullfilename);

            System.IO.FileInfo fi = new System.IO.FileInfo(fullfilename);
            if (!System.IO.Directory.Exists(fi.Directory.FullName))
            {
                System.IO.Directory.CreateDirectory(fi.Directory.FullName);
            }

            fi = new System.IO.FileInfo(newFileName);
            if (!System.IO.Directory.Exists(fi.Directory.FullName))
            {
                System.IO.Directory.CreateDirectory(fi.Directory.FullName);
            }
            using (WebClient client = new WebClient())
            {
                string exep = string.Empty;
                do
                {
                    try
                    {
                        client.DownloadFile(Url, fullfilename);
                        System.IO.File.Copy(fullfilename, newFileName, true);
                        //System.IO.File.Delete(fullfilename);

                        return true;
                    }
                    catch (System.Net.WebException wex)
                    {
                        if (wex.Message == "An exception occurred during a WebClient request.")
                        {
                            exep = "occurred";
                        }
                        else
                        {
                            exep = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Err:{0}-{1}", Url, ex.Message);
                        return false;
                        //throw ex;

                    }
                    finally
                    {
                        if (System.IO.File.Exists(fullfilename))
                            System.IO.File.Delete(fullfilename);
                    }


                } while (exep == "occurred");
                return true;
            }
        }


        public static byte[] GetFileContent(string url, Dictionary<string,string> urlParam)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    NameValueCollection values = new NameValueCollection();
                    if (urlParam.Any())
                    {
                        foreach (var kv in urlParam)
                        {
                            values.Add(kv.Key, kv.Value);
                        }
                    }
                    var bytes = client.UploadValues(url, values);//
                    return bytes;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
                finally
                {
                }
            }
        }

        private static void DeleteCacheFile(string fullfilename)
        {
            try
            {
                if (File.Exists(fullfilename))
                    File.Delete(fullfilename);
            }
            catch (Exception) { }
        }

        public static void DownLoad(string Url, string newFileName)
        {
            WebClient client = new WebClient();

            System.IO.Stream stream = client.OpenRead(Url);
            List<byte> bytes = new List<byte>();
            System.IO.BinaryReader br = new System.IO.BinaryReader(stream);
            while (br.BaseStream.CanRead)
            {
                byte[] bs = br.ReadBytes(512);
                if (bs.Length > 0)
                    bytes.AddRange(bs);
                else
                    break;
            }

            br.Close();
            stream.Close();

            System.IO.DirectoryInfo dinfo = new System.IO.DirectoryInfo(System.Environment.CurrentDirectory);
            //bool bl = System.IO.Directory.Exists(dinfo.Parent.FullName + "/tmp");


            string fullfilename = "";
            if (!newFileName.Contains(dinfo.Parent.FullName))
            {
                string tmpDirection = GetTempDirectionStr();//dinfo.Parent.FullName + "/tmp/";
                fullfilename = tmpDirection + newFileName;
            }
            else
            {
                fullfilename = newFileName;
            }

            if (System.IO.File.Exists(fullfilename))
                System.IO.File.Delete(fullfilename);

            System.IO.FileInfo fi = new System.IO.FileInfo(fullfilename);
            if (!System.IO.Directory.Exists(fi.Directory.FullName))
            {
                System.IO.Directory.CreateDirectory(fi.Directory.FullName);
            }

            System.IO.FileStream fs = System.IO.File.Create(fullfilename);
            fs.Write(bytes.ToArray(), 0, bytes.Count);
            fs.Close();
        }        
    }
}

