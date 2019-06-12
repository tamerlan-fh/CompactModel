using CompactModel.Helpers;

namespace CompactModel.ViewModels
{
    internal class ProcessViewModel : ViewModelBase
    {
        public ProcessViewModel(uint index)
        {
            Index = index;
            Sign = $"P{FootnotesHelper.Parse(Index)}";
        }

        public uint Index { get; }

        public string Sign { get; }

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }
        private string name;
    }
}
