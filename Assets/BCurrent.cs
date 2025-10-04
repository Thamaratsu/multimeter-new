using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Bcurrent : MonoBehaviour
{
    [Header("Buttons")]
    public Button buttonResistor;
    public Button buttonVoltage;
    public Button buttonCurrent;

    public Button buttonVoltageDD;

    public Button buttonVoltageVR;

    public Button buttonfirtpage; // ตัวแปรนี้ควรถูกลากปุ่ม UI มาใส่ใน Inspector

    // *** วิธีที่ 1: ใช้โค้ด ***
    void Start()
    {
        // โค้ดนี้จะทำการเชื่อมโยงปุ่มกับฟังก์ชันเปลี่ยน Scene โดยอัตโนมัติเมื่อเกมเริ่ม
        
        // ตรวจสอบค่า Null ก่อน AddListener ทุกครั้ง เพื่อป้องกัน NullReferenceException
        if (buttonResistor != null)
        {
            buttonResistor.onClick.AddListener(() => LoadScene("IR1"));
        }
        
        if (buttonVoltage != null)
        {
            buttonVoltage.onClick.AddListener(() => LoadScene("IR2"));
        }
        
        if (buttonCurrent != null)
        {
            buttonCurrent.onClick.AddListener(() => LoadScene("IR3"));
        }

        // ตรวจสอบปุ่มอื่นๆ ที่มีปัญหา NullReference บ่อย
        if (buttonVoltageVR != null)
        {
            buttonVoltageVR.onClick.AddListener(() => LoadScene("Voltagetotal"));
        }
        
        if (buttonVoltageDD != null)
        {
            buttonVoltageDD.onClick.AddListener(() => LoadScene("VoltageScene"));
        }
        
        // ปุ่ม FirstPage (เดิมมีปัญหา Null)
        if (buttonfirtpage != null)
        {
            buttonfirtpage.onClick.AddListener(() => LoadScene("FirstPage"));
        }
    }
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}