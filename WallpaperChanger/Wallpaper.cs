namespace WallpaperChanger
{
	using System;
	using System.IO;
	using Microsoft.Win32;

    public sealed class Wallpaper
    {
        private const int SPISETDESKWALLPAPER = 20;
        private const int SPIFUPDATEINIFILE = 0x01;
        private const int SPIFSENDWININICHANGE = 0x02;

        private Wallpaper() 
        {
        }

        public enum Style : int
        {
            Tiled,
            Centered,
            Stretched
        }

        public static void Set(string path)
        {
            Set(path, Style.Centered);
        }

        public static void Set(string path, Style style)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            var uri = new Uri(path);

            Set(uri, style);
        }

        public static void Set(Uri uri)
        {
            Set(uri, Style.Centered);
        }

        public static void Set(Uri uri, Style style)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            if (style == Style.Stretched)
            {
                key.SetValue(@"WallpaperStyle", 2.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }

            if (style == Style.Centered)
            {
                key.SetValue(@"WallpaperStyle", 1.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }

            if (style == Style.Tiled)
            {
                key.SetValue(@"WallpaperStyle", 1.ToString());
                key.SetValue(@"TileWallpaper", 1.ToString());
            }

            string path;

            if (uri.IsFile)
            {
                path = uri.ToString();
            }
            else
            {
                using (Stream s = new System.Net.WebClient().OpenRead(uri.ToString()))
                {
                    try
                    {
                        System.Drawing.Image img = System.Drawing.Image.FromStream(s);
                        path = Path.Combine(Path.GetTempPath(), "wallpaper.bmp");
                        img.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                    catch (ArgumentException)
                    {
                        // The imgur image is a gallery or album.... stupid imgur
                        return;
                    }
                }
            }

            NativeMethods.SystemParametersInfo(
            	SPISETDESKWALLPAPER,
                0,
                path,
                SPIFUPDATEINIFILE | SPIFSENDWININICHANGE);            
        }
    }
}
