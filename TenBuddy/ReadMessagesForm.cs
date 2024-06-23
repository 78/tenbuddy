using Interop.UIAutomationClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TenBuddy
{
    public partial class ReadMessagesForm : Form
    {
        // 定义输入类型常量
        private const int INPUT_MOUSE = 0;
        private const uint MOUSEEVENTF_WHEEL = 0x0800;

        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public int type;
            public MOUSEINPUT mi;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        public MainForm mainForm;
        public int requestMessageCount;

        public ReadMessagesForm(MainForm main)
        {
            this.mainForm = main;
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // check if the message count is valid
            if (!int.TryParse(this.comboCount.Text, out this.requestMessageCount))
            {
                MessageBox.Show("Invalid message count.");
                return;
            }
            this.btnStart.Enabled = false;
            this.ReadTimer.Enabled = true;
        }

        private void ReadTimer_Tick(object sender, EventArgs e)
        {
            var chat = this.mainForm.GetCurrentChat();
            if (chat == null || chat.PendingUpdate)
            {
                Debug.WriteLine("Waiting for messages to load...");
                return;
            }

            if (!chat.HasMoreMessages)
            {
                Debug.WriteLine("No more messages to read.");
                this.ReadTimer.Enabled = false;
                this.Close();
                return;
            }

            if (chat.Messages.Count >= this.requestMessageCount)
            {
                Debug.WriteLine("Read message count reached.");
                this.ReadTimer.Enabled = false;
                this.Close();
                return;
            }

            var messageListElement = this.mainForm.GetMessageListElement();
            if (messageListElement == null)
            {
                Debug.WriteLine("Message list element not found.");
                return;
            }

            // scroll to the top of the message list
            var scrollPattern = messageListElement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollPatternId) as IUIAutomationScrollPattern;
            scrollPattern?.SetScrollPercent(scrollPattern.CurrentHorizontalScrollPercent, 0);

            // move cursor to the center of the message list and scroll up
            // get bounding rectangle of the message list
            var rect = messageListElement.CurrentBoundingRectangle;
            int x = rect.left + (rect.right - rect.left) / 2;
            int y = rect.top + (rect.bottom - rect.top) / 2;
            var oldPos = Cursor.Position;
            Cursor.Position = new Point(x, y);

            // 设置滚动量，正值表示向上滚动，负值表示向下滚动
            int scrollAmount = 120; // 每次滚动一个单位（通常是120）

            var inputs = new INPUT[1];
            inputs[0] = new INPUT
            {
                type = INPUT_MOUSE,
                mi = new MOUSEINPUT
                {
                    dx = 0,
                    dy = 0,
                    mouseData = (uint)scrollAmount,
                    dwFlags = MOUSEEVENTF_WHEEL,
                    time = 0,
                    dwExtraInfo = IntPtr.Zero
                }
            };

            // 发送输入
            uint result = SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));

            if (result == 0)
            {
                Console.WriteLine("Mouse wheel scroll failed.");
            }
            else
            {
                Console.WriteLine("Mouse wheel scrolled.");
            }

            Cursor.Position = oldPos;
        }

        private void ReadMessagesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ReadTimer.Enabled = false;
        }
    }
}
