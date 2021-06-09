//using Open.Nat;
//using UnityEngine;
//using System.Threading;
//using System.Threading.Tasks;

//public class UPnP : MonoBehaviour {
//    private static int _port = 7777;
//    private static string _description = "Mapping created by Slap";

//    public static async Task SetMapping() {
//        NatDiscoverer discoverer = new NatDiscoverer();
//        CancellationTokenSource cts = new CancellationTokenSource(10000);
//        NatDevice device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);

//        await device.CreatePortMapAsync(new Mapping(Protocol.Udp, _port, _port, _description));
//    }
//}