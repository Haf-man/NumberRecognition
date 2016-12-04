namespace InterfaceForCV
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
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.OpenButton = new System.Windows.Forms.Button();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.clearButton = new System.Windows.Forms.Button();
      this.recognizeButton = new System.Windows.Forms.Button();
      this.outputLabel = new System.Windows.Forms.Label();
      this.eventLog1 = new System.Diagnostics.EventLog();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).BeginInit();
      this.SuspendLayout();
      // 
      // pictureBox1
      // 
      this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
      this.pictureBox1.Location = new System.Drawing.Point(8, 8);
      this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(711, 347);
      this.pictureBox1.TabIndex = 1;
      this.pictureBox1.TabStop = false;
      this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
      this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
      this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
      // 
      // OpenButton
      // 
      this.OpenButton.Location = new System.Drawing.Point(632, 359);
      this.OpenButton.Margin = new System.Windows.Forms.Padding(2);
      this.OpenButton.Name = "OpenButton";
      this.OpenButton.Size = new System.Drawing.Size(87, 23);
      this.OpenButton.TabIndex = 2;
      this.OpenButton.Text = "Open";
      this.OpenButton.UseVisualStyleBackColor = true;
      this.OpenButton.Visible = false;
      this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
      // 
      // openFileDialog1
      // 
      this.openFileDialog1.FileName = "openFileDialog";
      this.openFileDialog1.Filter = "PNG|*.png";
      this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
      // 
      // clearButton
      // 
      this.clearButton.Location = new System.Drawing.Point(536, 359);
      this.clearButton.Margin = new System.Windows.Forms.Padding(2);
      this.clearButton.Name = "clearButton";
      this.clearButton.Size = new System.Drawing.Size(92, 23);
      this.clearButton.TabIndex = 3;
      this.clearButton.Text = "Clear";
      this.clearButton.UseVisualStyleBackColor = true;
      this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
      // 
      // recognizeButton
      // 
      this.recognizeButton.Location = new System.Drawing.Point(442, 359);
      this.recognizeButton.Margin = new System.Windows.Forms.Padding(2);
      this.recognizeButton.Name = "recognizeButton";
      this.recognizeButton.Size = new System.Drawing.Size(90, 23);
      this.recognizeButton.TabIndex = 4;
      this.recognizeButton.Text = "Recognize";
      this.recognizeButton.UseVisualStyleBackColor = true;
      this.recognizeButton.Click += new System.EventHandler(this.recognizeButton_Click);
      // 
      // outputLabel
      // 
      this.outputLabel.AutoSize = true;
      this.outputLabel.Location = new System.Drawing.Point(36, 384);
      this.outputLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.outputLabel.Name = "outputLabel";
      this.outputLabel.Size = new System.Drawing.Size(0, 13);
      this.outputLabel.TabIndex = 5;
      // 
      // eventLog1
      // 
      this.eventLog1.SynchronizingObject = this;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(729, 445);
      this.Controls.Add(this.outputLabel);
      this.Controls.Add(this.recognizeButton);
      this.Controls.Add(this.clearButton);
      this.Controls.Add(this.OpenButton);
      this.Controls.Add(this.pictureBox1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Margin = new System.Windows.Forms.Padding(2);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Form1";
      this.Text = "Form1";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout(); 
    }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button recognizeButton;
        private System.Windows.Forms.Label outputLabel;
    private System.Diagnostics.EventLog eventLog1;
  }
}

