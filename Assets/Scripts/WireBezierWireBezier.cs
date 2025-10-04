using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WireFourPoints : MonoBehaviour
{
    public Transform p0; // จุดเริ่ม (Battery)
    public Transform p1; // มุม 1
    public Transform p2; // มุม 2
    public Transform p3; // จุดปลาย (Resistor)

    private LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 4; // 4 จุด
    }

    void Update()
    {
        lr.SetPosition(0, p0.position);
        lr.SetPosition(1, p1.position);
        lr.SetPosition(2, p2.position);
        lr.SetPosition(3, p3.position);
    }
}
