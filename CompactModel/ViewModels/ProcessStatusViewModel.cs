using CompactModel.Helpers;

namespace CompactModel.ViewModels
{
    internal class ProcessStatusViewModel : ViewModelBase
    {
        private readonly ProcessViewModel process;

        public ProcessStatusViewModel(ProcessViewModel process, uint index)
        {
            this.process = process;
            Index = index;
            Sign = $"S{FootnotesHelper.Parse(process.Index)}{FootnotesHelper.Parse(Index)}";
        }
        public uint Index { get; }

        public string Sign { get; }

        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(nameof(Status)); }
        }
        private string status;
    }
}
