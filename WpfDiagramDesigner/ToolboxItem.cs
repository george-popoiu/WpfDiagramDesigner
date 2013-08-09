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
using System.Windows.Markup;

namespace WpfDiagramDesigner {
    public class ToolboxItem : ContentControl {

        Point? dragStartPoint = null;

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e) {
            base.OnPreviewMouseDown(e);
            dragStartPoint = new Point?(e.GetPosition(this));
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            if (e.LeftButton != MouseButtonState.Pressed) {
                dragStartPoint = null;
            }

            if (dragStartPoint.HasValue) {                
                Point position = e.GetPosition(this);
                
                if (SystemParameters.MinimumHorizontalDragDistance <= Math.Abs(dragStartPoint.Value.X - position.X) ||
                    SystemParameters.MinimumVerticalDragDistance <= Math.Abs(dragStartPoint.Value.Y - position.Y)) {
                        string xamlString = XamlWriter.Save(Content);
                        DataObject dataObject = new DataObject("DESIGNER_ITEM", xamlString);
                        if (dataObject != null) {
                            DragDrop.DoDragDrop(this, dataObject, DragDropEffects.Copy);
                        }
                }

                e.Handled = true;
            }
        }

    }
}
