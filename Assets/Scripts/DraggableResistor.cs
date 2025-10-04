using UnityEngine;
using UnityEngine.Tilemaps;

public class DraggableResistor : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    public Tilemap tilemap;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] hits = Physics2D.OverlapPointAll(mouseWorld);

            foreach (var hit in hits)
            {
                if (hit.CompareTag("SelectableObject") && hit.gameObject == this.gameObject)
                {
                    isDragging = true;
                    offset = transform.position - (Vector3)mouseWorld;
                    Debug.Log("Start Drag Resistor: " + name);
                }
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPos = mouseWorld + (Vector2)offset;
            Vector3Int cellPos = tilemap.WorldToCell(targetPos);
            transform.position = tilemap.GetCellCenterWorld(cellPos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}
