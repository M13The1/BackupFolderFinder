using System.Windows;
using System.Windows.Input;
using BackupFolderFinder.ViewModels;

namespace BackupFolderFinder
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DetailDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MainViewModel vm && vm.OpenFolderCommand.CanExecute(null))
            {
                vm.OpenFolderCommand.Execute(null);
            }
        }
    }
}
