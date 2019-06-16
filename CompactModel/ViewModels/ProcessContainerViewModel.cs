using System.Linq;
using System.Windows.Input;

namespace CompactModel.ViewModels
{
    internal class ProcessContainerViewModel : WorkspaceViewModel
    {
        public ProcessContainerViewModel(uint index)
        {
            Index = index;
            Process = new ProcessViewModel(Index);
            Processes = new ProcessViewModel[] { Process };

            SetProcessStatusCountCommand = new RelayCommand(obj =>
            {
                if (int.TryParse(obj?.ToString(), out int count))
                    SetProcessStatusCount(count);
            },
            obj =>
            {
                if (obj is null) return false;
                if (!int.TryParse(obj.ToString(), out int count))
                    return false;
                return CanSetProcessStatusCount(count);
            });

            SetArcProcessCountCommand = new RelayCommand(obj =>
            {
                if (int.TryParse(obj?.ToString(), out int count))
                    SetArcProcessCount(count);
            },
            obj =>
            {
                if (obj is null) return false;
                if (!int.TryParse(obj.ToString(), out int count))
                    return false;
                return CanSetArcProcessCount(count);
            });

            SaveCommand = new RelayCommand(obj => Save(), obj => CanSave());
            GraphShowCommand = new RelayCommand(obj => GraphShow(), obj => CanGraphShow());

            DisplayName = Process.Sign;

            SetProcessStatusCount(4);
            SetArcProcessCount(7);
        }

        public uint Index { get; }

        public ProcessViewModel Process { get; }

        public ProcessViewModel[] Processes { get; }

        public ProcessStatusViewModel[] ProcessStatuses
        {
            get { return processStatuses; }
            private set { processStatuses = value; OnPropertyChanged(nameof(ProcessStatuses)); }
        }
        private ProcessStatusViewModel[] processStatuses;

        public ArcProcessViewModel[] ArcProcesses
        {
            get { return acProcesses; }
            private set { acProcesses = value; OnPropertyChanged(nameof(ArcProcesses)); }
        }
        private ArcProcessViewModel[] acProcesses;

        public ProcessViewModel[] OtherProcesses
        {
            get { return otherProcesses; }
            set { otherProcesses = value; OnPropertyChanged(nameof(OtherProcesses)); }
        }
        private ProcessViewModel[] otherProcesses;

        #region SetProcessStatusCountCommand

        public RelayCommand SetProcessStatusCountCommand { get; }

        private void SetProcessStatusCount(int count)
        {
            ProcessStatuses = Enumerable.Range(1, count).Select(index => new ProcessStatusViewModel(Process, (uint)index)).ToArray();
        }

        private bool CanSetProcessStatusCount(int count)
        {
            return count > 1;
        }

        #endregion SetProcessStatusCountCommand

        #region SetArcProcessCountCommand

        public RelayCommand SetArcProcessCountCommand { get; }

        private void SetArcProcessCount(int count)
        {
            ArcProcesses = Enumerable.Range(1, count).Select(index => new ArcProcessViewModel(this, (uint)index)).ToArray();
        }

        private bool CanSetArcProcessCount(int count)
        {
            return count > 1;
        }

        #endregion SetArcProcessCountCommand

        #region SaveCommand

        public ICommand SaveCommand { get; }

        private void Save()
        {

        }

        private bool CanSave()
        {
            return true;
        }

        #endregion SaveCommand

        #region GraphShowCommand

        public ICommand GraphShowCommand { get; }

        private void GraphShow()
        {
            var window = new Views.GraphWindow()
            {
                Title = $"Граф процесса {Process.Sign}",
                DataContext = new GraphViewModel(this)
            };
            window.Show();
        }

        private bool CanGraphShow()
        {
            return true;
        }

        #endregion GraphShowCommand
    }
}
