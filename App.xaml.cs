#if DEBUG
using do9Rename.Utils;
#endif
using System.Windows;

namespace do9Rename
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
#if DEBUG
            this.Startup += (s, e) => new ConsoleManager().OpenConsole();
#endif
        }
    }
}
