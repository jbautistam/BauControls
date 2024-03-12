using System.Windows;
using System.Windows.Controls;

namespace Bau.Controls.Editors;

/// <summary>
///		Control de usuario para edición de fechas con horas
/// </summary>
public partial class DateTimeEditor : UserControl
{
	// Propiedades
	public static readonly DependencyProperty DateProperty = DependencyProperty.Register(nameof(Date), typeof(DateTime), 
																						 typeof(DateTimeEditor), 
																						 new FrameworkPropertyMetadata(DateTime.Now,
																													   FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																													   OnDateChanged));
	// Eventos
	public static readonly RoutedEvent DateChangedEvent = EventManager.RegisterRoutedEvent(nameof(DateChanged), RoutingStrategy.Direct, 
																						   typeof(RoutedPropertyChangedEventHandler<int>), 
																						   typeof(DateTimeEditor));
	// Variables privadas
	private bool _isUpdating = false;

	public DateTimeEditor()
	{
		InitializeComponent();
	}

	/// <summary>
	///		Modifica el valor de la fecha
	/// </summary>
	private void UpdateDateValue()
	{
		if (!_isUpdating)
		{
			int hour = Math.Clamp(txtHour.Value, 0, 23);
			int minute = Math.Clamp(txtMinute.Value, 0, 59);
			DateTime date = txtDate.SelectedDate ?? DateTime.Now;

				// Indica que está modificando
				_isUpdating = true;
				// Asigna la fecha a los controles
				if (hour != txtHour.Value)
					txtHour.Value = hour;
				if (minute != txtMinute.Value)
					txtMinute.Value = minute;
				if (date.Date != (txtDate.SelectedDate ?? DateTime.Now).Date)
					txtDate.SelectedDate = date;
				// Asigna la propiedad
				if (date != Date)
					Date = new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
				// Indica que ha terminado de modificar
				_isUpdating = false;
		}
	}

	/// <summary>
	///		Trata el evento de modificación de la fecha
	/// </summary>
	private static void OnDateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
	{
		if (sender is DateTimeEditor dateTimeEditor && (DateTime) args.NewValue != (DateTime) args.OldValue)
			dateTimeEditor.Date = (DateTime) args.NewValue;
	}

	/// <summary>
	///		Evento de modificación de fecha
	/// </summary>
	public event RoutedPropertyChangedEventHandler<DateTime> DateChanged
	{
		add { AddHandler(DateChangedEvent, value); }
		remove { RemoveHandler(DateChangedEvent, value); }
	}

	/// <summary>
	///		Fecha
	/// </summary>
	public DateTime Date
	{
		get { return (DateTime) GetValue(DateProperty); }
		set 
		{ 
			SetValue(DateProperty, value); 
			_isUpdating = true;
			txtDate.SelectedDate = value.Date;
			txtHour.Value = value.Hour;
			txtMinute.Value = value.Minute;
			_isUpdating = false;
		}
	}

	private void DateTimeEditorControls_ValueChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
	{
		UpdateDateValue();
	}

	private void txtDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
	{
		UpdateDateValue();
	}
}
