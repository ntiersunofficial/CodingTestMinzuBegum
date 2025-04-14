using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryAPP.Core.Model;

public class CountryDetailResponseModel
{
    public string Name { get; set; }
    public double? Area { get; set; }
    public long? Population { get; set; }
    public string Timezone { get; set; }
    public string Tld { get; set; }
    public string Capital { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string DrivingSide { get; set; }
    public string Continent { get; set; }
}

