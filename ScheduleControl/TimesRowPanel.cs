using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ScheduleControl
{
    public class TimesRowPanel : Panel
    {
        public static readonly DependencyProperty MinRowHeightProperty =
           DependencyProperty.Register("MinRowHeight", typeof(int), typeof(TimesRowPanel),
               new FrameworkPropertyMetadata(10, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty MinutePerPixelProperty =
           DependencyProperty.Register("MinutePerPixel", typeof(double), typeof(TimesRowPanel),
               new FrameworkPropertyMetadata(3.2, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty MinTimeProperty =
           DependencyProperty.Register("MinTime", typeof(DateTime), typeof(TimesRowPanel), 
               new FrameworkPropertyMetadata(DateTime.MinValue,FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty MaxTimeProperty =
           DependencyProperty.Register("MaxTime", typeof(DateTime), typeof(TimesRowPanel),
               new FrameworkPropertyMetadata(DateTime.MaxValue, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty StartJobbProperty =
           DependencyProperty.RegisterAttached("StartJobb", typeof(DateTime), typeof(TimesRowPanel), 
               new FrameworkPropertyMetadata(DateTime.MinValue, FrameworkPropertyMetadataOptions.AffectsParentArrange));

        public static readonly DependencyProperty EndJobbProperty =
            DependencyProperty.RegisterAttached("EndJobb", typeof(DateTime), typeof(TimesRowPanel), 
                new FrameworkPropertyMetadata(DateTime.MaxValue, FrameworkPropertyMetadataOptions.AffectsParentArrange));

        public double MinutePerPixel
        {
            get { return (double)GetValue(MinutePerPixelProperty); }
            set { SetValue(MinutePerPixelProperty, value); }
        }
        public int MinRowHeight
        {
            get { return (int)GetValue(MinRowHeightProperty); }
            set { SetValue(MinRowHeightProperty, value); }
        }
        public DateTime MinTime
        {
            get { return (DateTime)GetValue(MinTimeProperty); }
            set { SetValue(MinTimeProperty, value); }
        }
        public DateTime MaxTime
        {
            get { return (DateTime)GetValue(MaxTimeProperty); }
            set { SetValue(MaxTimeProperty, value); }
        }

        public static DateTime GetStartJobb(DependencyObject obj)
        {
            return (DateTime)obj.GetValue(StartJobbProperty);
        }

        public static void SetStartJobb(DependencyObject obj, DateTime value)
        {
            obj.SetValue(StartJobbProperty, value);
        }

        public static DateTime GetEndJobb(DependencyObject obj)
        {
            return (DateTime)obj.GetValue(EndJobbProperty);
        }

        public static void SetEndJobb(DependencyObject obj, DateTime value)
        {
            obj.SetValue(EndJobbProperty, value);
        }


        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
            }
            double width = MinutePerPixel * (MaxTime - MinTime).TotalMinutes;
            if( width < 0 )
            {
                width = 0;
            }
            return new Size(width, MinRowHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            DateTime start;
            DateTime end;
            double width;
            double offset = finalSize.Width;

            foreach (UIElement child in Children)
            {
                start = GetStartJobb(child);
                end = GetEndJobb(child);
                width = MinutePerPixel * (end - start).TotalMinutes;
                offset = MinutePerPixel * (start - MinTime).TotalMinutes;
                if( offset < 0 )
                {
                    width += offset;
                    offset = 0;
                }

                child.Arrange(new Rect(offset, 0, width, finalSize.Height));

            }
            return finalSize;
        }
    }
}
