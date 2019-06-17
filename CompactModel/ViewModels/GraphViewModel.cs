using CompactModel.Helpers;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace CompactModel.ViewModels
{
    internal class GraphViewModel : ViewModelBase
    {
        private readonly ProcessContainerViewModel container;
        private readonly ProcessContainerViewModel[] containers;
        public GraphViewModel(ProcessContainerViewModel container)
        {
            this.container = container;
            DrawGraph();
        }

        public GraphViewModel(ProcessContainerViewModel[] containers)
        {
            this.containers = containers;
            DrawCompactGraph();
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

        private void DrawGraph()
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
                        var _radius = 0.8 * radius;
                        var _vector = new Vector(-Math.Cos(Math.PI / 3), Math.Sin(Math.PI / 3)) * 1.2 * radius;
                        // вычисление точки пересечения
                        //var l1 = _vector.Length;
                        //var l2 = radius;
                        //var l3 = _radius;
                        //var p = (l1 + l2 + l3) / 2;
                        //var s = Math.Sqrt(p * (p - l1) * (p - l2) * (p - l3));
                        //var h = 2 * s / l1;
                        //var b = Math.Sqrt(radius * radius - h * h);
                        //var v = new Vector(_vector.X, _vector.Y);
                        //v.Normalize();
                        //var n = new Vector(v.Y, -v.X);
                        var _v = new Vector(5.9346588560844, 29.4071390016421);

                        dc.DrawEllipse(null, pen, point + _vector, _radius, _radius);
                        DrawArrow(dc, point + _v, new Vector(0, -1));
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
                            var start = point + vRadius;
                            var end = point + vNextNode - vRadius;
                            dc.DrawLine(pen, start, end);
                            DrawArrow(dc, end, new Vector(vNextNode.X, vNextNode.Y));
                            var textArc = new FormattedText($"{arc.ArcSign},{arc.CSign},{arc.ActivePredicateSign},{arc.Priority}",
                            CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), mFont, pen.Brush, 60);
                            dc.DrawText(textArc, point + new Vector(1.3 * radius, 0.3 * radius));
                        }
                    }
                }
            }
            Drawing = host;
        }

        private void DrawCompactGraph()
        {
            var host = new DrawingVisual();

            var count = containers.Max(x => x.ProcessStatuses.Length);

            using (var dc = host.RenderOpen())
            {
                for (int index = 0; index < count; index++)
                {
                    var point = new Point(1.5 * radius, 0) + index * vNextNode;

                    //петля
                    var nose = GetNooseEx(index);
                    if (nose.Any())
                    {
                        var _radius = 0.8 * radius;
                        var _vector = new Vector(-Math.Cos(Math.PI / 3), Math.Sin(Math.PI / 3)) * 1.2 * radius;
                        var _v = new Vector(5.9346588560844, 29.4071390016421);

                        dc.DrawEllipse(null, pen, point + _vector, _radius, _radius);
                        DrawArrow(dc, point + _v, new Vector(0, -1));

                        var textNoose = new FormattedText($"{nose.Max(x => x.Priority)}\r\n[{string.Join(";", nose.Select(x => x.ActivePredicateSign))}]\r\n[{string.Join(";", nose.Select(x => x.CSign))}]",
                            CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), mFont, pen.Brush, 60);
                        dc.DrawText(textNoose, point + new Vector(-1.5 * radius, 2 * radius));
                    }

                    dc.DrawEllipse(brush, pen, point, radius, radius);
                    var textNode = new FormattedText($"S{FootnotesHelper.Parse((uint)index + 1)}",
                        CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), lFont, pen.Brush, 60);
                    dc.DrawText(textNode, point + vTextNode);


                    if (index + 1 < count)
                    {
                        var arc = GetArcEx(index, index + 1);
                        if (arc.Any())
                        {
                            var start = point + vRadius;
                            var end = point + vNextNode - vRadius;
                            dc.DrawLine(pen, start, end);
                            DrawArrow(dc, end, new Vector(vNextNode.X, vNextNode.Y));
                            var textArc = new FormattedText($"{arc.Max(x => x.Priority)}\r\n[{string.Join(";", arc.Select(x => x.ActivePredicateSign))}]\r\n[{string.Join(";", arc.Select(x => x.CSign))}]",
                            CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), mFont, pen.Brush, 60);
                            dc.DrawText(textArc, point + new Vector(1.3 * radius, 0.3 * radius));
                        }
                    }
                }
            }
            Drawing = host;
        }

        private void DrawArrow(DrawingContext dc, Point point, Vector vector)
        {
            vector.Normalize();
            var normal = (radius / 5) * new Vector(vector.Y, -vector.X);
            vector *= (radius / 3);

            var start = point;
            var left = start - vector + normal;
            var right = start - vector - normal;

            var segments = new LineSegment[]
            {
                new LineSegment(left, true),
                new LineSegment(right, true)
            };

            var figure = new PathFigure(start, segments, true);
            var geometry = new PathGeometry(new[] { figure });

            dc.DrawGeometry(pen.Brush, null, geometry);
        }

        private ArcProcessViewModel GetNoose(ProcessStatusViewModel status)
        {
            return GetArc(status, status);
        }

        private ArcProcessViewModel[] GetNooseEx(int index)
        {
            return GetArcEx(index, index);
        }

        private ArcProcessViewModel GetArc(ProcessStatusViewModel source, ProcessStatusViewModel target)
        {
            if (target is null || source is null)
                return null;

            return container.ArcProcesses.FirstOrDefault(x => x.StatusSource == source && x.StatusTarget == target);
        }

        private ArcProcessViewModel[] GetArcEx(int sourceIndex, int targetIndex)
        {
            if (sourceIndex < 0 || targetIndex < 0)
                return new ArcProcessViewModel[] { };

            return containers.Where(c => c.ProcessStatuses.Length > sourceIndex && c.ProcessStatuses.Length > targetIndex)
                .Select(c =>
                {
                    var source = c.ProcessStatuses[sourceIndex];
                    var target = c.ProcessStatuses[targetIndex];
                    return c.ArcProcesses.FirstOrDefault(x => x.StatusSource == source && x.StatusTarget == target);
                })
                .Where(x => x != null)
                .ToArray();
        }

        public DrawingVisual Drawing
        {
            get { return drawing; }
            private set { drawing = value; OnPropertyChanged(nameof(Drawing)); }
        }
        private DrawingVisual drawing;
    }
}
