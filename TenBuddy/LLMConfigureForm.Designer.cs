namespace TenBuddy
{
    partial class LLMConfigureForm
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
            label1 = new Label();
            label2 = new Label();
            txtApiHost = new TextBox();
            label3 = new Label();
            txtToken = new TextBox();
            btnSave = new Button();
            label4 = new Label();
            comboModel = new ComboBox();
            txtVersion = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 23);
            label1.Name = "label1";
            label1.Size = new Size(168, 17);
            label1.TabIndex = 0;
            label1.Text = "可以使用 OpenAI 兼容接口。";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(26, 56);
            label2.Name = "label2";
            label2.Size = new Size(58, 17);
            label2.TabIndex = 1;
            label2.Text = "API Host";
            // 
            // txtApiHost
            // 
            txtApiHost.Location = new Point(90, 53);
            txtApiHost.Name = "txtApiHost";
            txtApiHost.PlaceholderText = "域名";
            txtApiHost.Size = new Size(242, 23);
            txtApiHost.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(42, 94);
            label3.Name = "label3";
            label3.Size = new Size(44, 17);
            label3.TabIndex = 3;
            label3.Text = "Token";
            // 
            // txtToken
            // 
            txtToken.Location = new Point(90, 91);
            txtToken.Name = "txtToken";
            txtToken.PlaceholderText = "授权码";
            txtToken.Size = new Size(334, 23);
            txtToken.TabIndex = 4;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(335, 170);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(89, 31);
            btnSave.TabIndex = 5;
            btnSave.Text = "保存";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(38, 133);
            label4.Name = "label4";
            label4.Size = new Size(46, 17);
            label4.TabIndex = 3;
            label4.Text = "Model";
            // 
            // comboModel
            // 
            comboModel.FormattingEnabled = true;
            comboModel.Location = new Point(90, 130);
            comboModel.Name = "comboModel";
            comboModel.Size = new Size(334, 25);
            comboModel.TabIndex = 6;
            // 
            // txtVersion
            // 
            txtVersion.Location = new Point(338, 53);
            txtVersion.Name = "txtVersion";
            txtVersion.PlaceholderText = "版本";
            txtVersion.Size = new Size(86, 23);
            txtVersion.TabIndex = 7;
            // 
            // LLMConfigureForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(459, 218);
            Controls.Add(txtVersion);
            Controls.Add(comboModel);
            Controls.Add(btnSave);
            Controls.Add(label4);
            Controls.Add(txtToken);
            Controls.Add(label3);
            Controls.Add(txtApiHost);
            Controls.Add(label2);
            Controls.Add(label1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LLMConfigureForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "大语言模型配置";
            Load += LLMConfigureForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtApiHost;
        private Label label3;
        private TextBox txtToken;
        private Button btnSave;
        private Label label4;
        private ComboBox comboModel;
        private TextBox txtVersion;
    }
}