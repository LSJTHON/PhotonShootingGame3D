using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Room : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void OnStartButtonClicked()
    {

        //���� ���� �� ���� ���� ����
        PhotonNetwork.CurrentRoom.IsOpen = false; 

        //������ �� �κ񿡼� �������� �����ϴ� �ڵ�
        PhotonNetwork.CurrentRoom.IsVisible = false; 

        PhotonNetwork.LoadLevel("Game");


    }
}
