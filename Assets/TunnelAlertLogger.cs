
using UnityEngine;

public class TunnelAlertLogger : MonoBehaviour
{
    [Header("ESP8266 Connection")]
    public ESP8266Client espClient;

    private void OnEnable()
    {
        if (espClient != null)
        {
            espClient.OnSensorDataReceived += LogAlerts;
        }
    }

    private void OnDisable()
    {
        if (espClient != null)
        {
            espClient.OnSensorDataReceived -= LogAlerts;
        }
    }

    private void LogAlerts(SensorData data)
    {
        if (data == null) return;

        if (data.gas_available && data.gas_detected)
        {
            Debug.LogWarning("TUNNEL ALERT: GAS DETECTED!");
        }

        if (data.temperature_alert)
        {
            Debug.LogWarning("TUNNEL ALERT: HIGH TEMPERATURE!");
        }

        if (data.humidity_alert)
        {
            Debug.LogWarning("TUNNEL ALERT: HIGH HUMIDITY!");
        }

        if (data.wet_area)
        {
            Debug.LogWarning("TUNNEL ALERT: WET AREA!");
        }
    }
}
