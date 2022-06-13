using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeGenerator : MonoBehaviour
{
    private List<Node> nodes;
    private List<Stick> sticks;

    private Node nodeTemplate;
    private Stick stickTemplate;

    private int heightInNodes = 4;
    private int widthInNodes = 3;
    private float distanceBetweenNodes = 2f;

    private void Awake()
    {
        nodeTemplate = transform.Find("nodeTemplate").GetComponent<Node>();
        stickTemplate = transform.Find("stickTemplate").GetComponent<Stick>();

        nodeTemplate.gameObject.SetActive(false);
        stickTemplate.gameObject.SetActive(false);

        nodes = new List<Node>();
        sticks = new List<Stick>();

        Node previousNode = null;

        for (int y = 0; y < heightInNodes; y++)
        {
            for (int x = 0; x < widthInNodes; x++)
            {
                Vector3 pos = new Vector3(x * distanceBetweenNodes, y * distanceBetweenNodes, 0);

                Node node = Instantiate(nodeTemplate);
                node.gameObject.SetActive(true);
                node.transform.position = pos;

                node.isFixed = true;
                nodes.Add(node);

                if (previousNode != null)
                {
                    node.isFixed = false;
                    Stick stick = Instantiate(stickTemplate);
                    stick.gameObject.SetActive(true);
                    stick.Init(previousNode, node);

                    sticks.Add(stick);
                }
                previousNode = node;
            }
        }
    }

    private void FixedUpdate()
    {
        foreach (Node node in nodes)
        {
            node.UpdateNode();
        }

        for (int i = 0; i < 3; i++)
        {
            foreach (Stick stick in sticks)
            {
                if (stick.gameObject.activeSelf)
                    stick.UpdateStick();
            }
        }
    }
}
