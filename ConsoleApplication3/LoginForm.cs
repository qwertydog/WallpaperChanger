using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
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

        private void OpenMainForm(Reddit reddit)
        {
            MainForm Mainform;

            Mainform = new MainForm(reddit);

            Mainform.FormClosing += (s, args) => Close();

            Hide();
            Mainform.Show();
        }

        public LoginForm()
        {
            InitializeComponent();

            LoginButton.Select();

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
            if (UsernameBox.Text.Length == 0 && PasswordBox.Text.Length == 0)
            {
                UsernameBox.Visible = true;
                PasswordBox.Visible = true;
                label1.Visible = true;
                label2.Visible = true;
                CreateAccountButton.Visible = false;
                LoginButton.Text = "OK";
                LoginButton.Enabled = false;
                UsernameBox.Focus();
            }
            else
            {

                Cursor = Cursors.WaitCursor;

                try
                {
                    reddit.LogIn(UsernameBox.Text, PasswordBox.Text);
                    OpenMainForm(reddit);

                }
                catch (AuthenticationException ex)
                {
                    Console.WriteLine(ex);

                    Cursor = Cursors.Default;
                    MessageBox.Show("Please check your details and try again", "Unable To Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CreateAccountButton_Click(object sender, EventArgs e)
        {
            if (UsernameBox.Text.Length == 0 && PasswordBox.Text.Length == 0)
            {
                UsernameBox.Visible = true;
                PasswordBox.Visible = true;
                EmailBox.Visible = true;
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                CreateAccountButton.Text = "OK";
                CreateAccountButton.Enabled = false;
                LoginButton.Visible = false;
                AcceptButton = CreateAccountButton;
                UsernameBox.Focus();
            }
            else
            {
                if (EmailBox.Text.Length != 0)
                {
                    try
                    {
                        var emailAddress = new MailAddress(EmailBox.Text);
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(ex);

                        var dialogResult = MessageBox.Show("Press OK to continue without using an email address or Cancel to edit", "Invalid Email Format", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                        if (dialogResult == DialogResult.OK)
                        {
                            EmailBox.Text = "";

                        } else if (dialogResult == DialogResult.Cancel)
                        {
                            return;
                        }
                    }
                }

                try
                {
                    reddit.RegisterAccount(UsernameBox.Text, PasswordBox.Text, EmailBox.Text);
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine(ex);
                    try
                    {
                        reddit.LogIn(UsernameBox.Text, PasswordBox.Text);
                        OpenMainForm(reddit);
                    }
                    catch (AuthenticationException except)
                    {
                        Console.WriteLine(except);

                        MessageBox.Show("Please try again", "Unable To Register", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void UsernameBox_TextChanged(object sender, EventArgs e)
        {
            TextBoxChanged();
        }

        private void PasswordBox_TextChanged(object sender, EventArgs e)
        {
            TextBoxChanged();
        }

        private void TextBoxChanged()
        {
            if (UsernameBox.Text.Length == 0 || PasswordBox.Text.Length == 0)
            {
                LoginButton.Enabled = false;
                CreateAccountButton.Enabled = false;
            }
            else
            {
                LoginButton.Enabled = true;
                CreateAccountButton.Enabled = true;
            }
        }
    }
}
