using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Bau.Controls.Editors
{
	[TemplatePart(Name = "PART_NumericTextBox", Type = typeof(TextBox))]
	[TemplatePart(Name = "PART_IncreaseButton", Type = typeof(RepeatButton))]
	[TemplatePart(Name = "PART_DecreaseButton", Type = typeof(RepeatButton))]
	/// <summary>
	///		Control de usuario para edición de un valor numérico decimal
	/// </summary>
	public partial class DoubleUpDown : UserControl
	{
		// Propiedades
		public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum), typeof(double), 
																								typeof(DoubleUpDown), 
																								new FrameworkPropertyMetadata(double.MinValue, 
																															  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																															  OnMinimumChanged));
		public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(DoubleUpDown), 
																								new FrameworkPropertyMetadata(double.MaxValue, 
																															  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																															  OnMaximumChanged));
		public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(nameof(Increment), typeof(int), 
																								  typeof(DoubleUpDown), 
																								  new FrameworkPropertyMetadata(1, 
																																FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																																OnIncrementChanged));
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(DoubleUpDown), 
																							  new FrameworkPropertyMetadata(new double(), 
																															FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																															OnValueChanged));
		public static readonly DependencyProperty ValueFormatProperty = DependencyProperty.Register(nameof(ValueFormat), typeof(string), 
																									typeof(DoubleUpDown), 
																									new FrameworkPropertyMetadata("0.0000000", 
																																  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																																  OnValueFormatChanged));
		// Eventos
		public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(nameof(ValueChanged), RoutingStrategy.Direct, 
																								typeof(RoutedPropertyChangedEventHandler<double>), 
																								typeof(DoubleUpDown));

		public DoubleUpDown()
		{
			InitializeComponent();
		}

		/// <summary>
		///		Incrementa el valor
		/// </summary>
		private void IncreaseValue()
		{
			Value = Math.Min(Maximum, Value + Increment);
		}

		/// <summary>
		///		Decrementa el valor
		/// </summary>
		private void DecreaseValue()
		{
			Value = Math.Max(Minimum, Value - Increment);
		}

		/// <summary>
		///		Aplica las plantillas al cambiar de estilo
		/// </summary>
		public override void OnApplyTemplate()
		{
			// Apliba la plantilla base
			base.OnApplyTemplate();
			// Aplica la plantilla al botón de incrementar
			if (GetTemplateChild("PART_IncreaseButton") is RepeatButton increaseButton)
				increaseButton.Click += increaseBtn_Click;
			// Aplica la plantilla al botón de decrementar
			if (GetTemplateChild("PART_DecreaseButton") is RepeatButton decreaseButton)
				decreaseButton.Click += decreaseBtn_Click;
			// Aplica la plantilla al cuadro de texto
			if (GetTemplateChild("PART_NumericTextBox") is TextBox textBox)
			{
				PART_NumericTextBox = textBox;
				PART_NumericTextBox.Text = Value.ToString(ValueFormat);
				PART_NumericTextBox.PreviewTextInput += numericBox_PreviewTextInput;
				PART_NumericTextBox.MouseWheel += numericBox_MouseWheel;
			}
		}

		new public Brush Foreground
		{
			get { return PART_NumericTextBox.Foreground; }
			set { PART_NumericTextBox.Foreground = value; }
		}

		private static void OnMinimumChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is DoubleUpDown numericBoxControl && (double) args.NewValue != (double) args.OldValue)
				numericBoxControl.Minimum = (double) args.NewValue;
		}

		public double Minimum
		{
			get { return (double) GetValue(MinimumProperty); }
			set { SetValue(MinimumProperty, value); }
		}

		private static void OnMaximumChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is DoubleUpDown numericBoxControl && (double) args.NewValue != (double) args.OldValue)
				numericBoxControl.Maximum = (double) args.NewValue;
		}

		public double Maximum
		{
			get { return (double) GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}

		private static void OnIncrementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is DoubleUpDown numericBoxControl && (int) args.NewValue != (int) args.OldValue)
				numericBoxControl.Increment = (int) args.NewValue;
		}

		public int Increment
		{
			get { return (int) GetValue(IncrementProperty); }
			set { SetValue(IncrementProperty, value); }
		}

		private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is DoubleUpDown numericBoxControl && (double) args.NewValue != (double) args.OldValue)
			{
				numericBoxControl.Value = (double) args.NewValue;
				numericBoxControl.PART_NumericTextBox.Text = numericBoxControl.Value.ToString(numericBoxControl.ValueFormat);
				numericBoxControl.OnValueChanged((double) args.OldValue, (double) args.NewValue);
			}
		}

		public double Value
		{
			get { return (double) GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		private static void OnValueFormatChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is DoubleUpDown numericBoxControl && (string) args.NewValue != (string) args.OldValue)
				numericBoxControl.ValueFormat = (string) args.NewValue;
		}

		public string ValueFormat
		{
			get { return (string) GetValue(ValueFormatProperty); }
			set { SetValue(ValueFormatProperty, value); }
		}

		public event RoutedPropertyChangedEventHandler<double> ValueChanged
		{
			add { AddHandler(ValueChangedEvent, value); }
			remove { RemoveHandler(ValueChangedEvent, value); }
		}

		private void OnValueChanged(double oldValue, double newValue)
		{
			if (oldValue != newValue)
				RaiseEvent(new RoutedPropertyChangedEventArgs<double>(oldValue, newValue)
																		{
																			RoutedEvent = ValueChangedEvent
																		}
																	);
		}

		private void increaseBtn_Click(object sender, RoutedEventArgs e)
		{
			IncreaseValue();
		}

		private void decreaseBtn_Click(object sender, RoutedEventArgs e)
		{
			DecreaseValue();
		}

		private void numericBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (sender is TextBox textbox)
			{
				int caretIndex = textbox.CaretIndex;

					// Asigna el texto
					try
					{
						bool error = !double.TryParse(e.Text, out double newvalue);
						string text = textbox.Text;

							if (!error)
							{
								text = text.Insert(textbox.CaretIndex, e.Text);
								error = !double.TryParse(text, out newvalue);
								if (!error)
									error = newvalue < Minimum || newvalue > Maximum;
							}
							if (error)
							{
								SystemSounds.Hand.Play();
								textbox.CaretIndex = caretIndex;
							}
							else
							{
								PART_NumericTextBox.Text = text;
								textbox.CaretIndex = caretIndex + e.Text.Length;
								Value = newvalue;
							}
					}
					catch {}
					// Indica que se ha manejado el evento
					e.Handled = true;
			}
		}

		private void numericBox_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (e.Delta > 0)
				IncreaseValue();
			else if (e.Delta < 0)
				DecreaseValue();
		}
	}
}