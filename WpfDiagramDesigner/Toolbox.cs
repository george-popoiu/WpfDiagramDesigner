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
    public class Toolbox : ItemsControl {

        Size defaultItemSize = new Size(65, 65);

        public Size DefaultItemSize {
            get { return defaultItemSize; }
            set { defaultItemSize = value; }
        }

        protected override DependencyObject GetContainerForItemOverride() {
            return new ToolboxItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item) {
            return (item is ToolboxItem);
        }
    }
}
