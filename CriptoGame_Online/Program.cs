using Warrior_and_Wealth.GUI;

namespace Warrior_and_Wealth
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
            ApplicationConfiguration.Initialize();
            Login login = new Login();

            if (login.ShowDialog() == DialogResult.OK) // login riuscito
                Application.Run(new Gioco());
            else
                Application.Exit(); // chiudi tutto se login non completato

        }
    }
}