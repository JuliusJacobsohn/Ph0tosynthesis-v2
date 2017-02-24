using SteamKit2;
using System.Collections.Generic;
using SteamTrade;
using SteamTrade.TradeOffer;
using SteamTrade.TradeWebAPI;
using SteamKit2.Internal;
using System.Threading;
using System;
using Newtonsoft.Json;
using System.Linq;
using SteamBot.Model;

namespace SteamBot
{
    public class HelperbotUserHandler : UserHandler
    {
        Thread CurrentMarketThread { get; set; }
        public HelperbotUserHandler(Bot bot, SteamID sid) : base(bot, sid)
        {
        }

        public override void OnLoginCompleted()
        {
            Log.Info("Hey b0ss");
        }

        public override void OnTradeOfferUpdated(TradeOffer offer)
        {

            switch (offer.OfferState)
            {
                case TradeOfferState.TradeOfferStateAccepted:
                    Log.Info($"Trade offer {offer.TradeOfferId} has been completed!");
                    //SendChatMessage("Trade completed, thank you!");
                    break;
                case TradeOfferState.TradeOfferStateActive:
                    if (IsAdmin)
                    {
                        offer.Accept();
                        Bot.AcceptAllMobileTradeConfirmations();
                        SendChatMessage("Accepted admin tradeoffer");
                    }
                    break;
                case TradeOfferState.TradeOfferStateNeedsConfirmation:
                case TradeOfferState.TradeOfferStateInEscrow:
                    //Trade is still active but incomplete
                    break;
                case TradeOfferState.TradeOfferStateCountered:
                    Log.Info($"Trade offer {offer.TradeOfferId} was countered");
                    break;
                default:
                    Log.Info($"Trade offer {offer.TradeOfferId} failed");
                    break;
            }
        }

        #region Community
        public override bool OnGroupAdd()
        {
            return false;
        }

        public override bool OnFriendAdd()
        {
            return IsAdmin;
        }
        public override void OnFriendRemove()
        {
        }
        #endregion

        #region Chat
        public override void OnMessage(string message, EChatEntryType type)
        {
            CurrentMarketThread = new Thread(t =>
            {
                while (true)
                {
                    bool cancel = false;
                    int start = 0;
                    int amount = 100;
                    while (!cancel)
                    {
                        try
                        {
                            var mResp = Bot.GetMarketResponse(message, start, amount);
                            var inexpensiveSkins = mResp.Listings.Where(v => v.SubTotal <= 5);
                            Log.Success("Found " + inexpensiveSkins.Count() + " in the range from " + start + " to " + (start + amount));
                            if (!inexpensiveSkins.Any())
                            {
                                cancel = true;
                            }
                            else
                            {
                                foreach (var listing in inexpensiveSkins)
                                {
                                    BuyIfLowerThan(listing, 0.016f);
                                    //Log.Debug("Sleeping 3 secs");
                                    Thread.Sleep(1000);
                                }
                                start = start + amount;
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Warn("Error occurred in buy thread: " + e);
                        }
                    }
                }
            });
            if (message == "stop")
            {
                CurrentMarketThread.Abort();
                CurrentMarketThread = null;
            }
            else
            {
                CurrentMarketThread.Start();
            }
        }

        public void BuyIfLowerThan(ListingInfo listing, float wear)
        {
            Bot.RequestFloat(listing.InspectLink, v =>
            {
                if (v <= wear)
                {
                    Bot.BuyMarketItem(listing);
                    SendChatMessage("Bought listing " + listing.InternalId + ", Float: " + v);
                    Log.Success("Bought listing " + listing.InternalId + ", Float: " + v);
                }
                else
                {
                    Log.Warn("Didn't buy " + listing.Name + ", Float too high: " + v);
                }
            });
        }

        public override void OnChatRoomMessage(SteamID chatID, SteamID sender, string message)
        {
            Log.Info(Bot.SteamFriends.GetFriendPersonaName(sender) + ": " + message);
        }
        #endregion

        #region Trade
        public override bool OnTradeRequest()
        {
            return false;
        }
        public override void OnTradeAwaitingConfirmation(long tradeOfferID) { }
        public override void OnTradeAccept() { }
        public override void OnTradeReady(bool ready) { }
        public override void OnTradeMessage(string message) { }
        public override void OnTradeAddItem(Schema.Item schemaItem, Inventory.Item inventoryItem) { }
        public override void OnTradeRemoveItem(Schema.Item schemaItem, Inventory.Item inventoryItem) { }
        public override void OnTradeTimeout()
        {
            Log.Info("User was kicked because he was AFK.");
        }
        public override void OnTradeError(string error)
        {
            Log.Warn(error);
        }
        public override void OnTradeInit() { }
        #endregion

    }

}

