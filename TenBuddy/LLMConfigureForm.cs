using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenAI;

namespace TenBuddy
{
    public partial class LLMConfigureForm : Form
    {
        public LLMConfigureForm()
        {
            InitializeComponent();
        }

        private void LLMConfigureForm_Load(object sender, EventArgs e)
        {
            var inferenceServer = Settings.Instance.InferenceServer;
            var instance = Settings.Instance;
            this.txtApiHost.Text = inferenceServer.ApiHost;
            this.txtVersion.Text = inferenceServer.ApiVersion;
            this.txtToken.Text = inferenceServer.ApiToken;
            this.comboModel.Text = inferenceServer.Model;

            // Try to load models from server
            Task.Run(async () =>
            {
                var settings = new OpenAIClientSettings(domain: inferenceServer.ApiHost, apiVersion: inferenceServer.ApiVersion);
                var auth = new OpenAIAuthentication(inferenceServer.ApiToken);
                using var client = new OpenAIClient(auth, settings);
                try
                {
                    var models = await client.ModelsEndpoint.GetModelsAsync();
                    this.Invoke(() =>
                    {
                        this.comboModel.Items.Clear();
                        foreach (var model in models)
                        {
                            this.comboModel.Items.Add(model.Id);
                        }
                    });
                } catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return;
                }
            });
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var instance = Settings.Instance;
            instance.InferenceServer.ApiHost = this.txtApiHost.Text;
            instance.InferenceServer.ApiVersion = this.txtVersion.Text;
            instance.InferenceServer.ApiToken = this.txtToken.Text;
            instance.InferenceServer.Model = this.comboModel.Text;
            instance.Save();
            this.Close();
        }
    }
}
