using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderBehaviour : MonoBehaviour
{
    Rigidbody rb;
    public int speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Walk(speed);
    }

    void Walk(int speed)
    {
        rb.AddForce(Vector3.forward * speed);
    }
}
