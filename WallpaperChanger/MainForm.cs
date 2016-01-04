namespace WallpaperChanger
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using Microsoft.Win32;
	using RedditSharp;
	using RedditSharp.Things;
	
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
        ////private List<string> seenList;

        public MainForm(Reddit reddit)
        {
            InitializeComponent();

            FormClosing += (s, args) => NotifyIcon1.Visible = false;

            this.reddit = reddit;
            
            rand = new Random();

            rk = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);

            currentImage = (string)rk.GetValue("Wallpaper");

            imagePaths = new List<string>();

            ////nextImage = GetNextImage();

            if (reddit.User != null && reddit.User.SubscribedSubreddits.Any())
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
                subredditList = new List<string> 
                {
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

            ////images = new List<string>();

            ////seenList = new List<string>();

            TimeBox.Items.Add(TimeInMilliseconds.Secs);
            TimeBox.Items.Add(TimeInMilliseconds.Mins);
            TimeBox.Items.Add(TimeInMilliseconds.Hours);

            TimeBox.SelectedItem = TimeInMilliseconds.Mins;

            Timer1.Interval = (int)TimeBox.SelectedItem * (int)Interval.Value;
            
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

            CheckItemsAs(true);

            AddEvents(Controls);

            NotifyIcon1.Icon = Properties.Resources.TrayIcon;

            PopulateImages();
        }

        private void CheckItemsAs(bool value)
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
                if (DirectoryTextBox.Text.Any())
                {
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
            while (imagePaths.Any())
            {
                var filePath = imagePaths.ElementAt(rand.Next(imagePaths.Count));

                if (@filePath == currentImage)
                {
                	continue;
                }

                var image = Image.FromFile(@filePath);

                if (image.Width >= Convert.ToInt32(MinWidthTextBox.Text) && image.Width <= Convert.ToInt32(MaxWidthTextBox.Text))
                {
                    if (image.Height >= Convert.ToInt32(MinHeightTextBox.Text) && image.Height <= Convert.ToInt32(MaxHeightTextBox.Text))
                    {
                        return @filePath;
                    }
                }

                imagePaths.Remove(filePath);
            }

            return string.Empty;
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
                AllCheckbox.CheckState = CheckState.Checked;
            } 
            else if ((e.NewValue == CheckState.Unchecked) &&
                (SubsChecklist.CheckedItems.Count - 1) == 0)
            {
                AllCheckbox.CheckState = CheckState.Unchecked;
            }
            else
            {
                AllCheckbox.CheckState = CheckState.Indeterminate;
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
            	if (!subredditList.Contains(subName))
            	{
            		subredditList.Add(subName);
            	}
            }
        }

        private void AllCheckbox_Click(object sender, EventArgs e)
        {
            CheckItemsAs(AllCheckbox.Checked);
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
                NotifyIcon1.Visible = true;
                NotifyIcon1.Text = "Reddit Wallpaper Changer";
                NotifyIcon1.ShowBalloonTip(100);
                Hide();
            }
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenWindow();   
        }

        private void MyHandler(object obj, EventArgs e)
        {
            ApplyButton.Enabled = true;
        }

        private void AddEvents(Control.ControlCollection controls)
        {
            foreach (Control c in controls)
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

            ////WindowState = FormWindowState.Minimized;

            PopulateImages();

            SetWallpaper();
        }

        private void SetWallpaper()
        {
        	/*
            while (seenList.Contains(nextImage))
            {
                GetNextImage();
            }*/

            try
            {
                Wallpaper.Set(nextImage);

                Timer1.Start();

                currentImage = nextImage;

                nextImage = GetNextImage();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("Check that the selected folder and subreddits contain images the correct size", "No matching images found");
            }
            
            ////seenList.Add(currentImage);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            SetWallpaper();
        }

        private async void AddSubButton_Click(object sender, EventArgs e)
        {
            if (AddSubTextBox.Text.Any())
            {
                if (subredditList.Contains(AddSubTextBox.Text))
                {
                    MessageBox.Show("Subreddit already in list!");
                }
                else
                {
                    var sub = await reddit.GetSubredditAsync(AddSubTextBox.Text);

                    if (sub != null)
                    {
                        subredditList.Add(sub.Name);
                        SubsChecklist.Items.Add(sub.Name, true);
                    }
                    else
                    {
                        MessageBox.Show("Subreddit doesn't exist  :(");
                    }
                }
            }
            
            AddSubTextBox.Text = string.Empty;
        }

        private void MinimizedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // TODO
        }

        private void DirectoryTextBox_Leave(object sender, EventArgs e)
        {
            if (DirectoryTextBox.Text.Any() && !Directory.Exists(DirectoryTextBox.Text))
            { 
                MessageBox.Show("Invalid File Path, please try again");
                DirectoryTextBox.Text = string.Empty;
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
            var selectedItem = SubsChecklist.IndexFromPoint(e.Location);
            SubsChecklist.SelectedIndex = selectedItem;
        }

        private void RemoveMenuItem_Click(object sender, EventArgs e)
        {
            if (SubsChecklist.SelectedItems.Count > 0)
            {
                subredditList.Remove(SubsChecklist.SelectedItem.ToString());
                SubsChecklist.Items.Remove(SubsChecklist.SelectedItem);
            }
        }

        private void Interval_ValueChanged(object sender, EventArgs e)
        {
            Timer1.Interval = GetTimerInterval();
        }

        private int GetTimerInterval()
        {
            return Convert.ToInt32(Interval.Value) * Convert.ToInt32(TimeBox.SelectedItem);
        }

        private void TimeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Timer1.Interval = GetTimerInterval();
        }

        private void ValidateSizeTextBoxes(TextBox sender)
        {
            if (sender.Name.StartsWith("Min"))
            {
                if (Convert.ToInt32(MaxWidthTextBox.Text) < Convert.ToInt32(MinWidthTextBox.Text))
                {
                    MaxWidthTextBox.Text = (Convert.ToInt32(MinWidthTextBox.Text) + 1).ToString();
                }
                
                if (Convert.ToInt32(MaxHeightTextBox.Text) < Convert.ToInt32(MinHeightTextBox.Text))
                {
                    MaxHeightTextBox.Text = (Convert.ToInt32(MinHeightTextBox.Text) + 1).ToString();
                }
            }
            else
            {
                if (Convert.ToInt32(MaxWidthTextBox.Text) < Convert.ToInt32(MinWidthTextBox.Text))
                {
                    MinWidthTextBox.Text = (Convert.ToInt32(MaxWidthTextBox.Text) - 1).ToString();
                }
                
                if (Convert.ToInt32(MaxHeightTextBox.Text) < Convert.ToInt32(MinHeightTextBox.Text))
                {
                    MinHeightTextBox.Text = (Convert.ToInt32(MaxHeightTextBox.Text) - 1).ToString();
                }
            }
        }

        private void MinWidthTextBox_Leave(object sender, EventArgs e)
        {
            ValidateSizeTextBoxes((TextBox)sender);
        }

        private void MinHeightTextBox_Leave(object sender, EventArgs e)
        {
            ValidateSizeTextBoxes((TextBox)sender);
        }

        private void MaxWidthTextBox_Leave(object sender, EventArgs e)
        {
            ValidateSizeTextBoxes((TextBox)sender);
        }

        private void MaxHeightTextBox_Leave(object sender, EventArgs e)
        {
            ValidateSizeTextBoxes((TextBox)sender);
        }

        private void OpenWindow()
        {
            Show();
            WindowState = FormWindowState.Normal;
            NotifyIcon1.Visible = false;
        }
    }
}
