namespace TenBuddy
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            // Prevent multiple instances of the application
            bool createdNew;
            using (Mutex mutex = new(true, "TenBuddy", out createdNew))
            {
                if (!createdNew)
                {
                    MessageBox.Show("TenBuddy is already running.", "TenBuddy", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ApplicationConfiguration.Initialize();
                Application.Run(new MainForm());
            }
        }
    }
}