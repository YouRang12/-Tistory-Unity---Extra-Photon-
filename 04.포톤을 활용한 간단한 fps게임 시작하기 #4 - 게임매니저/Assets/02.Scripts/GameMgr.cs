using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour {

    private PhotonView pv;
    public Transform[] SpawnPoint;
    public float createTime = 3.0f;

	void Start()
    {
        pv = PhotonView.Get(this);

        SpawnPoint = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        if (PhotonNetwork.connected && PhotonNetwork.isMasterClient)
        {
            if (Time.time > createTime)
            {
                MakeEnemy();
                createTime = Time.time + 3.0f;
            }
        }
    }

    void MakeEnemy()
    {
        StartCoroutine(this.CreateEnemy());
    }

    IEnumerator CreateEnemy()
    {
        int idx = Random.Range(1, SpawnPoint.Length);
        PhotonNetwork.InstantiateSceneObject("Enemy", SpawnPoint[idx].position
                                            , Quaternion.identity
                                            , 0
                                            , null);
        yield return null;
    }
}
