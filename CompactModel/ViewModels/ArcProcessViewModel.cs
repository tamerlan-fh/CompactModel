using CompactModel.Helpers;
using System.ComponentModel;
using System.Linq;

namespace CompactModel.ViewModels
{
    internal class ArcProcessViewModel : ViewModelBase
    {
        private ProcessContainerViewModel container;

        public ArcProcessViewModel(ProcessContainerViewModel container, uint index)
        {
            this.container = container;
            Index = index;
            ArcSign = $"d{FootnotesHelper.Parse(container.Index)}{FootnotesHelper.Parse(Index)}";
            CSign = $"C{FootnotesHelper.Parse(container.Index)}{FootnotesHelper.Parse(Index)}";
            ActivePredicateSign = $"P{FootnotesHelper.Parse(container.Index)}{FootnotesHelper.Parse(Index)}";

            container.PropertyChanged += ContainerPropertyChanged;
        }

        private void ContainerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ProcessContainerViewModel.ProcessStatuses))
                OnPropertyChanged(nameof(ProcessStatuses));
        }

        public uint Index { get; }

        /// <summary>
        /// Обозначение дуги
        /// </summary>
        public string ArcSign { get; }

        /// <summary>
        /// Символ алфавита C
        /// </summary>
        public string CSign { get; }

        /// <summary>
        /// Обозначение предиката активновти
        /// </summary>
        public string ActivePredicateSign { get; }

        /// <summary>
        /// Приоритет
        /// </summary>
        public uint Priority
        {
            get { return priority; }
            set { priority = value; OnPropertyChanged(nameof(Priority)); }
        }
        private uint priority;

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged(nameof(Message)); }
        }
        private string message;

        /// <summary>
        /// Условие
        /// </summary>
        public string Condition
        {
            get { return condition; }
            set { condition = value; OnPropertyChanged(nameof(Condition)); }
        }
        private string condition;

        /// <summary>
        /// Источник
        /// </summary>
        public ProcessStatusViewModel StatusSource
        {
            get { return statusSource; }
            set { statusSource = value; OnPropertyChanged(nameof(StatusSource)); }
        }
        private ProcessStatusViewModel statusSource;

        /// <summary>
        /// Назначение
        /// </summary>
        public ProcessStatusViewModel StatusTarget
        {
            get { return statusTarget; }
            set { statusTarget = value; OnPropertyChanged(nameof(StatusTarget)); }
        }
        private ProcessStatusViewModel statusTarget;

        /// <summary>
        /// Запуск процесса
        /// </summary>
        public ProcessViewModel StartProcess
        {
            get { return startProcess; }
            set
            {
                if (StartProcess == value)
                    return;

                startProcess = value; OnPropertyChanged(nameof(StartProcess));
            }
        }
        private ProcessViewModel startProcess;

        public ProcessStatusViewModel[] ProcessStatuses
        {
            get { return container.ProcessStatuses; }
        }

        public ProcessViewModel[] Processes
        {
            get { return container.OtherProcesses; }
        }
    }
}
