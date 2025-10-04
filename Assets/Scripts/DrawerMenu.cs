using UnityEngine;
using UnityEngine.UI;

public class DrawerMenu : MonoBehaviour
{
    [Header("Panel ที่จะเปิด/ปิด")]
    public GameObject panel;          

    // ไม่ต้องใช้ menuButton และ logic interactable แล้ว
    // [Header("ปุ่ม Menu ที่จะควบคุม Interactable")] 
    // public Button menuButton; 
    
    // private bool isOpen = false; // ไม่จำเป็นต้องเก็บสถานะแล้ว

    void Start()
    {
        if(panel == null)
        {
            Debug.LogError("DrawerMenu: Panel ยังไม่ได้ assign ใน Inspector!");
            return;
        }
        panel.SetActive(false);
    }

    // ฟังก์ชันนี้จะผูกกับปุ่ม Menu หลัก (ใช้สำหรับ "เปิด" เท่านั้น)
    public void OpenDrawer()
    {
        if(panel == null) return;
        
        panel.SetActive(true);
        // Debug.Log("Panel OPENED by Main Menu Button.");
    }
    
    // ฟังก์ชันนี้จะผูกกับปุ่ม IR1, IR2, IR3 (ใช้สำหรับ "ปิด")
    public void CloseDrawer()
    {
        if(panel == null) return;
        
        panel.SetActive(false);
        // Debug.Log("Panel CLOSED by Option Button.");
    }
    
    // ตัวอย่างฟังก์ชันสำหรับปุ่ม IR1
    public void SelectIR1()
    {
        Debug.Log("Option IR1 selected. Closing menu.");
        // **เพิ่มโค้ดที่ต้องการให้เกิดขึ้นเมื่อเลือก IR1 ที่นี่**
        
        // สั่งปิดเมนู
        CloseDrawer();
    }
    
    // **คุณสามารถทำซ้ำฟังก์ชัน SelectIR2() และ SelectIR3() ได้**
}