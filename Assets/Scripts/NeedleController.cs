using UnityEngine;

public class NeedleControllerUI : MonoBehaviour
{
    public RectTransform needleRect;
    public MeterMode currentMode = MeterMode.OFF;

    // ใช้ property setter เพื่อ debug ทุกครั้งที่มีการเปลี่ยนค่า
    [SerializeField]
    private float _measuredValue = 0f;
    public float measuredValue
    {
        get { return _measuredValue; }
        set
        {
            _measuredValue = value;
            Debug.Log($"[Needle Debug] {name} received new measuredValue: {_measuredValue} | Mode: {currentMode}");
        }
    }

    public float smoothSpeed = 5f;
    private float currentAngle = 0f;

    void Update()
    {
        if (needleRect == null) return;

        // ป้องกัน measuredValue เป็น NaN
        float safeValue = float.IsNaN(measuredValue) ? 0f : measuredValue;

        float targetAngle = GetNeedleAngle(currentMode, safeValue);
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * smoothSpeed);
        needleRect.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }

    public float GetNeedleAngle(MeterMode mode, float value)
    {
        switch (mode)
        {
            // AC Voltage
            case MeterMode.ACV1000: return Mathf.Lerp(45f, -45f, Mathf.Clamp01(value / 1000f));
            case MeterMode.ACV250:  return Mathf.Lerp(45f, -45f, Mathf.Clamp01(value / 250f));
            case MeterMode.ACV50:   return Mathf.Lerp(45f, -45f, Mathf.Clamp01(value / 50f));
            case MeterMode.ACV10:   return Mathf.Lerp(45f, -45f, Mathf.Clamp01(value / 10f));

            // Resistance (Ohm)
            case MeterMode.OHM10K: return GetOhmAngle(value, 10000f);
            case MeterMode.OHM1K:  return GetOhmAngle(value, 1000f);
            case MeterMode.OHM100: return GetOhmAngle(value, 100f);
            case MeterMode.OHM10:  return GetOhmAngle(value, 10f);
            case MeterMode.OHM1:   return GetOhmAngle(value, 1f);

            // DC Current
            case MeterMode.DCMAX250: return Mathf.Lerp(45f, -45f, Mathf.Clamp01(value / 250f));
            case MeterMode.DCMAX25:  return Mathf.Lerp(45f, -45f, Mathf.Clamp01(value / 25f));
            case MeterMode.DCMAX2_5: return Mathf.Lerp(45f, -45f, Mathf.Clamp01(value / 2.5f));
            case MeterMode.DC100uA:  
            case MeterMode.DCV0_25:  return Mathf.Lerp(45f, -45f, Mathf.Clamp01(value / 0.25f));

            // DC Voltage
            case MeterMode.DCV0_5:  return Mathf.Lerp(45f, -45f, Mathf.Clamp01(value / 0.5f));
            case MeterMode.DCV2_5:  return Mathf.Lerp(45f, -45f, Mathf.Clamp01(value / 2.5f));
            case MeterMode.DCV10:   return Mathf.Lerp(45f, -45f, Mathf.Clamp01(value / 10f));
            case MeterMode.DCV50:   return Mathf.Lerp(45f, -45f, Mathf.Clamp01(value / 50f));
            case MeterMode.DCV250:  return Mathf.Lerp(45f, -45f, Mathf.Clamp01(value / 250f));
            case MeterMode.DCV100:  return Mathf.Lerp(45f, -45f, Mathf.Clamp01(value / 100f));

            default: return 45f;
        }
    }

    private float GetOhmAngle(float resistance, float scale)
    {
        float[] resistances;
        float[] angles;

        switch (scale)
        {
            case 10000f:
                resistances = new float[] { 0f, 10000f, 50000f, 200000f, 9000000f };
                angles      = new float[] { -45f, -39f, -27f, -1f, 45f };
                break;
            case 1000f:
                resistances = new float[] { 0f, 1000f, 2000f, 20000f, 900000f };
                angles      = new float[] { -45f, -39f, -27f, -1f, 45f };
                break;
            case 100f:
                resistances = new float[] { 0f, 100f, 500f, 2000f, 90000f };
                angles      = new float[] { -45f, -39f, -27f, -1f, 45f };
                break;
            case 10f:
                resistances = new float[] { 0f, 10f, 50f, 200f, 9000f };
                angles      = new float[] { -45f, -39f, -27f, -1f, 45f };
                break;
            case 1f:
                resistances = new float[] { 0f, 1f, 5f, 20f, 900f };
                angles      = new float[] { -45f, -39f, -27f, -1f, 45f };
                break;
            default:
                return 45f;
        }

        if (resistance >= resistances[resistances.Length - 1])
            return angles[angles.Length - 1];

        for (int i = 0; i < resistances.Length - 1; i++)
        {
            if (resistance >= resistances[i] && resistance <= resistances[i + 1])
            {
                float t = (resistance - resistances[i]) / (resistances[i + 1] - resistances[i]);
                return Mathf.Lerp(angles[i], angles[i + 1], t);
            }
        }

        return -45f;
    }
}
