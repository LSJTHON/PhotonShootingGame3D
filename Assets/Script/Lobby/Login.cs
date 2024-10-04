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
            Debug.Log("�÷��̾� �г��� ������ �̸��̾�");
        }
    }

        public override void OnConnected()
    {
        Debug.Log("���� ������ �� �̸����� - " + PhotonNetwork.LocalPlayer.NickName);

        gameObject.SetActive(false);
        lobbyPanel.SetActive(true);
    }
}
