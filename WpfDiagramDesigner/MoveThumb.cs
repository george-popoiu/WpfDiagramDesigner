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

        public MoveThumb() {
            DragDelta += new DragDeltaEventHandler(MoveThumb_DragDelta);
        }

        void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e) {
            var item = this.DataContext as Control;

            if (item != null) {
                double left = Canvas.GetLeft(item);
                double top = Canvas.GetTop(item);

                Canvas.SetLeft(item, left + e.HorizontalChange);
                Canvas.SetTop(item, top + e.VerticalChange);
            }
        }

    }
}
