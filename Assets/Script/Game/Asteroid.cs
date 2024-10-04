using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class Asteroid : MonoBehaviourPun
{
    private bool isDestroyed;

    private new Rigidbody rigidbody;
    public void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();

        if (photonView.InstantiationData != null)
        {
            rigidbody.AddForce((Vector3)photonView.InstantiationData[0]);
            rigidbody.AddTorque((Vector3)photonView.InstantiationData[1]);
        }
    }

    public void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (Mathf.Abs(transform.position.x) > Camera.main.orthographicSize * Camera.main.aspect ||
            Mathf.Abs(transform.position.z) > Camera.main.orthographicSize)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (isDestroyed)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Bullet")){
            if (photonView.IsMine)
            {
                Bullet bullet = collision.gameObject.GetComponent<Bullet>();
                bullet.Owner.AddScore(10);
                DestroyAsteroidGlobally();
            }
            else
            {
               DestroyAsteroidGlobally();
            }
        }else if (collision.gameObject.CompareTag("Player"))
        {
            if (photonView.IsMine)
            {
                collision.gameObject.GetComponent<PhotonView>().RPC("DestroySpaceship", RpcTarget.All);

                DestroyAsteroidLocally();
            }
        }
    }

    private void DestroyAsteroidGlobally()
    {
        isDestroyed = true;

        PhotonNetwork.Destroy(gameObject);
    }
    private void DestroyAsteroidLocally()
    {
        isDestroyed = true;

        GetComponent<Renderer>().enabled = false;
    }
}

