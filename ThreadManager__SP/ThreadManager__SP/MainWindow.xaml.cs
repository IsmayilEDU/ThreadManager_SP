
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Input;


namespace ThreadManager__SP
{

    public partial class MainWindow : Window
    {

        #region Property changed

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnNotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Fields
        public int SemaphoreCount { get; set; }
        public MyThread SelectedCreatedMyThread { get; set; }
        public MyThread SelectedWorkingMyThread { get; set; }
        public ObservableCollection<MyThread> WorkingThreads { get; set; }
        public ObservableCollection<MyThread> WaitingThreads { get; set; }
        public ObservableCollection<MyThread> CreatedThreads { get; set; }

        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            SemaphoreCount = 4;
            CreatedThreads = new();
            WaitingThreads = new();
            WorkingThreads = new();
            ProgramFunction();
        }

        #endregion

        #region DoubleClickEvents of listviews
        private void listview_CreatedThreads_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (WaitingThreads.Count < SemaphoreCount)
            {
                SelectedCreatedMyThread = listview_CreatedThreads.SelectedItem as MyThread;
                SelectedCreatedMyThread!.Situation = nameof(Situation.Waiting);
                WaitingThreads.Add(SelectedCreatedMyThread!);
                CreatedThreads.Remove(SelectedCreatedMyThread!);
            }
            else
            {
                MessageBox.Show("Count of Waiting threads can't be more than Count of semaphore");
            }

        }

        private void listview_WorkingThreads_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectedWorkingMyThread = listview_WorkingThreads.SelectedItem as MyThread;
            WorkingThreads.Remove(SelectedWorkingMyThread!);
        }

        #endregion

        #region Click functions

        private void button_CreateNew_Click(object sender, RoutedEventArgs e)
        {
            if (numericUpDown_SemaphoreCount.IsEnabled == true)
            {
                numericUpDown_SemaphoreCount.IsEnabled = false;
            }
            CreatedThreads.Add(new MyThread());
        }

        #endregion

        #region Program functions
        private void ProgramFunction()
        {
            Semaphore semaphore = new Semaphore(SemaphoreCount, SemaphoreCount, "WorkInProgress");
            System.Timers.Timer timer = new System.Timers.Timer(3000);
            timer.Elapsed += SendThreadFromWaitingToWorking!;
            timer.Start();

        }

        private void SendThreadFromWaitingToWorking(object sender, ElapsedEventArgs e)
        {
            
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                if (WaitingThreads.Count != 0)
                {
                    SelectedWorkingMyThread = WaitingThreads[0];
                    SelectedWorkingMyThread.Situation = nameof(Situation.Working);
                    WorkingThreads.Add(SelectedWorkingMyThread);
                    WaitingThreads.Remove(SelectedWorkingMyThread);
                }
            });
        }

        #endregion

    }
}
