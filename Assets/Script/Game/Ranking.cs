using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    public Text scoreText;
    public Text nickNameText;
    public Text timestampText;

    private int score;
    private string nickname;
    private string timestamp;
    public int Score
    {
        get { return score; }
        set { score = value; scoreText.text = value.ToString(); }
    }

    public string Nickname
    {
        get { return nickname; }
        set { nickname = value; nickNameText.text = value; }
    }

    public string Timestamp
    {
        get { return timestamp; }
        set { timestamp = value; timestampText.text = value; }
    }

    public void Reset()
    {
        Score = 0;
        Nickname = string.Empty;
        timestamp = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
    }
}
