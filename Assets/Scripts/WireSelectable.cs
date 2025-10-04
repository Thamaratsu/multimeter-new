using UnityEngine;

public class WireSelectable_Fixed : MonoBehaviour
{
    // ตัว Wire ที่ถูกเลือกตอนนี้ (ให้ Delete Button ใช้)
    public static WireSelectable_Fixed selectedWire = null;

    private void OnMouseDown()
    {
        SelectWire();
    }

    public void SelectWire()
    {
        selectedWire = this;
        Debug.Log("Wire ถูกเลือก: " + name);
    }

    public void DeleteWire()
    {
        Destroy(gameObject);
        Debug.Log("Wire ถูกลบ: " + name);

        if (selectedWire == this)
            selectedWire = null;
    }
}

