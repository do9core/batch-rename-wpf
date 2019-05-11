using do9Rename.Utils;
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
            this.Startup += (s, e) => Singleton<ConsoleManager>.Instance.OpenConsole();
#endif
        }
    }
}
