namespace TenBuddy
{
    using Interop.UIAutomationClient;
    using System;
    using System.Runtime.InteropServices;
    using System.Text.Json;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using OpenAI;
    using OpenAI.Chat;
    using System.Text.Encodings.Web;

    public partial class MainForm : Form,
        IUIAutomationPropertyChangedEventHandler,
        IUIAutomationStructureChangedEventHandler
    {
        private const string WeChatClassName = "WeChatMainWndForPC";
        private readonly CUIAutomation uiAuto = new();
        private readonly Dictionary<string, Chat> chatDict = new();
        private Chat? currentChat;
        private IUIAutomationElement? messageListElement;
        private IUIAutomationElement? nameElement;
        private IUIAutomationElement? editElement;
        private string userName = "Unknown";
        private bool checking = false;

        public MainForm()
        {
            try
            {
                Settings.Instance.Load();
            } catch (Exception ex)
            {
                MessageBox.Show("���������ļ�ʧ�ܣ�" + ex.Message);
            }
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var location = Settings.Instance.MainFormLocation;
            if (location.LocationX != -1 && location.LocationY != -1)
            {
                this.Location = new Point(location.LocationX, location.LocationY);
                this.Size = new Size(location.Width, location.Height);
            }
        }

        private void RenderMarkdown()
        {
            string markdownText = richTextBox1.Text;
            // ��ͣ�����Ա�����˸
            richTextBox1.SuspendLayout();
            // ���RichTextBox
            richTextBox1.Clear();

            // �������
            Regex boldRegex = new Regex(@"\*\*(.+?)\*\*");
            MatchCollection boldMatches = boldRegex.Matches(markdownText);
            int lastIndex = 0;

            foreach (Match match in boldMatches)
            {
                // ���֮ǰ����ͨ�ı�
                if (match.Index > lastIndex)
                {
                    richTextBox1.AppendText(markdownText.Substring(lastIndex, match.Index - lastIndex));
                }

                // ��Ӵ����ı�
                richTextBox1.SelectionFont = new System.Drawing.Font(richTextBox1.Font, System.Drawing.FontStyle.Bold);
                richTextBox1.AppendText(match.Groups[1].Value);
                richTextBox1.SelectionFont = new System.Drawing.Font(richTextBox1.Font, System.Drawing.FontStyle.Regular);

                lastIndex = match.Index + match.Length;
            }

            // ���ʣ�����ͨ�ı�
            if (lastIndex < markdownText.Length)
            {
                richTextBox1.AppendText(markdownText.Substring(lastIndex));
            }

            // �ָ�����
            richTextBox1.ResumeLayout();
        }

        private void ReadTimer_Tick(object sender, EventArgs e)
        {
            if (this.checking)
            {
                return;
            }
            if (this.nameElement == null || this.editElement == null || this.messageListElement == null)
            {
                this.checking = true;
                this.lbTargetName.Text = "����ʶ�����촰��...";
                Task.Run(() =>
                {
                    CheckWeChat();
                    this.checking = false;
                });
            }
            else if (this.currentChat != null)
            {
                if (this.currentChat.PendingUpdate)
                {
                    UpdateMessageList();
                }
                else
                {
                    UpdateInputBox();
                }
            }
        }

        public Chat? GetCurrentChat()
        {
            return this.currentChat;
        }

        private void SetCurrentChat(string text)
        {
            if (chatDict.ContainsKey(text))
            {
                this.currentChat = chatDict[text];
            }
            else
            {
                this.currentChat = new Chat(text);
                chatDict[text] = this.currentChat;
            }

            // check if it is a group chat
            var walker = uiAuto.ControlViewWalker;
            var groupMemberCountElement = walker.GetNextSiblingElement(this.nameElement);
            if (groupMemberCountElement != null)
            {
                this.currentChat.IsGroup = true;
                // current name is like " (3)", extract the number
                string temp = groupMemberCountElement.CurrentName;
                this.currentChat.GroupMemberCount = int.Parse(temp.Substring(2, temp.Length - 3));
            }
            else
            {
                this.currentChat.IsGroup = false;
            }

            this.Invoke(new Action(() =>
            {
                if (this.currentChat.IsGroup)
                {
                    this.lbTargetName.Text = "Ⱥ�ģ�" + this.currentChat.Name + "��" + this.currentChat.GroupMemberCount + "��";
                    this.UpdateInstructionPreset(Settings.Instance.GroupInstructionPreset);
                }
                else
                {
                    this.lbTargetName.Text = "���ѣ�" + this.currentChat.Name;
                    this.UpdateInstructionPreset(Settings.Instance.FriendInstructionPreset);
                }
                var introduction = Settings.Instance.ChatIntroductions.GetValueOrDefault(this.currentChat.Name, "");
                this.txtTargetIntroduction.Text = introduction;
            }));
        }

        private void UpdateInputBox()
        {
            if (this.editElement == null || this.currentChat == null)
            {
                return;
            }

            // Get value pattern of the edit element
            try
            {
                var valuePattern = (IUIAutomationValuePattern)this.editElement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                if (valuePattern == null)
                {
                    return;
                }

                string text = valuePattern.CurrentValue;
                if (text == this.currentChat.InputValue)
                {
                    return;
                }
                this.currentChat.InputValue = text;
            }
            catch (COMException e)
            {
                Debug.WriteLine("Error: " + e.Message);
                this.messageListElement = null;
                this.nameElement = null;
                this.editElement = null;
                this.currentChat = null;
                return;
            }


            if (this.currentChat.InputValue == "")
            {
                this.lbInput.Text = "��ǰ���룺��";
                if (this.currentChat.IsGroup)
                {
                    this.UpdateInstructionPreset(Settings.Instance.GroupInstructionPreset);
                }
                else
                {
                    this.UpdateInstructionPreset(Settings.Instance.FriendInstructionPreset);
                }
            }
            else
            {
                this.lbInput.Text = "��ǰ���룺" + this.currentChat.InputValue;
                this.UpdateInstructionPreset(Settings.Instance.RefineInstructionPreset);
            }
        }

        private void UpdateInstructionPreset(List<string> presets)
        {
            this.comboQuestion.Items.Clear();
            foreach (var question in presets)
            {
                this.comboQuestion.Items.Add(question);
            }
            this.comboQuestion.Text = presets.FirstOrDefault();
        }

        private void UpdateMessageList()
        {
            var startTime = DateTime.Now;
            var walker = uiAuto.RawViewWalker;
            if (null == this.currentChat || null == this.messageListElement)
            {
                return;
            }

            this.currentChat.Messages.Clear();
            this.currentChat.HasMoreMessages = false;

            var children = this.messageListElement.FindAll(TreeScope.TreeScope_Children, uiAuto.CreateTrueCondition());
            for (int i = 0; i < children.Length; i++)
            {
                var item = children.GetElement(i);
                var itemChild = walker.GetFirstChildElement(item);
                if (itemChild == null)
                    continue;

                var itemName = item.CurrentName;
                if (itemName.Contains("�鿴������Ϣ"))
                {
                    this.currentChat.HasMoreMessages = true;
                    continue;
                }

                if (itemChild.CurrentControlType == UIA_ControlTypeIds.UIA_PaneControlTypeId)
                {
                    var button = itemChild.FindFirst(TreeScope.TreeScope_Children, uiAuto.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ButtonControlTypeId));
                    if (button != null)
                    {
                        string message = button.CurrentName;
                        message += "��" + itemName.Replace("���� ", "\n���� ");
                        var value = item.GetCurrentPropertyValue(UIA_PropertyIds.UIA_ValueValuePropertyId) as string;
                        if (value != null && value != "")
                        {
                            message += "\n<block>\n" + value + "\n</block>";
                        }
                        this.currentChat.Messages.Add(message);
                    }
                    else
                    {
                        // If button not found, use the text
                        var text = itemChild.FindFirst(TreeScope.TreeScope_Descendants, uiAuto.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_TextControlTypeId));
                        if (text != null)
                        {
                            this.currentChat.Messages.Add(text.CurrentName);
                        }
                    }
                }
                else if (itemChild.CurrentControlType == UIA_ControlTypeIds.UIA_TextControlTypeId)
                {
                    // Message time
                    this.currentChat.Messages.Add("[" + itemName + "]");
                }

            }

