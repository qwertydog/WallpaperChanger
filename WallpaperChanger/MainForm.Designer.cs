namespace WallpaperChanger
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SubsChecklist = new System.Windows.Forms.CheckedListBox();
            this.SubsChecklistMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.RemoveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SubsLabel = new System.Windows.Forms.Label();
            this.Interval = new System.Windows.Forms.NumericUpDown();
            this.IntervalLabel = new System.Windows.Forms.Label();
            this.MinWidthTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.MaxWidthTextBox = new System.Windows.Forms.TextBox();
            this.MaxHeightTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.MinHeightTextBox = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.AboutMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.AllCheckbox = new System.Windows.Forms.CheckBox();
            this.MinLabel = new System.Windows.Forms.Label();
            this.MaxLabel = new System.Windows.Forms.Label();
            this.TimeBox = new System.Windows.Forms.ComboBox();
            this.StartupCheckBox = new System.Windows.Forms.CheckBox();
            this.RedditRadioButton = new System.Windows.Forms.RadioButton();
            this.DirectoryRadioButton = new System.Windows.Forms.RadioButton();
            this.RedditDirectoryRadioButton = new System.Windows.Forms.RadioButton();
            this.DirectoryTextBox = new System.Windows.Forms.TextBox();
            this.DirectoryLabel = new System.Windows.Forms.Label();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.NotifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.NotificationMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.RestoreMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NextWallpaperMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.MinimizedCheckBox = new System.Windows.Forms.CheckBox();
            this.AddSubTextBox = new System.Windows.Forms.TextBox();
            this.AddSubButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SubsChecklistMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Interval)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.NotificationMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // SubsChecklist
            // 
            this.SubsChecklist.CheckOnClick = true;
            this.SubsChecklist.ContextMenuStrip = this.SubsChecklistMenuStrip;
            this.SubsChecklist.FormattingEnabled = true;
            this.SubsChecklist.HorizontalScrollbar = true;
            this.SubsChecklist.Location = new System.Drawing.Point(10, 50);
            this.SubsChecklist.Name = "SubsChecklist";
            this.SubsChecklist.Size = new System.Drawing.Size(189, 199);
            this.SubsChecklist.Sorted = true;
            this.SubsChecklist.TabIndex = 0;
            this.SubsChecklist.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.SubsChecklist_ItemCheck);
            this.SubsChecklist.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SubsChecklist_MouseDown);
            // 
            // SubsChecklistMenuStrip
            // 
            this.SubsChecklistMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RemoveMenuItem});
            this.SubsChecklistMenuStrip.Name = "SubsChecklistMenuStrip";
            this.SubsChecklistMenuStrip.Size = new System.Drawing.Size(118, 26);
            // 
            // RemoveMenuItem
            // 
            this.RemoveMenuItem.Name = "RemoveMenuItem";
            this.RemoveMenuItem.Size = new System.Drawing.Size(117, 22);
            this.RemoveMenuItem.Text = "Remove";
            this.RemoveMenuItem.Click += new System.EventHandler(this.RemoveMenuItem_Click);
            // 
            // SubsLabel
            // 
            this.SubsLabel.AutoSize = true;
            this.SubsLabel.Location = new System.Drawing.Point(33, 35);
            this.SubsLabel.Name = "SubsLabel";
            this.SubsLabel.Size = new System.Drawing.Size(166, 13);
            this.SubsLabel.TabIndex = 1;
            this.SubsLabel.Text = "Subreddits - Right click to remove";
            // 
            // Interval
            // 
            this.Interval.Location = new System.Drawing.Point(331, 31);
            this.Interval.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.Interval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Interval.Name = "Interval";
            this.Interval.Size = new System.Drawing.Size(36, 20);
            this.Interval.TabIndex = 2;
            this.Interval.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.Interval.ValueChanged += new System.EventHandler(this.Interval_ValueChanged);
            // 
            // IntervalLabel
            // 
            this.IntervalLabel.AutoSize = true;
            this.IntervalLabel.Location = new System.Drawing.Point(205, 33);
            this.IntervalLabel.Name = "IntervalLabel";
            this.IntervalLabel.Size = new System.Drawing.Size(123, 13);
            this.IntervalLabel.TabIndex = 3;
            this.IntervalLabel.Text = "Time until next wallpaper";
            // 
            // MinWidthTextBox
            // 
            this.MinWidthTextBox.Location = new System.Drawing.Point(205, 70);
            this.MinWidthTextBox.Name = "MinWidthTextBox";
            this.MinWidthTextBox.Size = new System.Drawing.Size(56, 20);
            this.MinWidthTextBox.TabIndex = 6;
            this.MinWidthTextBox.Text = "1920";
            this.MinWidthTextBox.Leave += new System.EventHandler(this.MinWidthTextBox_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(277, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Horizontal Size";
            // 
            // MaxWidthTextBox
            // 
            this.MaxWidthTextBox.Location = new System.Drawing.Point(368, 70);
            this.MaxWidthTextBox.Name = "MaxWidthTextBox";
            this.MaxWidthTextBox.Size = new System.Drawing.Size(56, 20);
            this.MaxWidthTextBox.TabIndex = 8;
            this.MaxWidthTextBox.Text = "1920";
            this.MaxWidthTextBox.Leave += new System.EventHandler(this.MaxWidthTextBox_Leave);
            // 
            // MaxHeightTextBox
            // 
            this.MaxHeightTextBox.Location = new System.Drawing.Point(369, 96);
            this.MaxHeightTextBox.Name = "MaxHeightTextBox";
            this.MaxHeightTextBox.Size = new System.Drawing.Size(56, 20);
            this.MaxHeightTextBox.TabIndex = 11;
            this.MaxHeightTextBox.Text = "1080";
            this.MaxHeightTextBox.Leave += new System.EventHandler(this.MaxHeightTextBox_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(283, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Vertical Size";
            // 
            // MinHeightTextBox
            // 
            this.MinHeightTextBox.Location = new System.Drawing.Point(205, 96);
            this.MinHeightTextBox.Name = "MinHeightTextBox";
            this.MinHeightTextBox.Size = new System.Drawing.Size(56, 20);
            this.MinHeightTextBox.TabIndex = 9;
            this.MinHeightTextBox.Text = "1080";
            this.MinHeightTextBox.Leave += new System.EventHandler(this.MinHeightTextBox_Leave);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(438, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // AboutMenu
            // 
            this.AboutMenu.Name = "AboutMenu";
            this.AboutMenu.Size = new System.Drawing.Size(52, 20);
            this.AboutMenu.Text = "About";
            this.AboutMenu.Click += new System.EventHandler(this.AboutMenu_Click);
            // 
            // AllCheckbox
            // 
            this.AllCheckbox.AutoSize = true;
            this.AllCheckbox.Checked = true;
            this.AllCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AllCheckbox.Location = new System.Drawing.Point(13, 34);
            this.AllCheckbox.Name = "AllCheckbox";
            this.AllCheckbox.Size = new System.Drawing.Size(15, 14);
            this.AllCheckbox.TabIndex = 13;
            this.AllCheckbox.UseVisualStyleBackColor = true;
            this.AllCheckbox.Click += new System.EventHandler(this.AllCheckbox_Click);
            // 
            // MinLabel
            // 
            this.MinLabel.AutoSize = true;
            this.MinLabel.Location = new System.Drawing.Point(221, 54);
            this.MinLabel.Name = "MinLabel";
            this.MinLabel.Size = new System.Drawing.Size(24, 13);
            this.MinLabel.TabIndex = 14;
            this.MinLabel.Text = "Min";
            // 
            // MaxLabel
            // 
            this.MaxLabel.AutoSize = true;
            this.MaxLabel.Location = new System.Drawing.Point(383, 54);
            this.MaxLabel.Name = "MaxLabel";
            this.MaxLabel.Size = new System.Drawing.Size(27, 13);
            this.MaxLabel.TabIndex = 15;
            this.MaxLabel.Text = "Max";
            // 
            // TimeBox
            // 
            this.TimeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TimeBox.FormattingEnabled = true;
            this.TimeBox.Location = new System.Drawing.Point(373, 31);
            this.TimeBox.MaxDropDownItems = 3;
            this.TimeBox.Name = "TimeBox";
            this.TimeBox.Size = new System.Drawing.Size(53, 21);
            this.TimeBox.TabIndex = 16;
            this.TimeBox.SelectedIndexChanged += new System.EventHandler(this.TimeBox_SelectedIndexChanged);
            // 
            // StartupCheckBox
            // 
            this.StartupCheckBox.AutoSize = true;
            this.StartupCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.StartupCheckBox.Checked = true;
            this.StartupCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.StartupCheckBox.Location = new System.Drawing.Point(334, 123);
            this.StartupCheckBox.Name = "StartupCheckBox";
            this.StartupCheckBox.Size = new System.Drawing.Size(93, 17);
            this.StartupCheckBox.TabIndex = 17;
            this.StartupCheckBox.Text = "Run at startup";
            this.StartupCheckBox.UseVisualStyleBackColor = true;
            this.StartupCheckBox.CheckedChanged += new System.EventHandler(this.StartupCheckBox_CheckedChanged);
            // 
            // RedditRadioButton
            // 
            this.RedditRadioButton.AutoSize = true;
            this.RedditRadioButton.Checked = true;
            this.RedditRadioButton.Location = new System.Drawing.Point(205, 122);
            this.RedditRadioButton.Name = "RedditRadioButton";
            this.RedditRadioButton.Size = new System.Drawing.Size(80, 17);
            this.RedditRadioButton.TabIndex = 18;
            this.RedditRadioButton.TabStop = true;
            this.RedditRadioButton.Text = "Reddit Only";
            this.RedditRadioButton.UseVisualStyleBackColor = true;
            // 
            // DirectoryRadioButton
            // 
            this.DirectoryRadioButton.AutoSize = true;
            this.DirectoryRadioButton.Enabled = false;
            this.DirectoryRadioButton.Location = new System.Drawing.Point(205, 146);
            this.DirectoryRadioButton.Name = "DirectoryRadioButton";
            this.DirectoryRadioButton.Size = new System.Drawing.Size(130, 17);
            this.DirectoryRadioButton.TabIndex = 19;
            this.DirectoryRadioButton.TabStop = true;
            this.DirectoryRadioButton.Text = "Chosen Directory Only";
            this.DirectoryRadioButton.UseVisualStyleBackColor = true;
            // 
            // RedditDirectoryRadioButton
            // 
            this.RedditDirectoryRadioButton.AutoSize = true;
            this.RedditDirectoryRadioButton.Enabled = false;
            this.RedditDirectoryRadioButton.Location = new System.Drawing.Point(204, 170);
            this.RedditDirectoryRadioButton.Name = "RedditDirectoryRadioButton";
            this.RedditDirectoryRadioButton.Size = new System.Drawing.Size(149, 17);
            this.RedditDirectoryRadioButton.TabIndex = 20;
            this.RedditDirectoryRadioButton.TabStop = true;
            this.RedditDirectoryRadioButton.Text = "Reddit + Chosen Directory";
            this.RedditDirectoryRadioButton.UseVisualStyleBackColor = true;
            // 
            // DirectoryTextBox
            // 
            this.DirectoryTextBox.Location = new System.Drawing.Point(203, 215);
            this.DirectoryTextBox.Name = "DirectoryTextBox";
            this.DirectoryTextBox.Size = new System.Drawing.Size(192, 20);
            this.DirectoryTextBox.TabIndex = 21;
            this.DirectoryTextBox.Leave += new System.EventHandler(this.DirectoryTextBox_Leave);
            // 
            // DirectoryLabel
            // 
            this.DirectoryLabel.AutoSize = true;
            this.DirectoryLabel.Location = new System.Drawing.Point(203, 199);
            this.DirectoryLabel.Name = "DirectoryLabel";
            this.DirectoryLabel.Size = new System.Drawing.Size(49, 13);
            this.DirectoryLabel.TabIndex = 22;
            this.DirectoryLabel.Text = "Directory";
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(400, 214);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(25, 21);
            this.BrowseButton.TabIndex = 23;
            this.BrowseButton.Text = "...";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // ApplyButton
            // 
            this.ApplyButton.Location = new System.Drawing.Point(238, 255);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(75, 23);
            this.ApplyButton.TabIndex = 24;
            this.ApplyButton.Text = "Apply";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ExitButton.Location = new System.Drawing.Point(320, 255);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(75, 23);
            this.ExitButton.TabIndex = 25;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // NotifyIcon1
            // 
            this.NotifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.NotifyIcon1.BalloonTipText = "Double-Click To Open";
            this.NotifyIcon1.ContextMenuStrip = this.NotificationMenuStrip;
            this.NotifyIcon1.Text = "notifyIcon1";
            this.NotifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
            // 
            // NotificationMenuStrip
            // 
            this.NotificationMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RestoreMenuItem,
            this.NextWallpaperMenuItem,
            this.toolStripSeparator1,
            this.ExitMenuItem});
            this.NotificationMenuStrip.Name = "NotificationMenuStrip";
            this.NotificationMenuStrip.Size = new System.Drawing.Size(155, 76);
            // 
            // RestoreMenuItem
            // 
            this.RestoreMenuItem.Name = "RestoreMenuItem";
            this.RestoreMenuItem.Size = new System.Drawing.Size(154, 22);
            this.RestoreMenuItem.Text = "Restore";
            this.RestoreMenuItem.Click += new System.EventHandler(this.RestoreMenuItem_Click);
            // 
            // NextWallpaperMenuItem
            // 
            this.NextWallpaperMenuItem.Name = "NextWallpaperMenuItem";
            this.NextWallpaperMenuItem.Size = new System.Drawing.Size(154, 22);
            this.NextWallpaperMenuItem.Text = "Next Wallpaper";
            this.NextWallpaperMenuItem.Click += new System.EventHandler(this.NextWallpaperMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(151, 6);
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Size = new System.Drawing.Size(154, 22);
            this.ExitMenuItem.Text = "Exit";
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // Timer1
            // 
            this.Timer1.Interval = 1000;
            this.Timer1.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // MinimizedCheckBox
            // 
            this.MinimizedCheckBox.AutoSize = true;
            this.MinimizedCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.MinimizedCheckBox.Location = new System.Drawing.Point(348, 146);
            this.MinimizedCheckBox.Name = "MinimizedCheckBox";
            this.MinimizedCheckBox.Size = new System.Drawing.Size(79, 17);
            this.MinimizedCheckBox.TabIndex = 26;
            this.MinimizedCheckBox.Text = "Start in tray";
            this.MinimizedCheckBox.UseVisualStyleBackColor = true;
            this.MinimizedCheckBox.Visible = false;
            this.MinimizedCheckBox.CheckedChanged += new System.EventHandler(this.MinimizedCheckBox_CheckedChanged);
            // 
            // AddSubTextBox
            // 
            this.AddSubTextBox.Location = new System.Drawing.Point(10, 257);
            this.AddSubTextBox.Name = "AddSubTextBox";
            this.AddSubTextBox.Size = new System.Drawing.Size(127, 20);
            this.AddSubTextBox.TabIndex = 27;
            // 
            // AddSubButton
            // 
            this.AddSubButton.Location = new System.Drawing.Point(143, 255);
            this.AddSubButton.Name = "AddSubButton";
            this.AddSubButton.Size = new System.Drawing.Size(56, 23);
            this.AddSubButton.TabIndex = 28;
            this.AddSubButton.Text = "Add Sub";
            this.AddSubButton.UseVisualStyleBackColor = true;
            this.AddSubButton.Click += new System.EventHandler(this.AddSubButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(386, 179);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(40, 20);
            this.textBox1.TabIndex = 29;
            // 
            // MainForm
            // 
            this.CancelButton = this.ExitButton;
            this.ClientSize = new System.Drawing.Size(438, 291);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.AddSubButton);
            this.Controls.Add(this.AddSubTextBox);
            this.Controls.Add(this.MinimizedCheckBox);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.ApplyButton);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.DirectoryLabel);
            this.Controls.Add(this.DirectoryTextBox);
            this.Controls.Add(this.RedditDirectoryRadioButton);
            this.Controls.Add(this.DirectoryRadioButton);
            this.Controls.Add(this.RedditRadioButton);
            this.Controls.Add(this.StartupCheckBox);
            this.Controls.Add(this.TimeBox);
            this.Controls.Add(this.MaxLabel);
            this.Controls.Add(this.MinLabel);
            this.Controls.Add(this.AllCheckbox);
            this.Controls.Add(this.MaxHeightTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MinHeightTextBox);
            this.Controls.Add(this.MaxWidthTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MinWidthTextBox);
            this.Controls.Add(this.IntervalLabel);
            this.Controls.Add(this.Interval);
            this.Controls.Add(this.SubsLabel);
            this.Controls.Add(this.SubsChecklist);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reddit Wallpaper Changer";
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.SubsChecklistMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Interval)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.NotificationMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox SubsChecklist;
        private System.Windows.Forms.Label SubsLabel;
        private System.Windows.Forms.NumericUpDown Interval;
        private System.Windows.Forms.Label IntervalLabel;
        private System.Windows.Forms.TextBox MinWidthTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox MaxWidthTextBox;
        private System.Windows.Forms.TextBox MaxHeightTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox MinHeightTextBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem AboutMenu;
        private System.Windows.Forms.CheckBox AllCheckbox;
        private System.Windows.Forms.Label MinLabel;
        private System.Windows.Forms.Label MaxLabel;
        private System.Windows.Forms.ComboBox TimeBox;
        private System.Windows.Forms.CheckBox StartupCheckBox;
        private System.Windows.Forms.RadioButton RedditRadioButton;
        private System.Windows.Forms.RadioButton DirectoryRadioButton;
        private System.Windows.Forms.RadioButton RedditDirectoryRadioButton;
        private System.Windows.Forms.TextBox DirectoryTextBox;
        private System.Windows.Forms.Label DirectoryLabel;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Timer Timer1;
        private System.Windows.Forms.CheckBox MinimizedCheckBox;
        private System.Windows.Forms.TextBox AddSubTextBox;
        private System.Windows.Forms.Button AddSubButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ContextMenuStrip NotificationMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem RestoreMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NextWallpaperMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
        private System.Windows.Forms.ContextMenuStrip SubsChecklistMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem RemoveMenuItem;
        private System.Windows.Forms.NotifyIcon NotifyIcon1;
        private System.Windows.Forms.TextBox textBox1;
    }
}