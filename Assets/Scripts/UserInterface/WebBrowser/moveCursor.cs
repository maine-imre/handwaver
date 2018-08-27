/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script does ___.
/// The main contributor(s) to this script is __
/// Status: ???
/// </summary>
public class moveCursor : MonoBehaviour {
    //oncollision enter for click. On collision stay for cursor tracking.
    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            //Debug.Log(contact.thisCollider.name + " hit " + contact.otherCollider.name);
            Debug.DrawRay(contact.point, contact.normal, Color.blue);
            //Debug.Log(contact.point.x.ToString() + " " + contact.point.y.ToString()+" " + contact.point.z.ToString());
            Vector3 position = contact.point - this.transform.position;
            position = Vector3.ProjectOnPlane(position, this.transform.right);
            Debug.Log(position.x.ToString() + " " + position.y.ToString() + " " + position.z.ToString());
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        this.GetComponent<Renderer>().material.color = Color.HSVToRGB(Vector3.Magnitude(collision.relativeVelocity), 1, 1);
    }

}
