using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boater.Models
{
    public class BoatingArea
    {
        public string Title { get; set; }
        public List<StationSource> Stations { get; private set; }
    }
}
