using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CompactModel.ViewModels
{
    internal class GraphViewModel : ViewModelBase
    {
        private readonly ProcessContainerViewModel container;
        public GraphViewModel(ProcessContainerViewModel container)
        {
            this.container = container;
            Draw();
        }

        private const double radius = 30;
        private const double lFont = 2 * radius / 3;
        private const double mFont = radius / 2;
        private readonly Brush brush = Brushes.Yellow;
        private readonly Pen pen = new Pen(Brushes.Black, 2);
        private readonly Vector vRadius = new Vector(radius, 0);
        private readonly Vector vArc = new Vector(4 * radius, 0);
        private readonly Vector vNextNode = new Vector(6 * radius, 0);
        private readonly Vector vTextNode = new Vector(-radius * Math.Cos(Math.PI / 4), -radius * Math.Sin(Math.PI / 4)) * 2 / 3;

        private void Draw()
        {
            var host = new DrawingVisual();
            using (var dc = host.RenderOpen())
            {
                for (int index = 0; index < container.ProcessStatuses.Length; index++)
                {
                    var point = new Point(1.5 * radius, 0) + index * vNextNode;

                    //петля
                    var nose = GetNoose(container.ProcessStatuses[index]);
                    if (nose != null)
                    {
                        dc.DrawEllipse(null, pen, point + new Vector(-Math.Cos(Math.PI / 3), Math.Sin(Math.PI / 3)) * 1.2 * radius, 0.8 * radius, 0.8 * radius);
                        var textNoose = new FormattedText($"{nose.ArcSign},{nose.CSign},{nose.ActivePredicateSign},{nose.Priority}",
                            CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), mFont, pen.Brush, 60);
                        dc.DrawText(textNoose, point + new Vector(-1.5 * radius, 2 * radius));
                    }

                    dc.DrawEllipse(brush, pen, point, radius, radius);
                    var textNode = new FormattedText(container.ProcessStatuses[index].Sign,
                        CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), lFont, pen.Brush, 60);
                    dc.DrawText(textNode, point + vTextNode);


                    if (index + 1 < container.ProcessStatuses.Length)
                    {
                        var arc = GetArc(container.ProcessStatuses[index], container.ProcessStatuses[index + 1]);
                        if (arc != null)
                        {
                            dc.DrawLine(pen, point + vRadius, point + vNextNode);
                            var textArc = new FormattedText($"{arc.ArcSign},{arc.CSign},{arc.ActivePredicateSign},{arc.Priority}",
                            CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), mFont, pen.Brush, 60);
                            dc.DrawText(textArc, point + new Vector(1.3 * radius, 0.3 * radius));
                        }
                    }
                }
            }
            Drawing = host;
        }

        private ArcProcessViewModel GetNoose(ProcessStatusViewModel status)
        {
            return GetArc(status, status);
        }

        private ArcProcessViewModel GetArc(ProcessStatusViewModel source, ProcessStatusViewModel target)
        {
            if (target is null || source is null)
                return null;

            return container.ArcProcesses.FirstOrDefault(x => x.StatusSource == source && x.StatusTarget == target);
        }

        public DrawingVisual Drawing
        {
            get { return drawing; }
            private set { drawing = value; OnPropertyChanged(nameof(Drawing)); }
        }
        private DrawingVisual drawing;
    }
}
