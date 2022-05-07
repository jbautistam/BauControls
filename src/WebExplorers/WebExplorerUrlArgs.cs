using System;

namespace Bau.Controls.WebExplorers
{
	/// <summary>
	///		Argumentos del evento indicando una URL
	/// </summary>
	public class WebExplorerUrlArgs : EventArgs
	{
		public WebExplorerUrlArgs(string url)
		{
			Url = url;
		}

		/// <summary>
		///		Url que se debe abrir en el navegador
		/// </summary>
		public string Url { get; }
	}
}
