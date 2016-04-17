namespace WallpaperChanger
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Imgur.API;
    using Imgur.API.Authentication.Impl;
    using Imgur.API.Endpoints.Impl;
    using Microsoft.Win32;
    using RedditSharp;
    using RedditSharp.Things;

    public partial class MainForm : Form
    {
        private Reddit reddit;

        private ImgurClient imgur;

        private RegistryKey rk;

        private Random rand;

        private Uri currentImage, nextImage;

        // list of subreddits to select from
        private List<Subreddit> subredditList;

        // list of all subreddits
        private List<Subreddit> subredditMasterList;

        // master list of all images
        private List<Uri> imagePaths;

        public MainForm(Reddit reddit)
        {
            InitializeComponent();

            Icon = Properties.Resources.TrayIcon;
            NotifyIcon1.Icon = Icon;

            FormClosing += (s, args) => NotifyIcon1.Visible = false;

            this.reddit = reddit;

            imgur = new ImgurClient(ImgurDetails.ClientId);

            rand = new Random();

            rk = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);

            currentImage = new Uri((string)rk.GetValue("Wallpaper"));

            imagePaths = new List<Uri>();

            subredditList = new List<Subreddit>();

            subredditMasterList = new List<Subreddit>();

            ////nextImage = GetNextImage();

            ////images = new List<string>();

            ////seenList = new List<string>();

            TimeBox.Items.Add(TimeInMilliseconds.Secs);
            TimeBox.Items.Add(TimeInMilliseconds.Mins);
            TimeBox.Items.Add(TimeInMilliseconds.Hours);

            TimeBox.SelectedItem = TimeInMilliseconds.Secs;

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

            AddEvents(Controls);
        }

        protected async override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (reddit.User != null && reddit.User.SubscribedSubreddits.Any())
            {
                foreach (var sub in reddit.User.SubscribedSubreddits)
                {
                    subredditMasterList.Add(sub);
                    SubsChecklist.Items.Add(sub.DisplayName);
                }
            }
            else
            {
                var tasks = new List<Task<Subreddit>>();

                Console.WriteLine("Adding tasks");

                tasks.Add(reddit.GetSubredditAsync("wallpapers"));
                tasks.Add(reddit.GetSubredditAsync("wallpaper"));
                tasks.Add(reddit.GetSubredditAsync("woahdude"));
                tasks.Add(reddit.GetSubredditAsync("interestingasfuck"));

                Console.WriteLine("Tasks added");

                while (tasks.Any())
                {
                    var finishedTask = await Task.WhenAny(tasks);

                    Console.WriteLine(finishedTask.Result.ToString() + " has completed");

                    tasks.Remove(finishedTask);

                    subredditMasterList.Add(await finishedTask);

                    Console.WriteLine(finishedTask.Result.ToString() + " added to master list");
                }

                foreach (var subreddit in subredditMasterList)
                {
                    SubsChecklist.Items.Add(subreddit.DisplayName);
                }
            }

            CheckItemsAs(true);
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
            imagePaths.Clear();

            if (RedditDirectoryRadioButton.Checked || DirectoryRadioButton.Checked)
            {
                if (DirectoryTextBox.Text.Any())
                {
                    var files = Directory.EnumerateFiles(DirectoryTextBox.Text, "*.*", SearchOption.AllDirectories)
                        .Where(s => s.ToLower().EndsWith(".bmp") || s.ToLower().EndsWith(".jpg") || s.ToLower().EndsWith(".jpeg") || s.ToLower().EndsWith(".png"));

                    foreach (var file in files)
                    {
                        imagePaths.Add(new Uri(file));
                    }
                }
            }
            
            if (RedditDirectoryRadioButton.Checked || RedditRadioButton.Checked)
            {
                foreach (var subreddit in subredditList)
                {
                    imagePaths.AddRange(GetImageURIsFromSubreddit(subreddit));
                }
            }
        }

        private List<Uri> GetImageURIsFromSubreddit(Subreddit subreddit)
        {
            var posts = subreddit.Hot.Take(25).ToList();

            var postUris = new List<Uri>();

            foreach (var post in posts)
            {
                if (post.Url.ToString().Contains("imgur.com"))
                {
                    if (!post.Url.ToString().Split('.').Last().Contains("gif") &&
                        !post.Url.ToString().Contains("imgur.com/a/") &&
                        !post.Url.ToString().Contains("imgur.com/gallery/"))
                    {
                        postUris.Add(post.Url);
                    }
                }
            }

            return postUris;
        }

        private async Task<Uri> GetNextImage()
        {
            while (imagePaths.Any())
            {
                var potentialImage = imagePaths.ElementAt(rand.Next(imagePaths.Count));

                if (potentialImage == currentImage)
                {
                	continue;
                }

                if (potentialImage.IsFile)
                {
                    var image = Image.FromFile(potentialImage.ToString());

                    if (image.Width >= Convert.ToInt32(MinWidthTextBox.Text) && image.Width <= Convert.ToInt32(MaxWidthTextBox.Text))
                    {
                        if (image.Height >= Convert.ToInt32(MinHeightTextBox.Text) && image.Height <= Convert.ToInt32(MaxHeightTextBox.Text))
                        {
                            return potentialImage;
                        }
                    }
                }
                else
                {
                    try
                    {
                        var endpoint = new ImageEndpoint(imgur);
                        var imgurImageDetails = await endpoint.GetImageAsync(potentialImage.ToString().Split('/').Last().Split('.').First());

                        if (imgurImageDetails.Width >= Convert.ToInt32(MinWidthTextBox.Text) && imgurImageDetails.Width <= Convert.ToInt32(MaxWidthTextBox.Text))
                        {
                            if (imgurImageDetails.Height >= Convert.ToInt32(MinHeightTextBox.Text) && imgurImageDetails.Height <= Convert.ToInt32(MaxHeightTextBox.Text))
                            {
                                return potentialImage;
                            }
                        }
                    }
                    catch (ImgurException ex)
                    {
                        Console.WriteLine(ex);
                    }
                }

                imagePaths.Remove(potentialImage); // Image not suitable so remove it
            }

            return currentImage;
        }

        private void AboutMenu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Built by Campbell Harding-Deason\n\nAll rights reserved ©", "About");
        }

        private async void SubsChecklist_ItemCheck(object sender, ItemCheckEventArgs e)
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
                subredditList.RemoveAll(x => x.Name == subName);
            }
            else if (e.NewValue == CheckState.Checked)
            {
                foreach (Subreddit masterSub in subredditMasterList)
                {
                    if (subName == masterSub.DisplayName)
                    {
                        subredditList.Add(masterSub);
                        return;
                    }
                }

                var sub = await reddit.GetSubredditAsync(subName);
                subredditList.Add(sub);
                subredditMasterList.Add(sub);
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

        private async void ApplyButton_Click(object sender, EventArgs e)
        {
            ApplyButton.Enabled = false;

            ////WindowState = FormWindowState.Minimized;

            PopulateImages();

            nextImage = await GetNextImage();

            SetWallpaper();
        }

        private async void SetWallpaper()
        {
        	/*
            while (seenList.Contains(nextImage))
            {
                GetNextImage();
            }*/

            try
            {
                Wallpaper.Set(nextImage);

                textBox1.Text = imagePaths.Count.ToString();

                imagePaths.Remove(currentImage);

                if (imagePaths.Count < 5)
                {
                    PopulateImages();

                    nextImage = await GetNextImage();
                }

                Timer1.Start();

                currentImage = nextImage;

                nextImage = await GetNextImage();
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
                if (subredditList.Exists(x => x.Name == AddSubTextBox.Text))
                {
                    MessageBox.Show("Subreddit already in list!");
                }
                else
                {
                    var sub = await reddit.GetSubredditAsync(AddSubTextBox.Text);

                    if (sub != null)
                    {
                        subredditList.Add(sub);
                        subredditMasterList.Add(sub);
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

        private async void NextWallpaperMenuItem_Click(object sender, EventArgs e)
        {
            if (imagePaths.Count < 5)
            {
                PopulateImages();

                nextImage = await GetNextImage();
            }

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
                subredditList.RemoveAll(x => x.Name == SubsChecklist.SelectedItem.ToString());
                subredditMasterList.RemoveAll(x => x.Name == SubsChecklist.SelectedItem.ToString());
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

        private void AddSubTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                AddSubButton.PerformClick();
            }
        }

        private void OpenWindow()
        {
            Show();
            WindowState = FormWindowState.Normal;
            NotifyIcon1.Visible = false;
        }
    }
}
