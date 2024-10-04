using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    private class RankingInfo
    {
        public int score;
        public string nickname;
        public string timestamp;
    }



    private List<RankingInfo> rankingInfoList = new List<RankingInfo>();
    public List<Ranking> rankingList;

    private void Awake()
    {
        LoadFromFile();
        RefreshUI();
    }

    private void RefreshUI()
    {
        foreach (var r in rankingList)
            r.Reset();

        for(int i = 0; i < rankingInfoList.Count; i++)
        {
            rankingList[i].Score = rankingInfoList[i].score;
            rankingList[i].Nickname= rankingInfoList[i].nickname;
            rankingList[i].Timestamp = rankingInfoList[i].timestamp ;
        }
    }

    public void AddScore(int score)
    {
        RankingInfo ranking = new RankingInfo()
        {
            score = score,
            nickname = PhotonNetwork.LocalPlayer.NickName,
            timestamp = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")
        };
        rankingInfoList.Add(ranking);
        rankingInfoList.Sort((a, b) => b.score.CompareTo(a.score));

        if(rankingInfoList.Count > rankingList.Count)
            rankingInfoList.RemoveAt(rankingInfoList.Count - 1);

        SaveToFile();
        RefreshUI();
    }

    private void SaveToFile()
    {
        for (int i = 0; i < rankingInfoList.Count && i < rankingList.Count; i++)
        {
            PlayerPrefs.SetInt("SCORE_" + i, rankingInfoList[i].score);
            PlayerPrefs.SetString("NICKNAME_" + i, rankingInfoList[i].nickname);
            PlayerPrefs.SetString("TIMESTAMP_" + i, rankingInfoList[i].timestamp);
        }
    }

    private void LoadFromFile()
    {
        rankingInfoList.Clear();

        for(int i = 0; i < rankingList.Count; i++)
        {
            var rankingInfo = new RankingInfo()
            {
                score = PlayerPrefs.GetInt("SCORE_" + i, 0),
                nickname = PlayerPrefs.GetString("NICKNAME_" + i, ""),
                timestamp = PlayerPrefs.GetString("TIMESTAMP_"+i,"")
            };
            rankingInfoList.Add(rankingInfo);
            
        }
    }
}
