using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeGenerator : MonoBehaviour
{
    private List<Node> nodes;
    private List<Stick> sticks;

    private Node nodeTemplate;
    private Stick stickTemplate;

    [SerializeField] private int heightInNodes = 4;
    [SerializeField] private float distanceBetweenNodes = 2f;

    private void Awake()
    {
        nodeTemplate = transform.Find("nodeTemplate").GetComponent<Node>();
        stickTemplate = transform.Find("stickTemplate").GetComponent<Stick>();

        nodeTemplate.gameObject.SetActive(false);
        stickTemplate.gameObject.SetActive(false);

        nodes = new List<Node>();
        sticks = new List<Stick>();

        Node previousNode = null;



        for (int y = heightInNodes; y > 0; y--)
        {
            Vector3 pos = new Vector3(transform.position.x, y * distanceBetweenNodes, transform.position.z);

            Node node = Instantiate(nodeTemplate, transform, false);
            node.transform.position = pos;
            node.gameObject.SetActive(true);

            if (y == heightInNodes)
            {
                node.isFixed = true;
            }
            else
                node.isFixed = false;

            nodes.Add(node);

            if (y == heightInNodes)
            {
                previousNode = null;
            }

            if (previousNode != null)
            {
                Stick stick = Instantiate(stickTemplate, gameObject.transform, false);
                stick.gameObject.SetActive(true);
                stick.Init(previousNode, node);

                sticks.Add(stick);
            }
            previousNode = node;
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
