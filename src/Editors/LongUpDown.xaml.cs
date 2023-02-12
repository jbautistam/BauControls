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
	public partial class LongUpDown : UserControl
	{
		// Propiedades
		public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum), typeof(long), 
																								typeof(LongUpDown), 
																								new FrameworkPropertyMetadata(long.MinValue, 
																															  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																															  OnMinimumChanged));
		public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum), typeof(long), typeof(LongUpDown), 
																								new FrameworkPropertyMetadata(long.MaxValue, 
																															  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																															  OnMaximumChanged));
		public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(nameof(Increment), typeof(int), 
																								  typeof(LongUpDown), 
																								  new FrameworkPropertyMetadata(1, 
																																FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																																OnIncrementChanged));
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(long), typeof(LongUpDown), 
																							  new FrameworkPropertyMetadata(new long(), 
																															FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																															OnValueChanged));
		public static readonly DependencyProperty ValueFormatProperty = DependencyProperty.Register(nameof(ValueFormat), typeof(string), 
																									typeof(LongUpDown), 
																									new FrameworkPropertyMetadata("0", 
																																  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																																  OnValueFormatChanged));
		// Eventos
		public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(nameof(ValueChanged), RoutingStrategy.Direct, 
																								typeof(RoutedPropertyChangedEventHandler<long>), 
																								typeof(LongUpDown));

		public LongUpDown()
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
			// Asigna las plantillas base
			base.OnApplyTemplate();
			// Asigna las plantillas a los botones
			if (GetTemplateChild("PART_IncreaseButton") is RepeatButton increaseButton)
				increaseButton.Click += increaseBtn_Click;
			if (GetTemplateChild("PART_DecreaseButton") is RepeatButton decreaseButton)
				decreaseButton.Click += decreaseBtn_Click;
			// Asigna las plantillas al cuadro de texto
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
			if (sender is LongUpDown numericBoxControl && (long) args.NewValue != (long) args.OldValue)
				numericBoxControl.Minimum = (long) args.NewValue;
		}

		public long Minimum
		{
			get { return (long) GetValue(MinimumProperty); }
			set { SetValue(MinimumProperty, value); }
		}

		private static void OnMaximumChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is LongUpDown numericBoxControl && (long) args.NewValue != (long) args.OldValue)
				numericBoxControl.Maximum = (long) args.NewValue;
		}

		public long Maximum
		{
			get { return (long) GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}

		private static void OnIncrementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is LongUpDown numericBoxControl && (int) args.NewValue != (int) args.OldValue)
				numericBoxControl.Increment = (int) args.NewValue;
		}

		public int Increment
		{
			get { return (int) GetValue(IncrementProperty); }
			set { SetValue(IncrementProperty, value); }
		}

		private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is LongUpDown numericBoxControl && (long) args.NewValue != (long) args.OldValue)
			{
				numericBoxControl.Value = (long) args.NewValue;
				numericBoxControl.PART_NumericTextBox.Text = numericBoxControl.Value.ToString(numericBoxControl.ValueFormat);
				numericBoxControl.OnValueChanged((long) args.OldValue, (long) args.NewValue);
			}
		}

		public long Value
		{
			get { return (long) GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		private static void OnValueFormatChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is LongUpDown numericBoxControl && (string) args.NewValue != (string) args.OldValue)
				numericBoxControl.ValueFormat = (string) args.NewValue;
		}

		public string ValueFormat
		{
			get { return (string) GetValue(ValueFormatProperty); }
			set { SetValue(ValueFormatProperty, value); }
		}

		public event RoutedPropertyChangedEventHandler<long> ValueChanged
		{
			add { AddHandler(ValueChangedEvent, value); }
			remove { RemoveHandler(ValueChangedEvent, value); }
		}

		private void OnValueChanged(long oldValue, long newValue)
		{
			if (oldValue != newValue)
				RaiseEvent(new RoutedPropertyChangedEventArgs<long>(oldValue, newValue)
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

					try
					{
						bool error = !long.TryParse(e.Text, out long newvalue);
						string text = textbox.Text;

							if (!error)
							{
								text = text.Insert(textbox.CaretIndex, e.Text);
								error = !long.TryParse(text, out newvalue);
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
					catch {}
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
