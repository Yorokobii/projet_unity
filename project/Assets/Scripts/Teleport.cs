using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    public Transform destination;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")
        {
            other.transform.position = destination.position;
        }
    }
}
