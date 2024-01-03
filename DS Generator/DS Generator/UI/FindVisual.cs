using System.Windows;
using System.Windows.Media;

namespace DS_Generator.UI;

public class FindVisual
{
    // eccoci
    public T FindVisualChild<T>(DependencyObject parent, string name) where T : FrameworkElement
    {
        if (parent == null)
            return null;

        if (parent is T frameworkElement && frameworkElement.Name == name)
        {
            return frameworkElement;
        }

        T foundChild = null;
        int childCount = VisualTreeHelper.GetChildrenCount(parent);

        for (int i = 0; i < childCount; i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(parent, i);
            foundChild = FindVisualChild<T>(child, name);

            if (foundChild != null)
                break;
        }

        return foundChild;
    }

    
    public T FindVisualParent<T>(DependencyObject? child) where T : DependencyObject
    {
        while (child != null && !(child is T))
        {
            child = VisualTreeHelper.GetParent(child);
        }

        return child as T;
    }
}