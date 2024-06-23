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
    public partial class ViewMessagesForm : Form
    {
        public ViewMessagesForm()
        {
            InitializeComponent();
        }

        private void ViewMessagesForm_Load(object sender, EventArgs e)
        {
            if (this.Owner != null)
            {
                // Get parent form
                var mainForm = (MainForm)this.Owner;
                // Get chat data
                var chat = mainForm.GetCurrentChat();
                // Display chat data
                if (chat != null)
                {
                    this.txtMessages.Text = string.Join("\r\n", chat.Messages);
                    this.txtMessages.SelectionStart = this.txtMessages.Text.Length;
                }
            }

        }
    }
}
