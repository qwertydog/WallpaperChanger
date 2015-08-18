using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows.Forms;
using RedditSharp;
using RedditSharp.Things;

namespace WallpaperChanger
{
    public partial class MainForm : Form
    {
        private Reddit reddit;
        private AuthenticatedUser user;

        private RegistryKey rk;

        private Random rand;

        private string currentImage, nextImage;

        // list of subreddits to select from
        private List<string> subredditList;

        // master list of all images
        private List<string> images;

        // list of images already displayed
        private List<string> seenList;


        public MainForm(Reddit reddit)
        {
            InitializeComponent();

            this.reddit = reddit;

            rand = new Random();

            rk = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);

            currentImage = (string)rk.GetValue("Wallpaper");

            //nextImage = GetNextImage();

            // default subreddits
            subredditList = new List<string> {
                "wallpapers",
                "wallpaper",
                "woahdude",
                "interestingasfuck"
            };

            images = new List<string>();

            seenList = new List<string>();


            TimeBox.Items.Add(TimeInMilliseconds.Secs);
            TimeBox.Items.Add(TimeInMilliseconds.Mins);
            TimeBox.Items.Add(TimeInMilliseconds.Hours);

            TimeBox.SelectedItem = TimeInMilliseconds.Mins;

            timer.Interval = (int)TimeBox.SelectedItem * (int)Interval.Value;
            
            rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (rk.GetValue("WallpaperChanger") == null)
            {
                // The value doesn't exist, the application is not set to run at startup
                StartupCheckBox.Checked = false;
            }
            else
            {
                // The value exists, the application is set to run at startup
                StartupCheckBox.Checked = true;
            }

            foreach (var subreddit in subredditList)
            {
                SubsChecklist.Items.Add(subreddit);
            }

            CheckAllItems(true);

            AddEvents(Controls);

            notifyIcon.Icon = Properties.Resources.TrayIcon;

        }

        public MainForm(Reddit reddit, AuthenticatedUser user) : this(reddit)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            this.user = user;

            subredditList = new List<string>();

            SubsChecklist.Items.Clear();

            foreach (var sub in user.SubscribedSubreddits)
            {
                subredditList.Add(sub.Name);
            }

            foreach (var sub in subredditList)
            {
                SubsChecklist.Items.Add(sub);
            }

            CheckAllItems(true);
        }

        private void CheckAllItems(bool value)
        {
            for (int i = 0; i < SubsChecklist.Items.Count; ++i)
            {
                SubsChecklist.SetItemChecked(i, value);

            }
        }

        private void PopulateImages()
        {
            if (RedditDirectoryRadioButton.Checked || DirectoryRadioButton.Checked)
            {
                if (DirectoryTextBox.Text.Length > 0)
                {
                    var path = DirectoryTextBox.Text;

                    var dir = new DirectoryInfo(path);

                    var files = dir.GetFiles("*", SearchOption.AllDirectories);

                    foreach (var file in files)
                    {
                        images.Add(file.FullName);
                    }

                    DirectoryTextBox.Text = images.First();

                }
            }
            if (RedditDirectoryRadioButton.Checked || RedditRadioButton.Checked)
            {
                // do reddit related things



            }
        }

        // TO DO
        private string GetNextImage()
        {
            string temp = images[rand.Next(images.Count())];

            while (seenList.Contains(temp)) 
            {
                temp = images[rand.Next(images.Count())];
            }

            return temp;
        }

        private void AboutMenu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Built by Campbell Harding-Deason\n\nAll rights reserved ©", "About");
        }

        private void SubsChecklist_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if ((e.NewValue == CheckState.Checked) &&
                (SubsChecklist.CheckedItems.Count + 1) == SubsChecklist.Items.Count)
            {
                AllCheckbox.Checked = true;
            } else
            {
                AllCheckbox.Checked = false;
            }

            var subName = SubsChecklist.Items[e.Index].ToString();

            if (e.NewValue == CheckState.Unchecked)
            {
                if (subredditList.Contains(subName))
                {
                    subredditList.Remove(subName);
                }  
            }
            else if (e.NewValue == CheckState.Checked)
            {
                if (!subredditList.Contains(subName)) subredditList.Add(subName);
            }
        }

        private void AllCheckbox_Click(object sender, EventArgs e)
        {
            CheckAllItems(AllCheckbox.Checked);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void StartupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (StartupCheckBox.Checked)
            {
                rk.SetValue("WallpaperChanger", Application.ExecutablePath.ToString());
            }
            else
            {
                rk.DeleteValue("WallpaperChanger", false);
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {

                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(500);
                Hide();
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            
        }

        private void MyHandler(object obj, EventArgs e)
        {
            ApplyButton.Enabled = true;
        }

        private void AddEvents(Control.ControlCollection Controls)
        {
            foreach (Control c in Controls)
            {
                if (c is CheckBox)
                {
                    ((CheckBox)c).CheckedChanged += new EventHandler(MyHandler);
                }
                else if (c is TextBox)
                {
                    ((TextBox)c).TextChanged += new EventHandler(MyHandler);
                }
                else if (c is ComboBox)
                {
                    ((ComboBox)c).SelectedIndexChanged += new EventHandler(MyHandler);
                }
                else if (c is NumericUpDown)
                {
                    (c as NumericUpDown).ValueChanged += new EventHandler(MyHandler);
                }
                else if (c is RadioButton)
                {
                    ((RadioButton)c).CheckedChanged += new EventHandler(MyHandler);
                }
                else if (c is CheckedListBox)
                {
                    ((CheckedListBox)c).ItemCheck += new ItemCheckEventHandler(MyHandler);
                }
            }
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            ApplyButton.Enabled = false;

            timer.Start();

            SetWallpaper();
        }

        private void SetWallpaper()
        {
            while (seenList.Contains(nextImage))
            {
                GetNextImage();
            }

            Wallpaper.Set(nextImage);

            currentImage = nextImage;

            seenList.Add(currentImage);

            GetNextImage();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            SetWallpaper();
        }

        private void AddSubButton_Click(object sender, EventArgs e)
        {
            if (AddSubTextBox.Text.Count() > 0)
            {
                var sub = reddit.GetSubreddit(AddSubTextBox.Text);

                if (sub != null)
                {
                    subredditList.Add(sub.Name);

                    SubsChecklist.Items.Add(sub.Name,true);

                    AddSubTextBox.Text = "";
                }
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                DirectoryTextBox.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
