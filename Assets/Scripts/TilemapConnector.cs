using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapConnector_TagPriority : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject circleMarkerPrefab;
    public GameObject linePrefab;

    [Header("Settings")]
    public float selectRadius = 0.2f; // รัศมีหา Object ใกล้ Mouse

    private Vector3 firstMarkerPos;
    private GameObject firstMarker;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // ตรวจ Collider รอบ Mouse
            Collider2D[] hits = Physics2D.OverlapCircleAll(mouseWorld, selectRadius);
            if (hits.Length > 0)
            {
                Collider2D nearest = GetNearestCollider(hits, mouseWorld);

                // ถ้าเป็น Resistor → ปล่อยให้ DraggableResistor จัดการเอง
                if (nearest.CompareTag("SelectableObject"))
                {
                    Debug.Log("Click Resistor → ปล่อยให้ DraggableResistor จัดการ");
                    return;
                }
                // ถ้าเป็น Tilemap → ทำการวาง Marker / ต่อสาย
                else if (nearest.CompareTag("SelectableTilemap") && nearest.GetComponent<Tilemap>() != null)
                {
                    Tilemap tilemap = nearest.GetComponent<Tilemap>();
                    Vector3Int cellPos = tilemap.WorldToCell(mouseWorld);
                    Vector3 gridWorldPos = tilemap.GetCellCenterWorld(cellPos);
                    SelectPosition(gridWorldPos);
                    return;
                }
            }

            Debug.Log("ไม่พบ Object หรือ Tilemap ใกล้ Mouse");
        }
    }

    Collider2D GetNearestCollider(Collider2D[] colliders, Vector2 mousePos)
    {
        Collider2D nearest = colliders[0];
        float minDist = Vector2.Distance(mousePos, nearest.transform.position);
        foreach (Collider2D c in colliders)
        {
            float dist = Vector2.Distance(mousePos, c.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = c;
            }
        }
        return nearest;
    }

    void SelectPosition(Vector3 pos)
    {
        if (firstMarkerPos == Vector3.zero)
        {
            firstMarkerPos = pos;
            if (circleMarkerPrefab != null)
                firstMarker = Instantiate(circleMarkerPrefab, pos, Quaternion.identity);
        }
        else
        {
            // สร้างสายเชื่อม
            GameObject lineObj = Instantiate(linePrefab);
            LineRenderer line = lineObj.GetComponent<LineRenderer>();
            if (line == null)
            {
                line = lineObj.AddComponent<LineRenderer>();
                line.startWidth = 0.05f;
                line.endWidth = 0.05f;
                line.material = new Material(Shader.Find("Sprites/Default"));
            }
            line.positionCount = 2;
            line.SetPosition(0, firstMarkerPos);
            line.SetPosition(1, pos);

            if (firstMarker != null) Destroy(firstMarker);
            firstMarkerPos = Vector3.zero;
        }
    }
}
