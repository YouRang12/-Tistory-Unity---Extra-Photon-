using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCannon : MonoBehaviour {

    public Transform firePos;
    public GameObject cannon;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(cannon, firePos.position, firePos.rotation);
        }
    }
}
