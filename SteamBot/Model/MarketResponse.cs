using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamBot.Model
{
    public class MarketResponse
    {
        public string Name { get; set; }
        public int Start { get; set; }
        public int Size { get; set; }
        public int AmountTotal { get; set; }
        public List<ListingInfo> Listings { get; set; }
    }
}
