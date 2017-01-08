using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using SteamKit2.GC;
using SteamKit2;
using SteamKit2.GC.CSGO.Internal;

namespace SteamBot.Model
{
    public class InspectItem
    {
        public ulong param_s { get; set; } = 0;
        public ulong param_a { get; set; } = 0;
        public ulong param_d { get; set; } = 0;
        public ulong param_m { get; set; } = 0;

        public static InspectItem FromLink(string link)
        {
            Match matches = LINK_REGEX.Match(link.ToLower());

            if (!matches.Success)
                throw new ArgumentException("Bad inspection link");

            return new InspectItem
            {
                param_s = matches.Groups["S"].Success ? Convert.ToUInt64(matches.Groups["S"].Value) : 0,
                param_a = Convert.ToUInt64(matches.Groups["A"].Value),
                param_d = Convert.ToUInt64(matches.Groups["D"].Value),
                param_m = matches.Groups["M"].Success ? Convert.ToUInt64(matches.Groups["M"].Value) : 0,
            };
        }

        public bool Send(SteamGameCoordinator steamGC)
        {
            var request = new ClientGCMsgProtobuf<CMsgGCCStrike15_v2_Client2GCEconPreviewDataBlockRequest>((uint)ECsgoGCMsg.k_EMsgGCCStrike15_v2_Client2GCEconPreviewDataBlockRequest);

            request.Body.param_s = param_s;
            request.Body.param_a = param_a;
            request.Body.param_d = param_d;
            request.Body.param_m = param_m;

            Console.WriteLine($"Requesting skin: ASSETID[{param_a}], DICKID[{param_d}], STEAMID[{param_s}], MARKETID[{param_m}]");

            steamGC.Send(request, 730);

            return true;
        }

        public override string ToString()
        {
            return $"S[{param_s}] A[{param_a}] D[{param_d}] M[{param_m}]";
        }

        public static Regex LINK_REGEX = new Regex(@"(?:s(?<S>\d+)|m(?<M>\d+))a(?<A>\d+)d(?<D>\d+)", RegexOptions.Compiled);
    }
}
