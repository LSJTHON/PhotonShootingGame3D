using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Pun.Demo.PunBasics;
using JetBrains.Annotations;
using Photon.Pun.Demo.Asteroids;

public class Login : MonoBehaviourPunCallbacks
{
    public InputField idField;
    public GameObject lobbyPanel;

    private void Awake()
    {
        idField.text = "Player " + Random.Range(1000,10000);
    }

    public void OnLoginbuttonClicked()
    {
        string playername = idField.text;

        if (!playername.Equals(""))
        {
            PhotonNetwork.LocalPlayer.NickName = playername;
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.Log("플레이어 닉네임 오류다 이말이야");
        }
    }

        public override void OnConnected()
    {
        Debug.Log("연결 성공했 다 이마리야 - " + PhotonNetwork.LocalPlayer.NickName);

        gameObject.SetActive(false);
        lobbyPanel.SetActive(true);
    }
}
