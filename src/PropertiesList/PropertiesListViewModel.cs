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
    private ObservableCollection<UciOption> _options;

    public ObservableCollection<UciOption> Options
    {
        get => _options;
        set
        {
            _options = value;
            OnPropertyChanged();
        }
    }

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

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
