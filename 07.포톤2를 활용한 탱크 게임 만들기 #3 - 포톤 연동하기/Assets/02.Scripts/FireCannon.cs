using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class FireCannon : MonoBehaviourPunCallbacks
{
    public Transform firePos;
    public GameObject cannon;
    public AudioClip fireSfx;

    private AudioSource _audio;

    void Start()
    {
        _audio = this.gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetMouseButtonDown(0))
        {
            photonView.RPC("Fire", RpcTarget.Others, null);
            Fire();
        }
    }
    [PunRPC]
    void Fire()
    {
        _audio.PlayOneShot(fireSfx, 0.8f);
        Instantiate(cannon, firePos.position, firePos.rotation);
    }
}
