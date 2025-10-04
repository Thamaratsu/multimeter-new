using UnityEngine;

public enum ResistorType { Series, Parallel }

public class Resistor : MonoBehaviour
{
    [Header("Nodes")]
    public Node nodeA; // ทางเข้า (เชื่อมกับ terminalA)
    public Node nodeB; // ทางออก  (เชื่อมกับ terminalB)

    [Header("Terminals")]
    public Terminal terminalA;
    public Terminal terminalB;

    [Header("Settings")]
    public float resistance = 1000f;
    public ResistorType type = ResistorType.Series;
    public int parallelGroupID = 0;

    [Header("Calculated / Manual")]
    public float voltageDrop;
    public float current;

    [Header("Manual Mode")]
    public bool manualMode = false; // true → User ปรับค่าเอง

    [Header("Manual Values")]
    public float manualVoltageDrop = 0f;
    public float manualCurrent = 0f;
    public float manualResistance = 1000f;

    void Start()
    {
        // ส่งค่าเริ่มต้นให้ Terminal
        if (terminalA != null) terminalA.resistance = resistance;
        if (terminalB != null) terminalB.resistance = resistance;
    }

    void Update()
    {
        if (manualMode)
        {
            ApplyManualValues();
        }
    }

    /// <summary>
    /// ใช้ค่า Manual จาก Inspector
    /// </summary>
    public void ApplyManualValues()
    {
        voltageDrop = manualVoltageDrop;
        current = manualCurrent;
        resistance = manualResistance;

        // อัปเดต Terminal
        UpdateTerminalCurrent();

        Debug.Log($"[Manual] {name} | R={resistance} | ΔV={voltageDrop} | I={current} | NodeA={nodeA?.voltage} | NodeB={nodeB?.voltage}");
    }

    /// <summary>
    /// อัปเดต Terminal.current ให้ ProbeController อ่านค่าได้
    /// </summary>
    public void UpdateTerminalCurrent()
    {
        if (terminalA != null)
        {
            terminalA.current = current;
            terminalA.resistance = resistance;
        }
        if (terminalB != null)
        {
            terminalB.current = current;
            terminalB.resistance = resistance;
        }
    }

    /// <summary>
    /// ตรวจสอบว่า Probe คร่อม Resistor หรือไม่
    /// </summary>
    public bool IsProbed(Transform probe1, Transform probe2)
    {
        if (terminalA == null || terminalB == null) return false;
        bool case1 = terminalA.IsProbeTouching(probe1) && terminalB.IsProbeTouching(probe2);
        bool case2 = terminalA.IsProbeTouching(probe2) && terminalB.IsProbeTouching(probe1);
        return case1 || case2;
    }

    /// <summary>
    /// Auto connect Nodes
    /// </summary>
    public void AutoConnectNodes()
    {
        ConnectNodeIfNear(nodeA);
        ConnectNodeIfNear(nodeB);
        Debug.Log($"Resistor {name} → AutoConnectNodes เสร็จแล้ว");
    }

    void ConnectNodeIfNear(Node node)
    {
        if (node == null) return;

        Node[] allNodes = FindObjectsOfType<Node>();
        foreach (Node n in allNodes)
        {
            if (n != node)
            {
                float dy = Mathf.Abs(n.transform.position.y - node.transform.position.y);
                if (dy <= 0.2f)
                {
                    node.ConnectNode(n);
                }
            }
        }
    }
}
