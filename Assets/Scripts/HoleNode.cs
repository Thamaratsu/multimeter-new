using UnityEngine;

public class HoleNode : MonoBehaviour
{
    public Vector2Int gridPosition; // ตำแหน่งบน Tilemap
    public Node connectedNode;      // Node ของวงจร

    private void Awake()
    {
        if (connectedNode == null)
            connectedNode = new Node(); // สร้าง Node ถ้ายังไม่มี
    }
}
