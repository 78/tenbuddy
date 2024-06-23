using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TenBuddy
{
    public partial class InstructionPresetConfigureForm : Form
    {
        public InstructionPresetConfigureForm()
        {
            InitializeComponent();
        }

        private void InstructionPresetConfigureForm_Load(object sender, EventArgs e)
        {
            var instance = Settings.Instance;
            txtFriendInstructionPreset.Text = string.Join("\r\n", instance.FriendInstructionPreset);
            txtFriendInstructionPreset.SelectionStart = txtFriendInstructionPreset.Text.Length;
            txtGroupInstructionPreset.Text = string.Join("\r\n", instance.GroupInstructionPreset);
            txtGroupInstructionPreset.SelectionStart = txtGroupInstructionPreset.Text.Length;
            txtRefineInstructionPreset.Text = string.Join("\r\n", instance.RefineInstructionPreset);
            txtRefineInstructionPreset.SelectionStart = txtRefineInstructionPreset.Text.Length;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var instance = Settings.Instance;
            instance.FriendInstructionPreset = txtFriendInstructionPreset.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            instance.GroupInstructionPreset = txtGroupInstructionPreset.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            instance.RefineInstructionPreset = txtRefineInstructionPreset.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            instance.Save();
            this.Close();
        }
    }
}
