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

    public class RotateThumb : Thumb {
        Point centerPoint;
        Vector startVector;

        double initialAngle;

        Canvas designerCanvas;
        ContentControl designerItem;
        RotateTransform rotateTransform;

        public RotateThumb() {
            this.DragStarted += new DragStartedEventHandler(RotateThumb_DragStarted);
            this.DragDelta += new DragDeltaEventHandler(RotateThumb_DragDelta);
        }

        void RotateThumb_DragDelta(object sender, DragDeltaEventArgs e) {
            if (designerItem != null && designerCanvas != null) {
                Point currentPoint = Mouse.GetPosition(designerCanvas);
                Vector deltaVector = Point.Subtract(currentPoint, centerPoint);

                double angle = Vector.AngleBetween(startVector, deltaVector);

                RotateTransform rotateTransform = designerItem.RenderTransform as RotateTransform;
                rotateTransform.Angle = initialAngle + Math.Round(angle, 0);                
                designerItem.InvalidateMeasure();
            }
        }

        void RotateThumb_DragStarted(object sender, DragStartedEventArgs e) {
            designerItem = this.DataContext as ContentControl;

            if (designerItem != null) {

                designerCanvas = VisualTreeHelper.GetParent(designerItem) as Canvas;

                if (designerCanvas != null) {

                    this.centerPoint = this.designerItem.TranslatePoint(
                        new Point(designerItem.Width * designerItem.RenderTransformOrigin.X,
                                    designerItem.Height * designerItem.RenderTransformOrigin.Y),
                        designerCanvas);

                    Point startPoint = Mouse.GetPosition(designerCanvas);
                    startVector = Point.Subtract(startPoint, centerPoint);

                    this.rotateTransform = this.designerItem.RenderTransform as RotateTransform;
                    if (rotateTransform == null) {
                        designerItem.RenderTransform = new RotateTransform(0);
                        initialAngle = 0;
                    }
                    else {
                        initialAngle = rotateTransform.Angle;
                    }
                }
            }
        }

    }
}
