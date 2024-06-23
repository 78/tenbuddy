using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenBuddy
{
    // Chat data structure
    public class Chat
    {
        public string Name;
        public bool IsGroup = false;
        public int GroupMemberCount = 0;
        public bool PendingUpdate = true;
        public string InputValue = "";
        public bool HasMoreMessages = false;
        public List<string> Messages;
        public Chat(string name)
        {
            Name = name;
            Messages = new List<string>();
        }
    }
}
