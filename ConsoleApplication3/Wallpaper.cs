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

            NativeMethods.SystemParametersInfo(
            	SPISETDESKWALLPAPER,
                0,
                path,
                SPIFUPDATEINIFILE | SPIFSENDWININICHANGE);
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

            System.IO.Stream s = new System.Net.WebClient().OpenRead(uri.ToString());

            System.Drawing.Image img = System.Drawing.Image.FromStream(s);
            string tempPath = Path.Combine(Path.GetTempPath(), "wallpaper.bmp");
            img.Save(tempPath, System.Drawing.Imaging.ImageFormat.Bmp);

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

            NativeMethods.SystemParametersInfo(
            	SPISETDESKWALLPAPER,
                0,
                tempPath,
                SPIFUPDATEINIFILE | SPIFSENDWININICHANGE);

            s.Dispose();
        }
    }
}
