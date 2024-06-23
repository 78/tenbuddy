namespace TenBuddy
{
    partial class ReadMessagesForm
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
            components = new System.ComponentModel.Container();
            label1 = new Label();
            comboCount = new ComboBox();
            btnStart = new Button();
            label2 = new Label();
            ReadTimer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(27, 80);
            label1.Name = "label1";
            label1.Size = new Size(56, 17);
            label1.TabIndex = 0;
            label1.Text = "消息数量";
            // 
            // comboCount
            // 
            comboCount.FormattingEnabled = true;
            comboCount.Items.AddRange(new object[] { "100", "200", "300", "400", "500", "600" });
            comboCount.Location = new Point(89, 77);
            comboCount.Name = "comboCount";
            comboCount.Size = new Size(75, 25);
            comboCount.TabIndex = 1;
            comboCount.Text = "500";
            // 
            // btnStart
            // 
            btnStart.Location = new Point(174, 73);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(92, 30);
            btnStart.TabIndex = 2;
            btnStart.Text = "开始";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // label2
            // 
            label2.Location = new Point(27, 24);
            label2.Name = "label2";
            label2.Size = new Size(239, 37);
            label2.TabIndex = 3;
            label2.Text = "点击“开始”按钮，程序会控制您的鼠标滚动聊天界面，读取文本内容。";
            // 
            // ReadTimer
            // 
            ReadTimer.Interval = 500;
            ReadTimer.Tick += ReadTimer_Tick;
            // 
            // ReadMessagesForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(288, 133);
            Controls.Add(label2);
            Controls.Add(btnStart);
            Controls.Add(comboCount);
            Controls.Add(label1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ReadMessagesForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "读取消息";
            FormClosing += ReadMessagesForm_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox comboCount;
        private Button btnStart;
        private Label label2;
        private System.Windows.Forms.Timer ReadTimer;
    }
}