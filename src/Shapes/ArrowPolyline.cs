using System.Windows;
using System.Windows.Media;

namespace Bau.Controls.Shapes;

/// <summary>
///     Serie de líneas conectadas por líneas rectas con puntas de flecha al inicio y / o final
/// </summary>
public class ArrowPolyline : ArrowLineBase
{
    // Propiedades
    public static readonly DependencyProperty PointsProperty = DependencyProperty.Register(nameof(Points), typeof(PointCollection), typeof(ArrowPolyline),
                                                                                           new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure));

    public ArrowPolyline()
    {
        Points = new PointCollection();
    }

    /// <summary>
    ///     Colección de vértices
    /// </summary>
    public PointCollection Points
    {
        set { SetValue(PointsProperty, value); }
        get { return (PointCollection) GetValue(PointsProperty); }
    }

    /// <summary>
    ///     Calcula la geometría de la figura
    /// </summary>
    protected override Geometry DefiningGeometry
    {
        get 
        {
            // Limpia la geometría
            pathgeo.Figures.Clear();
            // Si hay algo que pintar
            if (Points.Count > 0)
            {
                // Define una PathFigure con los puntos
                pathfigLine.StartPoint = Points[0];
                polysegLine.Points.Clear();
                // Añade las líneas
                for (int point = 1; point < Points.Count; point++)
                    polysegLine.Points.Add(Points[point]);
                // Añade las figuras
                pathgeo.Figures.Add(pathfigLine);
            }
            // Añade las puntas de flecha llamando al control base
            return base.DefiningGeometry;
        }
    }
}
