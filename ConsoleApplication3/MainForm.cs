using System;
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

        // list of subreddits to select from
        private List<Subreddit> subredditList;


        private List<string> seenList;

        RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        private void CheckAllItems(bool value)
        {
            for (int i = 0; i < SubsChecklist.Items.Count; ++i)
            {
                SubsChecklist.SetItemChecked(i, value);

            }
        }

        private enum Time : int {
            //contains value of selected time period in milliseconds

            Secs = 1000,
            Mins = Secs*60,
            Hours = Mins*60
        }

        public MainForm(Reddit reddit)
        {
            InitializeComponent();

            TimeBox.Items.Add(Time.Secs);
            TimeBox.Items.Add(Time.Mins);
            TimeBox.Items.Add(Time.Hours);

            TimeBox.SelectedItem = Time.Mins;

            this.reddit = reddit;

            timer.Interval = (int)TimeBox.SelectedItem * (int)Interval.Value;
            timer.Start();
            
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

            // default subreddits
            subredditList = new List<Subreddit> {
                reddit.GetSubreddit("wallpapers"),
                reddit.GetSubreddit("wallpaper"),
                reddit.GetSubreddit("gentlemanboners"),
                reddit.GetSubreddit("woahdude"),
                reddit.GetSubreddit("interestingasfuck")
            };

            foreach (var subreddit in subredditList)
            {
                SubsChecklist.Items.Add(subreddit.Name);
            }

            CheckAllItems(true);

            AddEvents(Controls);

            notifyIcon.Icon = Properties.Resources.TrayIcon;

        }


        public MainForm(Reddit reddit, AuthenticatedUser user) : this(reddit)
        {

            this.user = user;

            subredditList = new List<Subreddit>();

            SubsChecklist.Items.Clear();

            foreach (var sub in user.SubscribedSubreddits)
            {
                subredditList.Add(sub);
            }

            foreach (var sub in subredditList)
            {
                SubsChecklist.Items.Add(sub.Name);
            }

            CheckAllItems(true);
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
                foreach (var listSub in subredditList)
                {
                    if (listSub.Name == subName) subredditList.Remove(listSub);
                }
            }
            else if (e.NewValue == CheckState.Checked)
            {
                var sub = subredditList.Find(item => item.Name == subName);

                if (sub == null) subredditList.Add(reddit.GetSubreddit(subName));

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
                    ((NumericUpDown)c).ValueChanged += new EventHandler(MyHandler);
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

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //Wallpaper.Set()
        }

        private void AddSubButton_Click(object sender, EventArgs e)
        {
            if (AddSubTextBox.Text.Count() > 0)
            {
                var sub = reddit.GetSubreddit(AddSubTextBox.Text);

                if (sub != null)
                {
                    subredditList.Add(sub);

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
