using Open.Nat;
using UnityEngine;
using TMPro;
using System.Threading;

public class UPnP : MonoBehaviour {
    [Header("Objects")]
    [SerializeField] private TextMeshProUGUI yourIpText;

    private async void Start() {
        NatDiscoverer discoverer = new NatDiscoverer();
        CancellationTokenSource cts = new CancellationTokenSource(10000);
        NatDevice device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);

        System.Net.IPAddress ip = await device.GetExternalIPAsync();
        yourIpText.text = "Your IP: " + ip;

        await device.CreatePortMapAsync(new Mapping(Protocol.Udp, 7777, 7777, "Slap mapping"));
    }
}