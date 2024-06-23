using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace TenBuddy
{
    internal class Settings
    {
        public class FormLocationSize {
            public int LocationX { get; set; } = -1;
            public int LocationY { get; set; } = -1;
            public int Width { get; set; } = 0;
            public int Height { get; set; } = 0;
        }
        public class InferenceServerSettings
        {
            public string ApiHost { get; set; } = "api.tenclass.net";
            public string ApiVersion { get; set; } = "v1";
            public string ApiToken { get; set; } = "tenbuddy-945a87f4";
            public string Model { get; set; } = "doubao-pro-32k";
        }
        private static Settings? instance;
        public string FilePath { get; }
        public Dictionary<string, string> ChatIntroductions { get; set; }
        public List<string> FriendInstructionPreset { get; set; }
        public List<string> GroupInstructionPreset { get; set; }
        public List<string> RefineInstructionPreset { get; set; }
        public FormLocationSize MainFormLocation;
        public InferenceServerSettings InferenceServer { get; set; }

        public Settings()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TenBuddy");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            this.FilePath = Path.Combine(path, "settings.json");
            this.ChatIntroductions = new Dictionary<string, string>();
            this.FriendInstructionPreset = new List<string>();
            this.GroupInstructionPreset = new List<string>();
            this.RefineInstructionPreset = new List<string>();
            this.MainFormLocation = new FormLocationSize();
            this.InferenceServer = new InferenceServerSettings();

            this.FriendInstructionPreset.Add("你能帮我回复什么？写出5条。");
            this.FriendInstructionPreset.Add("详细分析一下，对方是一个什么样的人？");
            this.FriendInstructionPreset.Add("根据聊天内容，分析对方的性格特点。");

            this.GroupInstructionPreset.Add("总结一下聊天内容。");
            this.GroupInstructionPreset.Add("分析主要群成员的属性。");

            this.RefineInstructionPreset.Add("修改输入文案，高情商的表达，丰富内容，写出5条");
            this.RefineInstructionPreset.Add("修改输入文案，低情商的表达，终结话题，写出5条");
        }

        public static Settings Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }
                instance = new Settings();
                return instance;
            }
        }

        public void Load()
        {
            if (!File.Exists(this.FilePath))
            {
                return;
            }
            // deserialize the json file
            string text = File.ReadAllText(this.FilePath);
            var json = JsonSerializer.Deserialize<JsonElement>(text);

            if (json.TryGetProperty("ChatIntroductions", out var chatIntroductions))
            {
                foreach (var item in chatIntroductions.EnumerateObject())
                {
                    var value = item.Value.GetString();
                    if (value != null)
                    {
                        ChatIntroductions[item.Name] = value;
                    }
                }
            }
            if (json.TryGetProperty("FriendInstructionPreset", out var friendInstructionPreset))
            {
                FriendInstructionPreset.Clear();
                foreach (var item in friendInstructionPreset.EnumerateArray())
                {
                    var value = item.GetString();
                    if (value != null)
                    {
                        FriendInstructionPreset.Add(value);
                    }
                }
            }
            if (json.TryGetProperty("GroupInstructionPreset", out var groupInstructionPreset))
            {
                GroupInstructionPreset.Clear();
                foreach (var item in groupInstructionPreset.EnumerateArray())
                {
                    var value = item.GetString();
                    if (value != null)
                    {
                        GroupInstructionPreset.Add(value);
                    }
                }
            }
            if (json.TryGetProperty("RefineInstructionPreset", out var refineInstructionPreset))
            {
                RefineInstructionPreset.Clear();
                foreach (var item in refineInstructionPreset.EnumerateArray())
                {
                    var value = item.GetString();
                    if (value != null)
                    {
                        RefineInstructionPreset.Add(value);
                    }
                }
            }
            if (json.TryGetProperty("MainFormLocation", out var formLocation))
            {
                MainFormLocation.LocationX = formLocation.GetProperty("LocationX").GetInt32();
                MainFormLocation.LocationY = formLocation.GetProperty("LocationY").GetInt32();
                MainFormLocation.Width = formLocation.GetProperty("Width").GetInt32();
                MainFormLocation.Height = formLocation.GetProperty("Height").GetInt32();
            }
            if (json.TryGetProperty("InferenceServer", out var inferenceServer))
            {
                InferenceServer.ApiHost = inferenceServer.GetProperty("ApiHost").GetString();
                InferenceServer.ApiVersion = inferenceServer.GetProperty("ApiVersion").GetString();
                InferenceServer.ApiToken = inferenceServer.GetProperty("ApiToken").GetString();
                InferenceServer.Model = inferenceServer.GetProperty("Model").GetString();
            }
        }

        public void Save()
        {
            // serialize the json file
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            var json = new Dictionary<string, object>
            {
                { "ChatIntroductions", ChatIntroductions },
                { "FriendInstructionPreset", FriendInstructionPreset },
                { "GroupInstructionPreset", GroupInstructionPreset },
                { "RefineInstructionPreset", RefineInstructionPreset },
                { "MainFormLocation", MainFormLocation },
                { "InferenceServer", InferenceServer }
            };
            string text = JsonSerializer.Serialize(json, options);
            File.WriteAllText(this.FilePath, text);
        }
    }
}
