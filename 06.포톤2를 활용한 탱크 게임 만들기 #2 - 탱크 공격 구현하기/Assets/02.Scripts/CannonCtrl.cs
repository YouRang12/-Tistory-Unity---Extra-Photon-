using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCtrl : MonoBehaviour {

    private Transform tr;
    public float rotSpeed = 300.0f;

    void Start()
    {
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        tr.Rotate(Vector3.right * Input.GetAxis("Mouse ScrollWheel")
                                * rotSpeed * Time.deltaTime);
    }
}
