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
	///		Control de usuario para edición de una contraseña
	/// </summary>
	public partial class PasswordTextBox : UserControl
	{
		// Propiedades
		public static readonly DependencyProperty PasswordTextProperty = DependencyProperty.Register(nameof(PasswordText), typeof(string), 
																									 typeof(PasswordTextBox), 
																									 new FrameworkPropertyMetadata(string.Empty, 
																																   FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
																																   OnPasswordTextChanged));

		public PasswordTextBox()
		{
			InitializeComponent();
		}

		private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
		{	
			PasswordText = (sender as PasswordBox)?.Password;
		}

		/// <summary>
		///		Tratamiento de la propiedad vinculada de cambio de contraseña
		/// </summary>
		private static void OnPasswordTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is PasswordTextBox passwordBoxControl && passwordBoxControl is not null && (string) args.NewValue != (string) args.OldValue)
			{
				// Sólo asigna el valor a la contraseña la primera vez
				if (string.IsNullOrWhiteSpace(passwordBoxControl.txtPassword.Password))
					passwordBoxControl.txtPassword.Password = (string) args.NewValue;
				// Asigna el texto de la contraseña
				passwordBoxControl.PasswordText = (string) args.NewValue;
			}
		}

		/// <summary>
		///		Texto de la contraseña
		/// </summary>
		public string PasswordText
		{
			get { return (string) GetValue(PasswordTextProperty); }
			set { SetValue(PasswordTextProperty, value); }
		}
	}
}
