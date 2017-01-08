using SteamKit2;
using System.Collections.Generic;
using SteamTrade;
using SteamTrade.TradeOffer;
using SteamTrade.TradeWebAPI;
using SteamKit2.Internal;
using System.Threading;

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

            SteamID ownId = new SteamID(76561198031559559);
            SteamID mId = new SteamID(76561198012831233);
            //SendGroupAnnouncement("SirPlease", "Testannouncement", "Testbody");
            //PostProfileComment(new SteamID(76561198031559559), "-rep wh");
            //var groups = GetGroupInvitelist(mId);
            //int i = 0;
            //foreach (var group in groups)
            //{
            //    i++;
            //    Log.Info("Found group: " + group.Title);
            //    if(i > 20)
            //    {
            //        Log.Success("Sent invitation: " + SendGroupInvitation(mId, group.GroupId));
            //    }
            //}
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
            Bot.RequestFloat(message, PrintFloat);
        }

        public void PrintFloat(float value)
        {
            SendChatMessage("Wear value: " + value);
            Log.Success("Wear value: " + value);
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

