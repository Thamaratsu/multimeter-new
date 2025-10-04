using UnityEngine;
using System.Linq;

public class VoltageAggregator : MonoBehaviour
{
    [Header("Circuit Setup")]
    public float batteryVoltage = 9f;
    public Node positiveNode;
    public Node groundNode;

    [Header("All Resistors")]
    public Resistor[] resistors;

    [Header("Results")]
    public float totalCurrent;
    public float totalResistance;

    void Start()
    {
        if (groundNode != null) groundNode.voltage = 0f;
        if (positiveNode != null) positiveNode.voltage = batteryVoltage;

        if (resistors == null || resistors.Length == 0)
            resistors = FindObjectsOfType<Resistor>();
    }

    void Update()
    {
        Aggregate();
    }

    void Aggregate()
    {
        if (resistors.Length == 0) return;

        // เฉพาะ Resistor Auto Mode
        var autoResistors = resistors.Where(r => !r.manualMode).ToArray();

        var seriesResistors = autoResistors.Where(r => r.type == ResistorType.Series).ToArray();
        var parallelGroups = autoResistors.Where(r => r.type == ResistorType.Parallel)
                                          .GroupBy(r => r.parallelGroupID)
                                          .Select(g => g.ToArray())
                                          .ToArray();

        // 1️⃣ ความต้านทานรวม
        totalResistance = seriesResistors.Sum(r => r.resistance);
        foreach (var group in parallelGroups)
        {
            float sumInv = group.Where(r => r.resistance > 0f).Sum(r => 1f / r.resistance);
            if (sumInv > 0f) totalResistance += 1f / sumInv;
        }

        // 2️⃣ กระแสรวม
        totalCurrent = totalResistance > 0f ? batteryVoltage / totalResistance : 0f;

        // 3️⃣ กระจายค่ากระแสให้ Series
        foreach (var r in seriesResistors)
        {
            r.current = totalCurrent;
            r.UpdateTerminalCurrent();
        }

        // 4️⃣ กระจายกระแสให้ Parallel
        foreach (var group in parallelGroups)
        {
            float V_group = batteryVoltage;
            foreach (var r in group)
            {
                r.current = r.resistance > 0f ? V_group / r.resistance : 0f;
                r.UpdateTerminalCurrent();
            }
        }

        // 5️⃣ อัปเดต Manual Resistor
        foreach (var r in resistors.Where(r => r.manualMode))
        {
            r.ApplyManualValues();
        }

        // 6️⃣ อัปเดต Node บวก / ลบ
        if (groundNode != null) groundNode.voltage = 0f;
        if (positiveNode != null) positiveNode.voltage = batteryVoltage;
    }
}
