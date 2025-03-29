using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Bau.Controls.Graphical;

/// <summary>
///     Imagen con un efecto de halo
/// </summary>
public class HaloImage : Image
{
    // Propiedades de dependencia
    public static readonly DependencyProperty IsHaloEnabledProperty = DependencyProperty.Register(nameof(IsHaloEnabled), typeof(bool), typeof(HaloImage), 
                                                                                                  new PropertyMetadata(true, OnHaloEnabledChanged));
    public static readonly DependencyProperty HaloColorProperty = DependencyProperty.Register(nameof(HaloColor), typeof(Color), typeof(HaloImage), 
                                                                                              new PropertyMetadata(Colors.Red, OnHaloColorChanged));
    public static readonly DependencyProperty HaloWidthProperty = DependencyProperty.Register(nameof(HaloWidth), typeof(double), typeof(HaloImage), 
                                                                                              new PropertyMetadata(20.0, OnHaloWidthChanged));

    /// <summary>
    ///     Crea el efecto de halo
    /// </summary>
    private void UpdateHaloEffect()
    {
        if (IsHaloEnabled)
            Effect = new DropShadowEffect
                            {
                                Color = HaloColor,
                                BlurRadius = HaloWidth,
                                ShadowDepth = 0
                            };
        else
            Effect = null;
    }

    /// <summary>
    ///     Tratamiento de la modificación de la propiedad <see cref="IsHaloEnabled"/>
    /// </summary>
    private static void OnHaloEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is HaloImage haloImage)
            haloImage.UpdateHaloEffect();
    }

    /// <summary>
    ///     Tratamiento de la modificación de la propiedad <see cref="HaloColor"/>
    /// </summary>
    private static void OnHaloColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is HaloImage haloImage)
            haloImage.UpdateHaloEffect();
    }

    /// <summary>
    ///     Tratamiento de la modificación de la propiedad <see cref="HaloWidth"/>
    /// </summary>
    private static void OnHaloWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is HaloImage haloImage)
            haloImage.UpdateHaloEffect();
    }

    /// <summary>
    ///     Indica si se debe dibujar o no el efecto de halo
    /// </summary>
    public bool IsHaloEnabled
    {
        get { return (bool) GetValue(IsHaloEnabledProperty); }
        set { SetValue(IsHaloEnabledProperty, value); }
    }

    /// <summary>
    ///     Color del efecto de halo
    /// </summary>
    public Color HaloColor
    {
        get { return (Color) GetValue(HaloColorProperty); }
        set { SetValue(HaloColorProperty, value); }
    }


    /// <summary>
    ///     Ancho del efecto de halo
    /// </summary>
    public double HaloWidth
    {
        get { return (double) GetValue(HaloWidthProperty); }
        set { SetValue(HaloWidthProperty, value); }
    }
}
