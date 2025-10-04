using UnityEngine;

public class wireA : MonoBehaviour
{
    public Node1 startNode;
    public Node1 endNode;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        if (startNode != null && endNode != null)
        {
            lineRenderer.SetPosition(0, startNode.transform.position);
            lineRenderer.SetPosition(1, endNode.transform.position);
        }
    }
}
