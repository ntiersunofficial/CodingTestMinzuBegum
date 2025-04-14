using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryAPP.Core.Model;

public class RawCountryModel
{
    public NameModel Name { get; set; }
    public FlagModel Flags { get; set; }
    public double? Area { get; set; }
    public long? Population { get; set; }
    public List<string> Timezones { get; set; }
    public List<string> Capital { get; set; }
    public List<string> Tld { get; set; }
    public List<double> Latlng { get; set; }
    public CarModel Car { get; set; }
    public List<string> Continents { get; set; }
}

public class NameModel
{
    public string Common { get; set; }
    public string Official { get; set; }
}

public class FlagModel
{
    public string Png { get; set; }
    public string Svg { get; set; }
}

public class CarModel
{
    public string Side { get; set; } // left or right
}


