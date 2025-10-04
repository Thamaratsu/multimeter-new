using UnityEngine;
using TMPro; // สำคัญ: ต้องใช้ TextMeshPro
using System;

// คลาสสำหรับควบคุมควิซที่ตรวจสอบเพียงค่าเดียว (Current)
public class QuizControllerCurrent : MonoBehaviour 
{
    // อ้างอิง UI: สำหรับรับค่าจากผู้ใช้ *เหลือเพียง 1 ช่อง*
    public TMP_InputField inputVR1; 
    
    // อ้างอิง UI: สำหรับแสดงผลลัพธ์
    public TextMeshProUGUI resultText;

    // คำตอบที่ถูกต้องเป็นช่วง (กำหนดจาก Inspector)
    [Header("Correct Answer Range (VR1 Only)")]
    public float minVR1 = 3.9f; 
    public float maxVR1 = 4.1f; // **แก้ไขแล้ว** (ลบ 'public' ซ้ำซ้อนออก)
    
    // **NOTE:** ตัวแปรสำหรับ VR2 และ VR3 ถูกลบออกแล้ว

    /// <summary>
    /// ฟังก์ชันหลักสำหรับตรวจสอบคำตอบ VR1 เพียงตัวเดียว
    /// จะถูกเรียกใช้เมื่อกดปุ่ม Submit
    /// </summary>
    public void CheckAnswer()
    {
        string inputText = inputVR1.text.Trim();

        // 1. ตรวจสอบว่ามีการป้อนข้อมูลหรือไม่
        if (string.IsNullOrEmpty(inputText))
        {
            resultText.color = Color.yellow;
            resultText.text = "Enter the answer..";
            return; 
        }

        // 2. แปลงค่า Input เป็นตัวเลข (float)
        bool isVR1Parsed = float.TryParse(inputText, out float userVR1);

        // 3. ตรวจสอบความถูกต้อง

        // --- ตรวจสอบ VR1: ว่าเป็นตัวเลขหรือไม่ ---
        if (!isVR1Parsed) 
        {
            resultText.color = Color.yellow;
            resultText.text = "Please enter only number.";
            return;
        }
        
        // --- ตรวจสอบ VR1: ว่าอยู่ในช่วงที่กำหนดหรือไม่ (min <= answer <= max) ---
        if (userVR1 >= minVR1 && userVR1 <= maxVR1) 
        {
            // ถูกต้อง
            resultText.color = Color.green;
            resultText.text = "Correct!!";
        }
        else
        {
            // ผิด
            resultText.color = Color.red;
            // แสดงช่วงคำตอบที่ถูกต้องให้ผู้ใช้ทราบ
            resultText.text = $"Wrong (Correct Range: {minVR1:F3} to {maxVR1:F3})";
        }
    }

    /// <summary>
    /// ฟังก์ชันสำหรับเปลี่ยนคำถามและช่วงคำตอบที่ถูกต้อง (ถ้ามีการเปลี่ยนคำถามในเกม)
    /// </summary>
    public void SetQuestion(float minV1, float maxV1)
    {
        minVR1 = minV1;
        maxVR1 = maxV1;
        
        // ล้างค่าและผลลัพธ์
        inputVR1.text = "";
        resultText.text = "";
    }
}