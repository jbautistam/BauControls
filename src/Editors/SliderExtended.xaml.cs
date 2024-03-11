using System.Windows;
using System.Windows.Controls;

namespace Bau.Controls.Editors;

/// <summary>
///		Control de usuario para extender un Slider
/// </summary>
public partial class SliderExtended : UserControl
{
	// Propiedades
	public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum), typeof(double), 
																							typeof(SliderExtended), 
																							new FrameworkPropertyMetadata(0.0, 
																														  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																														  OnMinimumChanged));
	public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(SliderExtended), 
																							new FrameworkPropertyMetadata(100.0, 
																														  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																														  OnMaximumChanged));
	public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(SliderExtended), 
																							new FrameworkPropertyMetadata(0.0, 
																														  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																														  OnValueChanged));
	public static readonly DependencyProperty TicksProperty = DependencyProperty.Register(nameof(Ticks), typeof(double), typeof(SliderExtended), 
																							new FrameworkPropertyMetadata(10.0, 
																														  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																														  OnTicksChanged));
	public static readonly DependencyProperty TickFrequencyProperty = DependencyProperty.Register(nameof(TickFrequency), typeof(double), typeof(SliderExtended), 
																								  new FrameworkPropertyMetadata(5.0,
																																FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																																OnTickFrequencyChanged));
	public static readonly DependencyProperty SmallChangeProperty = DependencyProperty.Register(nameof(SmallChange), typeof(double), typeof(SliderExtended), 
																								new FrameworkPropertyMetadata(0.1,
																															  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																															  OnSmallChangeChanged));
	public static readonly DependencyProperty LargeChangeProperty = DependencyProperty.Register(nameof(LargeChange), typeof(double), typeof(SliderExtended), 
																								new FrameworkPropertyMetadata(1.0,
																															  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																															  OnLargeChangeChanged));
	// Eventos
	public event EventHandler? Changed;

	public SliderExtended()
	{
		InitializeComponent();
		grdSlider.DataContext = this;
	}

	/// <summary> 
	///		Lanza el evento Changed
	/// </summary> 
	protected virtual void OnChanged()
	{
		Changed?.Invoke(this, EventArgs.Empty);
	}

	private void cmdPrevious_Click(object sender, RoutedEventArgs e)
	{
		if (Value > Minimum)
			Value -= SmallChange;
	}

	private void cmdNext_Click(object sender, RoutedEventArgs e)
	{
		if (Value < Maximum)
			Value += SmallChange;
	}

	private static void OnMinimumChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
	{
		if (sender is SliderExtended control && (double) args.NewValue != (double) args.OldValue)
			control.Minimum = (double) args.NewValue;
	}

	private static void OnMaximumChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
	{
		if (sender is SliderExtended control && (double) args.NewValue != (double) args.OldValue)
			control.Maximum = (double) args.NewValue;
	}

	private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
	{
		if (sender is SliderExtended control && (double) args.NewValue != (double) args.OldValue)
			control.Value = (double) args.NewValue;
	}

	private static void OnTicksChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
	{
		if (sender is SliderExtended control && (double) args.NewValue != (double) args.OldValue)
			control.Ticks = (double) args.NewValue;
	}

	private static void OnTickFrequencyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
	{
		if (sender is SliderExtended control && (double) args.NewValue != (double) args.OldValue)
			control.TickFrequency = (double) args.NewValue;
	}

	private static void OnSmallChangeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
	{
		if (sender is SliderExtended control && (double) args.NewValue != (double) args.OldValue)
			control.SmallChange = (double) args.NewValue;
	}

	private static void OnLargeChangeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
	{
		if (sender is SliderExtended control && (double) args.NewValue != (double) args.OldValue)
			control.LargeChange = (double) args.NewValue;
	}

	/// <summary>
	///		Valor mínimo del slider
	/// </summary>
	public double Minimum
	{
		get { return (double) GetValue(MinimumProperty); }
		set { SetValue(MinimumProperty, value); }
	}

	/// <summary>
	///		Valor máximo del slider
	/// </summary>
	public double Maximum
	{
		get { return (double) GetValue(MaximumProperty); }
		set { SetValue(MaximumProperty, value); }
	}

	/// <summary>
	///		Valor actual del slider
	/// </summary>
	public double Value
	{
		get { return (double) GetValue(ValueProperty); }
		set 
		{ 
			double result = value;

				// Limita los valores
				if (result < Minimum)
					result = Minimum;
				else if (result > Maximum)
					result = Maximum;
				// Asigna el valor
				SetValue(ValueProperty, result); 
		}
	}

	/// <summary>
	///		Número de ticks del control
	/// </summary>
	public double Ticks
	{
		get { return (double) GetValue(TicksProperty); }
		set { SetValue(TicksProperty, value); }
	}

	/// <summary>
	///		Frecuencia de ticks del control
	/// </summary>
	public double TickFrequency
	{
		get { return (double) GetValue(TickFrequencyProperty); }
		set { SetValue(TickFrequencyProperty, value); }
	}

	/// <summary>
	///		Incremento grande del control
	/// </summary>
	public double LargeChange
	{
		get { return (double) GetValue(LargeChangeProperty); }
		set { SetValue(LargeChangeProperty, value); }
	}

	/// <summary>
	///		Incremento pequeño del control
	/// </summary>
	public double SmallChange
	{
		get { return (double) GetValue(SmallChangeProperty); }
		set { SetValue(SmallChangeProperty, value); }
	}
}