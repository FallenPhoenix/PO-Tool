/*
 * Created with SharpDevelop 3.
 * User: F. Phoenix
 * Date: 08.01.2012
 * Time: 10:29
 * 
 */
namespace PO_Tool
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.bSourceBrowse = new System.Windows.Forms.Button();
			this.bUpdateBrowse = new System.Windows.Forms.Button();
			this.bDestBrowse = new System.Windows.Forms.Button();
			this.bStart = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.gUpdate = new System.Windows.Forms.GroupBox();
			this.chUpdPrevIDs = new System.Windows.Forms.CheckBox();
			this.chUpdAltStrings = new System.Windows.Forms.CheckBox();
			this.chUpdComments = new System.Windows.Forms.CheckBox();
			this.chUpdAutoComments = new System.Windows.Forms.CheckBox();
			this.chUpdLinks = new System.Windows.Forms.CheckBox();
			this.chUpdFlags = new System.Windows.Forms.CheckBox();
			this.chUpdStrings = new System.Windows.Forms.CheckBox();
			this.chUpdHeader = new System.Windows.Forms.CheckBox();
			this.gFormat = new System.Windows.Forms.GroupBox();
			this.cbFormatStrings = new System.Windows.Forms.ComboBox();
			this.cbFormatIDs = new System.Windows.Forms.ComboBox();
			this.cbFormatLinks = new System.Windows.Forms.ComboBox();
			this.cbSourceFile = new System.Windows.Forms.ComboBox();
			this.cbUpdateFile = new System.Windows.Forms.ComboBox();
			this.cbDestFile = new System.Windows.Forms.ComboBox();
			this.gRemove = new System.Windows.Forms.GroupBox();
			this.chRemPrevIDs = new System.Windows.Forms.CheckBox();
			this.chRemAltStrings = new System.Windows.Forms.CheckBox();
			this.chRemComments = new System.Windows.Forms.CheckBox();
			this.chRemAutoComments = new System.Windows.Forms.CheckBox();
			this.chRemLinks = new System.Windows.Forms.CheckBox();
			this.chRemFlags = new System.Windows.Forms.CheckBox();
			this.chRemStrings = new System.Windows.Forms.CheckBox();
			this.chRemRegions = new System.Windows.Forms.CheckBox();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.gUpdate.SuspendLayout();
			this.gFormat.SuspendLayout();
			this.gRemove.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(83, 13);
			this.label1.TabIndex = 999;
			this.label1.Text = "Исходный путь";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 60);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(94, 13);
			this.label2.TabIndex = 999;
			this.label2.Text = "Путь обновления";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 115);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(76, 13);
			this.label3.TabIndex = 999;
			this.label3.Text = "Целевой путь";
			// 
			// bSourceBrowse
			// 
			this.bSourceBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bSourceBrowse.Location = new System.Drawing.Point(575, 23);
			this.bSourceBrowse.Name = "bSourceBrowse";
			this.bSourceBrowse.Size = new System.Drawing.Size(27, 23);
			this.bSourceBrowse.TabIndex = 2;
			this.bSourceBrowse.Text = "...";
			this.bSourceBrowse.UseVisualStyleBackColor = true;
			this.bSourceBrowse.Click += new System.EventHandler(this.bBrowse_Click);
			// 
			// bUpdateBrowse
			// 
			this.bUpdateBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bUpdateBrowse.Location = new System.Drawing.Point(575, 74);
			this.bUpdateBrowse.Name = "bUpdateBrowse";
			this.bUpdateBrowse.Size = new System.Drawing.Size(27, 23);
			this.bUpdateBrowse.TabIndex = 4;
			this.bUpdateBrowse.Text = "...";
			this.bUpdateBrowse.UseVisualStyleBackColor = true;
			this.bUpdateBrowse.Click += new System.EventHandler(this.bBrowse_Click);
			// 
			// bDestBrowse
			// 
			this.bDestBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bDestBrowse.Location = new System.Drawing.Point(575, 129);
			this.bDestBrowse.Name = "bDestBrowse";
			this.bDestBrowse.Size = new System.Drawing.Size(27, 23);
			this.bDestBrowse.TabIndex = 6;
			this.bDestBrowse.Text = "...";
			this.bDestBrowse.UseVisualStyleBackColor = true;
			this.bDestBrowse.Click += new System.EventHandler(this.bBrowse_Click);
			// 
			// bStart
			// 
			this.bStart.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.bStart.Location = new System.Drawing.Point(263, 401);
			this.bStart.Name = "bStart";
			this.bStart.Size = new System.Drawing.Size(75, 23);
			this.bStart.TabIndex = 0;
			this.bStart.Text = "Поехали";
			this.bStart.UseVisualStyleBackColor = true;
			this.bStart.Click += new System.EventHandler(this.bStart_Click);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "Все файлы (*.*)|*.*|Файлы Gettext (*.po; *.pot)|*.po;*.pot";
			this.openFileDialog.FilterIndex = 2;
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.Filter = "Все файлы (*.*)|*.*|Файлы Gettext (*.po; *.pot)|*.po;*.pot";
			this.saveFileDialog.FilterIndex = 2;
			// 
			// gUpdate
			// 
			this.gUpdate.Controls.Add(this.chUpdPrevIDs);
			this.gUpdate.Controls.Add(this.chUpdAltStrings);
			this.gUpdate.Controls.Add(this.chUpdComments);
			this.gUpdate.Controls.Add(this.chUpdAutoComments);
			this.gUpdate.Controls.Add(this.chUpdLinks);
			this.gUpdate.Controls.Add(this.chUpdFlags);
			this.gUpdate.Controls.Add(this.chUpdStrings);
			this.gUpdate.Controls.Add(this.chUpdHeader);
			this.gUpdate.Location = new System.Drawing.Point(12, 170);
			this.gUpdate.Name = "gUpdate";
			this.gUpdate.Size = new System.Drawing.Size(165, 206);
			this.gUpdate.TabIndex = 7;
			this.gUpdate.TabStop = false;
			this.gUpdate.Text = "Обновление";
			// 
			// chUpdPrevIDs
			// 
			this.chUpdPrevIDs.AutoSize = true;
			this.chUpdPrevIDs.Location = new System.Drawing.Point(6, 134);
			this.chUpdPrevIDs.Name = "chUpdPrevIDs";
			this.chUpdPrevIDs.Size = new System.Drawing.Size(122, 17);
			this.chUpdPrevIDs.TabIndex = 5;
			this.chUpdPrevIDs.Tag = "PrevIDs";
			this.chUpdPrevIDs.Text = "Старые оригиналы";
			this.chUpdPrevIDs.UseVisualStyleBackColor = true;
			this.chUpdPrevIDs.CheckedChanged += new System.EventHandler(this.chUpdate_CheckedChanged);
			// 
			// chUpdAltStrings
			// 
			this.chUpdAltStrings.AutoSize = true;
			this.chUpdAltStrings.Location = new System.Drawing.Point(6, 65);
			this.chUpdAltStrings.Name = "chUpdAltStrings";
			this.chUpdAltStrings.Size = new System.Drawing.Size(99, 17);
			this.chUpdAltStrings.TabIndex = 2;
			this.chUpdAltStrings.Tag = "AltMsgStrings";
			this.chUpdAltStrings.Text = "Альтернативы";
			this.chUpdAltStrings.UseVisualStyleBackColor = true;
			this.chUpdAltStrings.CheckedChanged += new System.EventHandler(this.chUpdate_CheckedChanged);
			// 
			// chUpdComments
			// 
			this.chUpdComments.AutoSize = true;
			this.chUpdComments.Location = new System.Drawing.Point(6, 88);
			this.chUpdComments.Name = "chUpdComments";
			this.chUpdComments.Size = new System.Drawing.Size(96, 17);
			this.chUpdComments.TabIndex = 3;
			this.chUpdComments.Tag = "Comments";
			this.chUpdComments.Text = "Комментарии";
			this.chUpdComments.UseVisualStyleBackColor = true;
			this.chUpdComments.CheckedChanged += new System.EventHandler(this.chUpdate_CheckedChanged);
			// 
			// chUpdAutoComments
			// 
			this.chUpdAutoComments.AutoSize = true;
			this.chUpdAutoComments.Location = new System.Drawing.Point(6, 111);
			this.chUpdAutoComments.Name = "chUpdAutoComments";
			this.chUpdAutoComments.Size = new System.Drawing.Size(119, 17);
			this.chUpdAutoComments.TabIndex = 4;
			this.chUpdAutoComments.Tag = "AutoComments";
			this.chUpdAutoComments.Text = "Автокомментарии";
			this.chUpdAutoComments.UseVisualStyleBackColor = true;
			this.chUpdAutoComments.CheckedChanged += new System.EventHandler(this.chUpdate_CheckedChanged);
			// 
			// chUpdLinks
			// 
			this.chUpdLinks.AutoSize = true;
			this.chUpdLinks.Location = new System.Drawing.Point(6, 157);
			this.chUpdLinks.Name = "chUpdLinks";
			this.chUpdLinks.Size = new System.Drawing.Size(65, 17);
			this.chUpdLinks.TabIndex = 6;
			this.chUpdLinks.Tag = "Links";
			this.chUpdLinks.Text = "Ссылки";
			this.chUpdLinks.UseVisualStyleBackColor = true;
			this.chUpdLinks.CheckedChanged += new System.EventHandler(this.chUpdate_CheckedChanged);
			// 
			// chUpdFlags
			// 
			this.chUpdFlags.AutoSize = true;
			this.chUpdFlags.Location = new System.Drawing.Point(6, 180);
			this.chUpdFlags.Name = "chUpdFlags";
			this.chUpdFlags.Size = new System.Drawing.Size(60, 17);
			this.chUpdFlags.TabIndex = 7;
			this.chUpdFlags.Tag = "Flags";
			this.chUpdFlags.Text = "Флаги";
			this.chUpdFlags.UseVisualStyleBackColor = true;
			this.chUpdFlags.CheckedChanged += new System.EventHandler(this.chUpdate_CheckedChanged);
			// 
			// chUpdStrings
			// 
			this.chUpdStrings.AutoSize = true;
			this.chUpdStrings.Location = new System.Drawing.Point(6, 42);
			this.chUpdStrings.Name = "chUpdStrings";
			this.chUpdStrings.Size = new System.Drawing.Size(70, 17);
			this.chUpdStrings.TabIndex = 1;
			this.chUpdStrings.Tag = "MsgStrings";
			this.chUpdStrings.Text = "Перевод";
			this.chUpdStrings.UseVisualStyleBackColor = true;
			this.chUpdStrings.CheckedChanged += new System.EventHandler(this.chUpdate_CheckedChanged);
			// 
			// chUpdHeader
			// 
			this.chUpdHeader.AutoSize = true;
			this.chUpdHeader.Location = new System.Drawing.Point(6, 19);
			this.chUpdHeader.Name = "chUpdHeader";
			this.chUpdHeader.Size = new System.Drawing.Size(80, 17);
			this.chUpdHeader.TabIndex = 0;
			this.chUpdHeader.Tag = "Header";
			this.chUpdHeader.Text = "Заголовок";
			this.chUpdHeader.UseVisualStyleBackColor = true;
			this.chUpdHeader.CheckedChanged += new System.EventHandler(this.chUpdate_CheckedChanged);
			// 
			// gFormat
			// 
			this.gFormat.Controls.Add(this.cbFormatStrings);
			this.gFormat.Controls.Add(this.cbFormatIDs);
			this.gFormat.Controls.Add(this.cbFormatLinks);
			this.gFormat.Location = new System.Drawing.Point(344, 170);
			this.gFormat.Name = "gFormat";
			this.gFormat.Size = new System.Drawing.Size(256, 206);
			this.gFormat.TabIndex = 9;
			this.gFormat.TabStop = false;
			this.gFormat.Text = "Форматирование";
			// 
			// cbFormatStrings
			// 
			this.cbFormatStrings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbFormatStrings.FormattingEnabled = true;
			this.cbFormatStrings.Items.AddRange(new object[] {
									"Не трогать строки перевода",
									"Слить строки перевода",
									"Разделить строки перевода по \"\\n\"",
									"Разделить переводы по \"\\n\" (сокращ.)"});
			this.cbFormatStrings.Location = new System.Drawing.Point(6, 92);
			this.cbFormatStrings.Name = "cbFormatStrings";
			this.cbFormatStrings.Size = new System.Drawing.Size(244, 21);
			this.cbFormatStrings.TabIndex = 1;
			this.cbFormatStrings.Tag = "MsgStringsFormat";
			// 
			// cbFormatIDs
			// 
			this.cbFormatIDs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbFormatIDs.FormattingEnabled = true;
			this.cbFormatIDs.Items.AddRange(new object[] {
									"Не трогать оригинальные строки",
									"Слить оригинальные строки",
									"Разделить оригинальные строки по \"\\n\"",
									"Разделить оригиналы по \"\\n\" (сокращ.)"});
			this.cbFormatIDs.Location = new System.Drawing.Point(6, 65);
			this.cbFormatIDs.Name = "cbFormatIDs";
			this.cbFormatIDs.Size = new System.Drawing.Size(244, 21);
			this.cbFormatIDs.TabIndex = 0;
			this.cbFormatIDs.Tag = "MsgIDsFormat";
			// 
			// cbFormatLinks
			// 
			this.cbFormatLinks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbFormatLinks.FormattingEnabled = true;
			this.cbFormatLinks.Items.AddRange(new object[] {
									"Не трогать ссылки на исходник",
									"Слить ссылки на исходник",
									"Разделить ссылки на исходник"});
			this.cbFormatLinks.Location = new System.Drawing.Point(6, 119);
			this.cbFormatLinks.Name = "cbFormatLinks";
			this.cbFormatLinks.Size = new System.Drawing.Size(244, 21);
			this.cbFormatLinks.TabIndex = 2;
			this.cbFormatLinks.Tag = "LinksFormat";
			// 
			// cbSourceFile
			// 
			this.cbSourceFile.AllowDrop = true;
			this.cbSourceFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.cbSourceFile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cbSourceFile.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cbSourceFile.Location = new System.Drawing.Point(12, 25);
			this.cbSourceFile.Name = "cbSourceFile";
			this.cbSourceFile.Size = new System.Drawing.Size(557, 21);
			this.cbSourceFile.Sorted = true;
			this.cbSourceFile.TabIndex = 1;
			this.cbSourceFile.Tag = "";
			this.cbSourceFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.cbFiles_DragDrop);
			this.cbSourceFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.cbFiles_DragEnter);
			// 
			// cbUpdateFile
			// 
			this.cbUpdateFile.AllowDrop = true;
			this.cbUpdateFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.cbUpdateFile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cbUpdateFile.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cbUpdateFile.Location = new System.Drawing.Point(12, 76);
			this.cbUpdateFile.Name = "cbUpdateFile";
			this.cbUpdateFile.Size = new System.Drawing.Size(557, 21);
			this.cbUpdateFile.Sorted = true;
			this.cbUpdateFile.TabIndex = 3;
			this.cbUpdateFile.Tag = "";
			this.cbUpdateFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.cbFiles_DragDrop);
			this.cbUpdateFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.cbFiles_DragEnter);
			this.cbUpdateFile.TextChanged += new System.EventHandler(this.cbUpdateFile_TextChanged);
			// 
			// cbDestFile
			// 
			this.cbDestFile.AllowDrop = true;
			this.cbDestFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.cbDestFile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cbDestFile.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cbDestFile.Location = new System.Drawing.Point(12, 131);
			this.cbDestFile.Name = "cbDestFile";
			this.cbDestFile.Size = new System.Drawing.Size(557, 21);
			this.cbDestFile.Sorted = true;
			this.cbDestFile.TabIndex = 5;
			this.cbDestFile.Tag = "";
			this.cbDestFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.cbFiles_DragDrop);
			this.cbDestFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.cbFiles_DragEnter);
			// 
			// gRemove
			// 
			this.gRemove.Controls.Add(this.chRemPrevIDs);
			this.gRemove.Controls.Add(this.chRemAltStrings);
			this.gRemove.Controls.Add(this.chRemComments);
			this.gRemove.Controls.Add(this.chRemAutoComments);
			this.gRemove.Controls.Add(this.chRemLinks);
			this.gRemove.Controls.Add(this.chRemFlags);
			this.gRemove.Controls.Add(this.chRemStrings);
			this.gRemove.Controls.Add(this.chRemRegions);
			this.gRemove.Location = new System.Drawing.Point(183, 170);
			this.gRemove.Name = "gRemove";
			this.gRemove.Size = new System.Drawing.Size(155, 206);
			this.gRemove.TabIndex = 8;
			this.gRemove.TabStop = false;
			this.gRemove.Text = "Удаление";
			// 
			// chRemPrevIDs
			// 
			this.chRemPrevIDs.AutoSize = true;
			this.chRemPrevIDs.Location = new System.Drawing.Point(6, 134);
			this.chRemPrevIDs.Name = "chRemPrevIDs";
			this.chRemPrevIDs.Size = new System.Drawing.Size(122, 17);
			this.chRemPrevIDs.TabIndex = 5;
			this.chRemPrevIDs.Tag = "PrevIDs";
			this.chRemPrevIDs.Text = "Старые оригиналы";
			this.chRemPrevIDs.UseVisualStyleBackColor = true;
			this.chRemPrevIDs.CheckedChanged += new System.EventHandler(this.chRemove_CheckedChanged);
			// 
			// chRemAltStrings
			// 
			this.chRemAltStrings.AutoSize = true;
			this.chRemAltStrings.Location = new System.Drawing.Point(6, 42);
			this.chRemAltStrings.Name = "chRemAltStrings";
			this.chRemAltStrings.Size = new System.Drawing.Size(99, 17);
			this.chRemAltStrings.TabIndex = 1;
			this.chRemAltStrings.Tag = "AltMsgStrings";
			this.chRemAltStrings.Text = "Альтернативы";
			this.chRemAltStrings.UseVisualStyleBackColor = true;
			this.chRemAltStrings.CheckedChanged += new System.EventHandler(this.chRemove_CheckedChanged);
			// 
			// chRemComments
			// 
			this.chRemComments.AutoSize = true;
			this.chRemComments.Location = new System.Drawing.Point(6, 88);
			this.chRemComments.Name = "chRemComments";
			this.chRemComments.Size = new System.Drawing.Size(96, 17);
			this.chRemComments.TabIndex = 3;
			this.chRemComments.Tag = "Comments";
			this.chRemComments.Text = "Комментарии";
			this.chRemComments.UseVisualStyleBackColor = true;
			this.chRemComments.CheckedChanged += new System.EventHandler(this.chRemove_CheckedChanged);
			// 
			// chRemAutoComments
			// 
			this.chRemAutoComments.AutoSize = true;
			this.chRemAutoComments.Location = new System.Drawing.Point(6, 111);
			this.chRemAutoComments.Name = "chRemAutoComments";
			this.chRemAutoComments.Size = new System.Drawing.Size(119, 17);
			this.chRemAutoComments.TabIndex = 4;
			this.chRemAutoComments.Tag = "AutoComments";
			this.chRemAutoComments.Text = "Автокомментарии";
			this.chRemAutoComments.UseVisualStyleBackColor = true;
			this.chRemAutoComments.CheckedChanged += new System.EventHandler(this.chRemove_CheckedChanged);
			// 
			// chRemLinks
			// 
			this.chRemLinks.AutoSize = true;
			this.chRemLinks.Location = new System.Drawing.Point(6, 157);
			this.chRemLinks.Name = "chRemLinks";
			this.chRemLinks.Size = new System.Drawing.Size(65, 17);
			this.chRemLinks.TabIndex = 6;
			this.chRemLinks.Tag = "Links";
			this.chRemLinks.Text = "Ссылки";
			this.chRemLinks.UseVisualStyleBackColor = true;
			this.chRemLinks.CheckedChanged += new System.EventHandler(this.chRemove_CheckedChanged);
			// 
			// chRemFlags
			// 
			this.chRemFlags.AutoSize = true;
			this.chRemFlags.Location = new System.Drawing.Point(6, 180);
			this.chRemFlags.Name = "chRemFlags";
			this.chRemFlags.Size = new System.Drawing.Size(60, 17);
			this.chRemFlags.TabIndex = 7;
			this.chRemFlags.Tag = "Flags";
			this.chRemFlags.Text = "Флаги";
			this.chRemFlags.UseVisualStyleBackColor = true;
			this.chRemFlags.CheckedChanged += new System.EventHandler(this.chRemove_CheckedChanged);
			// 
			// chRemStrings
			// 
			this.chRemStrings.AutoSize = true;
			this.chRemStrings.Location = new System.Drawing.Point(6, 19);
			this.chRemStrings.Name = "chRemStrings";
			this.chRemStrings.Size = new System.Drawing.Size(70, 17);
			this.chRemStrings.TabIndex = 0;
			this.chRemStrings.Tag = "AltMsgStrings";
			this.chRemStrings.Text = "Перевод";
			this.chRemStrings.UseVisualStyleBackColor = true;
			this.chRemStrings.CheckedChanged += new System.EventHandler(this.chRemove_CheckedChanged);
			// 
			// chRemRegions
			// 
			this.chRemRegions.AutoSize = true;
			this.chRemRegions.Location = new System.Drawing.Point(6, 65);
			this.chRemRegions.Name = "chRemRegions";
			this.chRemRegions.Size = new System.Drawing.Size(71, 17);
			this.chRemRegions.TabIndex = 2;
			this.chRemRegions.Tag = "Regions";
			this.chRemRegions.Text = "Разделы";
			this.chRemRegions.UseVisualStyleBackColor = true;
			this.chRemRegions.CheckedChanged += new System.EventHandler(this.chRemove_CheckedChanged);
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.progressBar.Location = new System.Drawing.Point(12, 411);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(100, 13);
			this.progressBar.TabIndex = 999;
			this.progressBar.Visible = false;
			// 
			// backgroundWorker
			// 
			this.backgroundWorker.WorkerReportsProgress = true;
			this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
			this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
			this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(612, 436);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.gRemove);
			this.Controls.Add(this.cbDestFile);
			this.Controls.Add(this.cbUpdateFile);
			this.Controls.Add(this.cbSourceFile);
			this.Controls.Add(this.gFormat);
			this.Controls.Add(this.gUpdate);
			this.Controls.Add(this.bStart);
			this.Controls.Add(this.bDestBrowse);
			this.Controls.Add(this.bUpdateBrowse);
			this.Controls.Add(this.bSourceBrowse);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(500, 310);
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.Text = "PO Tool";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.gUpdate.ResumeLayout(false);
			this.gUpdate.PerformLayout();
			this.gFormat.ResumeLayout(false);
			this.gRemove.ResumeLayout(false);
			this.gRemove.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.CheckBox chRemPrevIDs;
		private System.Windows.Forms.CheckBox chUpdPrevIDs;
		private System.Windows.Forms.CheckBox chUpdAltStrings;
		private System.Windows.Forms.CheckBox chRemAltStrings;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.ComboBox cbFormatStrings;
		private System.ComponentModel.BackgroundWorker backgroundWorker;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.CheckBox chRemRegions;
		private System.Windows.Forms.CheckBox chRemStrings;
		private System.Windows.Forms.CheckBox chUpdComments;
		private System.Windows.Forms.CheckBox chRemFlags;
		private System.Windows.Forms.CheckBox chRemLinks;
		private System.Windows.Forms.CheckBox chRemAutoComments;
		private System.Windows.Forms.CheckBox chRemComments;
		private System.Windows.Forms.GroupBox gRemove;
		private System.Windows.Forms.GroupBox gFormat;
		private System.Windows.Forms.GroupBox gUpdate;
		private System.Windows.Forms.ComboBox cbDestFile;
		private System.Windows.Forms.ComboBox cbUpdateFile;
		private System.Windows.Forms.ComboBox cbSourceFile;
		private System.Windows.Forms.CheckBox chUpdAutoComments;
		private System.Windows.Forms.CheckBox chUpdLinks;
		private System.Windows.Forms.CheckBox chUpdFlags;
		private System.Windows.Forms.CheckBox chUpdStrings;
		private System.Windows.Forms.CheckBox chUpdHeader;
		private System.Windows.Forms.ComboBox cbFormatLinks;
		private System.Windows.Forms.ComboBox cbFormatIDs;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Button bStart;
		private System.Windows.Forms.Button bDestBrowse;
		private System.Windows.Forms.Button bUpdateBrowse;
		private System.Windows.Forms.Button bSourceBrowse;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
	}
}
