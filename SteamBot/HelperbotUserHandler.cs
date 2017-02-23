using SteamKit2;
using System.Collections.Generic;
using SteamTrade;
using SteamTrade.TradeOffer;
using SteamTrade.TradeWebAPI;
using SteamKit2.Internal;
using System.Threading;
using System;

namespace SteamBot
{
    public class HelperbotUserHandler : UserHandler
    {
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
            try
            {
                Log.Success("Code: " + Bot.BuyMarketItem(message, 2, "P250 | Sand Dune (Well-Worn)"));
            }
            catch (Exception e)
            {
                Log.Error("Error: " + e);
            }
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

