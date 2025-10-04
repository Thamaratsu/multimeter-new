using UnityEngine;

public class Battery : MonoBehaviour
{
    public string label = "Battery";
    public Node positiveNode;
    public Node negativeNode;

    public float Voltage
    {
        get
        {
            if (positiveNode == null || negativeNode == null) return 0f;
            return positiveNode.voltage - negativeNode.voltage;
        }
    }

    void Reset()
    {
        // สร้าง Collider2D ให้ Node ถ้ายังไม่มี
        if (positiveNode != null && positiveNode.GetComponent<Collider2D>() == null)
            positiveNode.gameObject.AddComponent<CircleCollider2D>().isTrigger = true;

        if (negativeNode != null && negativeNode.GetComponent<Collider2D>() == null)
            negativeNode.gameObject.AddComponent<CircleCollider2D>().isTrigger = true;
    }
}


