
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;

[Serializable]
public class SensorData
{
    public float temperature;
    public float humidity;

    public bool gas_detected;
    public bool gas_available;
    public bool dark_tunnel;
    public bool wet_area;

    public bool temperature_alert;
    public bool humidity_alert;

    [NonSerialized] public bool temperatureValid;
    [NonSerialized] public bool humidityValid;
}

public class ESP8266Client : MonoBehaviour
{
    [Header("ESP8266 Settings")]
    public string esp8266IP = "192.168.0.135";
    public int port = 80;
    public string endpoint = "/data";

    [Header("Polling Settings")]
    public float updateInterval = 3f;
    public int requestTimeout = 15;

    public SensorData LatestData { get; private set; }
    public bool IsConnected { get; private set; }

    public event Action<SensorData> OnSensorDataReceived;
    public event Action<bool> OnConnectionChanged;

    private Coroutine pollingCoroutine;

    private void Start()
    {
        Debug.Log("ESP8266 Client Started");
        Debug.Log("ESP URL: " + GetURL());

        StartPolling();
    }

    private string GetURL()
    {
        return "http://" + esp8266IP.Trim() + ":" +
               port + endpoint;
    }

    public void StartPolling()
    {
        if (pollingCoroutine == null)
        {
            pollingCoroutine = StartCoroutine(PollSensors());
        }
    }

    public void StopPolling()
    {
        if (pollingCoroutine != null)
        {
            StopCoroutine(pollingCoroutine);
            pollingCoroutine = null;
        }
    }

    private IEnumerator PollSensors()
    {
        while (true)
        {
            string url = GetURL();

            using (UnityWebRequest request =
                   UnityWebRequest.Get(url))
            {
                request.timeout = requestTimeout;

                Debug.Log("Sending request to: " + url);

                yield return request.SendWebRequest();

                Debug.Log("HTTP Response Code: " +
                          request.responseCode);

                if (request.result ==
                    UnityWebRequest.Result.Success)
                {
                    string json = request.downloadHandler.text;

                    Debug.Log("ESP8266 DATA: " + json);

                    SensorData data =
                        JsonUtility.FromJson<SensorData>(json);

                    if (data != null)
                    {
                        data.temperatureValid =
                            !json.Contains("\"temperature\":null");

                        data.humidityValid =
                            !json.Contains("\"humidity\":null");

                        LatestData = data;

                        SetConnection(true);

                        OnSensorDataReceived?.Invoke(data);
                    }
                    else
                    {
                        Debug.LogError("JSON parsing failed!");
                        SetConnection(false);
                    }
                }
                else
                {
                    Debug.LogError(
                        "ESP8266 REQUEST FAILED\n" +
                        "URL: " + url + "\n" +
                        "Error: " + request.error + "\n" +
                        "Response Code: " + request.responseCode
                    );

                    SetConnection(false);
                }
            }

            yield return new WaitForSeconds(updateInterval);
        }
    }

    private void SetConnection(bool connected)
    {
        if (IsConnected != connected)
        {
            IsConnected = connected;
            OnConnectionChanged?.Invoke(connected);

            Debug.Log(
                connected ?
                "ESP8266 CONNECTED!" :
                "ESP8266 DISCONNECTED!"
            );
        }
    }

    private void OnDestroy()
    {
        StopPolling();
    }
}
