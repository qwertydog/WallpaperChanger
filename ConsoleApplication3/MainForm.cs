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

        private RegistryKey rk;

        private Random rand;

        private string currentImage, nextImage;

        // list of subreddits to select from
        private List<string> subredditList;

        // master list of all images
        private List<string> imagePaths;

        // list of images already displayed
        //private List<string> seenList;


        public MainForm(Reddit reddit)
        {
            InitializeComponent();

            FormClosing += (s, args) => notifyIcon.Visible = false;

            this.reddit = reddit;

            rand = new Random();

            rk = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);

            currentImage = (string)rk.GetValue("Wallpaper");

            imagePaths = new List<string>();

            //nextImage = GetNextImage();

            if (reddit.User != null && reddit.User.SubscribedSubreddits.Count() > 0)
            {
                subredditList = new List<string>();

                foreach (var sub in reddit.User.SubscribedSubreddits)
                {
                    subredditList.Add(sub.Name);
                    SubsChecklist.Items.Add(sub.Name);
                }


            }
            else
            {
                // default subreddits
                subredditList = new List<string> {
                    "wallpapers",
                    "wallpaper",
                    "woahdude",
                    "interestingasfuck"
                };

                foreach (var subreddit in subredditList)
                {
                    SubsChecklist.Items.Add(subreddit);
                }
            }            

            //images = new List<string>();

            //seenList = new List<string>();


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

            CheckAllItems(true);

            AddEvents(Controls);

            notifyIcon.Icon = Properties.Resources.TrayIcon;

            PopulateImages();

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

                    var files = Directory.EnumerateFiles(DirectoryTextBox.Text, "*.*", SearchOption.AllDirectories)
                        .Where(s => s.ToLower().EndsWith(".bmp") || s.ToLower().EndsWith(".jpg") || s.ToLower().EndsWith(".jpeg") || s.ToLower().EndsWith(".png"));

                    imagePaths.AddRange(files);

                    nextImage = GetNextImage();
                }
            }
            if (RedditDirectoryRadioButton.Checked || RedditRadioButton.Checked)
            {
                // do reddit related things



            }
        }

        private string GetNextImage()
        {
            while (imagePaths.Count > 0)
            {
                var filePath = imagePaths.ElementAt(rand.Next(imagePaths.Count));

                if (@filePath == currentImage) continue;

                var image = Image.FromFile(@filePath);

                if (image.Width >= Convert.ToInt32(minWidthTextBox.Text) && image.Width <= Convert.ToInt32(maxWidthTextBox.Text))
                {
                    if (image.Height >= Convert.ToInt32(minHeightTextBox.Text) && image.Height <= Convert.ToInt32(maxHeightTextBox.Text))
                    {
                        return @filePath;
                    }
                }

                imagePaths.Remove(filePath);
            }

            return String.Empty;
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
            } 
            else
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
                notifyIcon.Text = "Reddit Wallpaper Changer";
                notifyIcon.ShowBalloonTip(100);
                Hide();
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenWindow();
            
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

            //WindowState = FormWindowState.Minimized;

            PopulateImages();

            SetWallpaper();
        }

        private void SetWallpaper()
        {/*
            while (seenList.Contains(nextImage))
            {
                GetNextImage();
            }*/

            timer.Start();

            Wallpaper.Set(nextImage);

            currentImage = nextImage;

            nextImage = GetNextImage();

            //seenList.Add(currentImage);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            SetWallpaper();
        }

        async private void AddSubButton_Click(object sender, EventArgs e)
        {
            if (AddSubTextBox.Text.Count() > 0 && !subredditList.Contains(AddSubTextBox.Text))
            {
                var sub = await reddit.GetSubredditAsync(AddSubTextBox.Text);

                if (sub != null)
                {
                    subredditList.Add(sub.Name);
                    SubsChecklist.Items.Add(sub.Name,true);
                } else
                {
                    MessageBox.Show("Invalid Subreddit");
                }
            }
            AddSubTextBox.Text = "";
        }

        private void MinimizedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // TODO
        }

        private void DirectoryTextBox_Leave(object sender, EventArgs e)
        {
            if (DirectoryTextBox.Text.Length != 0 && !Directory.Exists(DirectoryTextBox.Text))
            { 
                MessageBox.Show("Invalid File Path, please try again");
                DirectoryTextBox.Text = "";
                DirectoryRadioButton.Enabled = false;
                RedditDirectoryRadioButton.Enabled = false;
                RedditRadioButton.Checked = true;
            }
        }

        private void RestoreMenuItem_Click(object sender, EventArgs e)
        {
            OpenWindow();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                DirectoryTextBox.Text = folderBrowserDialog1.SelectedPath;
                DirectoryRadioButton.Enabled = true;
                RedditDirectoryRadioButton.Enabled = true;
            }
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void NextWallpaperMenuItem_Click(object sender, EventArgs e)
        {
            SetWallpaper();
        }

        private void SubsChecklist_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var selectedItem = SubsChecklist.IndexFromPoint(e.Location);
                SubsChecklist.SelectedIndex = selectedItem;
            }
        }

        private void RemoveMenuItem_Click(object sender, EventArgs e)
        {
            subredditList.Remove(SubsChecklist.SelectedItem.ToString());
            SubsChecklist.Items.Remove(SubsChecklist.SelectedItem);
            
        }

        private void Interval_ValueChanged(object sender, EventArgs e)
        {
            timer.Interval = GetTimerInterval();
        }

        private int GetTimerInterval()
        {
            return Convert.ToInt32(Interval.Value) * Convert.ToInt32(TimeBox.SelectedItem);
        }

        private void TimeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            timer.Interval = GetTimerInterval();
        }

        private void ValidateSizeTextBoxes(TextBox sender)
        {
            if (sender.Name.StartsWith("min"))
            {
                if (Convert.ToInt32(maxWidthTextBox.Text) < Convert.ToInt32(minWidthTextBox.Text))
                {
                    maxWidthTextBox.Text = (Convert.ToInt32(minWidthTextBox.Text) + 1).ToString();
                }
                if (Convert.ToInt32(maxHeightTextBox.Text) < Convert.ToInt32(minHeightTextBox.Text))
                {
                    maxHeightTextBox.Text = (Convert.ToInt32(minHeightTextBox.Text) + 1).ToString();
                }
            }
            else
            {
                if (Convert.ToInt32(maxWidthTextBox.Text) < Convert.ToInt32(minWidthTextBox.Text))
                {
                    minWidthTextBox.Text = (Convert.ToInt32(maxWidthTextBox.Text) - 1).ToString();
                }
                if (Convert.ToInt32(maxHeightTextBox.Text) < Convert.ToInt32(minHeightTextBox.Text))
                {
                    minHeightTextBox.Text = (Convert.ToInt32(maxHeightTextBox.Text) - 1).ToString();
                }
            }
        }

        private void minWidthTextBox_Leave(object sender, EventArgs e)
        {
            ValidateSizeTextBoxes((TextBox)sender);
        }

        private void minHeightTextBox_Leave(object sender, EventArgs e)
        {
            ValidateSizeTextBoxes((TextBox)sender);
        }

        private void maxWidthTextBox_Leave(object sender, EventArgs e)
        {
            ValidateSizeTextBoxes((TextBox)sender);
        }

        private void maxHeightTextBox_Leave(object sender, EventArgs e)
        {
            ValidateSizeTextBoxes((TextBox)sender);
        }

        private void OpenWindow()
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }
    }
}
