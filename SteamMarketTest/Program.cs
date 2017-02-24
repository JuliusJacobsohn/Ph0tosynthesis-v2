using Newtonsoft.Json;
using SteamBot.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SteamMarketTest
{
    //
    class Program
    {
        public string MarketLink = "https://steamcommunity.com/market/listings/730/{0}/render/?query=&start={1}&count={2}&country=DE&language=english&currency=3";
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            GetMarketResponse("SCAR-20 | Carbon Fiber (Factory New)", 0, 10);
        }

        public MarketResponse GetMarketResponse(string item, int start, int amount)
        {
            string marketUrl = GetMarketUrl(item, start, amount);
            var response = HttpGet(marketUrl);
            dynamic jsonObject = JsonConvert.DeserializeObject(response);
            MarketResponse mResp = new MarketResponse();
            mResp.AmountTotal = jsonObject.total_count;
            mResp.Name = item;
            mResp.Size = jsonObject.pagesize;
            mResp.Start = jsonObject.start;

            List<ListingInfo> listings = new List<ListingInfo>();
            var listingInfosJson = jsonObject.listinginfo;
            foreach(var listingInfoJson in listingInfosJson)
            {
                ListingInfo newListing = new ListingInfo();
                newListing.Name = item;
                newListing.ListingId = listingInfoJson.Name;
                newListing.InternalId = listingInfoJson.Value.asset.id;
                newListing.InspectLink = listingInfoJson.Value.asset.market_actions[0].link;
                newListing.SubTotal = listingInfoJson.Value.converted_price - listingInfoJson.Value.converted_fee;

                listings.Add(newListing);
            }
            mResp.Listings = listings;
            return mResp;
        }
        public string GetMarketUrl(string item, int start, int amount)
        {
            string itemLink = Uri.EscapeDataString(item);
            return string.Format(MarketLink, itemLink, start, amount);
        }

        public string HttpGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            string html = "";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            return html;
        }
    }
}
