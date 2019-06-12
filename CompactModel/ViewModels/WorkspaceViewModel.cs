using System;

namespace CompactModel.ViewModels
{
    internal abstract class WorkspaceViewModel : ViewModelBase
    {
        public string DisplayName { get; protected set; }

        protected WorkspaceViewModel()
        {
            CloseCommand = new RelayCommand(obj => OnRequestClose());
        }

        public RelayCommand CloseCommand { get; }

        public event EventHandler RequestClose;

        void OnRequestClose()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}
