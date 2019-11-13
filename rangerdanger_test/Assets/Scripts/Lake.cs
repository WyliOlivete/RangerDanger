using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lake : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Hill")
        {
            Vector3 posDir = (other.transform.position - transform.position).normalized * (transform.localScale.magnitude * 2f);
            posDir.y = other.transform.position.y;
            other.transform.position = posDir;
        }
    }
}
