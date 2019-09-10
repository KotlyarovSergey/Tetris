namespace Tetris
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.MoveDownTimer = new System.Windows.Forms.Timer(this.components);
            this.lblScore = new System.Windows.Forms.Label();
            this.lblLineCount = new System.Windows.Forms.Label();
            this.lblStart = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.picBox_RtRight = new System.Windows.Forms.PictureBox();
            this.picBox_RtLeft = new System.Windows.Forms.PictureBox();
            this.pictBoxNextFigure = new System.Windows.Forms.PictureBox();
            this.pnlField = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picBox_RtRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox_RtLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxNextFigure)).BeginInit();
            this.SuspendLayout();
            // 
            // MoveDownTimer
            // 
            this.MoveDownTimer.Tick += new System.EventHandler(this.MoveDownTimer_Tick);
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblScore.ForeColor = System.Drawing.Color.Maroon;
            this.lblScore.Location = new System.Drawing.Point(306, 285);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(61, 22);
            this.lblScore.TabIndex = 4;
            this.lblScore.Text = "Очки: ";
            // 
            // lblLineCount
            // 
            this.lblLineCount.AutoSize = true;
            this.lblLineCount.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblLineCount.ForeColor = System.Drawing.Color.Maroon;
            this.lblLineCount.Location = new System.Drawing.Point(306, 311);
            this.lblLineCount.Name = "lblLineCount";
            this.lblLineCount.Size = new System.Drawing.Size(82, 22);
            this.lblLineCount.TabIndex = 5;
            this.lblLineCount.Text = "Линий: 0";
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Font = new System.Drawing.Font("Times New Roman", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStart.ForeColor = System.Drawing.Color.Chartreuse;
            this.lblStart.Location = new System.Drawing.Point(320, 9);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(76, 36);
            this.lblStart.TabIndex = 6;
            this.lblStart.Text = "Start";
            this.lblStart.Click += new System.EventHandler(this.lblStart_Click);
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSpeed.ForeColor = System.Drawing.Color.Maroon;
            this.lblSpeed.Location = new System.Drawing.Point(306, 355);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(103, 22);
            this.lblSpeed.TabIndex = 7;
            this.lblSpeed.Text = "Скорость: 1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(307, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Следующая фигура";
            this.label2.Visible = false;
            // 
            // picBox_RtRight
            // 
            this.picBox_RtRight.Image = global::Tetris.ImagesRsc.RtRightOn;
            this.picBox_RtRight.Location = new System.Drawing.Point(364, 179);
            this.picBox_RtRight.Name = "picBox_RtRight";
            this.picBox_RtRight.Size = new System.Drawing.Size(51, 51);
            this.picBox_RtRight.TabIndex = 10;
            this.picBox_RtRight.TabStop = false;
            this.picBox_RtRight.Click += new System.EventHandler(this.picBox_RtRight_Click);
            // 
            // picBox_RtLeft
            // 
            this.picBox_RtLeft.Image = global::Tetris.ImagesRsc.RtLeftOff;
            this.picBox_RtLeft.Location = new System.Drawing.Point(313, 179);
            this.picBox_RtLeft.Name = "picBox_RtLeft";
            this.picBox_RtLeft.Size = new System.Drawing.Size(51, 51);
            this.picBox_RtLeft.TabIndex = 9;
            this.picBox_RtLeft.TabStop = false;
            this.picBox_RtLeft.Click += new System.EventHandler(this.picBox_RtLeft_Click);
            // 
            // pictBoxNextFigure
            // 
            this.pictBoxNextFigure.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictBoxNextFigure.Location = new System.Drawing.Point(313, 75);
            this.pictBoxNextFigure.Name = "pictBoxNextFigure";
            this.pictBoxNextFigure.Size = new System.Drawing.Size(102, 102);
            this.pictBoxNextFigure.TabIndex = 2;
            this.pictBoxNextFigure.TabStop = false;
            // 
            // pnlField
            // 
            this.pnlField.BackgroundImage = global::Tetris.ImagesRsc.Field;
            this.pnlField.Cursor = System.Windows.Forms.Cursors.Default;
            this.pnlField.Location = new System.Drawing.Point(7, 7);
            this.pnlField.Name = "pnlField";
            this.pnlField.Size = new System.Drawing.Size(294, 486);
            this.pnlField.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Olive;
            this.ClientSize = new System.Drawing.Size(420, 498);
            this.Controls.Add(this.picBox_RtRight);
            this.Controls.Add(this.picBox_RtLeft);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblSpeed);
            this.Controls.Add(this.lblStart);
            this.Controls.Add(this.lblLineCount);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.pictBoxNextFigure);
            this.Controls.Add(this.pnlField);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tetris";
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Leave += new System.EventHandler(this.MainForm_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.picBox_RtRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox_RtLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxNextFigure)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlField;
        private System.Windows.Forms.Timer MoveDownTimer;
        private System.Windows.Forms.PictureBox pictBoxNextFigure;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Label lblLineCount;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox picBox_RtLeft;
        private System.Windows.Forms.PictureBox picBox_RtRight;
    }
}

