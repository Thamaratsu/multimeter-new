using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class ProbeCase
{
    public Terminal expectedTerminalA;   // ขั้วที่ Probe A ต้องแตะ
    public Terminal expectedTerminalB;   // ขั้วที่ Probe B ต้องแตะ
    public NeedleControllerUI needle;

    [Header("Expected Measurement Values")]
    public float voltage = 0f;
    public float resistance = 0f;
    public float current = 0f;
}

public class ProbeController : MonoBehaviour
{
    [Header("Probe Transforms (ใช้ในการลาก)")]
    public Transform probe1Start;
    public Transform probe1End;   // Probe แดง
    public Transform probe2Start;
    public Transform probe2End;   // Probe ดำ

    [Header("Wire Rendering (Optional)")]
    public LineRenderer wire1;
    public LineRenderer wire2;

    [Header("Grid / UI (Optional)")]
    public Tilemap gridTilemap;

    [Header("Measurement Cases (ตั้งเงื่อนไขได้หลายแบบ)")]
    public ProbeCase[] probeCases;

    private Camera cam;
    private Transform draggingProbe = null;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleDragging();
        UpdateWires();
        UpdateMeasurement();
    }

    // ------------------- Dragging -------------------
    void HandleDragging()
    {
        if (cam == null) return;

        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        if (Input.GetMouseButtonDown(0))
            TryStartDrag(mouseWorldPos);
        else if (Input.GetMouseButton(0) && draggingProbe != null)
        {
            if (gridTilemap != null)
            {
                Vector3Int cellPos = gridTilemap.WorldToCell(mouseWorldPos);
                draggingProbe.position = gridTilemap.GetCellCenterWorld(cellPos);
            }
            else
            {
                draggingProbe.position = mouseWorldPos;
            }
        }
        else if (Input.GetMouseButtonUp(0) && draggingProbe != null)
        {
            SnapToGrid(draggingProbe);
            draggingProbe = null;
        }
    }

    void TryStartDrag(Vector3 inputWorldPos)
    {
        float dragRadius = 0.25f;
        if (probe1End != null && Vector3.Distance(inputWorldPos, probe1End.position) < dragRadius)
            draggingProbe = probe1End;
        else if (probe2End != null && Vector3.Distance(inputWorldPos, probe2End.position) < dragRadius)
            draggingProbe = probe2End;
    }

    void SnapToGrid(Transform probe)
    {
        if (gridTilemap == null) return;
        Vector3Int cellPosition = gridTilemap.WorldToCell(probe.position);
        probe.position = gridTilemap.GetCellCenterWorld(cellPosition);
    }

    // ------------------- Update Wire Positions -------------------
    void UpdateWires()
    {
        if (wire1 != null && probe1Start != null && probe1End != null)
        {
            wire1.SetPosition(0, probe1Start.position);
            wire1.SetPosition(1, probe1End.position);
        }
        if (wire2 != null && probe2Start != null && probe2End != null)
        {
            wire2.SetPosition(0, probe2Start.position);
            wire2.SetPosition(1, probe2End.position);
        }
    }

    // ------------------- Measurement -------------------
    void UpdateMeasurement()
    {
        Terminal t1 = GetTerminalAtEndProbe(probe1End);
        Terminal t2 = GetTerminalAtEndProbe(probe2End);

        bool anyMatched = false;

        foreach (var pc in probeCases)
        {
            if (pc.needle == null || pc.expectedTerminalA == null || pc.expectedTerminalB == null)
                continue;

            // Debug: แสดง Probe และ Terminal ที่ตรวจจับได้
            Debug.Log($"[DEBUG] Probe1End {probe1End.name} → Terminal: {(t1 != null ? t1.name : "None")}");
            Debug.Log($"[DEBUG] Probe2End {probe2End.name} → Terminal: {(t2 != null ? t2.name : "None")}");

            // เช็คว่าคู่โพรบตรงกับที่ตั้งไว้หรือไม่ (ไม่สนว่าแดงหรือดำอยู่ด้านไหน)
            bool matched =
                (t1 == pc.expectedTerminalA && t2 == pc.expectedTerminalB) ||
                (t1 == pc.expectedTerminalB && t2 == pc.expectedTerminalA);

            if (matched)
            {
                anyMatched = true;
                float value = 0f;
                string mode = pc.needle.currentMode.ToString();

                if (mode.StartsWith("DCV") || mode.StartsWith("AC_V"))
                    value = pc.voltage;
                else if (mode.StartsWith("DCMAX") || mode.StartsWith("DC100uA"))
                    value = pc.current;
                else
                    value = pc.resistance;

                pc.needle.measuredValue = value;

                Debug.Log($"✅ {pc.needle.name} MATCHED: {pc.expectedTerminalA.name} ↔ {pc.expectedTerminalB.name}");
                Debug.Log($"   Mode: {mode}");
                Debug.Log($"   Sent Values → Voltage: {pc.voltage}, Current: {pc.current}, Resistance: {pc.resistance}");
                Debug.Log($"   [Needle Debug] Needle received new measuredValue: {value}");
            }
        }

        // ถ้าไม่มี case ไหน matched ถึง reset เป็น 0
        if (!anyMatched)
        {
            foreach (var pc in probeCases)
            {
                if (pc.needle != null)
                {
                    pc.needle.measuredValue = 0f;
                    Debug.Log($"❌ {pc.needle.name} NO MATCH, measuredValue=0");
                    Debug.Log($"   [Needle Debug] Needle received new measuredValue: 0");
                }
            }
        }
    }

    // ------------------- Terminal Detection (Collider + Debug) -------------------
    Terminal GetTerminalAtEndProbe(Transform endProbe)
    {
        float snapRadius = 0.1f; // รัศมีตรวจจับรอบ ๆ ปลาย Probe
        Collider2D[] hits = Physics2D.OverlapCircleAll(endProbe.position, snapRadius);

        if (hits.Length == 0)
        {
            Debug.Log($"[DEBUG] Probe {endProbe.name} at {endProbe.position} ไม่เจอ Terminal รอบ ๆ");
        }

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent<Terminal>(out Terminal t))
            {
                float distance = Vector3.Distance(endProbe.position, t.transform.position);
                Debug.Log($"[DEBUG] Probe {endProbe.name} at {endProbe.position} พบ Terminal {t.name} at {t.transform.position}, ระยะ {distance:F3}");
                return t; // เจอ Terminal ตัวแรกก็รีเทิร์น
            }
        }

        return null; // ไม่เจอ Terminal
    }
}
