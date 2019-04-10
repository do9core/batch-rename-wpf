using CommonServiceLocator;

using do9Rename.Core;

using GalaSoft.MvvmLight.Ioc;

namespace do9Rename.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IRemoveExtCommand>(() => new RemoveExtCommand());
            SimpleIoc.Default.Register<IAppendExtCommand>(() => new AppendExtCommand());
            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}