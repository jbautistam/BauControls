using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Bau.Controls.Graphical;

/// <summary>
///     Botón con una imagen: modifica el fondo de la imagen cuando se inhabilita
/// </summary>
public class ImageButton : Button
{
    // Propiedades de dependencia
    public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource), 
                                                                                                typeof(ImageButton), new PropertyMetadata(null));

    static ImageButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
    }

    /// <summary>
    ///     Origen de la imagen
    /// </summary>
    public ImageSource ImageSource
    {
        get { return (ImageSource) GetValue(ImageSourceProperty); }
        set { SetValue(ImageSourceProperty, value); }
    }
}
