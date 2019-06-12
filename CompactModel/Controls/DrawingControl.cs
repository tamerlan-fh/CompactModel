using System;
using System.Windows;
using System.Windows.Media;

namespace CompactModel.Controls
{
    internal class DrawingControl : FrameworkElement
    {
        public static readonly DependencyProperty SourceProperty =
          DependencyProperty.Register(nameof(Source), typeof(DrawingVisual), typeof(DrawingControl),
              new FrameworkPropertyMetadata(new PropertyChangedCallback(SourcePropertyChanged)));

        private static void SourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DrawingControl control)
                control.Apply();
        }

        public DrawingVisual Source
        {
            get { return (DrawingVisual)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private void Apply()
        {
            visuals.Clear();
            visuals.Add(Source);
            Width = Source?.ContentBounds.Width ?? 0;
            Height = Source?.ContentBounds.Height ?? 0;
        }

        public DrawingControl()
        {
            visuals = new VisualCollection(this);
        }

        private VisualCollection visuals;

        protected override int VisualChildrenCount { get { return visuals.Count; } }

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= visuals.Count) { throw new ArgumentOutOfRangeException(); }

            return visuals[index];
        }
    }
}
