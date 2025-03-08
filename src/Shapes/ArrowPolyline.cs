using System.Windows;
using System.Windows.Media;

namespace Bau.Controls.Shapes;

/// <summary>
///     Serie de l�neas conectadas por l�neas rectas con puntas de flecha al inicio y / o final
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
    ///     Colecci�n de v�rtices
    /// </summary>
    public PointCollection Points
    {
        set { SetValue(PointsProperty, value); }
        get { return (PointCollection) GetValue(PointsProperty); }
    }

    /// <summary>
    ///     Calcula la geometr�a de la figura
    /// </summary>
    protected override Geometry DefiningGeometry
    {
        get 
        {
            // Limpia la geometr�a
            pathgeo.Figures.Clear();
            // Si hay algo que pintar
            if (Points.Count > 0)
            {
                // Define una PathFigure con los puntos
                pathfigLine.StartPoint = Points[0];
                polysegLine.Points.Clear();
                // A�ade las l�neas
                for (int point = 1; point < Points.Count; point++)
                    polysegLine.Points.Add(Points[point]);
                // A�ade las figuras
                pathgeo.Figures.Add(pathfigLine);
            }
            // A�ade las puntas de flecha llamando al control base
            return base.DefiningGeometry;
        }
    }
}
