using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCtrl : MonoBehaviour {

    private Transform tr;
    public float rotSpeed = 10.0f;

    private RaycastHit hit;

    void Start()
    {
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 50.0f, Color.green);
    }
}
