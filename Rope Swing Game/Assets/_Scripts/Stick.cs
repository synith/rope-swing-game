using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    private float length;

    private LineRenderer lineRenderer;

    private Node node1;
    private Node node2;

    public void Init(Node node1, Node node2)
    {
        this.node1 = node1;
        this.node2 = node2;
    }
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
    }
    private void Start()
    {
        length = Vector3.Distance(node1.transform.position, node2.transform.position);
        UpdateLineRenderer();
    }
    private void UpdateLineRenderer()
    {
        lineRenderer.SetPositions(new[]
        {
            node1.transform.position,
            node2.transform.position
        });
    }

    public void UpdateStick()
    {
        Vector3 center = (node1.transform.position + node2.transform.position) / 2;

        if (!node1.isFixed)
        {
            Vector3 node1DirTowardCenter = (node1.transform.position - center).normalized;
            node1.transform.position = node1DirTowardCenter * length / 2 + center;
        }

        if (!node2.isFixed)
        {
            Vector3 node2DirTowardCenter = (node2.transform.position - center).normalized;
            node2.transform.position = node2DirTowardCenter * length / 2 + center;
        }

        UpdateLineRenderer();
    }
}
