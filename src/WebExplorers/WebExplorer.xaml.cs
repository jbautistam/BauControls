using System;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace Bau.Controls.WebExplorers
{
	/// <summary>
	///		Control de usuario para extensión del explorador Web
	/// </summary>
	public partial class WebExplorer : UserControl
	{ 
		// Propiedades de dependencia
		public static readonly DependencyProperty HiddenScriptErrorsProperty = DependencyProperty.Register(nameof(HiddenScriptErrors), typeof(bool), typeof(WebExplorer),
																										   new FrameworkPropertyMetadata() { DefaultValue = true });
		public static readonly DependencyProperty HtmlContentProperty = DependencyProperty.Register(nameof(HtmlContent), typeof(string), typeof(WebExplorer),
																									new FrameworkPropertyMetadata(string.Empty,
																																  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		// Eventos públicos
		public event EventHandler<WebExplorerFunctionEventArgs> FunctionExecute;
		public event EventHandler<WebExplorerNavigateToEventArgs> BeforeNavigateTo;

		public WebExplorer()
		{ 
			InitializeComponent();
		}

		/// <summary>
		///		Inicializa la vista
		/// </summary>
		public async Task InitializeCoreWebViewAsync()
		{
			// Inicializa el core
			await wbExplorer.EnsureCoreWebView2Async();
			// Inicializa el objeto que atiende las llamadas de JavaScript
			wbExplorer.CoreWebView2.WebMessageReceived += ReceiveJavaScriptMessage;
			wbExplorer.NavigationStarting += TreatNavigationStart;
		}

		/// <summary>
		///		Trata el evento de inicio de navegación a una URL
		/// </summary>
		private void TreatNavigationStart(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(e.Uri) && !e.Uri.StartsWith("data:", StringComparison.CurrentCultureIgnoreCase) && 
				!e.Uri.StartsWith("about:", StringComparison.CurrentCultureIgnoreCase))
			{
				WebExplorerNavigateToEventArgs args = new WebExplorerNavigateToEventArgs(e.Uri);

					// Llama al evento
					BeforeNavigateTo?.Invoke(this, args);
					// Indica si se cancela la navegación
					e.Cancel = args.Cancel;
			}
		}

		/// <summary>
		///		Método que recibe los mensajes de javaScript
		/// </summary>
		private void ReceiveJavaScriptMessage(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs args)
		{
			FunctionExecute?.Invoke(this, new WebExplorerFunctionEventArgs(args.TryGetWebMessageAsString()));
			//String uri = args.TryGetWebMessageAsString();
			//addressBar.Text = uri;
			//webView.CoreWebView2.PostWebMessageAsString(uri);
		}

		/// <summary>
		///		Muestra una URL
		/// </summary>
		public async Task ShowUrlAsync(string url)
		{
			try
			{
				// Espera hasta que esté inicializada WebView
				while (wbExplorer.CoreWebView2 == null)
					await Task.Delay(500);
				// Oculta los errores
				HideScriptErrors(HiddenScriptErrors);
				// Muestra la web
				wbExplorer.CoreWebView2.Navigate(url);
			}
			catch (Exception exception)
			{
				System.Diagnostics.Debug.WriteLine(exception.Message);
			}
		}

		/// <summary>
		///		Muestra una cadena HTML
		/// </summary>
		public async Task ShowHtmlAsync(string html, bool mustRemoveIFrame = true)
		{
			try
			{
				// Espera hasta que esté inicializada WebView
				while (wbExplorer.CoreWebView2 == null)
					await Task.Delay(500);
				// Elimina los datos adicionales del HTML
				if (mustRemoveIFrame)
					html = RemoveIframe(html);
				HideScriptErrors(HiddenScriptErrors);
				// Llama al explorador
				wbExplorer.Source = new Uri("about:blank");
				wbExplorer.NavigateToString(html);
			}
			catch (Exception exception)
			{
				System.Diagnostics.Debug.WriteLine(exception.Message);
			}
		}

		/// <summary>
		///		Elimina el contenido de las etiquetas iFrame que pueden bloquear el explorador
		/// </summary>
		private string RemoveIframe(string text)
		{
			string result = text;

				// Quita la etiqueta "iframe"
				while (!string.IsNullOrEmpty(result) && result.IndexOf("<iframe") >= 0)
					result = System.Text.RegularExpressions.Regex.Replace(result, "<iframe(.|\n)*?</iframe>", string.Empty);
				// Devuelve el resultado
				return result;
		}

		/// <summary>
		///		Llama a un método de JavaScript
		/// </summary>
		public async Task InvokeJavaScript(string method, string[] arguments)
		{
			string call = method + "(";

				// Añade los argumentos
				for (int index = 0; index < arguments.Length; index++)
				{
					// Añade el separador
					if (index > 0)
						call += ", ";
					// Añade el argumento
					call += "'" + arguments + "'";
				}
				// Llama al método
				await wbExplorer.CoreWebView2.ExecuteScriptAsync(call);
		}

		/// <summary>
		///		Oculta los errores de script
		/// </summary>
		private void HideScriptErrors(bool hideErrors)
		{
			// await wbExplorer.CoreWebView2.ExecuteScriptAsync("window.addEventListener('contextmenu', window => {window.preventDefault();});");

			//Dispatcher.Invoke(new Action(() =>
			//							{
			//								FieldInfo axiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);

			//									if (axiComWebBrowser != null)
			//									{
			//										var comWebBrowser = axiComWebBrowser.GetValue(wbExplorer);

			//											if (comWebBrowser == null) // ... en este caso aún no se ha cargado el explorador
			//												wbExplorer.Loaded += (o, s) => HideScriptErrors(hideErrors);
			//											else
			//												comWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, comWebBrowser, new object [] { hideErrors });
			//									}
			//							}
			//				), null);
		}

		/// <summary>
		///		Va una página atrás
		/// </summary>
		public void GoBack()
		{
			if (CanGoBack)
				wbExplorer.GoBack();
		}

		/// <summary>
		///		Va una página adelante
		/// </summary>
		public void GoForward()
		{
			if (CanGoForward)
				wbExplorer.GoForward();
		}

		/// <summary>
		///		Elimina de la memoria
		/// </summary>
		public void Dispose()
		{
			wbExplorer.Dispose();
		}

		/// <summary>
		///		Indica si se deben mostrar o no los errores de JavaScript
		/// </summary>
		public bool HiddenScriptErrors
		{
			get { return (bool) GetValue(HiddenScriptErrorsProperty); }
			set { SetValue(HiddenScriptErrorsProperty, value); }
		}

		/// <summary>
		///		Texto HTML a mostrar en el navegador
		/// </summary>
		public string HtmlContent
		{
			get { return (string) GetValue(HtmlContentProperty); }
			set
			{ 
				// Asigna el valor
				SetValue(HtmlContentProperty, value);
				// Muestra el HTML
				ShowHtmlAsync(value).ConfigureAwait(true);
			}
		}

		/// <summary>
		///		Comprueba si puede ir una página hacia atrás
		/// </summary>
		public bool CanGoBack
		{
			get { return wbExplorer.CanGoBack; }
		}

		/// <summary>
		///		Comprueba si puede ir una página hacia delante
		/// </summary>
		public bool CanGoForward
		{
			get { return wbExplorer.CanGoForward; }
		}

		private async void UserControl_Initialized(object sender, EventArgs e)
		{
			await InitializeCoreWebViewAsync();
		}
	}
}
