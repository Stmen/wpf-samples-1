using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IGExtensions.Framework.Controls
{
    /// <summary>
    /// Represents the Content Scale Viewer that scales its content with aspect ratio
    /// </summary>
    public class ContentScaleControl : Panel //Panel  
    {
        /// <summary>
        /// Constructs an instance of the AppScaleContainer
        /// </summary>
        public ContentScaleControl()
        {
            //this.DefaultStyleKey = typeof(ContentScaleControl);
            // add the generic style to the control's resources
            const string stylePath = "/IGExtensions.Framework;component/Controls/ContentScaleControl.xaml";
            this.ApplyStyle(stylePath);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            Size desiredSize = availableSize;

            foreach (Panel child in Children)
            {
                child.Measure(availableSize);
                desiredSize = child.DesiredSize;
            }


            //var child = this.Content as Panel;
            //if (child != null)
            //{
            //    child.Measure(availableSize);
            //    desiredSize = child.DesiredSize;
            //}

            return desiredSize;
            //return base.MeasureOverride(availableSize);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            //var child = this.Content as Panel;
            //if (child != null)

            foreach (Panel child in Children)
            {
                child.Arrange(new Rect(0, 0, finalSize.Width, Math.Max(finalSize.Height, child.MinHeight)));

                if (finalSize.Height < child.MinHeight)
                {
                    double scale = finalSize.Height / child.MinHeight;

                    var transformGroup = new TransformGroup();
                    transformGroup.Children.Add(new ScaleTransform { ScaleY = scale, ScaleX = scale });
                    transformGroup.Children.Add(new TranslateTransform { X = 0.5 * (finalSize.Width - scale * finalSize.Width) });

                    child.RenderTransform = transformGroup;
                }
                else
                {
                    child.RenderTransform = null;
                }
            }

            return finalSize;

            //return base.ArrangeOverride(finalSize);
        }

    }
}