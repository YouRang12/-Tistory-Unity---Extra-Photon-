using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    public float force = 2000.0f;
    public GameObject expEffect;
    public int actorNumber = -1;

    void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * force);
        Destroy(this.gameObject, 10.0f);
    }
    void OnCollisionEnter(Collision coll)
    {
        GameObject obj = Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        Destroy(obj, 2.0f);
    }
}
