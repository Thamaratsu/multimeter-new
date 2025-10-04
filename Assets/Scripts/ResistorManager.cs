using UnityEngine;

public class ResistorManager : MonoBehaviour
{
    [Header("All Resistors in Scene")]
    public Resistor[] allResistors;

    void Start()
    {
        // ถ้าไม่ได้กำหนดใน Inspector ให้ค้นหาเอง
        if (allResistors == null || allResistors.Length == 0)
        {
            allResistors = FindObjectsOfType<Resistor>();
        }

        Debug.Log("ResistorManager → พบ Resistor จำนวน: " + allResistors.Length);

        // เรียก AutoConnectNodes() ของทุก Resistor
        foreach (var r in allResistors)
        {
            if (r != null)
            {
                Debug.Log("ResistorManager → เรียก AutoConnectNodes() สำหรับ: " + r.name);
                r.AutoConnectNodes();
            }
        }

        Debug.Log("ResistorManager → AutoConnectNodes() เรียบร้อยทั้งหมด");
    }
}
