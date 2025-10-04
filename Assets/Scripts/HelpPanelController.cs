using UnityEngine;
using UnityEngine.Video; // สำคัญ: ต้องใช้สำหรับ VideoPlayer

public class HelpPanelController : MonoBehaviour
{
    // ลาก HowToPlay_Panel มาใส่
    public GameObject howToPlayPanel; 

    // ลาก HowToPlay_VideoPlayer มาใส่
    public VideoPlayer videoPlayer; 

    // ฟังก์ชันสำหรับเปิด Panel
    public void OpenPanel()
    {
        howToPlayPanel.SetActive(true); // เปิด Panel
        videoPlayer.Play();            // เริ่มเล่นวิดีโอ
    }

    // ฟังก์ชันสำหรับปิด Panel
    public void ClosePanel()
    {
        howToPlayPanel.SetActive(false); // ปิด Panel
        videoPlayer.Stop();             // หยุดเล่นวิดีโอ
        // videoPlayer.frame = 0;        // (ทางเลือก) รีเฟรมวิดีโอไปเริ่มต้น
    }
}