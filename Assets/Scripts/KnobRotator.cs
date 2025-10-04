using UnityEngine;
using UnityEngine.EventSystems;

public class KnobSnapRotator : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Knob Settings")]
    public int numberOfPositions = 20;   // จำนวนย่าน
    public float offsetAngle = -90f;     // ปรับให้ 0° อยู่บน 12 นาฬิกา

    private float stepAngle;
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        stepAngle = 360f / numberOfPositions;  // 360 / 20 = 18°
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateKnobRotation(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateKnobRotation(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Snap ทำทันทีใน UpdateKnobRotation
    }

    private void UpdateKnobRotation(PointerEventData eventData)
    {
        // แปลงตำแหน่งเมาส์เป็น World Space
        Vector3 worldPos = mainCam.ScreenToWorldPoint(eventData.position);
        worldPos.z = transform.position.z;

        Vector2 dir = worldPos - transform.position;

        // หา angle target และ normalize 0-360
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        targetAngle = (targetAngle + 360f) % 360f;

        // Snap ไปที่ย่านใกล้ที่สุด (ทีละ 18°)
        int nearestStep = Mathf.RoundToInt(targetAngle / stepAngle) % numberOfPositions;
        float snappedAngle = nearestStep * stepAngle;

        // ตั้ง rotation knob
        transform.rotation = Quaternion.Euler(0f, 0f, snappedAngle + offsetAngle);
    }

    public int GetSelectedPosition()
    {
        float zAngle = (transform.eulerAngles.z - offsetAngle + 360f) % 360f;
        int position = Mathf.RoundToInt(zAngle / stepAngle) % numberOfPositions;
        return position;
    }
}
