using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carwheel : MonoBehaviour
{

    public GameObject originalWheel;

    void Update()
    {
        transform.Rotate (50*Time.deltaTime,0,0); //rotates 50 degrees per second around z axis
    }
}
