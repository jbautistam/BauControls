using System;
using System.Windows;
using System.Windows.Controls;

namespace Bau.Controls.Editors;

/// <summary>
///		Control de edición que muestra diferentes editores dependiendo del tipo 
/// </summary>
public partial class MultiTypeEditor : UserControl
{
	// Propiedades
	public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(object), typeof(MultiTypeEditor), 
																						  new FrameworkPropertyMetadata(new object(), 
																														FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																														OnValueChanged));
	// Eventos
	public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(nameof(ValueChanged), RoutingStrategy.Direct, 
																							typeof(RoutedPropertyChangedEventHandler<double>), 
																							typeof(DoubleUpDown));

	public MultiTypeEditor()
	{
		InitializeComponent();
		DataContext = this;
	}

	/// <summary>
	///		Modifica la visibilidad de los controles dependiendo del tipo del valor
	/// </summary>
	private void UpdateControls(object value)
	{
		dtmValue.Visibility = GetVisibility(value is not null && value is DateTime);
		txtValue.Visibility = GetVisibility(value is not null && value is string);
		chkValue.Visibility = GetVisibility(value is not null && value is bool);

		// Obtiene el valor de visibilidad
		Visibility GetVisibility(bool isType)
		{
			if (isType)
				return Visibility.Hidden;
			else
				return Visibility.Visible;
		}
	}

	/// <summary>
	///		Suscripción al evento lanzado cuando se cambia el valor
	/// </summary>
	public event RoutedPropertyChangedEventHandler<object> ValueChanged
	{
		add { AddHandler(ValueChangedEvent, value); }
		remove { RemoveHandler(ValueChangedEvent, value); }
	}

	/// <summary>
	///		Tratamiento del evento <see cref="ValueChanged"/>
	/// </summary>
	private void OnValueChanged(object oldValue, object newValue)
	{
		if (oldValue != newValue)
			RaiseEvent(new RoutedPropertyChangedEventArgs<object>(oldValue, newValue)
																	{
																		RoutedEvent = ValueChangedEvent
																	}
																);
	}

	/// <summary>
	///		YTratamiento del evento <see cref="ValueChanged"/>
	/// </summary>
	private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
	{
		if (sender is MultiTypeEditor control && args.NewValue != args.OldValue)
		{
			control.Value = args.NewValue;
			control.OnValueChanged(args.OldValue, args.NewValue);
		}
	}

	/// <summary>
	///		Valor
	/// </summary>
	public object Value
	{
		get { return GetValue(ValueProperty); }
		set 
		{
			SetValue(ValueProperty, value); 
			UpdateControls(value);
		}
	}
}
