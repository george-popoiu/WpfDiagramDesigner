using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml;

namespace WpfDiagramDesigner {
    class DesignerCanvas : Canvas {

        public IEnumerable<DesignerItem> SelectedItems {
            get {
                var selectedItems = from item in this.Children.OfType<DesignerItem>()
                                    where item.IsSelected == true
                                    select item;
                return selectedItems;
            }
        }

        public void DeselectAll() {
            foreach (DesignerItem item in SelectedItems) {
                item.IsSelected = false;
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e) {
            base.OnMouseDown(e);

            if (e.Source == this) {
                DeselectAll();
                e.Handled = true;
            }            
        }

        protected override void OnDrop(DragEventArgs e) {
            base.OnDrop(e);
            string xamlString = e.Data.GetData("DESIGNER_ITEM") as string;
            if(!string.IsNullOrEmpty(xamlString)) {
                DesignerItem newItem = null;
                FrameworkElement content = XamlReader.Load(XmlReader.Create(new StringReader(xamlString))) as FrameworkElement;

                if (content != null) {
                    newItem = new DesignerItem();
                    newItem.Content = content;

                    if (content.MinWidth != 0 && content.MinHeight != 0) {
                        newItem.Width = content.MinWidth * 3;
                        newItem.Height = content.MinHeight * 2;
                    }
                    else {
                        newItem.Height = 60;
                        newItem.Width = 120;
                    }

                    Point position = e.GetPosition(this);
                    DesignerCanvas.SetLeft(newItem, Math.Max(0, position.X - newItem.Width / 2));
                    DesignerCanvas.SetTop(newItem, Math.Max(0, position.Y - newItem.Height / 2));

                    newItem.Style = (Style)this.FindResource("DesignerItemStyle");
                    Children.Add(newItem);

                    DeselectAll();
                    newItem.IsSelected = true;
                }

                e.Handled = true;
            }
        }

        protected override Size MeasureOverride(Size constraint) {
            Size size = new Size();
            
            foreach (UIElement element in base.Children) {
                double left = Canvas.GetLeft(element);
                double top = Canvas.GetTop(element);
                left = double.IsNaN(left) ? 0 : left;
                top = double.IsNaN(top) ? 0 : top;

                element.Measure(constraint);
                Size desiredSize = element.DesiredSize;
                if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height)) {
                    size.Width = Math.Max(size.Width, left + desiredSize.Width);
                    size.Height = Math.Max(size.Height, top + desiredSize.Height);
                }
            }

            size.Width += 10;
            size.Height += 10;
            return size;
        }

    }
}
