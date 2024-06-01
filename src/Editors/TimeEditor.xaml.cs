using System.Windows;
using System.Windows.Controls;

namespace Bau.Controls.Editors;

/// <summary>
///		Control de usuario para edición de horas
/// </summary>
public partial class TimeEditor : UserControl
{
	// Propiedades
	public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(nameof(Time), typeof(TimeOnly), 
																						 typeof(TimeEditor), 
																						 new FrameworkPropertyMetadata(TimeOnly.FromDateTime(DateTime.Now),
																													   FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																													   OnDateChanged));
	// Eventos
	public static readonly RoutedEvent TimeChangedEvent = EventManager.RegisterRoutedEvent(nameof(TimeChanged), RoutingStrategy.Direct, 
																						   typeof(RoutedPropertyChangedEventHandler<int>), 
																						   typeof(TimeEditor));
	// Variables privadas
	private bool _isUpdating = false;

	public TimeEditor()
	{
		InitializeComponent();
	}

	/// <summary>
	///		Modifica el valor de la hora
	/// </summary>
	private void UpdateTimeValue()
	{
		if (!_isUpdating)
		{
			int hour = Math.Clamp(txtHour.Value, 0, 23);
			int minute = Math.Clamp(txtMinute.Value, 0, 59);
			TimeOnly time = TimeOnly.FromDateTime(DateTime.Now);

				// Indica que está modificando
				_isUpdating = true;
				// Asigna la fecha a los controles
				if (hour != txtHour.Value)
					txtHour.Value = hour;
				if (minute != txtMinute.Value)
					txtMinute.Value = minute;
				// Asigna la propiedad
				if (time != Time)
					Time = new TimeOnly(hour, minute, 0);
				// Indica que ha terminado de modificar
				_isUpdating = false;
		}
	}

	/// <summary>
	///		Trata el evento de modificación de la fecha
	/// </summary>
	private static void OnDateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
	{
		if (sender is TimeEditor TimeEditor && (TimeOnly) args.NewValue != (TimeOnly) args.OldValue)
			TimeEditor.Time = (TimeOnly) args.NewValue;
	}

	/// <summary>
	///		Evento de modificación de hora
	/// </summary>
	public event RoutedPropertyChangedEventHandler<TimeOnly> TimeChanged
	{
		add { AddHandler(TimeChangedEvent, value); }
		remove { RemoveHandler(TimeChangedEvent, value); }
	}

	/// <summary>
	///		Hora
	/// </summary>
	public TimeOnly Time
	{
		get { return (TimeOnly) GetValue(TimeProperty); }
		set 
		{ 
			SetValue(TimeProperty, value); 
			_isUpdating = true;
			txtHour.Value = value.Hour;
			txtMinute.Value = value.Minute;
			_isUpdating = false;
		}
	}

	private void TimeEditorControls_ValueChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
	{
		UpdateTimeValue();
	}

	private void txtDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
	{
		UpdateTimeValue();
	}
}
