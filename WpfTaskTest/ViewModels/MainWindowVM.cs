using MvvmStandard;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WpfTaskTest.Business;

namespace WpfTaskTest.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        private TaskTestProcessor ttproc = new TaskTestProcessor();

        public ICommand TestCommand { get; set; }

        private bool _CoffeeCream;
        private bool _CoffeeSugar;
        private bool _ToastButter;
        private bool _ToastJelly;

        private int _StatusProgress;

        private string _StatusBarMessage;
        private ObservableCollection<string> _MessageHistory = new ObservableCollection<string>();

        public string StatusBarMessage { get => _StatusBarMessage; set { _StatusBarMessage = value; OnPropertyChanged("StatusBarMessage"); } }

        public ObservableCollection<string> MessageHistory { get => _MessageHistory; set { _MessageHistory = value; } }

        public int StatusProgress { get => _StatusProgress; set { _StatusProgress = value; OnPropertyChanged("StatusProgress"); } }

        public bool CoffeeCream { get => _CoffeeCream; set { _CoffeeCream = value; OnPropertyChanged("CoffeeCream"); } }
        public bool CoffeeSugar { get => _CoffeeSugar; set { _CoffeeSugar = value; OnPropertyChanged("CoffeeSugar"); } }
        public bool ToastButter { get => _ToastButter; set { _ToastButter = value; OnPropertyChanged("ToastButter"); } }
        public bool ToastJelly { get => _ToastJelly; set { _ToastJelly = value; OnPropertyChanged("ToastJelly"); } }

        public MainWindowVM()
        {
            StatusBarMessage = "Ready";
            ttproc.Message = (m) =>
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    StatusBarMessage = m;
                    MessageHistory.Add(m);
                }));
            };
            ttproc.Progress = (p) =>
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    StatusProgress = p;
                }));
            };

            ttproc.CoffeeCondiments = (l,s) => 
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    CoffeeCream = l;
                    CoffeeSugar = s;
                }));
            };

            ttproc.ToastCondiments = (b, j) => 
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    ToastButter = b;
                    ToastJelly = j;
                }));
            };

            TestCommand = new RelayCommand((o) =>
            {
                MessageHistory.Clear();
                StatusBarMessage = "";

                Task.Run(() =>
                {
                    ttproc.CookBreakfast();
                });
            });
        }
    }
}
