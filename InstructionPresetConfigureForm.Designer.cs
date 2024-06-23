namespace TenBuddy
{
    partial class InstructionPresetConfigureForm
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
            txtFriendInstructionPreset = new TextBox();
            label2 = new Label();
            txtGroupInstructionPreset = new TextBox();
            label3 = new Label();
            txtRefineInstructionPreset = new TextBox();
            btnSave = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 22);
            label1.Name = "label1";
            label1.Size = new Size(176, 17);
            label1.TabIndex = 0;
            label1.Text = "朋友聊天预设指令（每行一个）";
            // 
            // txtFriendInstructionPreset
            // 
            txtFriendInstructionPreset.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtFriendInstructionPreset.Location = new Point(24, 42);
            txtFriendInstructionPreset.Multiline = true;
            txtFriendInstructionPreset.Name = "txtFriendInstructionPreset";
            txtFriendInstructionPreset.ScrollBars = ScrollBars.Both;
            txtFriendInstructionPreset.Size = new Size(479, 120);
            txtFriendInstructionPreset.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 181);
            label2.Name = "label2";
            label2.Size = new Size(164, 17);
            label2.TabIndex = 0;
            label2.Text = "群聊天预设指令（每行一个）";
            // 
            // txtGroupInstructionPreset
            // 
            txtGroupInstructionPreset.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtGroupInstructionPreset.Location = new Point(24, 201);
            txtGroupInstructionPreset.Multiline = true;
            txtGroupInstructionPreset.Name = "txtGroupInstructionPreset";
            txtGroupInstructionPreset.ScrollBars = ScrollBars.Both;
            txtGroupInstructionPreset.Size = new Size(479, 120);
            txtGroupInstructionPreset.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 340);
            label3.Name = "label3";
            label3.Size = new Size(200, 17);
            label3.TabIndex = 0;
            label3.Text = "优化输入文案预设指令（每行一个）";
            // 
            // txtRefineInstructionPreset
            // 
            txtRefineInstructionPreset.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtRefineInstructionPreset.Location = new Point(24, 360);
            txtRefineInstructionPreset.Multiline = true;
            txtRefineInstructionPreset.Name = "txtRefineInstructionPreset";
            txtRefineInstructionPreset.ScrollBars = ScrollBars.Both;
            txtRefineInstructionPreset.Size = new Size(479, 120);
            txtRefineInstructionPreset.TabIndex = 1;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.Location = new Point(405, 498);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(98, 32);
            btnSave.TabIndex = 2;
            btnSave.Text = "保存";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // InstructionPresetConfigureForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(523, 550);
            Controls.Add(btnSave);
            Controls.Add(txtRefineInstructionPreset);
            Controls.Add(label3);
            Controls.Add(txtGroupInstructionPreset);
            Controls.Add(label2);
            Controls.Add(txtFriendInstructionPreset);
            Controls.Add(label1);
            Name = "InstructionPresetConfigureForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "指令预设配置";
            Load += InstructionPresetConfigureForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtFriendInstructionPreset;
        private Label label2;
        private TextBox txtGroupInstructionPreset;
        private Label label3;
        private TextBox txtRefineInstructionPreset;
        private Button btnSave;
    }
}