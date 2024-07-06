using System.Windows;
using System.Windows.Controls;

namespace Bau.Controls.AttachedProperties;

/// <summary>
///		Helper para el tratamiento de contraseñas
/// </summary>
public static class PasswordHelper
{
	// Propiedades de dependiencia
	public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordHelper),
																									 new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));
	public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(PasswordHelper), 
																								   new PropertyMetadata(false, Attach));
	private static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordHelper));

	/// <summary>
	///		Asigna la dependencia <see cref="AttachProperty"/>
	/// </summary>
	public static void SetAttach(DependencyObject dp, bool value)
	{
		dp.SetValue(AttachProperty, value);
	}

	/// <summary>
	///		Obtiene la dependencia <see cref="AttachProperty"/>
	/// </summary>
	public static bool GetAttach(DependencyObject dp) => (bool) dp.GetValue(AttachProperty);

	/// <summary>
	///		Obtiene la contraseña
	/// </summary>
	public static string GetPassword(DependencyObject dp) => (string) dp.GetValue(PasswordProperty);

	/// <summary>
	///		Modifica la contraseña
	/// </summary>
	public static void SetPassword(DependencyObject dp, string value)
	{
		dp.SetValue(PasswordProperty, value);
	}

	/// <summary>
	///		Comprueba si se está modificando
	/// </summary>
	private static bool GetIsUpdating(DependencyObject dp) => (bool) dp.GetValue(IsUpdatingProperty);

	/// <summary>
	///		Asigna el valor que indica si se está modificando
	/// </summary>
	private static void SetIsUpdating(DependencyObject dp, bool value)
	{
		dp.SetValue(IsUpdatingProperty, value);
	}

	/// <summary>
	///		Evento que se lanza cuando se modifica la contraseña
	/// </summary>
	private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		if (sender is PasswordBox passwordBox)
		{
			// Desvincula el evento
			passwordBox.PasswordChanged -= PasswordChanged;
			// Modifica el nuevo valor
			if (!GetIsUpdating(passwordBox))
				passwordBox.Password = (string) e.NewValue;
			// Vuelve a vincular el evento
			passwordBox.PasswordChanged += PasswordChanged;
		}
	}

	/// <summary>
	///		Vincula / desvincula la propiedad
	/// </summary>
	private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		if (sender is PasswordBox passwordBox && passwordBox is not null)
		{
			if ((bool) e.OldValue)
				passwordBox.PasswordChanged -= PasswordChanged;
			if ((bool) e.NewValue)
				passwordBox.PasswordChanged += PasswordChanged;
		}
	}

	/// <summary>
	///		Evento que se lanza cuando se ha modificado la contraseña
	/// </summary>
	private static void PasswordChanged(object sender, RoutedEventArgs e)
	{
		if (sender is PasswordBox passwordBox)
		{
			SetIsUpdating(passwordBox, true);
			SetPassword(passwordBox, passwordBox.Password);
			SetIsUpdating(passwordBox, false);
		}
	}
}