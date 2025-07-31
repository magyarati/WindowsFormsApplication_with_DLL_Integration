
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
            this.multiGetIDs = new System.Windows.Forms.NumericUpDown();
            this.labelMultiStatus = new System.Windows.Forms.Label();
            this.buttonMultiStop = new System.Windows.Forms.Button();
            this.buttonMultiGo = new System.Windows.Forms.Button();
            this.buttonGoAnyway = new System.Windows.Forms.Button();
            this.grpSingleOperations.SuspendLayout();
            this.grpMultiOperations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.multiGetIDs)).BeginInit();
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
            this.textBoxOutput.TabIndex = 0;
            // 
            // labelTextBoxOutput
            // 
            this.labelTextBoxOutput.AutoSize = true;
            this.labelTextBoxOutput.Location = new System.Drawing.Point(12, 29);
            this.labelTextBoxOutput.Name = "labelTextBoxOutput";
            this.labelTextBoxOutput.Size = new System.Drawing.Size(402, 13);
            this.labelTextBoxOutput.TabIndex = 1;
            this.labelTextBoxOutput.Text = "A TextBoxban a DLL által generált értékek jelennek meg az új sorban megjelöléssel" +
    ":";
            // 
            // buttonGo
            // 
            this.buttonGo.Location = new System.Drawing.Point(14, 16);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(75, 23);
            this.buttonGo.TabIndex = 2;
            this.buttonGo.Text = "Go";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(95, 16);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 3;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(176, 16);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Mentés";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(820, 21);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelStatus.Size = new System.Drawing.Size(86, 13);
            this.labelStatus.TabIndex = 5;
            this.labelStatus.Text = "Állapot: Leállítva";
            // 
            // grpSingleOperations
            // 
            this.grpSingleOperations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSingleOperations.Controls.Add(this.buttonGoAnyway);
            this.grpSingleOperations.Controls.Add(this.labelStatus);
            this.grpSingleOperations.Controls.Add(this.buttonSave);
            this.grpSingleOperations.Controls.Add(this.buttonStop);
            this.grpSingleOperations.Controls.Add(this.buttonGo);
            this.grpSingleOperations.Location = new System.Drawing.Point(12, 244);
            this.grpSingleOperations.Name = "grpSingleOperations";
            this.grpSingleOperations.Size = new System.Drawing.Size(932, 45);
            this.grpSingleOperations.TabIndex = 6;
            this.grpSingleOperations.TabStop = false;
            this.grpSingleOperations.Text = "Egyszeri műveletek";
            // 
            // grpMultiOperations
            // 
            this.grpMultiOperations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMultiOperations.Controls.Add(this.multiGetIDs);
            this.grpMultiOperations.Controls.Add(this.labelMultiStatus);
            this.grpMultiOperations.Controls.Add(this.buttonMultiStop);
            this.grpMultiOperations.Controls.Add(this.buttonMultiGo);
            this.grpMultiOperations.Location = new System.Drawing.Point(12, 312);
            this.grpMultiOperations.Name = "grpMultiOperations";
            this.grpMultiOperations.Size = new System.Drawing.Size(931, 48);
            this.grpMultiOperations.TabIndex = 7;
            this.grpMultiOperations.TabStop = false;
            this.grpMultiOperations.Text = "Többszörös műveletek";
            // 
            // multiGetIDs
            // 
            this.multiGetIDs.Location = new System.Drawing.Point(282, 19);
            this.multiGetIDs.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.multiGetIDs.Name = "multiGetIDs";
            this.multiGetIDs.Size = new System.Drawing.Size(120, 20);
            this.multiGetIDs.TabIndex = 10;
            this.multiGetIDs.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // labelMultiStatus
            // 
            this.labelMultiStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMultiStatus.AutoSize = true;
            this.labelMultiStatus.Location = new System.Drawing.Point(820, 24);
            this.labelMultiStatus.Name = "labelMultiStatus";
            this.labelMultiStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelMultiStatus.Size = new System.Drawing.Size(87, 13);
            this.labelMultiStatus.TabIndex = 6;
            this.labelMultiStatus.Text = "Állapot: 0 szál fut";
            // 
            // buttonMultiStop
            // 
            this.buttonMultiStop.Location = new System.Drawing.Point(95, 19);
            this.buttonMultiStop.Name = "buttonMultiStop";
            this.buttonMultiStop.Size = new System.Drawing.Size(75, 23);
            this.buttonMultiStop.TabIndex = 9;
            this.buttonMultiStop.Text = "MutiStop";
            this.buttonMultiStop.UseVisualStyleBackColor = true;
            this.buttonMultiStop.Click += new System.EventHandler(this.buttonMultiStop_Click);
            // 
            // buttonMultiGo
            // 
            this.buttonMultiGo.Location = new System.Drawing.Point(14, 19);
            this.buttonMultiGo.Name = "buttonMultiGo";
            this.buttonMultiGo.Size = new System.Drawing.Size(75, 23);
            this.buttonMultiGo.TabIndex = 8;
            this.buttonMultiGo.Text = "MultiGo";
            this.buttonMultiGo.UseVisualStyleBackColor = true;
            this.buttonMultiGo.Click += new System.EventHandler(this.buttonMultiGo_Click);
            // 
            // buttonGoAnyway
            // 
            this.buttonGoAnyway.Location = new System.Drawing.Point(282, 16);
            this.buttonGoAnyway.Name = "buttonGoAnyway";
            this.buttonGoAnyway.Size = new System.Drawing.Size(75, 23);
            this.buttonGoAnyway.TabIndex = 6;
            this.buttonGoAnyway.Text = "Go anyway";
            this.buttonGoAnyway.UseVisualStyleBackColor = true;
            this.buttonGoAnyway.Click += new System.EventHandler(this.buttonGoAnyway_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 441);
            this.Controls.Add(this.grpMultiOperations);
            this.Controls.Add(this.grpSingleOperations);
            this.Controls.Add(this.labelTextBoxOutput);
            this.Controls.Add(this.textBoxOutput);
            this.Name = "FormMain";
            this.Text = "Windows Forms Application projekt DLL fájl referenciával";
            this.grpSingleOperations.ResumeLayout(false);
            this.grpSingleOperations.PerformLayout();
            this.grpMultiOperations.ResumeLayout(false);
            this.grpMultiOperations.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.multiGetIDs)).EndInit();
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
        private System.Windows.Forms.Label labelMultiStatus;
        private System.Windows.Forms.Button buttonMultiStop;
        private System.Windows.Forms.Button buttonMultiGo;
        private System.Windows.Forms.NumericUpDown multiGetIDs;
        private System.Windows.Forms.Button buttonGoAnyway;
    }
}

