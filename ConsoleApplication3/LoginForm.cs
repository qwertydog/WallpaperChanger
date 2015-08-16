using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Authentication;
using RedditSharp;
using RedditSharp.Things;

namespace WallpaperChanger
{
    public partial class LoginForm : Form
    {
        private Reddit reddit;

        private void OpenMainForm(Reddit reddit, AuthenticatedUser user = null)
        {
            MainForm Mainform;

            if (user != null)
            {
                Mainform = new MainForm(reddit, user);
            } else
            {
                Mainform = new MainForm(reddit);
            }
                        
            Mainform.FormClosing += (s, args) => Close();

            Hide();
            Mainform.Show();
        }

        public LoginForm()
        {
            InitializeComponent();

            reddit = new Reddit();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SkipButton_Click(object sender, EventArgs e)
        {
            OpenMainForm(reddit);
            
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                var user = reddit.LogIn(UsernameBox.Text, PasswordBox.Text);
                OpenMainForm(reddit, user);

            } catch
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Please check your details and try again", "Unable To Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

            

        }

        private void CreateAccountButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://www.reddit.com/login");
        
        }

        private void UsernameBox_TextChanged(object sender, EventArgs e)
        {
            if (UsernameBox.Text.Length == 0 || PasswordBox.Text.Length == 0)
            {
                LoginButton.Enabled = false;
            }
            else
            {
                LoginButton.Enabled = true;
            }
        }

        private void PasswordBox_TextChanged(object sender, EventArgs e)
        {
            if (UsernameBox.Text.Length == 0 || PasswordBox.Text.Length == 0)
            {
                LoginButton.Enabled = false;
            }
            else
            {
                LoginButton.Enabled = true;
            }
        }
    }
}
