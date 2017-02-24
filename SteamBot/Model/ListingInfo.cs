namespace SteamBot.Model
{
    public class ListingInfo
    {
        public string ListingId { get; set; }
        public string InternalId { get; set; }
        public int SubTotal { get; set; }
        public string Name { get; set; }
        public string InspectLink { get; set; }
        public MarketItem MarketItem
        {
            get
            {
                return new MarketItem
                {
                    InspectLink = InspectLink,
                    ListingId = ListingId,
                    Name = Name,
                    SubTotal = SubTotal
                };
            }
        }
    }
}