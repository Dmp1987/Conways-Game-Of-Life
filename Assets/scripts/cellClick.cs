using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cellClick : MonoBehaviour
{
    void OnMouseDown() 
    {
        if (this.tag == "dead")
        {
            this.tag = "alive";
            this.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            this.tag = "dead";
            this.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
}
