namespace BackupFolderFinder
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtRootFolder = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnScan = new System.Windows.Forms.Button();
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.dgvDeleteList = new System.Windows.Forms.DataGridView();
            this.clmn_Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmn_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmn_Folder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDeleteSelected = new System.Windows.Forms.Button();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.clmn_Project = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmn_Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmn_Keep = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmn_Delete = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmn_DeleteCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmn_Protected = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmn_Protectedcount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeleteList)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Backup Folder";
            // 
            // txtRootFolder
            // 
            this.txtRootFolder.Location = new System.Drawing.Point(98, 5);
            this.txtRootFolder.Margin = new System.Windows.Forms.Padding(2);
            this.txtRootFolder.Name = "txtRootFolder";
            this.txtRootFolder.Size = new System.Drawing.Size(451, 20);
            this.txtRootFolder.TabIndex = 1;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(553, 2);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(2);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(56, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(284, 38);
            this.btnScan.Margin = new System.Windows.Forms.Padding(2);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(108, 25);
            this.btnScan.TabIndex = 3;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // dgvResults
            // 
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.AllowUserToDeleteRows = false;
            this.dgvResults.AutoGenerateColumns = false;
            this.dgvResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmn_Project,
            this.clmn_Date,
            this.clmn_Keep,
            this.clmn_Delete,
            this.clmn_DeleteCount,
            this.clmn_Protected,
            this.clmn_Protectedcount});
            this.dgvResults.Location = new System.Drawing.Point(9, 67);
            this.dgvResults.Margin = new System.Windows.Forms.Padding(2);
            this.dgvResults.MultiSelect = false;
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.ReadOnly = true;
            this.dgvResults.RowHeadersVisible = false;
            this.dgvResults.RowHeadersWidth = 51;
            this.dgvResults.RowTemplate.Height = 24;
            this.dgvResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResults.Size = new System.Drawing.Size(1119, 272);
            this.dgvResults.TabIndex = 4;
            this.dgvResults.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResults_CellDoubleClick);
            this.dgvResults.SelectionChanged += new System.EventHandler(this.dgvResults_SelectionChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 508);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1139, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 17);
            this.lblStatus.Text = "Ready";
            // 
            // dgvDeleteList
            // 
            this.dgvDeleteList.AllowUserToAddRows = false;
            this.dgvDeleteList.AllowUserToDeleteRows = false;
            this.dgvDeleteList.AutoGenerateColumns = false;
            this.dgvDeleteList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDeleteList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDeleteList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmn_Time,
            this.clmn_Status,
            this.clmn_Folder});
            this.dgvDeleteList.Location = new System.Drawing.Point(9, 343);
            this.dgvDeleteList.Margin = new System.Windows.Forms.Padding(2);
            this.dgvDeleteList.MultiSelect = false;
            this.dgvDeleteList.Name = "dgvDeleteList";
            this.dgvDeleteList.ReadOnly = true;
            this.dgvDeleteList.RowHeadersVisible = false;
            this.dgvDeleteList.RowHeadersWidth = 51;
            this.dgvDeleteList.RowTemplate.Height = 24;
            this.dgvDeleteList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDeleteList.Size = new System.Drawing.Size(1119, 122);
            this.dgvDeleteList.TabIndex = 6;
            this.dgvDeleteList.SelectionChanged += new System.EventHandler(this.dgvDeleteList_SelectionChanged);
            // 
            // clmn_Time
            // 
            this.clmn_Time.DataPropertyName = "Time";
            this.clmn_Time.FillWeight = 106.9519F;
            this.clmn_Time.HeaderText = "Time";
            this.clmn_Time.MinimumWidth = 6;
            this.clmn_Time.Name = "clmn_Time";
            this.clmn_Time.ReadOnly = true;
            // 
            // clmn_Status
            // 
            this.clmn_Status.DataPropertyName = "Status";
            this.clmn_Status.FillWeight = 80.21391F;
            this.clmn_Status.HeaderText = "Status";
            this.clmn_Status.MinimumWidth = 6;
            this.clmn_Status.Name = "clmn_Status";
            this.clmn_Status.ReadOnly = true;
            // 
            // clmn_Folder
            // 
            this.clmn_Folder.DataPropertyName = "FolderName";
            this.clmn_Folder.FillWeight = 157.3901F;
            this.clmn_Folder.HeaderText = "Folder";
            this.clmn_Folder.MinimumWidth = 6;
            this.clmn_Folder.Name = "clmn_Folder";
            this.clmn_Folder.ReadOnly = true;
            // 
            // btnDeleteSelected
            // 
            this.btnDeleteSelected.Location = new System.Drawing.Point(424, 469);
            this.btnDeleteSelected.Margin = new System.Windows.Forms.Padding(2);
            this.btnDeleteSelected.Name = "btnDeleteSelected";
            this.btnDeleteSelected.Size = new System.Drawing.Size(139, 31);
            this.btnDeleteSelected.TabIndex = 7;
            this.btnDeleteSelected.Text = "Delete Selected";
            this.btnDeleteSelected.UseVisualStyleBackColor = true;
            this.btnDeleteSelected.Click += new System.EventHandler(this.btnDeleteSelected_Click);
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Location = new System.Drawing.Point(567, 469);
            this.btnOpenFolder.Margin = new System.Windows.Forms.Padding(2);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(84, 31);
            this.btnOpenFolder.TabIndex = 8;
            this.btnOpenFolder.Text = "Open Folder";
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // clmn_Project
            // 
            this.clmn_Project.DataPropertyName = "ProjectName";
            this.clmn_Project.HeaderText = "Project";
            this.clmn_Project.MinimumWidth = 6;
            this.clmn_Project.Name = "clmn_Project";
            this.clmn_Project.ReadOnly = true;
            // 
            // clmn_Date
            // 
            this.clmn_Date.DataPropertyName = "BackupDate";
            this.clmn_Date.HeaderText = "Date";
            this.clmn_Date.MinimumWidth = 6;
            this.clmn_Date.Name = "clmn_Date";
            this.clmn_Date.ReadOnly = true;
            // 
            // clmn_Keep
            // 
            this.clmn_Keep.DataPropertyName = "KeepTime";
            this.clmn_Keep.HeaderText = "Keep";
            this.clmn_Keep.MinimumWidth = 6;
            this.clmn_Keep.Name = "clmn_Keep";
            this.clmn_Keep.ReadOnly = true;
            // 
            // clmn_Delete
            // 
            this.clmn_Delete.DataPropertyName = "DeleteTimes";
            this.clmn_Delete.HeaderText = "Delete";
            this.clmn_Delete.MinimumWidth = 6;
            this.clmn_Delete.Name = "clmn_Delete";
            this.clmn_Delete.ReadOnly = true;
            // 
            // clmn_DeleteCount
            // 
            this.clmn_DeleteCount.DataPropertyName = "DeleteCount";
            this.clmn_DeleteCount.HeaderText = "Delete Count";
            this.clmn_DeleteCount.MinimumWidth = 6;
            this.clmn_DeleteCount.Name = "clmn_DeleteCount";
            this.clmn_DeleteCount.ReadOnly = true;
            // 
            // clmn_Protected
            // 
            this.clmn_Protected.DataPropertyName = "ProtectedTimes";
            this.clmn_Protected.HeaderText = "Protected";
            this.clmn_Protected.Name = "clmn_Protected";
            this.clmn_Protected.ReadOnly = true;
            // 
            // clmn_Protectedcount
            // 
            this.clmn_Protectedcount.DataPropertyName = "Protected Count";
            this.clmn_Protectedcount.HeaderText = "Protected Count";
            this.clmn_Protectedcount.Name = "clmn_Protectedcount";
            this.clmn_Protectedcount.ReadOnly = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1139, 530);
            this.Controls.Add(this.btnOpenFolder);
            this.Controls.Add(this.btnDeleteSelected);
            this.Controls.Add(this.dgvDeleteList);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.dgvResults);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtRootFolder);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Backup Folder Finder";
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeleteList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRootFolder;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.DataGridView dgvDeleteList;
        private System.Windows.Forms.Button btnDeleteSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmn_Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmn_Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmn_Folder;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmn_Project;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmn_Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmn_Keep;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmn_Delete;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmn_DeleteCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmn_Protected;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmn_Protectedcount;
    }
}

