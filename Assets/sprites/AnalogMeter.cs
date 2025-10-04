using UnityEngine;

public class AnalogMeter : MonoBehaviour
{
    [Header("Needle Settings")]
    public Transform needle; // ลากเข็มเข้ามาใน Inspector
    public float minAngle = -90f; // มุมซ้ายสุด
    public float maxAngle = 90f;  // มุมขวาสุด

    [Header("Resistance Settings")]
    public float minResistance = 0f;     // ค่าต่ำสุด (0 Ω)
    public float maxResistance = 10000f; // ค่าสูงสุด (10 kΩ)

    public void SetResistance(float resistance)
    {
        // จำกัดค่าไม่ให้ออกนอกช่วง
        resistance = Mathf.Clamp(resistance, minResistance, maxResistance);

        // แปลงค่า R → มุมเข็ม
        float t = (resistance - minResistance) / (maxResistance - minResistance);
        float angle = Mathf.Lerp(minAngle, maxAngle, t);

        // หมุนเข็ม (ทันที)
        needle.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
