using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Bau.Controls.Graphical
{
	/// <summary>
	///		Imagen que no bloquea el archivo que carga
	/// </summary>
	public class ImageNoLock : Image
	{
		// Propiedades
		public static readonly DependencyProperty SourceFileProperty = DependencyProperty.Register(nameof(SourceFile), typeof(string), typeof(ImageNoLock),
																								   new FrameworkPropertyMetadata(string.Empty,
																															     FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
																																 OnSourceFileChanged));

		/// <summary>
		///		Crea la imagen en memoria (útil para cargar una imagen desde un archivo y que no se bloquee el acceso)
		/// </summary>
		private ImageSource CreateBitmapImage(string fileName)
		{
			if (!string.IsNullOrWhiteSpace(fileName) && System.IO.File.Exists(fileName))
			{
				BitmapImage image = new BitmapImage();

					// Lee el archivo sobre la imagen
					image.BeginInit();
					image.StreamSource = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
					image.CacheOption = BitmapCacheOption.OnLoad;
					image.EndInit();
					// Libera el stream para evitar excepciones de acceso al archivo cuando se intenta borrar la imagen
					image.StreamSource.Dispose();
					// Asigna la imagen
					return image;
			}
			else
				return null;
		}

		/// <summary>
		///		Evento de cambio de archivo de imagen
		/// </summary>
		private static void OnSourceFileChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is ImageNoLock image && (string) args.NewValue != (string) args.OldValue)
				image.SourceFile = (string) args.NewValue;
		}

		/// <summary>
		///		Nombre del archivo de imagen
		/// </summary>
		public string SourceFile
		{
			get { return GetValue(SourceFileProperty) as string; }
			set
			{
				SetValue(SourceFileProperty, value);
				Source = CreateBitmapImage(value);
			}
		}

	}
}
