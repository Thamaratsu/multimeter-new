using UnityEngine;

public class Terminal : MonoBehaviour
{
    public Node node; // Node ที่ Terminal เชื่อมต่อ

    [Header("Electrical Settings")]
    public float current;   // กระแสที่ Terminal รับ (อัปเดตโดย CircuitSolver)
    public float resistance; // ตัวเก็บความต้านทานจาก Resistor (ตั้งโดย Resistor)

    [Header("Probe Settings")]
    public float connectThresholdX = 0.2f;
    public float connectThresholdY = 0.5f;

    void Awake()
    {
        // ถ้าอยากให้ตรวจจับด้วย Physics ก็เปิดคอลลิเดอร์ได้ที่นี่ (ไม่บังคับ)
        if (GetComponent<Collider2D>() == null)
        {
            var col = gameObject.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 0.12f;
        }
    }

    // ตรวจสอบว่า Probe คร่อม Terminal หรือไม่
    public bool IsProbeTouching(Transform probe)
    {
        if (probe == null) return false;
        float dx = Mathf.Abs(probe.position.x - transform.position.x);
        float dy = Mathf.Abs(probe.position.y - transform.position.y);
        return dx <= connectThresholdX && dy <= connectThresholdY;
    }
}
