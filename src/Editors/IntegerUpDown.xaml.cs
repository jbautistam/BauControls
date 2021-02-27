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
	///		Control de usuario para edición de un valor numérico
	/// </summary>
	public partial class IntegerUpDown : UserControl
	{
		// Propiedades
		public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum), typeof(int), 
																								typeof(IntegerUpDown), 
																								new FrameworkPropertyMetadata(int.MinValue, 
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
		///		Aplica las plantillas al cambiar de estilo
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			RepeatButton button = GetTemplateChild("PART_IncreaseButton") as RepeatButton;
			if (button != null)
				button.Click += increaseBtn_Click;

			button = GetTemplateChild("PART_DecreaseButton") as RepeatButton;
			if (button != null)
				button.Click += decreaseBtn_Click;

			TextBox textBox = GetTemplateChild("PART_NumericTextBox") as TextBox;
			if (textBox != null)
			{
				PART_NumericTextBox = textBox;
				PART_NumericTextBox.Text = Value.ToString(ValueFormat);
				PART_NumericTextBox.PreviewTextInput += numericBox_PreviewTextInput;
				PART_NumericTextBox.MouseWheel += numericBox_MouseWheel;
			}

			button = null;
			textBox = null;
		}

		new public Brush Foreground
		{
			get { return PART_NumericTextBox.Foreground; }
			set { PART_NumericTextBox.Foreground = value; }
		}

		private static void OnMinimumChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			IntegerUpDown numericBoxControl = sender as IntegerUpDown;

				if (numericBoxControl != null && (int) args.NewValue != (int) args.OldValue)
					numericBoxControl.Minimum = (int) args.NewValue;
		}

		public int Minimum
		{
			get { return (int) GetValue(MinimumProperty); }
			set { SetValue(MinimumProperty, value); }
		}

		private static void OnMaximumChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			IntegerUpDown numericBoxControl = sender as IntegerUpDown;

				if (numericBoxControl != null && (int) args.NewValue != (int) args.OldValue)
					numericBoxControl.Maximum = (int) args.NewValue;
		}

		public int Maximum
		{
			get { return (int) GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}

		private static void OnIncrementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			IntegerUpDown numericBoxControl = sender as IntegerUpDown;

				if (numericBoxControl != null && (int) args.NewValue != (int) args.OldValue)
					numericBoxControl.Increment = (int) args.NewValue;
		}

		public int Increment
		{
			get { return (int) GetValue(IncrementProperty); }
			set { SetValue(IncrementProperty, value); }
		}

		private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			IntegerUpDown numericBoxControl = sender as IntegerUpDown;

				if (numericBoxControl != null && (int) args.NewValue != (int) args.OldValue)
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
			IntegerUpDown numericBoxControl = sender as IntegerUpDown;

				if (numericBoxControl != null && (string) args.NewValue != (string) args.OldValue)
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

		private void numericBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			TextBox textbox = sender as TextBox;
			int caretIndex = textbox.CaretIndex;

				try
				{
					bool error = !int.TryParse(e.Text, out int newvalue);
					string text = textbox.Text;
					if (!error)
					{
						text = text.Insert(textbox.CaretIndex, e.Text);
						error = !int.TryParse(text, out newvalue);
						if (!error)
							error = (newvalue < Minimum || newvalue > Maximum);
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
				catch (FormatException)
				{
				}
				e.Handled = true;
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
