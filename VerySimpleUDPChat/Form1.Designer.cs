namespace VerySimpleUDPChat
{
	partial class Form1
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.panel1 = new System.Windows.Forms.Panel();
			this.createGroupButton = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.groupNameTextBox = new System.Windows.Forms.TextBox();
			this.inputButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.inputTextBox = new System.Windows.Forms.TextBox();
			this.chatTextBox = new System.Windows.Forms.RichTextBox();
			this.messageTextBox = new System.Windows.Forms.RichTextBox();
			this.sendButton = new System.Windows.Forms.Button();
			this.chatListView = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.label2 = new System.Windows.Forms.Label();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.joinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.leaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panel1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.contextMenuStrip2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.createGroupButton);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.groupNameTextBox);
			this.panel1.Controls.Add(this.inputButton);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.inputTextBox);
			this.panel1.Location = new System.Drawing.Point(12, 12);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(603, 84);
			this.panel1.TabIndex = 0;
			// 
			// createGroupButton
			// 
			this.createGroupButton.Enabled = false;
			this.createGroupButton.Location = new System.Drawing.Point(414, 53);
			this.createGroupButton.Margin = new System.Windows.Forms.Padding(2);
			this.createGroupButton.Name = "createGroupButton";
			this.createGroupButton.Size = new System.Drawing.Size(75, 21);
			this.createGroupButton.TabIndex = 5;
			this.createGroupButton.Text = "Create group";
			this.createGroupButton.UseVisualStyleBackColor = true;
			this.createGroupButton.Visible = false;
			this.createGroupButton.Click += new System.EventHandler(this.createGroupButton_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label3.Location = new System.Drawing.Point(19, 55);
			this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(98, 20);
			this.label3.TabIndex = 4;
			this.label3.Text = "Group name";
			this.label3.Visible = false;
			// 
			// groupNameTextBox
			// 
			this.groupNameTextBox.Enabled = false;
			this.groupNameTextBox.Location = new System.Drawing.Point(122, 55);
			this.groupNameTextBox.Margin = new System.Windows.Forms.Padding(2);
			this.groupNameTextBox.Name = "groupNameTextBox";
			this.groupNameTextBox.Size = new System.Drawing.Size(272, 20);
			this.groupNameTextBox.TabIndex = 3;
			this.groupNameTextBox.Visible = false;
			// 
			// inputButton
			// 
			this.inputButton.Location = new System.Drawing.Point(414, 21);
			this.inputButton.Name = "inputButton";
			this.inputButton.Size = new System.Drawing.Size(75, 23);
			this.inputButton.TabIndex = 2;
			this.inputButton.Text = "Input";
			this.inputButton.UseVisualStyleBackColor = true;
			this.inputButton.Click += new System.EventHandler(this.inputButton_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(27, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(87, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "User name";
			// 
			// inputTextBox
			// 
			this.inputTextBox.Location = new System.Drawing.Point(122, 24);
			this.inputTextBox.Name = "inputTextBox";
			this.inputTextBox.Size = new System.Drawing.Size(272, 20);
			this.inputTextBox.TabIndex = 0;
			// 
			// chatTextBox
			// 
			this.chatTextBox.BackColor = System.Drawing.SystemColors.Control;
			this.chatTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.chatTextBox.Location = new System.Drawing.Point(12, 103);
			this.chatTextBox.Name = "chatTextBox";
			this.chatTextBox.ReadOnly = true;
			this.chatTextBox.Size = new System.Drawing.Size(490, 234);
			this.chatTextBox.TabIndex = 1;
			this.chatTextBox.Text = "";
			// 
			// messageTextBox
			// 
			this.messageTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.messageTextBox.Location = new System.Drawing.Point(13, 344);
			this.messageTextBox.Name = "messageTextBox";
			this.messageTextBox.Size = new System.Drawing.Size(489, 43);
			this.messageTextBox.TabIndex = 2;
			this.messageTextBox.Text = "";
			// 
			// sendButton
			// 
			this.sendButton.Enabled = false;
			this.sendButton.Location = new System.Drawing.Point(508, 344);
			this.sendButton.Name = "sendButton";
			this.sendButton.Size = new System.Drawing.Size(107, 43);
			this.sendButton.TabIndex = 3;
			this.sendButton.Text = "Send";
			this.sendButton.UseVisualStyleBackColor = true;
			this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
			// 
			// chatListView
			// 
			this.chatListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
			this.chatListView.HideSelection = false;
			this.chatListView.Location = new System.Drawing.Point(508, 125);
			this.chatListView.Margin = new System.Windows.Forms.Padding(2);
			this.chatListView.MultiSelect = false;
			this.chatListView.Name = "chatListView";
			this.chatListView.Size = new System.Drawing.Size(107, 212);
			this.chatListView.TabIndex = 5;
			this.chatListView.UseCompatibleStateImageBehavior = false;
			this.chatListView.View = System.Windows.Forms.View.List;
			this.chatListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Width = 100;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label2.Location = new System.Drawing.Point(511, 99);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(103, 25);
			this.label2.TabIndex = 6;
			this.label2.Text = "Chat\'s List";
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.joinToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(96, 26);
			// 
			// joinToolStripMenuItem
			// 
			this.joinToolStripMenuItem.Name = "joinToolStripMenuItem";
			this.joinToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
			this.joinToolStripMenuItem.Text = "Join";
			this.joinToolStripMenuItem.Click += new System.EventHandler(this.joinToolStripMenuItem_Click);
			// 
			// contextMenuStrip2
			// 
			this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.leaveToolStripMenuItem});
			this.contextMenuStrip2.Name = "contextMenuStrip2";
			this.contextMenuStrip2.Size = new System.Drawing.Size(105, 26);
			// 
			// leaveToolStripMenuItem
			// 
			this.leaveToolStripMenuItem.Name = "leaveToolStripMenuItem";
			this.leaveToolStripMenuItem.Size = new System.Drawing.Size(104, 22);
			this.leaveToolStripMenuItem.Text = "Leave";
			this.leaveToolStripMenuItem.Click += new System.EventHandler(this.leaveToolStripMenuItem_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(627, 399);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.chatListView);
			this.Controls.Add(this.sendButton);
			this.Controls.Add(this.messageTextBox);
			this.Controls.Add(this.chatTextBox);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "Form1";
			this.Text = "Chat";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.contextMenuStrip1.ResumeLayout(false);
			this.contextMenuStrip2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox inputTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button inputButton;
		private System.Windows.Forms.RichTextBox chatTextBox;
		private System.Windows.Forms.RichTextBox messageTextBox;
		private System.Windows.Forms.Button sendButton;
		private System.Windows.Forms.ListView chatListView;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem joinToolStripMenuItem;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
		private System.Windows.Forms.ToolStripMenuItem leaveToolStripMenuItem;
		private System.Windows.Forms.Button createGroupButton;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox groupNameTextBox;
	}
}

