using UnityEngine;
using TMPro; 
using System;

// เปลี่ยนชื่อคลาสเป็น QuizControllerResistor เพื่อควบคุมคำตอบ 4 ตัว
public class QuizControllerResistor : MonoBehaviour 
{
    // อ้างอิง UI: สำหรับรับค่าจากผู้ใช้ 4 ช่อง
    public TMP_InputField inputR1;
    public TMP_InputField inputR2;
    public TMP_InputField inputR3;
    public TMP_InputField inputR4; // ช่องที่ 4

    // อ้างอิง UI: สำหรับแสดงผลลัพธ์
    public TextMeshProUGUI resultText;

    // คำตอบที่ถูกต้องเป็นช่วง (กำหนดจาก Inspector)
    [Header("Correct Answer Ranges (R1 to R4)")]
    public float minR1 = 3.9f; 
    public float maxR1 = 4.1f; 

    public float minR2 = 4.4f; 
    public float maxR2 = 4.6f; 

    public float minR3 = 4.9f; 
    public float maxR3 = 5.1f; 
    
    public float minR4 = 5.4f; // ช่วงคำตอบที่ 4
    public float maxR4 = 5.6f; 

    /// <summary>
    /// ฟังก์ชันหลักสำหรับตรวจสอบคำตอบทั้งหมด (R1, R2, R3, R4)
    /// </summary>
    public void CheckAnswer()
    {
        // 1. ตรวจสอบว่ามีการป้อนข้อมูลครบทั้ง 4 ช่องหรือไม่
        if (string.IsNullOrEmpty(inputR1.text) || string.IsNullOrEmpty(inputR2.text) || 
            string.IsNullOrEmpty(inputR3.text) || string.IsNullOrEmpty(inputR4.text))
        {
            resultText.color = Color.yellow;
            resultText.text = "Enter the answers.. (R1-R4)";
            return; 
        }

        // 2. แปลงค่า Input เป็นตัวเลข (float)
        bool isR1Parsed = float.TryParse(inputR1.text, out float userR1);
        bool isR2Parsed = float.TryParse(inputR2.text, out float userR2);
        bool isR3Parsed = float.TryParse(inputR3.text, out float userR3);
        bool isR4Parsed = float.TryParse(inputR4.text, out float userR4);

        // 3. ตรวจสอบความถูกต้องของคำตอบตามลำดับ (R1 -> R2 -> R3 -> R4)

        // --- ตรวจสอบ R1 ---
        if (!isR1Parsed) 
        {
            resultText.color = Color.yellow;
            resultText.text = "R1: Please enter only number.";
            return;
        }
        if (userR1 < minR1 || userR1 > maxR1) 
        {
            resultText.color = Color.red;
            resultText.text = $"R1 Wrong";
            return;
        }

        // --- ตรวจสอบ R2 ---
        if (!isR2Parsed) 
        {
            resultText.color = Color.yellow;
            resultText.text = "R2: Please enter only number.";
            return;
        }
        if (userR2 < minR2 || userR2 > maxR2) 
        {
            resultText.color = Color.red;
            resultText.text = $"R2 Wrong";
            return;
        }

        // --- ตรวจสอบ R3 ---
        if (!isR3Parsed) 
        {
            resultText.color = Color.yellow;
            resultText.text = "R3: Please enter only number.";
            return;
        }
        if (userR3 < minR3 || userR3 > maxR3) 
        {
            resultText.color = Color.red;
            resultText.text = $"R3 Wrong";
            return;
        }
        
        // --- ตรวจสอบ R4 ---
        if (!isR4Parsed) 
        {
            resultText.color = Color.yellow;
            resultText.text = "Rtotal: Please enter only number.";
            return;
        }
        if (userR4 < minR4 || userR4 > maxR4) 
        {
            resultText.color = Color.red;
            resultText.text = $"Rtotal Wrong";
            return;
        }
        
        // 4. ถ้าผ่านการตรวจสอบทั้งหมด = ถูกต้อง!
        resultText.color = Color.green;
        resultText.text = "Correct!!";
    }

    /// <summary>
    /// ฟังก์ชันสำหรับเปลี่ยนคำถามและช่วงคำตอบที่ถูกต้องสำหรับ R1, R2, R3, R4
    /// </summary>
    public void SetQuestion(float min1, float max1, float min2, float max2, float min3, float max3, float min4, float max4)
    {
        minR1 = min1;
        maxR1 = max1;
        minR2 = min2;
        maxR2 = max2;
        minR3 = min3;
        maxR3 = max3;
        minR4 = min4;
        maxR4 = max4;
        
        // ล้างค่าและผลลัพธ์
        inputR1.text = "";
        inputR2.text = "";
        inputR3.text = "";
        inputR4.text = "";
        resultText.text = "";
    }
}