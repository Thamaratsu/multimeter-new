using UnityEngine;
using TMPro; // สำคัญ: ต้องใช้ TextMeshPro
using System;

public class QuizController : MonoBehaviour
{
    // อ้างอิง UI: สำหรับรับค่าจากผู้ใช้ 3 ช่อง
    public TMP_InputField inputVR1;
    public TMP_InputField inputVR2;
    public TMP_InputField inputVR3;

    // อ้างอิง UI: สำหรับแสดงผลลัพธ์
    public TextMeshProUGUI resultText;

    // คำตอบที่ถูกต้องเป็นช่วง (กำหนดจาก Inspector)
    [Header("Correct Answer Ranges")]
    public float minVR1 = 3.9f; 
    public float maxVR1 = 4.1f; 

    public float minVR2 = 4.4f; 
    public float maxVR2 = 4.6f; 

    public float minVR3 = 4.9f; 
    public float maxVR3 = 5.1f; 
    
    // **NOTE:** ไม่จำเป็นต้องใช้ 'tolerance' ในการเปรียบเทียบแล้ว เพราะเราใช้ช่วง Min/Max แทน

    /// <summary>
    /// ฟังก์ชันหลักสำหรับตรวจสอบคำตอบทั้งหมด
    /// </summary>
    public void CheckAnswer()
    {
        // 1. ตรวจสอบว่ามีการป้อนข้อมูลครบทั้ง 3 ช่องหรือไม่
        if (string.IsNullOrEmpty(inputVR1.text) || string.IsNullOrEmpty(inputVR2.text) || string.IsNullOrEmpty(inputVR3.text))
        {
            resultText.color = Color.yellow;
            resultText.text = "Enter the answer..";
            return; // หยุดทำงานถ้าป้อนไม่ครบ
        }

        // 2. แปลงค่า Input เป็นตัวเลข (float)
        bool isVR1Parsed = float.TryParse(inputVR1.text, out float userVR1);
        bool isVR2Parsed = float.TryParse(inputVR2.text, out float userVR2);
        bool isVR3Parsed = float.TryParse(inputVR3.text, out float userVR3);

        // 3. ตรวจสอบความถูกต้องของคำตอบตามลำดับ **VR1 -> VR2 -> VR3**

        // --- ตรวจสอบ VR1 ---
        if (!isVR1Parsed) // ไม่ใช่ตัวเลข
        {
            resultText.color = Color.yellow;
            resultText.text = "VR1: Please enter only number.";
            return;
        }
        // ตรวจสอบว่าคำตอบ VR1 อยู่ในช่วง [minVR1, maxVR1] หรือไม่
        if (userVR1 < minVR1 || userVR1 > maxVR1) 
        {
            resultText.color = Color.red;
            resultText.text = "VR1 Wrong";
            return;
        }

        // --- ตรวจสอบ VR2 ---
        if (!isVR2Parsed) // ไม่ใช่ตัวเลข
        {
            resultText.color = Color.yellow;
            resultText.text = "VR2: Please enter only number.";
            return;
        }
        // ตรวจสอบว่าคำตอบ VR2 อยู่ในช่วง [minVR2, maxVR2] หรือไม่
        if (userVR2 < minVR2 || userVR2 > maxVR2)
        {
            resultText.color = Color.red;
            resultText.text = "VR2 Wrong";
            return;
        }

        // --- ตรวจสอบ VR3 ---
        if (!isVR3Parsed) // ไม่ใช่ตัวเลข
        {
            resultText.color = Color.yellow;
            resultText.text = "VR3: Please enter only number.";
            return;
        }
        // ตรวจสอบว่าคำตอบ VR3 อยู่ในช่วง [minVR3, maxVR3] หรือไม่
        if (userVR3 < minVR3 || userVR3 > maxVR3)
        {
            resultText.color = Color.red;
            resultText.text = "VR3 Wrong";
            return;
        }
        
        // 4. ถ้าผ่านการตรวจสอบทั้งหมด = ถูกต้อง!
        resultText.color = Color.green;
        resultText.text = "Correct!! 🎉";
    }

    /// <summary>
    /// ฟังก์ชันสำหรับเปลี่ยนคำถามและช่วงคำตอบที่ถูกต้อง
    /// </summary>
    public void SetQuestion(float minV1, float maxV1, float minV2, float maxV2, float minV3, float maxV3)
    {
        minVR1 = minV1;
        maxVR1 = maxV1;
        minVR2 = minV2;
        maxVR2 = maxV2;
        minVR3 = minV3;
        maxVR3 = maxV3;
        
        // ล้างค่าและผลลัพธ์
        inputVR1.text = "";
        inputVR2.text = "";
        inputVR3.text = "";
        resultText.text = "";
    }
}