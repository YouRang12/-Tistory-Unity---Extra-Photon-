using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TurretCtrl : MonoBehaviourPunCallbacks
{

    private Transform tr;
    public float rotSpeed = 10.0f;

    private RaycastHit hit;

    void Start()
    {
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 50.0f, Color.green);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1<<9))
        {
            Vector3 localPos = tr.InverseTransformPoint(hit.point);
            float angle = Mathf.Atan2(localPos.x, localPos.z) * Mathf.Rad2Deg;
            tr.Rotate(0, angle, 0);
        }
    }
}
