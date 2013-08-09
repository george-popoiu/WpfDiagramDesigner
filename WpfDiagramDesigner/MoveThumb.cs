using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDiagramDesigner {
    public class MoveThumb : Thumb {
        DesignerItem designerItem;
        DesignerCanvas designerCanvas;

        public MoveThumb() {
            DragDelta += new DragDeltaEventHandler(MoveThumb_DragDelta);
            DragStarted += new DragStartedEventHandler(MoveThumb_DragStarted);
        }

        void MoveThumb_DragStarted(object sender, DragStartedEventArgs e) {
            designerItem = this.DataContext as DesignerItem;

            if (designerItem != null) {
                designerCanvas = VisualTreeHelper.GetParent(designerItem) as DesignerCanvas;
            }
        }

        void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e) {
            var designerItem = this.DataContext as ContentControl;

            if (designerItem != null && designerCanvas != null) {
                double left = Canvas.GetLeft(designerItem);
                double top = Canvas.GetTop(designerItem);

                Point dragDelta = new Point(e.HorizontalChange, e.VerticalChange);

                var rotateTransform = designerItem.RenderTransform as RotateTransform;
                if (rotateTransform != null) {
                    dragDelta = rotateTransform.Transform(dragDelta);
                }

                Canvas.SetLeft(designerItem, left + dragDelta.X);
                Canvas.SetTop(designerItem, top + dragDelta.Y);

                designerCanvas.InvalidateMeasure();
                e.Handled = true;
            }
        }

    }
}
