
using System;

[Serializable]
public class TunnelTwinData
{
    public float temperature;
    public float humidity;

    public bool dht_valid;

    public bool gas_detected;
    public bool gas_available;

    public bool dark_tunnel;
    public bool wet_area;

    public bool temperature_alert;
    public bool humidity_alert;
}
