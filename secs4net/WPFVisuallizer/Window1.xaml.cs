using Secs4Net;
using SecsMessageVisuallizer.ViewModel;
using System.Windows;

namespace SecsMessageVisuallizer {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window {
        public Window1() {
            InitializeComponent();
            base.DataContext = new SecsMessageCollectionViewModel(new SecsMessageList("common.sml"));
        }
    }
}
