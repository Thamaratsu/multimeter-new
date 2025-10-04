using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    [Header("Electrical Settings")]
    public float voltage = 0f;

    [HideInInspector] public List<GameObject> attachedObjects = new List<GameObject>();
    [HideInInspector] public List<Node> connectedNodes = new List<Node>();

    void Awake()
    {
        if (GetComponent<Collider2D>() == null)
        {
            CircleCollider2D col = gameObject.AddComponent<CircleCollider2D>();
            col.isTrigger = false;
            col.radius = 0.15f;
        }
    }

    // เชื่อม Node ถ้าอยู่ใกล้กันในแกน Y (ใช้ใน Resistor.cs)
    public void ConnectIfSameColumn(Node otherNode, float thresholdY)
    {
        if (otherNode == null || otherNode == this) return;
        float dy = Mathf.Abs(otherNode.transform.position.y - this.transform.position.y);
        if (dy <= thresholdY)
        {
            if (!connectedNodes.Contains(otherNode))
            {
                connectedNodes.Add(otherNode);
                otherNode.connectedNodes.Add(this);
                Debug.Log($"🟡 Node {name} เชื่อมกับ Node {otherNode.name} (Y diff: {dy})");
            }
        }
    }

    // เชื่อม Node ด้วยสาย
    public void ConnectNode(Node otherNode)
    {
        if (otherNode == null || connectedNodes.Contains(otherNode)) return;
        connectedNodes.Add(otherNode);
        otherNode.connectedNodes.Add(this);
        Debug.Log($"🟢 Node {name} เชื่อมกับ Node {otherNode.name} ด้วยสาย");
    }
}
