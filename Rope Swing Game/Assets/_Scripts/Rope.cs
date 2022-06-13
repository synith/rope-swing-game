using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public Node[] nodes;

    public Stick[] sticks;

    private void Awake()
    {
        nodes = FindObjectsOfType<Node>();
        sticks = FindObjectsOfType<Stick>();
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
                if(stick.gameObject.activeSelf)
                    stick.UpdateStick();
            }
        }
    }
}
