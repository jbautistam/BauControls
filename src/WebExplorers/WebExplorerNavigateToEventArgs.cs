namespace Bau.Controls.WebExplorers;

/// <summary>
///		Argumentos del evento que se lanza cuando el navegador va a ir a una página
/// </summary>
public class WebExplorerNavigateToEventArgs : EventArgs
{
	public WebExplorerNavigateToEventArgs(string url)
	{
		Url = url;
	}

	/// <summary>
	///		Url destino
	/// </summary>
	public string Url { get; }

	/// <summary>
	///		Indica si se cancela la navegación
	/// </summary>
	public bool Cancel { get; set; }
}
