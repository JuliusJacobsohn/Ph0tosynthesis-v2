using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SteamBot.Model
{
    public class MarketItem
    {
        const decimal CSGO_FEE = 0.1M;
        const decimal STEAM_FEE = 0.05M;
        public string ListingId { get; set; }
        public string Name { get; set; }
        public string InspectLink { get; set; }
        public int SubTotal { get; set; }
        public int Fee
        {
            get
            {
                return (int)(Math.Ceiling(CSGO_FEE * SubTotal) + Math.Ceiling(STEAM_FEE * SubTotal));
            }
        }

        public int Total
        {
            get
            {
                return SubTotal + Fee;
            }
        }
    }
}
