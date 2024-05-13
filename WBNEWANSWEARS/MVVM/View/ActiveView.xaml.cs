using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static WBNEWANSWEARS.MVVM.ViewModel.ActiveViewModel;

namespace WBNEWANSWEARS.MVVM.View
{
    public partial class ActiveView : UserControl
    {
        public ActiveView()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show("ASD");
        }
    }
}
