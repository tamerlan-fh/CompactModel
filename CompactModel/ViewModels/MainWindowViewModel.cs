using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace CompactModel.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            Workspaces = new ObservableCollection<WorkspaceViewModel>();
            Workspaces.CollectionChanged += OnWorkspacesChanged;

            SetProcessCountCommand = new RelayCommand(obj =>
            {
                if (int.TryParse(obj?.ToString(), out int count))
                    SetProcessCount(count);
            },
            obj =>
            {
                if (obj is null) return false;
                if (!int.TryParse(obj.ToString(), out int count))
                    return false;
                return CanSetProcessCount(count);
            });
            CompactGraphShowCommand = new RelayCommand(obj => CompactGraphShow(), obj => CanCompactGraphShow());
            SetProcessCount(4);
        }

        public ObservableCollection<WorkspaceViewModel> Workspaces { get; }

        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                {
                    workspace.RequestClose += this.OnWorkspaceRequestClose;
                }
            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                {
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
                }
            SetOtherProcesses();
        }

        private void SetOtherProcesses()
        {
            var processes = Containers
                .Select(x => x.Process)
                .ToArray();

            foreach (var process in Containers)
            {
                process.OtherProcesses = processes
                    .Where(x => x != process.Process)
                    .ToArray();
            }
        }

        private ProcessContainerViewModel[] Containers
        {
            get
            {
                return Workspaces
                    .Where(x => x is ProcessContainerViewModel)
                    .Cast<ProcessContainerViewModel>()
                    .ToArray();
            }
        }

        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            if (sender is WorkspaceViewModel workspace)
            {
                workspace.Dispose();
                Workspaces.Remove(workspace);
            }
        }

        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            var collectionView = CollectionViewSource.GetDefaultView(Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }

        #region SetProcessCountCommand

        public ICommand SetProcessCountCommand { get; }

        private void SetProcessCount(int count)
        {
            Workspaces.Clear();
            foreach (var workspace in Enumerable.Range(0, count).Select(index => new ProcessContainerViewModel((uint)index)))
                Workspaces.Add(workspace);

            SetActiveWorkspace(Workspaces.FirstOrDefault());
        }

        private bool CanSetProcessCount(int count)
        {
            return count > 1;
        }

        #endregion SetArcProcessCountCommand

        #region CompactGraphShowCommand

        public ICommand CompactGraphShowCommand { get; }

        private void CompactGraphShow()
        {
            var window = new Views.GraphWindow()
            {
                Title = $"Компактная модель",
                DataContext = new GraphViewModel(Containers)
            };
            window.Show();
        }

        private bool CanCompactGraphShow()
        {
            return true;
        }

        #endregion CompactGraphShowCommand
    }
}
