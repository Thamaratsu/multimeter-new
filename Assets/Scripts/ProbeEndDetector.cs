using UnityEngine;

// สคริปต์นี้จะติดอยู่กับปลาย Probe
public class ProbeEndDetector : MonoBehaviour
{
    // ตัวแปรนี้จะเก็บ Terminal ที่เรากำลังชนอยู่
    [HideInInspector]
    public Terminal connectedTerminal = null;

    // ตรวจจับเมื่อ Probe ชนกับ Collider อื่น
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ตรวจสอบว่าวัตถุที่ชนมี Component 'Terminal' หรือไม่
        if (other.TryGetComponent<Terminal>(out Terminal t))
        {
            // ถ้าชน แสดงว่าเชื่อมต่อสำเร็จ
            connectedTerminal = t;
            // Debug.Log($"{gameObject.name} connected to {t.name}");
        }
    }

    // ตรวจจับเมื่อ Probe เลิกชนกับ Collider
    private void OnTriggerExit2D(Collider2D other)
    {
        // ตรวจสอบว่า Terminal ที่กำลังจะออกจากการชน คือ Terminal ตัวที่เราเชื่อมต่ออยู่หรือไม่
        if (other.TryGetComponent<Terminal>(out Terminal t) && t == connectedTerminal)
        {
            // ถ้าใช่ ให้ยกเลิกการเชื่อมต่อ
            connectedTerminal = null;
            // Debug.Log($"{gameObject.name} disconnected from {t.name}");
        }
    }

    // ฟังก์ชันนี้จะถูกเรียกเมื่อ Probe ถูกลาก (เพื่อ Clear ค่าชั่วคราว)
    // การเรียกใช้ใน UpdateMeasurement อาจมีปัญหาเมื่อวัตถุถูกลาก
    public void Disconnect()
    {
        connectedTerminal = null;
    }
}