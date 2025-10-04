using UnityEngine;

public class FillColumnWithHoles : MonoBehaviour
{
    [Header("HoleTile Settings")]
    public GameObject holePrefab;       // Prefab HoleTile
    public int width = 10;              // จำนวนคอลัมน์
    public int height = 10;             // จำนวนแถว
    [Range(0.1f, 1f)]
    public float holeScaleFactor = 1f;
    public float cellSize = 0.35f;

    [Header("Grid Origin in Local Space")]
    public Vector3 gridOrigin = new Vector3(-0.15f, 0.08f, 0f); // จุดซ้ายล่าง

    void Start()
    {
        Transform parentContainer = this.transform; // BreadboardRoot

        for (int x = 0; x < width; x++)
        {
            // --- สร้าง ColumnNode สำหรับคอลัมน์ ---
            GameObject colObj = new GameObject($"ColumnNode_{x}");
            Node columnNode = colObj.AddComponent<Node>();
            columnNode.transform.parent = parentContainer;
            columnNode.transform.localPosition = gridOrigin + new Vector3(x * cellSize, 0f, 0f);

            Debug.Log($"✅ สร้าง ColumnNode_{x} ที่ {columnNode.transform.localPosition}");

            for (int y = 0; y < height; y++)
            {
                // --- สร้าง HoleTile ---
                GameObject hole = Instantiate(holePrefab);
                hole.transform.parent = parentContainer;
                hole.transform.localPosition = gridOrigin + new Vector3(x * cellSize, y * cellSize, 0f);
                hole.transform.localScale = new Vector3(cellSize * holeScaleFactor, cellSize * holeScaleFactor, 1f);
                hole.name = $"Hole_{x}_{y}";

                // --- กำหนด HoleNode ---
                HoleNode hn = hole.GetComponent<HoleNode>();
                if (hn != null)
                {
                    hn.gridPosition = new Vector2Int(x, y);
                    hn.connectedNode = columnNode;

                    // --- Debug ---
                    Debug.Log($"Hole_{x}_{y} เชื่อมกับ {hn.connectedNode.name}");
                }
                else
                {
                    Debug.LogWarning($"⚠ HolePrefab {holePrefab.name} ไม่มี Script HoleNode ติดอยู่");
                }
            }
        }

        Debug.Log("✅ สร้าง ColumnNode และ HoleTile พร้อมเชื่อมแกน Y เสร็จแล้ว");
    }
}
