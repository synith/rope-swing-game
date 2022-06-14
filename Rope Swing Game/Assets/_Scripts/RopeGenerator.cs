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
    [SerializeField] private int widthInNodes = 3;
    [SerializeField] private float distanceBetweenNodes = 2f;

    private void Awake()
    {
        nodeTemplate = transform.Find("nodeTemplate").GetComponent<Node>();
        stickTemplate = transform.Find("stickTemplate").GetComponent<Stick>();

        nodeTemplate.gameObject.SetActive(false);
        stickTemplate.gameObject.SetActive(false);

        nodes = new List<Node>();
        sticks = new List<Stick>();


        Node previousNodeX = null;
        Node previousNodeY = null;

        for (int y = 0; y < heightInNodes; y++)
        {
            

            for (int x = 0; x < widthInNodes; x++)
            {
                Vector3 pos = new Vector3(x * distanceBetweenNodes, -y * distanceBetweenNodes, 0);

                Node node = Instantiate(nodeTemplate, pos, Quaternion.identity);
                node.gameObject.SetActive(true);

                if (y == 0 && x == 0)
                {
                    node.isFixed = true;
                }
                else
                    node.isFixed = false;
                
                nodes.Add(node);

                if (x == 0)
                {
                    previousNodeX = null;
                }

                if (previousNodeX != null)
                {
                    Stick stick = Instantiate(stickTemplate);
                    stick.gameObject.SetActive(true);
                    stick.Init(previousNodeX, node);

                    sticks.Add(stick);
                }
                previousNodeX = node;
            }
        }

        for (int x = 0; x < heightInNodes; x++)
        {
            
            for (int y = 0; y < widthInNodes; y++)
            {
                int nodeIndex = heightInNodes*y + x;
                Debug.Log(nodeIndex);
                Node node = nodes[nodeIndex];

                if (y == 0)
                {
                    previousNodeY = null;
                }

                if (previousNodeY != null)
                {
                    Stick stick = Instantiate(stickTemplate);
                    stick.gameObject.SetActive(true);
                    stick.Init(previousNodeY, node);
                    sticks.Add(stick);
                }
                previousNodeY = node;

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
