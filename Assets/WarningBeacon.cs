
using UnityEngine;

public class WarningBeacon : MonoBehaviour
{
    [Header("Beacon Components")]
    public Light beaconLight;
    public Renderer beaconRenderer;

    [Header("Beacon Colors")]
    public Color safeColor = Color.green;
    public Color cautionColor = Color.yellow;
    public Color criticalColor = Color.red;

    [Header("Safe Settings")]
    public float safeIntensity = 2f;

    [Header("Caution Flash Settings")]
    public float cautionFlashSpeed = 4f;
    public float cautionMinIntensity = 0.5f;
    public float cautionMaxIntensity = 4f;

    [Header("Critical Flash Settings")]
    public float criticalFlashSpeed = 12f;
    public float criticalMinIntensity = 0.2f;
    public float criticalMaxIntensity = 6f;

    public enum BeaconState
    {
        Safe,
        Caution,
        Critical
    }

    public BeaconState CurrentState { get; private set; }

    private Material beaconMaterial;

    private void Awake()
    {
        if (beaconRenderer != null)
        {
            beaconMaterial = beaconRenderer.material;

            if (beaconMaterial.HasProperty("_EmissionColor"))
            {
                beaconMaterial.EnableKeyword("_EMISSION");
            }
        }

        SetSafe();
    }

    private void Update()
    {
        Color currentColor;
        float currentIntensity;

        switch (CurrentState)
        {
            case BeaconState.Safe:

                currentColor = safeColor;
                currentIntensity = safeIntensity;
                break;

            case BeaconState.Caution:

                currentColor = cautionColor;

                float cautionValue =
                    Mathf.PingPong(Time.time * cautionFlashSpeed, 1f);

                currentIntensity = Mathf.Lerp(
                    cautionMinIntensity,
                    cautionMaxIntensity,
                    cautionValue
                );
                break;

            case BeaconState.Critical:

                currentColor = criticalColor;

                bool flashOn =
                    Mathf.Sin(Time.time * criticalFlashSpeed) > 0f;

                currentIntensity = flashOn
                    ? criticalMaxIntensity
                    : criticalMinIntensity;

                break;

            default:

                currentColor = safeColor;
                currentIntensity = safeIntensity;
                break;
        }

        ApplyBeaconAppearance(currentColor, currentIntensity);
    }

    public void SetSafe()
    {
        CurrentState = BeaconState.Safe;
    }

    public void SetCaution()
    {
        CurrentState = BeaconState.Caution;
    }

    public void SetCritical()
    {
        CurrentState = BeaconState.Critical;
    }

    public void SetState(BeaconState newState)
    {
        CurrentState = newState;
    }

    private void ApplyBeaconAppearance(Color color, float intensity)
    {
        if (beaconLight != null)
        {
            beaconLight.enabled = true;
            beaconLight.color = color;
            beaconLight.intensity = intensity;
        }

        if (beaconMaterial != null)
        {
            if (beaconMaterial.HasProperty("_Color"))
            {
                beaconMaterial.color = color;
            }

            if (beaconMaterial.HasProperty("_BaseColor"))
            {
                beaconMaterial.SetColor("_BaseColor", color);
            }

            if (beaconMaterial.HasProperty("_EmissionColor"))
            {
                beaconMaterial.SetColor(
                    "_EmissionColor",
                    color * intensity
                );
            }
        }
    }
}
