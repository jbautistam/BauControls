using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Bau.Controls.Shapes;

/// <summary>
///     Clase base para el dibujo de líneas
/// </summary>
public abstract class ArrowLineBase : Shape
{
    /// <summary>
    ///     Tipos de puntas de flecha asociados a una línea
    /// </summary>
    public enum ArrowEndTypes
    {
        /// <summary>Sin puntas de flecha</summary>
        None,
        /// <summary>Punta de flecha al inicio</summary>
        Start,
        /// <summary>Puntos de flecha al final</summary>
        End,
        /// <summary>Puntos de flecha en ambos lados</summary>
        Both
    }

    // Propiedades de dependencia
    public static readonly DependencyProperty ArrowAngleProperty = DependencyProperty.Register(nameof(ArrowAngle), typeof(double), typeof(ArrowLineBase),
                                                                                               new FrameworkPropertyMetadata(45.0, FrameworkPropertyMetadataOptions.AffectsMeasure));
    public static readonly DependencyProperty ArrowLengthProperty = DependencyProperty.Register(nameof(ArrowLength), typeof(double), typeof(ArrowLineBase),
                                                                                                new FrameworkPropertyMetadata(12.0, FrameworkPropertyMetadataOptions.AffectsMeasure));
    public static readonly DependencyProperty ArrowEndsProperty = DependencyProperty.Register(nameof(ArrowEnds), typeof(ArrowEndTypes), typeof(ArrowLineBase),
                                                                                              new FrameworkPropertyMetadata(ArrowEndTypes.End, 
                                                                                                                            FrameworkPropertyMetadataOptions.AffectsMeasure));
    public static readonly DependencyProperty IsArrowClosedProperty = DependencyProperty.Register(nameof(IsArrowClosed), typeof(bool), typeof(ArrowLineBase),
                                                                                                  new FrameworkPropertyMetadata(false,
                                                                                                                                FrameworkPropertyMetadataOptions.AffectsMeasure));
    // Variables privadas
    private PathFigure _pathfigHead1;
    private PolyLineSegment _polysegHead1;
    private PathFigure _pathfigHead2;
    private PolyLineSegment _polysegHead2;
    // Vairables protegidas
    protected PathGeometry pathgeo;
    protected PathFigure pathfigLine;
    protected PolyLineSegment polysegLine;

    public ArrowLineBase()
    {
        pathgeo = new PathGeometry();

        pathfigLine = new PathFigure();
        polysegLine = new PolyLineSegment();
        pathfigLine.Segments.Add(polysegLine);

        _pathfigHead1 = new PathFigure();
        _polysegHead1 = new PolyLineSegment();
        _pathfigHead1.Segments.Add(_polysegHead1);

        _pathfigHead2 = new PathFigure();
        _polysegHead2 = new PolyLineSegment();
        _pathfigHead2.Segments.Add(_polysegHead2);
    }

    /// <summary>
    ///     Calcula la figura de la flecha
    /// </summary>
    private PathFigure CalculateArrow(PathFigure pathfig, Point start, Point end)
    {
        Matrix matx = new();
        Vector vector = start - end;

            // Normaliza el vector
            vector.Normalize();
            vector *= ArrowLength;
            // Crea los puntos
            if (pathfig.Segments[0] is PolyLineSegment polyseg)
            {
                polyseg.Points.Clear();
                matx.Rotate(ArrowAngle / 2);
                pathfig.StartPoint = end + vector * matx;
                polyseg.Points.Add(end);

                matx.Rotate(-ArrowAngle);
                polyseg.Points.Add(end + vector * matx);
                pathfig.IsClosed = IsArrowClosed;
            }
            // Devuelve la figura
            return pathfig;
    }

    /// <summary>
    ///     Ángulo entre los lados de la cabeza de la flecha
    /// </summary>
    public double ArrowAngle
    {
        set { SetValue(ArrowAngleProperty, value); }
        get { return (double) GetValue(ArrowAngleProperty); }
    }

    /// <summary>
    ///     Longitud de los dos lazos de la cabeza de la flecha
    /// </summary>
    public double ArrowLength
    {
        set { SetValue(ArrowLengthProperty, value); }
        get { return (double) GetValue(ArrowLengthProperty); }
    }

    /// <summary>
    ///     Tipo de final de la línea que contiene flecha
    /// </summary>
    public ArrowEndTypes ArrowEnds
    {
        set { SetValue(ArrowEndsProperty, value); }
        get { return (ArrowEndTypes) GetValue(ArrowEndsProperty); }
    }

    /// <summary>
    ///     Gets or sets the property that determines if the arrow head
    ///     is closed to resemble a triangle.
    /// </summary>
    public bool IsArrowClosed
    {
        set { SetValue(IsArrowClosedProperty, value); }
        get { return (bool)GetValue(IsArrowClosedProperty); }
    }

    /// <summary>
    ///     Gets a value that represents the Geometry of the ArrowLine.
    /// </summary>
    protected override Geometry DefiningGeometry
    {
        get
        {
            int points = polysegLine.Points.Count;

                // Si realmente hay algo que pintar
                if (points > 0)
                {
                    // Dibuja la flecha al inicio de la línea
                    if (ArrowEnds == ArrowEndTypes.Start || ArrowEnds == ArrowEndTypes.Both)
                    {
                        Point start = pathfigLine.StartPoint;
                        Point end = polysegLine.Points[0];

                            // Añade la figura a la geometría
                            pathgeo.Figures.Add(CalculateArrow(_pathfigHead1, end, start));
                    }
                    // Dibuja la flecha al final de la línea
                    if (ArrowEnds == ArrowEndTypes.End || ArrowEnds == ArrowEndTypes.Both)
                    {
                        Point start = points == 1 ? pathfigLine.StartPoint : polysegLine.Points[points - 2];
                        Point end = polysegLine.Points[points - 1];

                            // Añade la figura a la geometría
                            pathgeo.Figures.Add(CalculateArrow(_pathfigHead2, start, end));
                    }
                }
                // Devuelve la geometría creada
                return pathgeo;
        }
    }
}
