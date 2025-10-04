using UnityEngine;
using UnityEngine.UI;

public class WireDeleteButton_Fixed : MonoBehaviour
{
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        if(button == null)
        {
            Debug.LogError("❌ Delete Button ต้องมี Component Button");
            return;
        }
        button.onClick.AddListener(OnClickDelete);
    }

    private void OnClickDelete()
    {
        if(WireSelectable_Fixed.selectedWire != null)
        {
            WireSelectable_Fixed.selectedWire.DeleteWire();
        }
        else
        {
            Debug.LogWarning("❌ ไม่มี Wire ที่ถูกเลือก");
        }
    }
}