            Debug.WriteLine("Children count: " + children.Length);
            Debug.WriteLine("Time: " + (DateTime.Now - startTime).TotalMilliseconds);

            this.currentChat.PendingUpdate = false;
            // join the messages
            this.Invoke(new Action(() =>
            {
                this.lbMessageCount.Text = "��ǰ��Ϣ��" + this.currentChat.Messages.Count;
            }));
        }

        private void CheckWeChat()
        {
            var current = uiAuto.GetRootElement().FindFirst(TreeScope.TreeScope_Children, uiAuto.CreatePropertyCondition(UIA_PropertyIds.UIA_ClassNamePropertyId, WeChatClassName));
            if (current == null)
            {
                return;
            }

            // Get a controlview walker
            var walker = uiAuto.ControlViewWalker;
            current = walker.GetLastChildElement(current);
            current = walker.GetLastChildElement(current);

            var toolbarElement = walker.GetFirstChildElement(current);
            if (toolbarElement.CurrentName != "����")
            {
                Debug.WriteLine("Not found toolbar");
                return;
            }
            var userNameElement = walker.GetFirstChildElement(toolbarElement);
            if (userNameElement == null || userNameElement.CurrentControlType != UIA_ControlTypeIds.UIA_ButtonControlTypeId)
            {
                Debug.WriteLine("Not found user name element");
                return;
            }

            this.userName = userNameElement.CurrentName;
            this.Invoke(new Action(() =>
            {
                this.Text = "TenBuddy - " + this.userName;
            }));

            var chatListElement = walker.GetNextSiblingElement(toolbarElement);
            var chatMessagesElement = walker.GetNextSiblingElement(chatListElement);

            // Get a element where it is a list and its name is "��Ϣ"
            var andCondition = uiAuto.CreateAndCondition(uiAuto.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListControlTypeId), uiAuto.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "��Ϣ"));
            this.messageListElement = chatMessagesElement.FindFirst(TreeScope.TreeScope_Descendants, andCondition);
            if (this.messageListElement == null)
            {
                Debug.WriteLine("Not found message list");
                return;
            }

            // Get the first edit element as the input box
            this.editElement = chatMessagesElement.FindFirst(TreeScope.TreeScope_Descendants, uiAuto.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_EditControlTypeId));
            if (this.editElement == null)
            {
                Debug.WriteLine("Not found edit element");
                return;
            }
            // Get value pattern of the edit element
            var valuePattern = (IUIAutomationValuePattern)this.editElement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
            if (valuePattern == null)
            {
                Debug.WriteLine("Not found value pattern");
                return;
            }

            // Get the first text element as the name of the chat
            this.nameElement = chatMessagesElement.FindFirst(TreeScope.TreeScope_Descendants, uiAuto.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_TextControlTypeId));
            if (this.nameElement == null)
            {
                Debug.WriteLine("Not found name element");
                return;
            }

            // Listen on name property change
            var properties = new int[] { UIA_PropertyIds.UIA_NamePropertyId };
            // uiAuto.AddPropertyChangedEventHandler(this.nameElement, TreeScope.TreeScope_Element, null, this, properties);
            uiAuto.AddPropertyChangedEventHandler(this.editElement, TreeScope.TreeScope_Element, null, this, properties);
            uiAuto.AddStructureChangedEventHandler(this.messageListElement, TreeScope.TreeScope_Descendants, null, this);

            this.SetCurrentChat(this.editElement.CurrentName);
            UpdateMessageList();
        }

        public IUIAutomationElement? GetMessageListElement()
        {
            return this.messageListElement;
        }

        public void HandlePropertyChangedEvent(IUIAutomationElement sender, int propertyId, object newValue)
        {
            if (propertyId == UIA_PropertyIds.UIA_NamePropertyId)
            {
                var name = newValue as string;
                if (name != null)
                {
                    this.Invoke(new Action(() =>
                    {
                        this.SetCurrentChat(name);
                        if (this.currentChat != null)
                        {
                            this.currentChat.PendingUpdate = true;
                        }
                    }));
                }
            }
            else
            {
                Debug.WriteLine("Unknown property change: " + propertyId);
            }
        }

        public void HandleStructureChangedEvent(IUIAutomationElement sender, StructureChangeType changeType, int[] runtimeId)
        {
            if (this.currentChat != null)
            {
                this.currentChat.PendingUpdate = true;
            }
        }

        private void btnReadMessages_Click(object sender, EventArgs e)
        {
            // Popup ReadMessagesForm and wait for it to close
            if (this.currentChat != null)
            {
                var form = new ReadMessagesForm(this);
                form.ShowDialog();
            }
        }

        private void btnViewMessages_Click(object sender, EventArgs e)
        {
            ViewMessagesForm form = new();
            form.Owner = this;
            form.ShowDialog();
        }

        private async void btnRequest_Click(object sender, EventArgs e)
        {
            var chat = this.currentChat;
            if (chat == null)
            {
                return;
            }
            if (this.comboQuestion.Text == "")
            {
                MessageBox.Show("���������⡣");
                return;
            }
            btnRequest.Enabled = false;
            if (chat.IsGroup)
            {
                richTextBox1.Text = "Ⱥ�ģ�" + chat.Name + "\n";
            }
            else
            {
                richTextBox1.Text = "���ѣ�" + chat.Name + "\n";
            }
            richTextBox1.AppendText("ָ�" + this.comboQuestion.Text + "\n�ش�\n");
            richTextBox1.SelectionStart = richTextBox1.Text.Length;

            var parts = new List<string>();
            parts.Add("# ��ɫ����");
            if (chat.IsGroup)
            {
                parts.Add(string.Format("���ǡ�{0}������Ⱥ��{1}���ĳ�Ա��", this.userName, chat.Name));
            }
            else
            {
                parts.Add(string.Format("���ǡ�{0}�������Է����ǡ�{1}����", this.userName, chat.Name));
            }
            string introduction = this.txtTargetIntroduction.Text;
            if (introduction != "")
            {
                parts.Add(introduction);
            }
            parts.Add("# �Ի�����");
            parts.AddRange(chat.Messages);
            if (chat.InputValue != "")
            {
                parts.Add("# �ҵ�����");
                parts.Add(chat.InputValue);
            }
            parts.Add("# ָ��");
            parts.Add(this.comboQuestion.Text);
            string userQuestion = string.Join("\n\n", parts);

            var messages = new List<OpenAI.Chat.Message>();
            messages.Add(new OpenAI.Chat.Message(Role.System, "����һ���ó�����ĸ��֣������û���ָ��Ҫ���������׼ȷ�ظ���"));
            messages.Add(new OpenAI.Chat.Message(Role.User, userQuestion));

            var inferenceServer = Settings.Instance.InferenceServer;
            var settings = new OpenAIClientSettings(domain: inferenceServer.ApiHost, apiVersion: inferenceServer.ApiVersion);
            var auth = new OpenAIAuthentication(inferenceServer.ApiToken);
            using var client = new OpenAIClient(auth, settings);
            var chatRequest = new ChatRequest(messages, model: inferenceServer.Model);
            try
            {
                await foreach (var partialResponse in client.ChatEndpoint.StreamCompletionEnumerableAsync(chatRequest))
                {
                    foreach (var choice in partialResponse.Choices.Where(choice => choice.Delta?.Content != null))
                    {
                        richTextBox1.AppendText(choice.Delta.Content);
                    }
                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                }
                RenderMarkdown();
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                btnRequest.Enabled = true;
            } catch (Exception ex)
            {
                if (!richTextBox1.IsDisposed)
                {
                    richTextBox1.AppendText("����ʧ�ܣ�" + ex.Message);
                }
            }
        }

        private void txtTargetIntroduction_TextChanged(object sender, EventArgs e)
        {
            var name = this.currentChat?.Name;
            if (name != null)
            {
                string value = txtTargetIntroduction.Text;
                if (value == string.Empty)
                {
                    Settings.Instance.ChatIntroductions.Remove(name);
                } else
                {
                    Settings.Instance.ChatIntroductions[name] = txtTargetIntroduction.Text;
                }
            }
        }

        private void comboQuestion_KeyPress(object sender, KeyPressEventArgs e)
        {
            // If Enter, invoke the request button
            if (e.KeyChar == (char)Keys.Return)
            {
                if (btnRequest.Enabled)
                {
                    btnRequest_Click(sender, e);
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var location = Settings.Instance.MainFormLocation;
            location.LocationX = this.Location.X;
            location.LocationY = this.Location.Y;
            location.Width = this.Size.Width;
            location.Height = this.Size.Height;
            Settings.Instance.Save();
        }

        private void btnInstructPresetConfig_Click(object sender, EventArgs e)
        {
            var form = new InstructionPresetConfigureForm();
            form.ShowDialog();
        }

        private void btnModelConfig_Click(object sender, EventArgs e)
        {
            var form = new LLMConfigureForm();
            form.ShowDialog();
        }
    }
}
