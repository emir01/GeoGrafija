namespace AdministrationPanel
{
    partial class Form1
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.comboLocationType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tboxLocationsParentId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCreateData = new System.Windows.Forms.Button();
            this.btnProcessInput = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.gridLocations = new System.Windows.Forms.DataGridView();
            this.btnOpenFileDialog = new System.Windows.Forms.Button();
            this.tboxFileBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbox_Output = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label6 = new System.Windows.Forms.Label();
            this.comboUsers = new System.Windows.Forms.ComboBox();
            this.comboDSetting = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLocations)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tbox_Output);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(1180, 546);
            this.splitContainer1.SplitterDistance = 552;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.comboDSetting);
            this.splitContainer2.Panel1.Controls.Add(this.label7);
            this.splitContainer2.Panel1.Controls.Add(this.comboUsers);
            this.splitContainer2.Panel1.Controls.Add(this.label6);
            this.splitContainer2.Panel1.Controls.Add(this.comboLocationType);
            this.splitContainer2.Panel1.Controls.Add(this.label4);
            this.splitContainer2.Panel1.Controls.Add(this.tboxLocationsParentId);
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            this.splitContainer2.Panel1.Controls.Add(this.btnCreateData);
            this.splitContainer2.Panel1.Controls.Add(this.btnProcessInput);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.label5);
            this.splitContainer2.Panel2.Controls.Add(this.gridLocations);
            this.splitContainer2.Panel2.Controls.Add(this.btnOpenFileDialog);
            this.splitContainer2.Panel2.Controls.Add(this.tboxFileBox);
            this.splitContainer2.Panel2.Controls.Add(this.label2);
            this.splitContainer2.Size = new System.Drawing.Size(552, 546);
            this.splitContainer2.SplitterDistance = 79;
            this.splitContainer2.TabIndex = 0;
            // 
            // comboLocationType
            // 
            this.comboLocationType.FormattingEnabled = true;
            this.comboLocationType.Location = new System.Drawing.Point(236, 9);
            this.comboLocationType.Name = "comboLocationType";
            this.comboLocationType.Size = new System.Drawing.Size(121, 21);
            this.comboLocationType.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(136, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Location Parent Id";
            // 
            // tboxLocationsParentId
            // 
            this.tboxLocationsParentId.Location = new System.Drawing.Point(236, 41);
            this.tboxLocationsParentId.Name = "tboxLocationsParentId";
            this.tboxLocationsParentId.Size = new System.Drawing.Size(121, 20);
            this.tboxLocationsParentId.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(136, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Location Type";
            // 
            // btnCreateData
            // 
            this.btnCreateData.Location = new System.Drawing.Point(12, 41);
            this.btnCreateData.Name = "btnCreateData";
            this.btnCreateData.Size = new System.Drawing.Size(107, 23);
            this.btnCreateData.TabIndex = 1;
            this.btnCreateData.Text = "Create Data";
            this.btnCreateData.UseVisualStyleBackColor = true;
            this.btnCreateData.Click += new System.EventHandler(this.btnCreateData_Click);
            // 
            // btnProcessInput
            // 
            this.btnProcessInput.Location = new System.Drawing.Point(12, 12);
            this.btnProcessInput.Name = "btnProcessInput";
            this.btnProcessInput.Size = new System.Drawing.Size(107, 23);
            this.btnProcessInput.TabIndex = 0;
            this.btnProcessInput.Text = "Process Input";
            this.btnProcessInput.UseVisualStyleBackColor = true;
            this.btnProcessInput.Click += new System.EventHandler(this.btnProcessInput_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Locations From Output";
            // 
            // gridLocations
            // 
            this.gridLocations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLocations.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridLocations.Location = new System.Drawing.Point(0, 102);
            this.gridLocations.Name = "gridLocations";
            this.gridLocations.Size = new System.Drawing.Size(552, 361);
            this.gridLocations.TabIndex = 9;
            this.gridLocations.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridLocations_CellEndEdit);
            // 
            // btnOpenFileDialog
            // 
            this.btnOpenFileDialog.Location = new System.Drawing.Point(257, 38);
            this.btnOpenFileDialog.Name = "btnOpenFileDialog";
            this.btnOpenFileDialog.Size = new System.Drawing.Size(157, 23);
            this.btnOpenFileDialog.TabIndex = 8;
            this.btnOpenFileDialog.Text = "Open File to Process";
            this.btnOpenFileDialog.UseVisualStyleBackColor = true;
            this.btnOpenFileDialog.Click += new System.EventHandler(this.btnOpenFileDialog_Click);
            // 
            // tboxFileBox
            // 
            this.tboxFileBox.Location = new System.Drawing.Point(12, 41);
            this.tboxFileBox.Name = "tboxFileBox";
            this.tboxFileBox.ReadOnly = true;
            this.tboxFileBox.Size = new System.Drawing.Size(219, 20);
            this.tboxFileBox.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Raw Input From Wolfram :";
            // 
            // tbox_Output
            // 
            this.tbox_Output.AcceptsReturn = true;
            this.tbox_Output.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbox_Output.Location = new System.Drawing.Point(0, 28);
            this.tbox_Output.Multiline = true;
            this.tbox_Output.Name = "tbox_Output";
            this.tbox_Output.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbox_Output.Size = new System.Drawing.Size(624, 518);
            this.tbox_Output.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Processed Output :";
            // 
            // fileDialog
            // 
            this.fileDialog.FileName = "openFileDialog1";
            this.fileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.fileDialog_FileOk);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(364, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "User :";
            // 
            // comboUsers
            // 
            this.comboUsers.FormattingEnabled = true;
            this.comboUsers.Location = new System.Drawing.Point(405, 9);
            this.comboUsers.Name = "comboUsers";
            this.comboUsers.Size = new System.Drawing.Size(134, 21);
            this.comboUsers.TabIndex = 9;
            // 
            // comboDSetting
            // 
            this.comboDSetting.FormattingEnabled = true;
            this.comboDSetting.Location = new System.Drawing.Point(405, 42);
            this.comboDSetting.Name = "comboDSetting";
            this.comboDSetting.Size = new System.Drawing.Size(134, 21);
            this.comboDSetting.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(364, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "User :";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 546);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Locations Processor - Wolfram Data";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridLocations)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbox_Output;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tboxLocationsParentId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCreateData;
        private System.Windows.Forms.Button btnProcessInput;
        private System.Windows.Forms.ComboBox comboLocationType;
        private System.Windows.Forms.OpenFileDialog fileDialog;
        private System.Windows.Forms.Button btnOpenFileDialog;
        private System.Windows.Forms.TextBox tboxFileBox;
        private System.Windows.Forms.DataGridView gridLocations;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboUsers;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboDSetting;
        private System.Windows.Forms.Label label7;
    }
}

