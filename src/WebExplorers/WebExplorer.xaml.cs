using System.Windows;
using System.Windows.Controls;
using Microsoft.Web.WebView2.Core;

namespace Bau.Controls.WebExplorers;

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
	public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(WebExplorer),
																						  new FrameworkPropertyMetadata(string.Empty,
																														FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
	public static readonly DependencyProperty UrlProperty = DependencyProperty.Register(nameof(Uri), typeof(string), typeof(WebExplorer),
																						  new FrameworkPropertyMetadata(string.Empty,
																														FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

	// Eventos públicos
	public event EventHandler<WebExplorerUrlArgs>? EndNavigate;
	public event EventHandler<WebExplorerFunctionEventArgs>? FunctionExecute;
	public event EventHandler<WebExplorerNavigateToEventArgs>? BeforeNavigateTo;
	public event EventHandler<WebExplorerUrlArgs>? OpenWindowRequested;

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
		wbExplorer.CoreWebView2InitializationCompleted += TreatInitialization;
		wbExplorer.NavigationStarting += TreatNavigationStart;
		wbExplorer.NavigationCompleted += TreatNavigationEnd;
		wbExplorer.CoreWebView2.NewWindowRequested += TreatNewWindowRequested;
	}

	/// <summary>
	///		Trata la configuración de inicio del explorador
	/// </summary>
	private void TreatInitialization(object sender, CoreWebView2InitializationCompletedEventArgs e)
	{
		wbExplorer.CoreWebView2.NewWindowRequested += TreatNewWindowRequested;
	}

	/// <summary>
	///		Trata el evento de final de navegación
	/// </summary>
	private void TreatNavigationEnd(object sender, CoreWebView2NavigationCompletedEventArgs e)
	{
		Title = wbExplorer.CoreWebView2.DocumentTitle;
		EndNavigate?.Invoke(this, new WebExplorerUrlArgs(wbExplorer.CoreWebView2.Source));
	}

	/// <summary>
	///		Trata el evento de inicio de navegación a una URL
	/// </summary>
	private void TreatNavigationStart(object sender, CoreWebView2NavigationStartingEventArgs e)
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
	///		Trata el evento de apertura de una nueva ventana
	/// </summary>
	private void TreatNewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
	{
		e.Handled = true;
		OpenNewWindow(e.Uri);
	}

	/// <summary>
	///		Indica al host que abra una nueva ventana
	/// </summary>
	private void OpenNewWindow(string url)
	{
		OpenWindowRequested?.Invoke(this, new WebExplorerUrlArgs(url));
	}

	/// <summary>
	///		Método que recibe los mensajes de javaScript
	/// </summary>
	private void ReceiveJavaScriptMessage(object sender, CoreWebView2WebMessageReceivedEventArgs args)
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
	public async Task ShowHtmlAsync(string html)
	{
		try
		{
			// Espera hasta que esté inicializada WebView
			while (wbExplorer.CoreWebView2 == null)
				await Task.Delay(500);
			// Elimina los datos adicionales del HTML
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

	///// <summary>
	/////		Elimina el contenido de las etiquetas iFrame que pueden bloquear el explorador
	///// </summary>
	//private string RemoveIframe(string text)
	//{
	//	string result = text;
	//	int loops = 0;

	//		// Quita la etiqueta "iframe"
	//		while (!string.IsNullOrEmpty(result) && result.Contains("<iframe") && result.Contains("</iframe>") && loops++ < 10)
	//			result = System.Text.RegularExpressions.Regex.Replace(result, "<iframe(.|\n)*?</iframe>", string.Empty, 
	//																  System.Text.RegularExpressions.RegexOptions.IgnoreCase | 
	//																	System.Text.RegularExpressions.RegexOptions.Multiline |
	//																	System.Text.RegularExpressions.RegexOptions.Compiled);
	//		// Elimina los iframe que hayan podido quedarse descolgados
	//		if (!string.IsNullOrWhiteSpace(result))
	//		{
	//			result = result.Replace("<iframe", "<span");
	//			result = result.Replace("</iframe", "</span>");
	//		}
	//		// Devuelve el resultado
	//		return result;
	//}

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
	///		Título del documento actual
	/// </summary>
	public string Title
	{
		get { return (string) GetValue(TitleProperty); }
		set { SetValue(TitleProperty, value); }
	}

	/// <summary>
	///		Url actual
	/// </summary>
	public string Url
	{
		get { return (string) GetValue(UrlProperty); }
		set { SetValue(UrlProperty, value); }
	}

	/// <summary>
	///		Comprueba si puede ir una página hacia atrás
	/// </summary>
	public bool CanGoBack => wbExplorer.CanGoBack;

	/// <summary>
	///		Comprueba si puede ir una página hacia delante
	/// </summary>
	public bool CanGoForward => wbExplorer.CanGoForward;

	private async void UserControl_Initialized(object sender, EventArgs e)
	{
		await InitializeCoreWebViewAsync();
	}
}
