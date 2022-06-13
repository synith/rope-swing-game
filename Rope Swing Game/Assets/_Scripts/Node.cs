using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool isFixed;
    public static float gravity = -9.8f;
    public Vector3 previousPosition;


    private void Start()
    {
        previousPosition = transform.position;
    }

    public void UpdateNode()
    {
        if (isFixed) return;

        Vector3 velocity = transform.position - previousPosition;
        previousPosition = transform.position;

        velocity += Vector3.up * (gravity * Time.deltaTime * Time.deltaTime);

        Move(velocity);
    }

    private void Move(Vector3 moveVector)
    {
        transform.Translate(moveVector);
    }
}
