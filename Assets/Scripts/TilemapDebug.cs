using UnityEngine;

public class TilemapDebug : MonoBehaviour
{
    public LayerMask tilemapLayer; // เลือก Layer ของ Tilemap

    private Vector3 debugPosition;
    private bool hasTilemap = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // ตรวจสอบ Tilemap
            Collider2D hitTilemap = Physics2D.OverlapPoint(mousePos, tilemapLayer);
            if (hitTilemap != null)
            {
                hasTilemap = true;
                debugPosition = hitTilemap.transform.position;
                Debug.Log("Tilemap ถูกเลือก: " + hitTilemap.name);
            }
            else
            {
                hasTilemap = false;
                Debug.Log("ไม่พบ Tilemap ที่ตำแหน่งคลิก");
            }
        }
    }

    // วาด Gizmo บน Scene View
    private void OnDrawGizmos()
    {
        if (hasTilemap)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(debugPosition, 0.2f);
        }
    }
}
