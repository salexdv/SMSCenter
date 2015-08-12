/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 05.05.2014
 * Time: 12:19
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace SMSCenter
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.UnsentMessages = new System.Windows.Forms.DataGridView();
			this.gbUnsentMsg = new System.Windows.Forms.GroupBox();
			this.Log = new System.Windows.Forms.RichTextBox();
			this.gbLog = new System.Windows.Forms.GroupBox();
			this.btStartHTTPService = new System.Windows.Forms.Button();
			this.btStopHTTPService = new System.Windows.Forms.Button();
			this.btStartSMPPService = new System.Windows.Forms.Button();
			this.btStopSMPPService = new System.Windows.Forms.Button();
			this.gbHTTPService = new System.Windows.Forms.GroupBox();
			this.gbSMPPService = new System.Windows.Forms.GroupBox();
			this.tcMain = new System.Windows.Forms.TabControl();
			this.tpMain = new System.Windows.Forms.TabPage();
			this.lbCountUnsentMessages = new System.Windows.Forms.Label();
			this.lbCountInfo = new System.Windows.Forms.Label();
			this.tpMessages = new System.Windows.Forms.TabPage();
			this.btFilter = new System.Windows.Forms.Button();
			this.cbStatus = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.dtpDateEnd = new System.Windows.Forms.DateTimePicker();
			this.dtpDateBegin = new System.Windows.Forms.DateTimePicker();
			this.AllMessages = new System.Windows.Forms.DataGridView();
			this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.UnsentMessages)).BeginInit();
			this.gbUnsentMsg.SuspendLayout();
			this.gbLog.SuspendLayout();
			this.gbHTTPService.SuspendLayout();
			this.gbSMPPService.SuspendLayout();
			this.tcMain.SuspendLayout();
			this.tpMain.SuspendLayout();
			this.tpMessages.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.AllMessages)).BeginInit();
			this.trayMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// UnsentMessages
			// 
			this.UnsentMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.UnsentMessages.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.UnsentMessages.BackgroundColor = System.Drawing.SystemColors.ActiveBorder;
			this.UnsentMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.UnsentMessages.GridColor = System.Drawing.SystemColors.ControlDarkDark;
			this.UnsentMessages.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.UnsentMessages.Location = new System.Drawing.Point(7, 15);
			this.UnsentMessages.MultiSelect = false;
			this.UnsentMessages.Name = "UnsentMessages";
			this.UnsentMessages.RowHeadersWidth = 20;
			this.UnsentMessages.Size = new System.Drawing.Size(725, 151);
			this.UnsentMessages.TabIndex = 0;
			this.UnsentMessages.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.UnsentMessagesCellFormatting);
			// 
			// gbUnsentMsg
			// 
			this.gbUnsentMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.gbUnsentMsg.Controls.Add(this.UnsentMessages);
			this.gbUnsentMsg.Location = new System.Drawing.Point(0, 217);
			this.gbUnsentMsg.Name = "gbUnsentMsg";
			this.gbUnsentMsg.Size = new System.Drawing.Size(737, 172);
			this.gbUnsentMsg.TabIndex = 1;
			this.gbUnsentMsg.TabStop = false;
			this.gbUnsentMsg.Text = "Неотправленные сообщения";
			// 
			// Log
			// 
			this.Log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.Log.BackColor = System.Drawing.SystemColors.WindowText;
			this.Log.ForeColor = System.Drawing.Color.LimeGreen;
			this.Log.Location = new System.Drawing.Point(7, 15);
			this.Log.Name = "Log";
			this.Log.ReadOnly = true;
			this.Log.Size = new System.Drawing.Size(725, 135);
			this.Log.TabIndex = 3;
			this.Log.Text = "";
			// 
			// gbLog
			// 
			this.gbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.gbLog.Controls.Add(this.Log);
			this.gbLog.Location = new System.Drawing.Point(0, 60);
			this.gbLog.Name = "gbLog";
			this.gbLog.Size = new System.Drawing.Size(737, 155);
			this.gbLog.TabIndex = 4;
			this.gbLog.TabStop = false;
			this.gbLog.Text = "Журнал событий";
			// 
			// btStartHTTPService
			// 
			this.btStartHTTPService.Image = global::SMSCenter.Properties.Resources.httpStart;
			this.btStartHTTPService.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btStartHTTPService.Location = new System.Drawing.Point(7, 19);
			this.btStartHTTPService.Name = "btStartHTTPService";
			this.btStartHTTPService.Size = new System.Drawing.Size(90, 27);
			this.btStartHTTPService.TabIndex = 5;
			this.btStartHTTPService.Text = "Запустить";
			this.btStartHTTPService.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btStartHTTPService.UseVisualStyleBackColor = true;
			this.btStartHTTPService.Click += new System.EventHandler(this.BtStartHTTPServiceClick);
			// 
			// btStopHTTPService
			// 
			this.btStopHTTPService.Image = global::SMSCenter.Properties.Resources.stopService;
			this.btStopHTTPService.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btStopHTTPService.Location = new System.Drawing.Point(102, 19);
			this.btStopHTTPService.Name = "btStopHTTPService";
			this.btStopHTTPService.Size = new System.Drawing.Size(90, 27);
			this.btStopHTTPService.TabIndex = 6;
			this.btStopHTTPService.Text = "Остановить";
			this.btStopHTTPService.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btStopHTTPService.UseVisualStyleBackColor = true;
			this.btStopHTTPService.Click += new System.EventHandler(this.BtStopHTTPServiceClick);
			// 
			// btStartSMPPService
			// 
			this.btStartSMPPService.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btStartSMPPService.Image = global::SMSCenter.Properties.Resources.smppStart;
			this.btStartSMPPService.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btStartSMPPService.Location = new System.Drawing.Point(7, 19);
			this.btStartSMPPService.Name = "btStartSMPPService";
			this.btStartSMPPService.Size = new System.Drawing.Size(90, 27);
			this.btStartSMPPService.TabIndex = 7;
			this.btStartSMPPService.Text = "Запустить";
			this.btStartSMPPService.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btStartSMPPService.UseVisualStyleBackColor = true;
			this.btStartSMPPService.Click += new System.EventHandler(this.BtStartSMPPServiceClick);
			// 
			// btStopSMPPService
			// 
			this.btStopSMPPService.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btStopSMPPService.Image = global::SMSCenter.Properties.Resources.stopService;
			this.btStopSMPPService.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btStopSMPPService.Location = new System.Drawing.Point(102, 19);
			this.btStopSMPPService.Name = "btStopSMPPService";
			this.btStopSMPPService.Size = new System.Drawing.Size(90, 27);
			this.btStopSMPPService.TabIndex = 8;
			this.btStopSMPPService.Text = "Остановить";
			this.btStopSMPPService.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btStopSMPPService.UseVisualStyleBackColor = true;
			this.btStopSMPPService.Click += new System.EventHandler(this.BtStopSMPPServiceClick);
			// 
			// gbHTTPService
			// 
			this.gbHTTPService.Controls.Add(this.btStopHTTPService);
			this.gbHTTPService.Controls.Add(this.btStartHTTPService);
			this.gbHTTPService.Location = new System.Drawing.Point(0, 5);
			this.gbHTTPService.Name = "gbHTTPService";
			this.gbHTTPService.Size = new System.Drawing.Size(200, 54);
			this.gbHTTPService.TabIndex = 9;
			this.gbHTTPService.TabStop = false;
			this.gbHTTPService.Text = "Служба приема сообщений";
			// 
			// gbSMPPService
			// 
			this.gbSMPPService.Controls.Add(this.btStopSMPPService);
			this.gbSMPPService.Controls.Add(this.btStartSMPPService);
			this.gbSMPPService.Location = new System.Drawing.Point(207, 5);
			this.gbSMPPService.Name = "gbSMPPService";
			this.gbSMPPService.Size = new System.Drawing.Size(200, 54);
			this.gbSMPPService.TabIndex = 10;
			this.gbSMPPService.TabStop = false;
			this.gbSMPPService.Text = "Служба отправки сообщений";
			// 
			// tcMain
			// 
			this.tcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.tcMain.Controls.Add(this.tpMain);
			this.tcMain.Controls.Add(this.tpMessages);
			this.tcMain.Location = new System.Drawing.Point(-1, 1);
			this.tcMain.Name = "tcMain";
			this.tcMain.SelectedIndex = 0;
			this.tcMain.Size = new System.Drawing.Size(746, 419);
			this.tcMain.TabIndex = 11;
			// 
			// tpMain
			// 
			this.tpMain.Controls.Add(this.lbCountUnsentMessages);
			this.tpMain.Controls.Add(this.lbCountInfo);
			this.tpMain.Controls.Add(this.gbHTTPService);
			this.tpMain.Controls.Add(this.gbUnsentMsg);
			this.tpMain.Controls.Add(this.gbLog);
			this.tpMain.Controls.Add(this.gbSMPPService);
			this.tpMain.Location = new System.Drawing.Point(4, 22);
			this.tpMain.Name = "tpMain";
			this.tpMain.Padding = new System.Windows.Forms.Padding(3);
			this.tpMain.Size = new System.Drawing.Size(738, 393);
			this.tpMain.TabIndex = 0;
			this.tpMain.Text = "Основное";
			this.tpMain.UseVisualStyleBackColor = true;
			// 
			// lbCountUnsentMessages
			// 
			this.lbCountUnsentMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lbCountUnsentMessages.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lbCountUnsentMessages.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.lbCountUnsentMessages.Location = new System.Drawing.Point(513, 25);
			this.lbCountUnsentMessages.Name = "lbCountUnsentMessages";
			this.lbCountUnsentMessages.Size = new System.Drawing.Size(218, 32);
			this.lbCountUnsentMessages.TabIndex = 12;
			this.lbCountUnsentMessages.Text = "0";
			this.lbCountUnsentMessages.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbCountUnsentMessages.Click += new System.EventHandler(this.LbCountUnsentMessagesClick);
			// 
			// lbCountInfo
			// 
			this.lbCountInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lbCountInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lbCountInfo.Location = new System.Drawing.Point(509, 5);
			this.lbCountInfo.Name = "lbCountInfo";
			this.lbCountInfo.Size = new System.Drawing.Size(226, 21);
			this.lbCountInfo.TabIndex = 11;
			this.lbCountInfo.Text = "Неотправленных сообщений:";
			// 
			// tpMessages
			// 
			this.tpMessages.Controls.Add(this.btFilter);
			this.tpMessages.Controls.Add(this.cbStatus);
			this.tpMessages.Controls.Add(this.label3);
			this.tpMessages.Controls.Add(this.label2);
			this.tpMessages.Controls.Add(this.label1);
			this.tpMessages.Controls.Add(this.dtpDateEnd);
			this.tpMessages.Controls.Add(this.dtpDateBegin);
			this.tpMessages.Controls.Add(this.AllMessages);
			this.tpMessages.Location = new System.Drawing.Point(4, 22);
			this.tpMessages.Name = "tpMessages";
			this.tpMessages.Padding = new System.Windows.Forms.Padding(3);
			this.tpMessages.Size = new System.Drawing.Size(738, 393);
			this.tpMessages.TabIndex = 1;
			this.tpMessages.Text = "Сообщения";
			this.tpMessages.UseVisualStyleBackColor = true;
			// 
			// btFilter
			// 
			this.btFilter.Image = global::SMSCenter.Properties.Resources.applyFilter;
			this.btFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btFilter.Location = new System.Drawing.Point(537, 5);
			this.btFilter.Name = "btFilter";
			this.btFilter.Size = new System.Drawing.Size(100, 23);
			this.btFilter.TabIndex = 7;
			this.btFilter.Text = "Фильтровать";
			this.btFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btFilter.UseVisualStyleBackColor = true;
			this.btFilter.Click += new System.EventHandler(this.BtFilterClick);
			// 
			// cbStatus
			// 
			this.cbStatus.FormattingEnabled = true;
			this.cbStatus.Items.AddRange(new object[] {
									"Любой",
									"Отправленные",
									"Не отправленные"});
			this.cbStatus.Location = new System.Drawing.Point(405, 6);
			this.cbStatus.Name = "cbStatus";
			this.cbStatus.Size = new System.Drawing.Size(126, 21);
			this.cbStatus.TabIndex = 6;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(358, 10);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(41, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Статус";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(198, 10);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(19, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "по";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(54, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Период с";
			// 
			// dtpDateEnd
			// 
			this.dtpDateEnd.Location = new System.Drawing.Point(223, 6);
			this.dtpDateEnd.Name = "dtpDateEnd";
			this.dtpDateEnd.Size = new System.Drawing.Size(129, 20);
			this.dtpDateEnd.TabIndex = 2;
			// 
			// dtpDateBegin
			// 
			this.dtpDateBegin.Location = new System.Drawing.Point(63, 6);
			this.dtpDateBegin.Name = "dtpDateBegin";
			this.dtpDateBegin.Size = new System.Drawing.Size(129, 20);
			this.dtpDateBegin.TabIndex = 1;
			// 
			// AllMessages
			// 
			this.AllMessages.AllowUserToAddRows = false;
			this.AllMessages.AllowUserToDeleteRows = false;
			this.AllMessages.AllowUserToOrderColumns = true;
			this.AllMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.AllMessages.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.AllMessages.BackgroundColor = System.Drawing.SystemColors.ActiveBorder;
			this.AllMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.AllMessages.Location = new System.Drawing.Point(0, 31);
			this.AllMessages.MultiSelect = false;
			this.AllMessages.Name = "AllMessages";
			this.AllMessages.ReadOnly = true;
			this.AllMessages.RowHeadersWidth = 20;
			this.AllMessages.Size = new System.Drawing.Size(738, 362);
			this.AllMessages.TabIndex = 0;
			this.AllMessages.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.AllMessagesCellFormatting);
			this.AllMessages.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.AllMessagesCellPainting);
			// 
			// trayIcon
			// 
			this.trayIcon.ContextMenuStrip = this.trayMenu;
			this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
			this.trayIcon.Text = "SMS Center";
			this.trayIcon.Visible = true;
			// 
			// trayMenu
			// 
			this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.OpenToolStripMenuItem,
									this.CloseToolStripMenuItem});
			this.trayMenu.Name = "trayMenu";
			this.trayMenu.Size = new System.Drawing.Size(132, 48);
			// 
			// OpenToolStripMenuItem
			// 
			this.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
			this.OpenToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
			this.OpenToolStripMenuItem.Text = "Открыть";
			this.OpenToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItemClick);
			// 
			// CloseToolStripMenuItem
			// 
			this.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem";
			this.CloseToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
			this.CloseToolStripMenuItem.Text = "Закрыть";
			this.CloseToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItemClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(742, 416);
			this.Controls.Add(this.tcMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.Text = "SMS Center";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFormFormClosed);
			((System.ComponentModel.ISupportInitialize)(this.UnsentMessages)).EndInit();
			this.gbUnsentMsg.ResumeLayout(false);
			this.gbLog.ResumeLayout(false);
			this.gbHTTPService.ResumeLayout(false);
			this.gbSMPPService.ResumeLayout(false);
			this.tcMain.ResumeLayout(false);
			this.tpMain.ResumeLayout(false);
			this.tpMessages.ResumeLayout(false);
			this.tpMessages.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.AllMessages)).EndInit();
			this.trayMenu.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.ToolStripMenuItem CloseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem OpenToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip trayMenu;
		private System.Windows.Forms.NotifyIcon trayIcon;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbStatus;
		private System.Windows.Forms.Button btFilter;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.DateTimePicker dtpDateBegin;
		private System.Windows.Forms.DateTimePicker dtpDateEnd;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lbCountInfo;
		private System.Windows.Forms.Label lbCountUnsentMessages;
		private System.Windows.Forms.DataGridView AllMessages;
		private System.Windows.Forms.TabPage tpMessages;
		private System.Windows.Forms.TabPage tpMain;
		private System.Windows.Forms.TabControl tcMain;
		private System.Windows.Forms.GroupBox gbSMPPService;
		private System.Windows.Forms.GroupBox gbHTTPService;
		private System.Windows.Forms.Button btStopSMPPService;
		private System.Windows.Forms.Button btStartSMPPService;
		private System.Windows.Forms.Button btStopHTTPService;
		private System.Windows.Forms.Button btStartHTTPService;
		private System.Windows.Forms.GroupBox gbLog;
		private System.Windows.Forms.RichTextBox Log;
		private System.Windows.Forms.GroupBox gbUnsentMsg;
		private System.Windows.Forms.DataGridView UnsentMessages;
	}
}
