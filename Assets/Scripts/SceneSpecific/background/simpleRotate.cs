using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleRotate : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.one, 0.5f, Space.World);
        transform.Rotate(Vector3.right, -0.5f, Space.World);
    }
}
