
namespace WindowsFormsApplication_with_DLL_Integration
{
    partial class FormMain
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
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.labelTextBoxOutput = new System.Windows.Forms.Label();
            this.buttonGo = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.grpSingleOperations = new System.Windows.Forms.GroupBox();
            this.grpMultiOperations = new System.Windows.Forms.GroupBox();
            this.labelTextboxMemory = new System.Windows.Forms.Label();
            this.labelMemory = new System.Windows.Forms.Label();
            this.checkBoxIgnoreRunningState = new System.Windows.Forms.CheckBox();
            this.checkBoxShowTimestamps = new System.Windows.Forms.CheckBox();
            this.checkBoxShowLineNumbers = new System.Windows.Forms.CheckBox();
            this._statusTimer = new System.Windows.Forms.Timer(this.components);
            this.grpSingleOperations.SuspendLayout();
            this.grpMultiOperations.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutput.Location = new System.Drawing.Point(12, 45);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxOutput.Size = new System.Drawing.Size(932, 183);
            this.textBoxOutput.TabIndex = 1;
            // 
            // labelTextBoxOutput
            // 
            this.labelTextBoxOutput.AutoSize = true;
            this.labelTextBoxOutput.Location = new System.Drawing.Point(12, 29);
            this.labelTextBoxOutput.Name = "labelTextBoxOutput";
            this.labelTextBoxOutput.Size = new System.Drawing.Size(402, 13);
            this.labelTextBoxOutput.TabIndex = 0;
            this.labelTextBoxOutput.Text = "A TextBoxban a DLL által generált értékek jelennek meg az új sorban megjelöléssel" +
    ":";
            // 
            // buttonGo
            // 
            this.buttonGo.Location = new System.Drawing.Point(14, 16);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(75, 23);
            this.buttonGo.TabIndex = 0;
            this.buttonGo.Text = "Go";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(95, 16);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 1;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(176, 16);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Mentés";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(807, 21);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelStatus.Size = new System.Drawing.Size(86, 13);
            this.labelStatus.TabIndex = 3;
            this.labelStatus.Text = "Állapot: Leállítva";
            // 
            // grpSingleOperations
            // 
            this.grpSingleOperations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSingleOperations.Controls.Add(this.labelStatus);
            this.grpSingleOperations.Controls.Add(this.buttonSave);
            this.grpSingleOperations.Controls.Add(this.buttonStop);
            this.grpSingleOperations.Controls.Add(this.buttonGo);
            this.grpSingleOperations.Location = new System.Drawing.Point(12, 244);
            this.grpSingleOperations.Name = "grpSingleOperations";
            this.grpSingleOperations.Size = new System.Drawing.Size(932, 45);
            this.grpSingleOperations.TabIndex = 2;
            this.grpSingleOperations.TabStop = false;
            this.grpSingleOperations.Text = "Vezérlőpanel";
            // 
            // grpMultiOperations
            // 
            this.grpMultiOperations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMultiOperations.Controls.Add(this.labelTextboxMemory);
            this.grpMultiOperations.Controls.Add(this.labelMemory);
            this.grpMultiOperations.Controls.Add(this.checkBoxIgnoreRunningState);
            this.grpMultiOperations.Controls.Add(this.checkBoxShowTimestamps);
            this.grpMultiOperations.Controls.Add(this.checkBoxShowLineNumbers);
            this.grpMultiOperations.Location = new System.Drawing.Point(12, 312);
            this.grpMultiOperations.Name = "grpMultiOperations";
            this.grpMultiOperations.Size = new System.Drawing.Size(931, 170);
            this.grpMultiOperations.TabIndex = 3;
            this.grpMultiOperations.TabStop = false;
            this.grpMultiOperations.Text = "Kiegészítő beállítások";
            // 
            // labelTextboxMemory
            // 
            this.labelTextboxMemory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTextboxMemory.AutoSize = true;
            this.labelTextboxMemory.Location = new System.Drawing.Point(737, 43);
            this.labelTextboxMemory.Name = "labelTextboxMemory";
            this.labelTextboxMemory.Size = new System.Drawing.Size(157, 13);
            this.labelTextboxMemory.TabIndex = 8;
            this.labelTextboxMemory.Text = "TextBox tartalom:               0 KB";
            // 
            // labelMemory
            // 
            this.labelMemory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMemory.AutoSize = true;
            this.labelMemory.Location = new System.Drawing.Point(737, 20);
            this.labelMemory.Name = "labelMemory";
            this.labelMemory.Size = new System.Drawing.Size(159, 13);
            this.labelMemory.TabIndex = 7;
            this.labelMemory.Text = "Memóriahasználat:             0 MB";
            // 
            // checkBoxIgnoreRunningState
            // 
            this.checkBoxIgnoreRunningState.AutoSize = true;
            this.checkBoxIgnoreRunningState.Checked = true;
            this.checkBoxIgnoreRunningState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxIgnoreRunningState.Location = new System.Drawing.Point(14, 19);
            this.checkBoxIgnoreRunningState.Name = "checkBoxIgnoreRunningState";
            this.checkBoxIgnoreRunningState.Size = new System.Drawing.Size(421, 17);
            this.checkBoxIgnoreRunningState.TabIndex = 0;
            this.checkBoxIgnoreRunningState.Text = "Go / Stop gombok többszöri meghívása engedélyezett a futási állapottól függetlenü" +
    "l";
            this.checkBoxIgnoreRunningState.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowTimestamps
            // 
            this.checkBoxShowTimestamps.AutoSize = true;
            this.checkBoxShowTimestamps.Location = new System.Drawing.Point(14, 65);
            this.checkBoxShowTimestamps.Name = "checkBoxShowTimestamps";
            this.checkBoxShowTimestamps.Size = new System.Drawing.Size(141, 17);
            this.checkBoxShowTimestamps.TabIndex = 2;
            this.checkBoxShowTimestamps.Text = "Időbélyeg megjelenítése";
            this.checkBoxShowTimestamps.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowLineNumbers
            // 
            this.checkBoxShowLineNumbers.AutoSize = true;
            this.checkBoxShowLineNumbers.Location = new System.Drawing.Point(14, 42);
            this.checkBoxShowLineNumbers.Name = "checkBoxShowLineNumbers";
            this.checkBoxShowLineNumbers.Size = new System.Drawing.Size(135, 17);
            this.checkBoxShowLineNumbers.TabIndex = 1;
            this.checkBoxShowLineNumbers.Text = "Sorszám megjelenítése";
            this.checkBoxShowLineNumbers.UseVisualStyleBackColor = true;
            // 
            // _statusTimer
            // 
            this._statusTimer.Enabled = true;
            this._statusTimer.Tick += new System.EventHandler(this._statusTimer_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 494);
            this.Controls.Add(this.grpMultiOperations);
            this.Controls.Add(this.grpSingleOperations);
            this.Controls.Add(this.labelTextBoxOutput);
            this.Controls.Add(this.textBoxOutput);
            this.Name = "FormMain";
            this.Text = "DLL integrációs WinForms alkalmazás";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.grpSingleOperations.ResumeLayout(false);
            this.grpSingleOperations.PerformLayout();
            this.grpMultiOperations.ResumeLayout(false);
            this.grpMultiOperations.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.Label labelTextBoxOutput;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.GroupBox grpSingleOperations;
        private System.Windows.Forms.GroupBox grpMultiOperations;
        private System.Windows.Forms.CheckBox checkBoxIgnoreRunningState;
        private System.Windows.Forms.CheckBox checkBoxShowTimestamps;
        private System.Windows.Forms.CheckBox checkBoxShowLineNumbers;
        private System.Windows.Forms.Timer _statusTimer;
        private System.Windows.Forms.Label labelMemory;
        private System.Windows.Forms.Label labelTextboxMemory;
    }
}

