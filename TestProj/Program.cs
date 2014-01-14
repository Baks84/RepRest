using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using RESTClient;

namespace TestProj
{
    class Program
    {
        static void Main(string[] args)
        {
            #region MediaFire
            RESTClient.MediaFire.MediaFireApi mfapi = new RESTClient.MediaFire.MediaFireApi();
            mfapi.GetSessionToken("kkromkowski@ftechno.pl", "Aniolek69");

            //var info = mfapi.UploadFile("D:\\Wrzutka9.txt", ActionOnDuplicate:RESTClient.MediaFire.UploadDuplicateActions.Replace);
            var info2 = mfapi.Upload_Poll("0be5u8rabm1");//info.Upload_Key);

            //var info = mfapi.PreUpload("D:\\DropBox\\faktura.pdf", MakeHash: true, Resumable:true);
            
            //var info = mfapi.GetWebUploads(true);

            //var info = mfapi.AddWebUpload(new Uri("http://indianautosblog.com/wp-content/uploads/2013/06/2014-Toyota-Corolla-for-Europe-rear.jpg"), "Toyota Corolla Przod.jpg", "4c9b9li11g4bx");

            //var info = mfapi.DownloadFileAsync("vgrhwwdxlg2o771", "D:\\FromMF.pdf");//"nb4p046mv6l8hkp");
            //while (!info.IsCompleted)
            //{
            //    Console.WriteLine("Still Waiting");
            //    System.Threading.Thread.Sleep(1000);
            //}
            //Console.WriteLine("Done.");
            //var info = mfapi.GetZip(new List<string>() { "nb4p046mv6l8hkp" });
            //var info = mfapi.UpdateFile("nb4p046mv6l8hkp", FileName:"NazwaPoZmianie.txt", Description:"Opis Tez", Tags:"MojPlik,DoSkasowania", Public:true);

            //var info = mfapi.RestoreFileToRevision("nb4p046mv6l8hkp", 301);

            //var info = mfapi.GetFileOneTimeDownloadLink("nb4p046mv6l8hkp");

            //var vers = mfapi.GetFileVersions("nb4p046mv6l8hkp");
            
            //var info = mfapi.GetFileLinks("vgrhwwdxlg2o771");

            //var info = mfapi.CreateFile("Stworzony z api.txt");
            //mfapi.DeleteFile(info.ID, true);
            //var nr = mfapi.CreateFileSnapshot("vgrhwwdxlg2o771");

            //var info = mfapi.CreateFile("Stworzony z api.txt", "4c9b9li11g4bx");

            //var info = mfapi.CopyFile("vgrhwwdxlg2o771", "ckqnmxowb2pbw");

            //mfapi.MoveFolder("ckqnmxowb2pbw");

            //mfapi.DeleteFolder("jub515p8wgc3m", true);
            //var info = mfapi.CreateFolder("ZApiDoTestu", "ckqnmxowb2pbw");

            //var finfo = mfapi.CopyFolder("ckqnmxowb2pbw", "4c9b9li11g4bx");
            
            //mfapi.UpdateFolder("ckqnmxowb2pbw", Description: "Nowy opis folderu");
            
            //var delta = mfapi.GetFolderRevision("");
            //var info = mfapi.GetFolderInfo("ckqnmxowb2pbw");

            //var content = mfapi.GetFolderContent("myfiles", RESTClient.MediaFire.MediaFireApi.GetFolderContentSettings.Folders);
            //var root = mfapi.Root;

            //var ui = mfapi.GetUserInfo();

            //System.Threading.Thread.Sleep(660000);
            //mfapi.RenewSessionToken();

            Console.ReadKey();
            #endregion

            #region BOX
            //BoxApi.V2.Authentication.OAuth2.OAuthToken token2 = new BoxApi.V2.Authentication.OAuth2.OAuthToken();
            //token2.AccessToken = "npluoNctaUzCAEHuW9waFhaFcFyTCjtK";
            //token2.RefreshToken = "Oc35ejEFrLp018S7i6mP9fPW2Z2HQ9hilmrMdxUfcF0nKawF9Zb8JIrEaSeSSIid";
            //RESTClient.Box.BOXApi ba = new RESTClient.Box.BOXApi(token2);
            ////var file = ba.ApiClient.CreateFile(BoxApi.V2.Model.Folder.Root, "faktura.pdf", File.ReadAllBytes(@"D:\DropBox\faktura.pdf"));
            //var file = ba.ApiClient.GetFolder(BoxApi.V2.Model.Folder.Root).Files.SingleOrDefault(fi => fi.Name.Contains(".pdf"));
            //File.WriteAllBytes(@"D:\sciagnietaFaktura.pdf", ba.ApiClient.Read(file)); 
            #endregion

            #region Mega - nie dziala
            //System.Net.ServicePointManager.DefaultConnectionLimit = 8;
            //RESTClient.Mega.MEGAApi m = new RESTClient.Mega.MEGAApi("baks84@g.pl", "Aniolek69");
            //var q = m.GetNodes();
            //m.test(); 
            #endregion

            #region SugarSync - nie dziala - bedzie platny
            //RESTClient.SugarSync.SugarSyncApiMy m = new RESTClient.SugarSync.SugarSyncApiMy();
            //m.Test();
            //RESTClient.SkyDrive.SkyApi api2 = new RESTClient.SkyDrive.SkyApi();
            //Console.WriteLine(api2.RegistrationURL);
            //Console.ReadKey();
            //string tokens = "";
            ////api2.AuthorizeToken(tokens);
            //api2.GetAccessToken(tokens); 
            #endregion

            #region SkyDrive
            RESTClient.SkyDrive.Implements.Token token = new RESTClient.SkyDrive.Implements.Token();
            token.Access_Token = "EwBAAq1DBAAUGCCXc8wU/zFu9QnLdZXy+YnElFkAARcwQVjKqjNWOdroMWjnJyxxiS2tTjteHtPrLeQRdUNBSijVAh9ayVEVAyJmthZrLUpI3IN0lhtWiUGZJGGrI6GtYXqBfz5Ju7qmdJFh3kaKsFcnY8f+am6YmfysCrEB52Hw1gcqxt9imlFygtnqe3Uv0a6aDF3gLvFVG1GVMAXDtyouo70ouGnKylPvJMwkrO5rmrMMABAmUGkCt7XB6FW5i8bSbgptiQhscHcLTS/TQplueiAP79aiRcaQtA3LsbIU3i1sj8MdicF5I16OZllTrHTH6x79Y44w/ij7Fzsj2W9qB3QhpDCap+wvV11JXw8GLF1dqmfFyGsLXMhT/EIDZgAACGRSBcePM4XuEAHl+fH6ovX5UUbJNhDO+TwVuoMa6xE3GJTyA0W6+2PN6vqo5cfRWjX5Qf3gG2riGGtDDgGM2n1W7Aa4ZpfxwEpJTd4Kqmex21iwkeUdU8b0AYco2d4PYYgOSzw3ojBUM727OVkJtsEFMc9HWwY28kas0Y52n7MMEVjgEJfiC3G6VE1TCvjAErYgXBNnjYUGFAkI1raBd8MMvLBEfGvB+s7lFeh0DOnDG7p2MxJUV003A4i+LlOQ06DHz0Uvtd9NevTlJwcAn3hfJHYV+66tP1dCJLAgg4XDZoHTJo/1JYuyvjSGqqg//zl57qRYWiliuBoadtpkP16vHudO+X/++ZRMgpjSVRMkPop9+d3howdRLgAA";
            token.Refresh_Token = "CjceEAnM6zwTcYhx8ICzHhhy8iw8izJHn0PHGTazG4y4AelrnYj1z*hAR31sOZPbO2WA1MySssAb38gXX6!RJu3TuaqPy2pQdtHNXCezKVE*r2DA36C6ZakaQUHWNJbTb5RNktHHReNHx3aSbOmEYZ6ltwQqo2wISMlKpzl4HyTU6YpHFKwG9YWiPL7!uaMB8JcgU8WTzmDCRHbhpBM7DdmMeHJJPPxFqPbZPlV05NISSDdjXO65QpstKMwmLEcVz9w2vR8J2mZSwO5F74f3i4IooKiHv73JohN2Ku2MTHsruei4mA5VIv3vpo!MK8xdwR06QGl0Lc9jeWzQI1IMq4J9cClGv93Z*8wxByRiyhIvC9nf!!daCx1yQ0trMxiFK3ZlnFggzTTYN9AvfftZ2MQdayL8bVKNFb7AzPPji2gJox!oJGwju8Y8NEHVkkb1ag$$";
            token.Expires_In = 3600;
            token.Scope = "wl.offline_access wl.skydrive wl.skydrive_update";
            token.Token_Type = "bearer";
            RESTClient.SkyDrive.SkyApi sa = new RESTClient.SkyDrive.SkyApi(token);

            //sa.RefreshToken();
            //sa.Test();
            //var aa = sa.GetElement<RESTClient.SkyDrive.IPhoto>("AllShare Play/AntekPhotosPublic/DSC_2115.jpg");
            //var aa = sa.GetElement<RESTClient.SkyDrive.IAlbum>("AllShare Play/AntekPhotosPublic");
            //var aa = sa.GetElement<RESTClient.SkyDrive.IVideo>("AllShare Play/Videos/FxGuru_20130220_144145.mp4");
            //var aa = sa.Copy("AllShare Play/faktura.pdf", "AllShare Play/Files");
            //var aa = sa.CreateFolder("ToBeDeleted", "TOBeDeleted");
            //var aa = sa.DeleteElement("Documents/DELETEME");
            //var aa = sa.MoveFile("Documents/faktura.pdf", "Documents/DELETEME");
            var aa = sa.RenameFile("AllShare Play/faktura.pdf", "fakturaDoSkasowania.pdf");

            var f = sa.Root;
            //sa.UploadFile("AllShare Play", "faktura.pdf", "D:\\DropBox\\faktura.pdf");
            //var fileInfo = sa.GetElement("AllShare Play/faktura.pdf");
            //var bytes = sa.DownloadFile("AllShare Play/faktura.pdf");
            //sa.DownloadAndSaveFile("AllShare Play/faktura.pdf", "D:\\Dropbox\\faktura2.pdf");
            //var ta = sa.DownloadAndSaveFileAsync("AllShare Play/AntekPhotosPublic/DSC_2115.jpg", "F:\\ZdjecieKrzysZOO.jpg");
            //while (!ta.IsCompleted)
            //{
            //    Console.WriteLine("Downloading Photo");
            //    System.Threading.Thread.Sleep(500);
            //}

            //var file = sa.GetElement("/AllShare Play/AntekPhotosPublic/IMAG0188.jpg"); 
            #endregion

            #region Dropbox
            RESTClient.Dropbox.DropboxAPI api = new RESTClient.Dropbox.DropboxAPI();

            if (false)
            {
                Console.WriteLine("Please use this URL to register: ");
                Console.WriteLine(api.RegistrationURL);
                Console.WriteLine("Please paste the code here and press enter:");
                string code = Console.ReadLine();
                if (api.AuthorizeToken(code))
                {
                    Console.WriteLine("Poprawnie zarejestrowany");
                }
                else
                {
                    Console.WriteLine("Nie poprawnie zarejestrowany");
                }
            }
            else
            {
                api.Token = new RESTClient.Dropbox.Implements.Token()
                {
                    Access_Token = "KlSM3FDuANQAAAAAAAAAAeopxkrvbF_a2BvOktatYCDJRn9a3PgJTeepqunz4QmZ",
                    Token_Type = "bearer",
                    UID = 132621988
                };
                //var a = api.MoveFile("Niemiecki przypadki.txt", "Photos/Niemiecki.txt");
                //var b = api.Revisions("Daniel Polo.txt");
                //var a = api.RestoreFile("Polo Daniel.txt", "4050cb9bbc4");
                //var a = api.CreateFolder("_TESTOWY");
                //var b = api.Delete("Polo Daniel.txt");
                //var b = api.GetFileCopyReference("Nikomp.txt");
                //var a = api.CopyFileByRef("", "Photos/Nikomp2.txt");
                //var a = api.GetFileInfo("Photos/Sample Album/Boston City Flow.jpg");
                //api.GetFileThumbnail("Photos/Sample Album/Boston City Flow.jpg");
                //var a = api.GetFileCopyReference("Niemiecki przypadki.txt");
                //var a = api.GetShareLink("1.avi");
                //var b = api.GetMediaShareLink("1.avi");
                //var a = api.LongpollDelta("AAHGtwovTtYYTTiNC5ip29BG1nlolYCBXInnH4A4-Mh4LnWdHfL2WkNICBqhe0VmMxrirAnJTN6Uk5yh3_t6SyZvcd0lGPxbJ-tUbVC6Z3sBrEWopYvk8NiyjfDLOWuUrGWuTLDWst1ZUBfDwbjQShsJ", 300);
                //if (a.Changes)
                //{
                //    System.Threading.Thread.Sleep(a.Backoff * 1000);
                //    var b = api.Delta(Cursor: "AAGGtR4ko2OHExZ74rqoEuZZ6vrUGnL2fxc-54TKQktuf_A159cARDT9aRFXttlksTnrQpd729qMNISIMHGWCIiHOLHSeoiyZY_LV8ivjd5HYb3Eq6tBVG5z5tSEnV98xrJMkIdSr-gTyHR71IpnON55");
                //}
                //var a = api.Delta(Cursor: "AAGGtR4ko2OHExZ74rqoEuZZ6vrUGnL2fxc-54TKQktuf_A159cARDT9aRFXttlksTnrQpd729qMNISIMHGWCIiHOLHSeoiyZY_LV8ivjd5HYb3Eq6tBVG5z5tSEnV98xrJMkIdSr-gTyHR71IpnON55");
                //var c = api.GetFileInfo("Niemiecki przypadki.txt");
                //var a = api.RestoreFile("Niemiecki przypadki.txt", "3f60cb9bbc4");
                //var b = api.Revisions("Niemiecki przypadki.txt");
                //var b = api.Root;
                //var c = api.GetFolderInfo(b.Elements[1]);
                //var d = api.GetFileInfo(b.Elements[0]);
                //var a = api.GetFileInfo("Antek.docx");
                //var fi = api.UploadFile("D:\\kromk1.jpg", "kromk1.jpg");
                //byte[] file = api.DownloadFile("Docs/polisa.pdf");
                //if (file.Length > 0)
                //{
                //    using (FileStream fs = new FileStream("D:\\file.pdf", FileMode.OpenOrCreate))
                //    {
                //        fs.Write(file, 0, file.Length);
                //    }
                //}


                var t = api.UploadAsync(
                    (i) =>
                    {
                        Console.Clear();
                        Console.WriteLine("Progress: " + i.ToString() + " %");
                    },
                    (i) =>
                    {
                        Console.Clear();
                        Console.WriteLine("Finished with succeed: " + i != null ? "True" : "False");
                    },
                    @"D:\Mein\kolor-360-video-example.mp4",
                    "DOKASACJI.mp4");

                while (!t.IsCompleted)
                {
                    //Console.WriteLine("Waiting.");
                    //System.Threading.Thread.Sleep(1000);
                    //Console.Clear();
                    //Console.WriteLine("Waiting..");
                    //System.Threading.Thread.Sleep(1000);
                    //Console.Clear();
                    //Console.WriteLine("Waiting...");
                    System.Threading.Thread.Sleep(1000);
                }

                //bool b = false;
                //var a = api.MySynchro((i) => { Console.WriteLine("Zrobilem " + i.ToString() + "% "); },
                //    () => { b = true; Console.WriteLine("Skonczylem"); });

                //while (!b)//a.IsCompleted)
                //{
                //    System.Threading.Thread.Sleep(750);
                //    Console.WriteLine("Waiting...");
                //}
                //Console.WriteLine(a.Result); 
            #endregion
            }

            //Console.ReadKey();
        }
    }
}
