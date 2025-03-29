using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace Bau.Controls.Panels;

[ContentProperty("InnerContent")]
public partial class CollapsiblePanel : UserControl
{
    // Propiedades de dependencia
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(CollapsiblePanel), new PropertyMetadata("Título"));

    public static readonly DependencyProperty IsCollapsedProperty =
        DependencyProperty.Register(nameof(IsCollapsed), typeof(bool), typeof(CollapsiblePanel), new PropertyMetadata(false, OnIsCollapsedChanged));

    public static readonly DependencyProperty PositionProperty =
        DependencyProperty.Register(nameof(Position), typeof(Dock), typeof(CollapsiblePanel), new PropertyMetadata(Dock.Left, OnPositionChanged));
    public static readonly DependencyProperty InnerContentProperty =
        DependencyProperty.Register("InnerContent", typeof(object), typeof(CollapsiblePanel));

    // Variables privadas
    private double _previousWidth, _previousHeight;

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public bool IsCollapsed
    {
        get { return (bool)GetValue(IsCollapsedProperty); }
        set { SetValue(IsCollapsedProperty, value); }
    }

    public Dock Position
    {
        get { return (Dock)GetValue(PositionProperty); }
        set { SetValue(PositionProperty, value); }
    }

    public object InnerContent
    {
        get { return GetValue(InnerContentProperty); }
        set { SetValue(InnerContentProperty, value); }
    }

    public CollapsiblePanel()
    {
        InitializeComponent();
        TitleTextBlock.Text = Title;
        TitleBorder.MouseLeftButtonDown += TitleBorder_MouseLeftButtonDown;
        UpdatePosition();
    }

    private static void OnIsCollapsedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (CollapsiblePanel)d;
        control.AnimateCollapse();
    }

    private static void OnPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (CollapsiblePanel)d;
        control.UpdatePosition();
    }

    private void TitleBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        IsCollapsed = !IsCollapsed;
    }

    private void UpdatePosition()
    {
        switch (Position)
        {
            case Dock.Left:
                    Grid.SetRow(TitleBorder, 0);
                    Grid.SetColumn(TitleBorder, 0);
                    Grid.SetRowSpan(TitleBorder, 3);
                    Grid.SetColumnSpan(TitleBorder, 1);
                    TitleTextBlock.LayoutTransform = new RotateTransform(-90);
                break;
            case Dock.Right:
                    Grid.SetRow(TitleBorder, 0);
                    Grid.SetColumn(TitleBorder, 2);
                    Grid.SetRowSpan(TitleBorder, 3);
                    Grid.SetColumnSpan(TitleBorder, 1);
                    TitleTextBlock.LayoutTransform = new RotateTransform(90);
                break;
            case Dock.Top:
                    Grid.SetRow(TitleBorder, 0);
                    Grid.SetColumn(TitleBorder, 0);
                    Grid.SetRowSpan(TitleBorder, 1);
                    Grid.SetColumnSpan(TitleBorder, 3);
                    TitleTextBlock.LayoutTransform = new RotateTransform(0);
                break;
            case Dock.Bottom:
                    Grid.SetRow(TitleBorder, 2);
                    Grid.SetColumn(TitleBorder, 0);
                    Grid.SetRowSpan(TitleBorder, 1);
                    Grid.SetColumnSpan(TitleBorder, 3);
                    TitleTextBlock.LayoutTransform = new RotateTransform(0);
                break;
        }
    }

    private void AnimateCollapse()
    {
        if (Position == Dock.Left || Position == Dock.Right)
        {
            if (IsCollapsed)
            {
                _previousWidth = ActualWidth;
                MinWidth = MaxWidth = 20;
            }
            else
                MinWidth = MaxWidth = Math.Max(_previousWidth, 50);
        }
        else
        {
            if (IsCollapsed)
            {
                _previousHeight = ActualHeight;
                MinHeight = MaxHeight = 20;
            }
            else
                MinHeight = MaxHeight = Math.Max(_previousHeight, 50);
        }
    }
}
