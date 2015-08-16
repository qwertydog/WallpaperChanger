using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WallpaperChanger
{
    public class Link
    {

        public string Url { get; set; }
        public string ID;
        public Img image;

        public void RetrieveImgurImage()
        {

            string imgurUrl = "https://api.imgur.com/3/image/" + ID;

            try
            {
                WebRequest wrURL = WebRequest.Create(imgurUrl);

                wrURL.Headers.Add(ImgurDetails.authenticationHeader);
                wrURL.Method = "GET";

                Stream stream = wrURL.GetResponse().GetResponseStream();

                StreamReader reader = new StreamReader(stream);

                string reply = reader.ReadToEnd();

                JObject lnk = JObject.Parse(reply);

                JToken result = lnk["data"];

                image = JsonConvert.DeserializeObject<Img>(result.ToString());

                Uri uri = new Uri(image.link);

                Wallpaper.Set(uri);
                /*//Console.ReadLine();

                //string tempfile = Path.GetTempFileName();

                //Console.WriteLine(tempfile);
                //Console.ReadLine();

                //string localname = Path.ChangeExtension(Path.GetTempFileName(), Path.GetExtension(url.Url));

                //Console.WriteLine(localname);

                //Image Dummy = Image.FromFile();

                //string tempPath = Path.Combine(Path.GetTempPath(), "wallpaper.bmp");

                //Dummy.Save(tempPath, System.Drawing.Imaging.ImageFormat.Bmp);

                //Console.WriteLine(uri.LocalPath);

                //using (WebClient webClient = new WebClient())
                //{
                //    webClient.DownloadFile(img.link, localname);
                //}

                //img.Print();
                */
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        return;
                    }
                }
            }


        }

        public void RetrieveImage()
        {

            Uri uri = new Uri(Url);

            Wallpaper.Set(uri);

        }

        public void Print()
        {
            Console.WriteLine("{0}", Url);
        }

    }
}
