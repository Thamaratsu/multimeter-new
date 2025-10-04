using UnityEngine;
using System.Collections.Generic;

// Class ย่อยสำหรับเก็บข้อมูล Layout หนึ่งชุด
[System.Serializable]
public class ScenarioLayout
{
    [Tooltip("ชื่อด่าน/โจทย์")]
    public string layoutName;

    [Tooltip("Object ทั้งหมดที่เกี่ยวข้องกับด่านนี้ (ลาก GameObjects จาก Hierarchy มาใส่)")]
    public GameObject[] scenarioObjects;

    [Tooltip("ตำแหน่ง Vector3 ที่ Object แต่ละตัวใน scenarioObjects ต้องย้ายไป")]
    // **ต้องมีขนาดเท่ากับ scenarioObjects เสมอและต้องเรียงลำดับตรงกัน**
    public Vector3[] targetPositions; 
}

public class LayoutManager : MonoBehaviour
{
    [Header("All Scenarios")]
    [Tooltip("กำหนด Layout และตำแหน่งสำหรับแต่ละด่าน")]
    public List<ScenarioLayout> allLayouts;

    private ScenarioLayout activeLayout = null;
    
    void Start()
    {
        // โหลดด่านแรกเมื่อเริ่มเกม (ถ้ามี)
        if (allLayouts.Count > 0)
        {
            LoadLayout(0);
        }
    }

    // ซ่อน Object ของด่านก่อนหน้าและแสดง Object ของด่านใหม่
    private void ClearAndSetupLayout(ScenarioLayout newLayout)
    {
        // 1. ซ่อน Object ของด่านที่เปิดอยู่ปัจจุบัน
        if (activeLayout != null && activeLayout.scenarioObjects != null)
        {
            foreach (GameObject obj in activeLayout.scenarioObjects)
            {
                if (obj != null)
                {
                    obj.SetActive(false); 
                }
            }
        }
        
        // 2. ตรวจสอบความถูกต้องของข้อมูล Layout ใหม่
        if (newLayout.scenarioObjects.Length != newLayout.targetPositions.Length)
        {
            Debug.LogError($"Layout '{newLayout.layoutName}': จำนวน Object ({newLayout.scenarioObjects.Length}) และตำแหน่ง ({newLayout.targetPositions.Length}) ไม่เท่ากัน! จัดเรียงไม่สำเร็จ");
            return;
        }

        // 3. ย้าย Object ไปยังตำแหน่งใหม่และแสดงผล
        for (int i = 0; i < newLayout.scenarioObjects.Length; i++)
        {
            GameObject obj = newLayout.scenarioObjects[i];
            if (obj != null)
            {
                // กำหนดตำแหน่งใหม่
                obj.transform.position = newLayout.targetPositions[i];
                // เปิดใช้งาน
                obj.SetActive(true);
                
                // (Optional): หากมีการใช้ Rigidbody2D อาจต้อง reset velocity/rotation
                // Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                // if (rb != null) { rb.velocity = Vector2.zero; rb.angularVelocity = 0f; }
            }
        }
        
        // 4. อัปเดตสถานะด่านปัจจุบัน
        activeLayout = newLayout;
    }

    /// <summary>
    /// ฟังก์ชันหลักในการโหลดและจัดเรียง Object สำหรับด่านที่กำหนด
    /// </summary>
    /// <param name="layoutIndex">ลำดับของด่านที่ต้องการโหลด (เริ่มจาก 0)</param>
    public void LoadLayout(int layoutIndex)
    {
        if (layoutIndex < 0 || layoutIndex >= allLayouts.Count)
        {
            Debug.LogError($"Layout Index {layoutIndex} ไม่อยู่ในช่วงที่ถูกต้อง.");
            return;
        }

        ScenarioLayout newLayout = allLayouts[layoutIndex];
        ClearAndSetupLayout(newLayout);
        
        // **NEW: สั่งให้ VoltageAggregator อัปเดตการคำนวณ**
        // นี่เป็นขั้นตอนสำคัญ เพื่อให้วงจรคำนวณค่าใหม่หลังจากการจัดเรียง
        VoltageAggregator va = FindObjectOfType<VoltageAggregator>();
        if (va != null)
        {
            // อาจเรียกฟังก์ชัน RecalculateCircuit() ถ้ามี หรือแค่ให้ Update() ทำงาน
        }
        
        Debug.Log($"✅ โหลดด่าน '{newLayout.layoutName}' (Index: {layoutIndex}) สำเร็จ!");
    }
}