using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bau.Controls.PropertiesList;

public abstract class UciOption
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string DefaultValue { get; set; }
}

public class CheckOption : UciOption
{
public bool Value { get; set; }

public CheckOption()
{
    Type = "check";
}
}

public class SpinOption : UciOption
{
public int MinValue { get; set; }
public int MaxValue { get; set; }
public int Value { get; set; }

public SpinOption()
{
    Type = "spin";
}
}

public class ComboOption : UciOption
{
public List<string> ComboValues { get; set; }
public string SelectedValue { get; set; }

public ComboOption()
{
    Type = "combo";
}
}


