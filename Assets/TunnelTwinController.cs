
using UnityEngine;

public class TunnelTwinController : MonoBehaviour
{
    [Header("ESP8266 Connection")]
    public ESP8266Client espClient;

    [Header("Warning Beacon")]
    public WarningBeacon warningBeacon;

    [Header("Tunnel Lights")]
    public Light[] tunnelLights;

    public float brightTunnelIntensity = 1f;
    public float darkTunnelIntensity = 5f;
    public float lightSmoothSpeed = 2f;

    [Header("Ventilation Fans")]
    public Transform[] fanBlades;
    public Vector3 fanRotationAxis = Vector3.forward;

    public float mediumFanSpeed = 180f;
    public float highFanSpeed = 360f;
    public float fanAcceleration = 200f;

    public float fanOnTemperature = 30f;
    public float fanHighTemperature = 34f;
    public float fanOffTemperature = 28f;
    public float fanHighToMediumTemperature = 32f;

    [Header("Temperature Alerts")]
    public float cautionTemperature = 30f;
    public float criticalTemperature = 40f;

    [Header("Humidity Alerts")]
    public float highHumidityLimit = 80f;

    [Header("Warning Clear Settings")]
    public float normalClearDelay = 5f;

    [Header("Wet Area")]
    public GameObject wetAreaEffect;

    private bool darkTunnel;
    private bool gasDetected;
    private bool wetAreaDetected;

    private bool temperatureCaution;
    private bool temperatureCritical;
    private bool humidityCaution;

    private float currentFanSpeed;
    private float targetFanSpeed;
    private float normalTimer;

    private bool sensorDataReceived;

    private enum FanMode
    {
        Off,
        Medium,
        High
    }

    private FanMode currentFanMode = FanMode.Off;

    private void OnEnable()
    {
        if (espClient != null)
        {
            espClient.OnSensorDataReceived += UpdateTunnel;
        }
    }

    private void OnDisable()
    {
        if (espClient != null)
        {
            espClient.OnSensorDataReceived -= UpdateTunnel;
        }
    }

    private void Start()
    {
        // Start with tunnel lights OFF until sensor data arrives.
        SetTunnelLights(false);

        if (wetAreaEffect != null)
        {
            wetAreaEffect.SetActive(false);
        }

        if (warningBeacon != null)
        {
            warningBeacon.SetSafe();
        }
    }

    private void Update()
    {
        UpdateTunnelLights();
        UpdateVentilationFans();
    }

    // ---------------- TUNNEL LIGHTING ----------------

    private void UpdateTunnelLights()
    {
        if (tunnelLights == null)
            return;

        if (!sensorDataReceived)
        {
            SetTunnelLights(false);
            return;
        }

        foreach (Light tunnelLight in tunnelLights)
        {
            if (tunnelLight == null)
                continue;

            if (darkTunnel)
            {
                // Dark tunnel: turn ON and increase brightness.
                tunnelLight.enabled = true;

                tunnelLight.intensity = Mathf.MoveTowards(
                    tunnelLight.intensity,
                    darkTunnelIntensity,
                    lightSmoothSpeed * Time.deltaTime
                );
            }
            else
            {
                // Bright tunnel: smoothly reduce brightness,
                // then turn OFF.
                tunnelLight.intensity = Mathf.MoveTowards(
                    tunnelLight.intensity,
                    0f,
                    lightSmoothSpeed * Time.deltaTime
                );

                if (tunnelLight.intensity <= 0.01f)
                {
                    tunnelLight.intensity = 0f;
                    tunnelLight.enabled = false;
                }
            }
        }
    }

    private void SetTunnelLights(bool state)
    {
        if (tunnelLights == null)
            return;

        foreach (Light tunnelLight in tunnelLights)
        {
            if (tunnelLight != null)
            {
                tunnelLight.enabled = state;

                if (!state)
                {
                    tunnelLight.intensity = 0f;
                }
            }
        }
    }

    // ---------------- VENTILATION FANS ----------------

