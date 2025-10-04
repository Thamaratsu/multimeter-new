using UnityEngine;

public class KnobToMeterLinker : MonoBehaviour
{
    public KnobSnapRotator knob;         // ลากปุ่ม (GameObject ที่มี KnobSnapRotator)
    public NeedleControllerUI needle;    // ลากเข็ม
    public MeterMode[] positionToMode;   // กรอกใน Inspector (Size = numberOfPositions)

    [Tooltip("ถ้าตั้งเป็น -1 จะคำนวณ topIndex อัตโนมัติ")]
    public int topIndex = -1;            // index ที่ตรงกับ 12 นาฬิกา
    public bool invertDirection = false; // ถ้าคุณเติม array เป็นลำดับตามเข็ม ให้ tick เป็น true

    private int positionsCount;

    void Start()
    {
        if (knob == null || needle == null)
        {
            Debug.LogError("[KnobToMeterLinker] knob or needle is null");
            return;
        }

        positionsCount = knob.numberOfPositions;

        if (positionToMode == null || positionToMode.Length != positionsCount)
        {
            Debug.LogWarning("[KnobToMeterLinker] กรุณาตั้ง positionToMode ให้เท่ากับจำนวนย่าน (Size = " + positionsCount + ")");
        }

        if (topIndex < 0 || topIndex >= positionsCount)
        {
            // คำนวณ topIndex จาก offsetAngle และ numberOfPositions
            float stepAngle = 360f / positionsCount;
            float offset = knob.offsetAngle;
            topIndex = Mathf.RoundToInt(((90f - offset + 360f) % 360f) / stepAngle) % positionsCount;
            Debug.Log("[KnobToMeterLinker] computed topIndex = " + topIndex);
        }
        else
        {
            Debug.Log("[KnobToMeterLinker] using inspector topIndex = " + topIndex);
        }
    }

    void Update()
    {
        if (positionToMode == null || positionToMode.Length == 0) return;

        int pos = knob.GetSelectedPosition(); // ตำแหน่งดิจิทัลที่ฟังก์ชันคืน (0..N-1)

        int logicalIndex;
        if (!invertDirection)
        {
            // pos นับตามฟังก์ชัน (CCW) → แปลงเป็น logical index เริ่มจาก topIndex
            logicalIndex = (pos - topIndex + positionsCount) % positionsCount;
        }
        else
        {
            // ถ้าคุณกรอก array ตามเข็ม ให้กลับทิศทาง
            logicalIndex = (topIndex - pos + positionsCount) % positionsCount;
        }

        if (logicalIndex >= 0 && logicalIndex < positionToMode.Length)
        {
            needle.currentMode = positionToMode[logicalIndex];
        }
    }
}
