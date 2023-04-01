using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Bau.Controls.Editors
{
	/// <summary>
	///		Control de usuario para edición de un valor numérico entero
	/// </summary>
	[TemplatePart(Name = "PART_NumericTextBox", Type = typeof(TextBox))]
	[TemplatePart(Name = "PART_IncreaseButton", Type = typeof(RepeatButton))]
	[TemplatePart(Name = "PART_DecreaseButton", Type = typeof(RepeatButton))]
	public partial class IntegerUpDown : UserControl
	{
		// Propiedades
		public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum), typeof(int), 
																								typeof(IntegerUpDown), 
																								new FrameworkPropertyMetadata(0, 
																															  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																															  OnMinimumChanged));
		public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum), typeof(int), typeof(IntegerUpDown), 
																								new FrameworkPropertyMetadata(int.MaxValue, 
																															  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																															  OnMaximumChanged));
		public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(nameof(Increment), typeof(int), 
																								  typeof(IntegerUpDown), 
																								  new FrameworkPropertyMetadata(1, 
																																FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																																OnIncrementChanged));
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(int), typeof(IntegerUpDown), 
																							  new FrameworkPropertyMetadata(new int(), 
																															FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																															OnValueChanged));
		public static readonly DependencyProperty ValueFormatProperty = DependencyProperty.Register(nameof(ValueFormat), typeof(string), 
																									typeof(IntegerUpDown), 
																									new FrameworkPropertyMetadata("0", 
																																  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																																  OnValueFormatChanged));
		// Eventos
		public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(nameof(ValueChanged), RoutingStrategy.Direct, 
																								typeof(RoutedPropertyChangedEventHandler<int>), 
																								typeof(IntegerUpDown));

		public IntegerUpDown()
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
		///		Convierte el valor a partir del texto
		/// </summary>
		private void ConvertValueFromText()
		{
			string text = Normalize(PART_NumericTextBox.Text);

				// Convierte el valor a partir del texto normalizado
				if (int.TryParse(text, out int newValue))
				{
					if (Value < Minimum)
						Value = Minimum;
					else if (Value > Maximum)
						Value = Maximum;
					else
						Value = newValue;
				}
				else
					Value = 0;
		}

		/// <summary>
		///		Normaliza el texto
		/// </summary>
		private string Normalize(string text)
		{
			string result = string.Empty;

				// Quita los caracteres que no sean dígitos o signos matemáticos
				foreach (char chr in text)
					if (char.IsDigit(chr))
						result += chr;
					else if (chr == '-' || chr == '+' && result.Length == 0)
						result += chr;
				// Si no hay nada, pone un 0
				if (string.IsNullOrWhiteSpace(result))
					result = "0";
				// Devuelve el resultado
				return result;
		}

		/// <summary>
		///		Aplica las plantillas al cambiar de estilo
		/// </summary>
		public override void OnApplyTemplate()
		{
			// Aplica la plantilla
			base.OnApplyTemplate();
			// Aplica la plantilla a los botones
			if (GetTemplateChild("PART_IncreaseButton") is RepeatButton incButton)
				incButton.Click += increaseBtn_Click;
			if (GetTemplateChild("PART_DecreaseButton") is RepeatButton decButton)
				decButton.Click += decreaseBtn_Click;
			// Aplica la plantilla al cuadro de texto
			if (GetTemplateChild("PART_NumericTextBox") is TextBox textBox)
			{
				PART_NumericTextBox = textBox;
				PART_NumericTextBox.Text = Value.ToString(ValueFormat);
				PART_NumericTextBox.PreviewTextInput += numericBox_PreviewTextInput;
				PART_NumericTextBox.MouseWheel += numericBox_MouseWheel;
			}
		}

		public new Brush Foreground
		{
			get { return PART_NumericTextBox.Foreground; }
			set { PART_NumericTextBox.Foreground = value; }
		}

		private static void OnMinimumChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is IntegerUpDown numericBoxControl && (int) args.NewValue != (int) args.OldValue)
				numericBoxControl.Minimum = (int) args.NewValue;
		}

		public int Minimum
		{
			get { return (int) GetValue(MinimumProperty); }
			set { SetValue(MinimumProperty, value); }
		}

		private static void OnMaximumChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is IntegerUpDown numericBoxControl && (int) args.NewValue != (int) args.OldValue)
				numericBoxControl.Maximum = (int) args.NewValue;
		}

		public int Maximum
		{
			get { return (int) GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}

		private static void OnIncrementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is IntegerUpDown numericBoxControl && (int) args.NewValue != (int) args.OldValue)
				numericBoxControl.Increment = (int) args.NewValue;
		}

		public int Increment
		{
			get { return (int) GetValue(IncrementProperty); }
			set { SetValue(IncrementProperty, value); }
		}

		private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is IntegerUpDown numericBoxControl && (int) args.NewValue != (int) args.OldValue)
			{
				numericBoxControl.Value = (int) args.NewValue;
				numericBoxControl.PART_NumericTextBox.Text = numericBoxControl.Value.ToString(numericBoxControl.ValueFormat);
				numericBoxControl.OnValueChanged((int) args.OldValue, (int) args.NewValue);
			}
		}

		public int Value
		{
			get { return (int) GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		private static void OnValueFormatChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is IntegerUpDown numericBoxControl && (string) args.NewValue != (string) args.OldValue)
				numericBoxControl.ValueFormat = (string) args.NewValue;
		}

		public string ValueFormat
		{
			get { return (string) GetValue(ValueFormatProperty); }
			set { SetValue(ValueFormatProperty, value); }
		}

		public event RoutedPropertyChangedEventHandler<int> ValueChanged
		{
			add { AddHandler(ValueChangedEvent, value); }
			remove { RemoveHandler(ValueChangedEvent, value); }
		}

		private void OnValueChanged(int oldValue, int newValue)
		{
			if (oldValue != newValue)
				RaiseEvent(new RoutedPropertyChangedEventArgs<int>(oldValue, newValue)
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

		protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			ConvertValueFromText();
			base.OnPreviewLostKeyboardFocus(e);
		}

		private void numericBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (sender is TextBox)
				ConvertValueFromText();
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
