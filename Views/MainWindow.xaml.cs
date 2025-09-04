using System.Windows;
using EbayBulk_Generator.ViewModels;

namespace EbayBulk_Generator.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