    private void UpdateVentilationFans()
    {
        currentFanSpeed = Mathf.MoveTowards(
            currentFanSpeed,
            targetFanSpeed,
            fanAcceleration * Time.deltaTime
        );

        if (fanBlades == null || currentFanSpeed <= 0f)
            return;

        foreach (Transform fan in fanBlades)
        {
            if (fan != null)
            {
                fan.Rotate(
                    fanRotationAxis.normalized,
                    currentFanSpeed * Time.deltaTime,
                    Space.Self
                );
            }
        }
    }

    private void UpdateFanMode(float temperature)
    {
        switch (currentFanMode)
        {
            case FanMode.Off:

                if (temperature >= fanHighTemperature)
                {
                    currentFanMode = FanMode.High;
                }
                else if (temperature >= fanOnTemperature)
                {
                    currentFanMode = FanMode.Medium;
                }

                break;

            case FanMode.Medium:

                if (temperature >= fanHighTemperature)
                {
                    currentFanMode = FanMode.High;
                }
                else if (temperature < fanOffTemperature)
                {
                    currentFanMode = FanMode.Off;
                }

                break;

            case FanMode.High:

                if (temperature < fanOffTemperature)
                {
                    currentFanMode = FanMode.Off;
                }
                else if (temperature < fanHighToMediumTemperature)
                {
                    currentFanMode = FanMode.Medium;
                }

                break;
        }

        switch (currentFanMode)
        {
            case FanMode.Off:
                targetFanSpeed = 0f;
                break;

            case FanMode.Medium:
                targetFanSpeed = mediumFanSpeed;
                break;

            case FanMode.High:
                targetFanSpeed = highFanSpeed;
                break;
        }
    }

    // ---------------- ESP8266 SENSOR DATA ----------------

    private void UpdateTunnel(SensorData data)
    {
        if (data == null)
            return;

        sensorDataReceived = true;

        // LDR LIGHTING
        darkTunnel = data.dark_tunnel;

        // SOIL MOISTURE
        wetAreaDetected = data.wet_area;

        if (wetAreaEffect != null)
        {
            wetAreaEffect.SetActive(wetAreaDetected);
        }

        // GAS SENSOR
        gasDetected = data.gas_available && data.gas_detected;

        // TEMPERATURE
        if (data.temperatureValid)
        {
            temperatureCaution =
                data.temperature >= cautionTemperature;

            temperatureCritical =
                data.temperature >= criticalTemperature;

            UpdateFanMode(data.temperature);
        }
        else
        {
            temperatureCaution = false;
            temperatureCritical = false;

            currentFanMode = FanMode.Off;
            targetFanSpeed = 0f;
        }

        // HUMIDITY
        if (data.humidityValid)
        {
            humidityCaution =
                data.humidity >= highHumidityLimit;
        }
        else
        {
            humidityCaution = false;
        }

        UpdateWarningBeacon();

        if (!data.gas_available)
        {
            Debug.LogWarning(
                "GAS MONITORING UNAVAILABLE - MQ2 is disabled."
            );
        }

        if (gasDetected)
            Debug.LogError("GAS DETECTED!");

        if (temperatureCritical)
            Debug.LogError("CRITICAL: HIGH TEMPERATURE!");

        if (temperatureCaution && !temperatureCritical)
            Debug.LogWarning("CAUTION: HIGH TEMPERATURE!");

        if (humidityCaution)
            Debug.LogWarning("CAUTION: HIGH HUMIDITY!");

        if (wetAreaDetected)
            Debug.LogWarning("CAUTION: WET AREA!");
    }

    // ---------------- WARNING BEACON ----------------

    private void UpdateWarningBeacon()
    {
        if (warningBeacon == null)
            return;

        bool criticalAlert =
            gasDetected || temperatureCritical;

        bool cautionAlert =
            temperatureCaution ||
            humidityCaution ||
            wetAreaDetected;

        if (criticalAlert)
        {
            normalTimer = 0f;
            warningBeacon.SetCritical();
        }
        else if (cautionAlert)
        {
            normalTimer = 0f;
            warningBeacon.SetCaution();
        }
        else
        {
            normalTimer += Time.deltaTime;

            if (normalTimer >= normalClearDelay)
            {
                warningBeacon.SetSafe();
            }
        }
    }
}
