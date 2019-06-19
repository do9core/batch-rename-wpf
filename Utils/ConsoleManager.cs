#if DEBUG
using System.Runtime.InteropServices;

namespace do9Rename.Utils
{
    class ConsoleManager
    {
        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        [DllImport("Kernel32")]
        public static extern void FreeConsole();

        public void OpenConsole()
        {
            AllocConsole();
        }

        public void CloseConsole()
        {
            CloseConsole();
        }
    }
}
#endif