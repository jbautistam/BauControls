using System.Windows;
using System.Windows.Media;

namespace Bau.Controls.Shapes;

/// <summary>
///     Línea con cabezas de flecha al principio y / o final
/// </summary>
public class ArrowLine : ArrowLineBase
{
    // Propiedades
    public static readonly DependencyProperty X1Property = DependencyProperty.Register(nameof(X1), typeof(double), typeof(ArrowLine), 
                                                                                       new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));
    public static readonly DependencyProperty Y1Property = DependencyProperty.Register(nameof(Y1), typeof(double), typeof(ArrowLine),
                                                                                       new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));
    public static readonly DependencyProperty X2Property = DependencyProperty.Register(nameof(X2), typeof(double), typeof(ArrowLine),
                                                                                       new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));
    public static readonly DependencyProperty Y2Property = DependencyProperty.Register(nameof(Y2), typeof(double), typeof(ArrowLine),
                                                                                       new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    ///     Coordenada X del punto inicial de la línea
    /// </summary>
    public double X1
    {
        set { SetValue(X1Property, value); }
        get { return (double) GetValue(X1Property); }
    }

    /// <summary>
    ///     Coordenada Y del punto inicial de la línea
    /// </summary>
    public double Y1
    {
        set { SetValue(Y1Property, value); }
        get { return (double) GetValue(Y1Property); }
    }

    /// <summary>
    ///     Coordenada X del punto final de la línea
    /// </summary>
    public double X2
    {
        set { SetValue(X2Property, value); }
        get { return (double) GetValue(X2Property); }
    }

    /// <summary>
    ///     Coordenada Y del punto final de la línea
    /// </summary>
    public double Y2
    {
        set { SetValue(Y2Property, value); }
        get { return (double) GetValue(Y2Property); }
    }

    /// <summary>
    ///     Calcula la geometría
    /// </summary>
    protected override Geometry DefiningGeometry
    {
        get
        {
            // Limpia la geometría
            pathgeo.Figures.Clear();
            // Define una PathFigure que une los puntos
            pathfigLine.StartPoint = new Point(X1, Y1);
            polysegLine.Points.Clear();
            polysegLine.Points.Add(new Point(X2, Y2));
            pathgeo.Figures.Add(pathfigLine);
            // Añade las puntas de flecha al inicio o al final llamando a la base
            return base.DefiningGeometry;
        }
    }
}
