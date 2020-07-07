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
            int actorNumver = photonView.Owner.ActorNumber;
            photonView.RPC("Fire", RpcTarget.Others, actorNumver);
            Fire(actorNumver);
        }
    }
    [PunRPC]  //RPC 이벤트 발생시 정보를 보내는
    void Fire(int number)
    {
        _audio.PlayOneShot(fireSfx, 0.8f);
        GameObject obj = Instantiate(cannon, firePos.position, firePos.rotation);
        obj.GetComponent<Cannon>().actorNumber = number;
    }
}
