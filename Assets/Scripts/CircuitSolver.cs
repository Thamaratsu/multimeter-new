using UnityEngine;
using System.Linq;

public class CircuitSolver : MonoBehaviour
{
    [Header("Power Source Setup")]
    public float BatteryVoltage = 9f;
    public Node PositiveNode;   // Node ขั้วบวก
    public Node GroundNode;     // Node ขั้วลบ

    private Resistor[] allResistors;

    void Start()
    {
        allResistors = FindObjectsOfType<Resistor>();

        if (PositiveNode != null) PositiveNode.voltage = BatteryVoltage;
        if (GroundNode != null) GroundNode.voltage = 0f;
    }

    void Update()
    {
        SolveCircuit();
    }

    void SolveCircuit()
    {
        if (allResistors == null || allResistors.Length == 0) return;

        // แยก Series / Parallel
        var seriesResistors = allResistors.Where(r => r.type == ResistorType.Series && !r.manualMode).ToArray();
        var parallelGroups = allResistors.Where(r => r.type == ResistorType.Parallel && !r.manualMode)
                                         .GroupBy(r => r.parallelGroupID)
                                         .Select(g => g.ToArray())
                                         .ToArray();

        // --- Series ---
        float totalSeriesResistance = seriesResistors.Sum(r => r.resistance);
        float currentSeries = totalSeriesResistance > 0 ? BatteryVoltage / totalSeriesResistance : 0f;
        float voltageCursor = BatteryVoltage;

        foreach (var r in seriesResistors)
        {
            r.current = currentSeries;
            r.voltageDrop = r.current * r.resistance;

            if (r.nodeA != null) r.nodeA.voltage = voltageCursor;
            voltageCursor -= r.voltageDrop;
            if (r.nodeB != null) r.nodeB.voltage = voltageCursor;

            r.UpdateTerminalCurrent();

            Debug.Log($"[Series] {r.name} | R={r.resistance} | I={r.current} | Vdrop={r.voltageDrop}");
        }

        // --- Parallel ---
        foreach (var group in parallelGroups)
        {
            if (group.Length == 0) continue;

            Node inputNode = group[0].nodeA;
            Node outputNode = group[0].nodeB;
            float V_in = inputNode != null ? inputNode.voltage : 0f;
            float V_out = outputNode != null ? outputNode.voltage : 0f;
            float voltageAcross = V_in - V_out;

            foreach (var r in group)
            {
                r.voltageDrop = voltageAcross;
                r.current = r.resistance > 0f ? voltageAcross / r.resistance : 0f;

                if (r.nodeB != null) r.nodeB.voltage = r.nodeA.voltage - r.voltageDrop;
                r.UpdateTerminalCurrent();

                Debug.Log($"[Parallel] {r.name} | R={r.resistance} | I={r.current} | Vdrop={r.voltageDrop}");
            }
        }

        // --- Node บวก/ลบ ---
        if (PositiveNode != null) PositiveNode.voltage = BatteryVoltage;
        if (GroundNode != null) GroundNode.voltage = 0f;

        // --- อัปเดต Resistor ที่ Manual Mode ---
        foreach (var r in allResistors.Where(r => r.manualMode))
        {
            r.ApplyManualValues();
        }
    }
}
