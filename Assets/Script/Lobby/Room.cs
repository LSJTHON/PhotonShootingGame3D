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

        //게임 시작 후 참여 가능 여부
        PhotonNetwork.CurrentRoom.IsOpen = false; 

        //생성된 방 로비에서 보여질지 결정하는 코드
        PhotonNetwork.CurrentRoom.IsVisible = false; 

        PhotonNetwork.LoadLevel("Game");


    }
}
