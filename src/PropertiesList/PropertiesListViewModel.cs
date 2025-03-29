using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bau.Controls.PropertiesList;

/// <summary>
///     ViewModel para una lista de propiedades
/// </summary>
public class PropertiesListViewModel : INotifyPropertyChanged
{
    // Eventos públicos
    public event PropertyChangedEventHandler? PropertyChanged;
    // Variables privadas
    private ObservableCollection<UciOption> _options = default!;

    public PropertiesListViewModel()
    {
        // Inicializa con opciones de ejemplo
        Options = new ObservableCollection<UciOption>
                            {
                                new SpinOption { Name = "Hash", MinValue = 1, MaxValue = 128, Value = 16 },
                                new CheckOption { Name = "Ponder", Value = false },
                                new ComboOption { Name = "Style", ComboValues = new List<string> { "Solid", "Aggressive", "Passive" }, SelectedValue = "Solid" }
                            };
    }

    /// <summary>
    ///     Tratamiento del evento <see cref="PropertyChanged"/>
    /// </summary>
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    /// <summary>
    ///     Opciones
    /// </summary>
    public ObservableCollection<UciOption> Options
    {
        get { return _options; }
        set
        {
            _options = value;
            OnPropertyChanged();
        }
    }
}
