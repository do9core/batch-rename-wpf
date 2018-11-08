using System.Windows;

using do9Rename.Helpers;
using do9Rename.ViewModels;

namespace do9Rename.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = Singleton<OptionViewModel>.Instance;
            InitializeComponent();
        }
    }
}
