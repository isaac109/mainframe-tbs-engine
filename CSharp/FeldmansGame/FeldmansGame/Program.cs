using System;
using Mainframe.Core;
using Mainframe.Forms;
using System.Windows.Forms;

namespace Mainframe
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AbilityEditor());

            /* Code to launch game
            using (MainframeGame game = new MainframeGame())
            {
                game.Run();
            }*/
        }
    }
#endif
}