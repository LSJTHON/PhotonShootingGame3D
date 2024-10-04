using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviourPunCallbacks
{
    public string spaceshipPrefabName = "LegendSeongJongHo";

    public string eggName = "LegendEgg";

    public Vector3 eggSpawnTime = new Vector2(3f, 6f);

    public Text nickNameText;
    public Text scoreText;

    public LeaderBoard leaderboard;

    private IEnumerator Start()
    {
        nickNameText.text = PhotonNetwork.LocalPlayer.NickName;
        scoreText.text = "Score : 0";

        leaderboard.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);
        StartGame();
    }

    private void StartGame()
    {
        float angularStart = Random.Range(0f, 360f);
        float x = Mathf.Sin(angularStart * Mathf.Deg2Rad);
        float z = Mathf.Cos(angularStart * Mathf.Deg2Rad);
        float range = Random.Range(5f, 25f);
        Vector3 position = new Vector3(x, 0.0f, z) * range;
        Quaternion rotation = Quaternion.Euler(0.0f, angularStart, 0.0f);

        PhotonNetwork.Instantiate(spaceshipPrefabName, position, rotation);

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnEgg());
        }
    }

    private IEnumerator SpawnEgg()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(eggSpawnTime.x, eggSpawnTime.y));

            Vector2 direction = Random.insideUnitCircle;
            Vector3 position = Vector3.zero;

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                position = new Vector3(Mathf.Sign(direction.x) * Camera.main.orthographicSize * Camera.main.aspect,
                    0,
                    direction.y * Camera.main.orthographicSize);
            }
            else
            {
                position = new Vector3(direction.x * Camera.main.orthographicSize * Camera.main.aspect,
                    0,
                    Mathf.Sign(direction.y) * Camera.main.orthographicSize);
            }

            position -= position.normalized * 0.1f;

            Vector3 force = -position.normalized * 1000.0f;
            Vector3 torque = Random.insideUnitSphere * Random.Range(500.0f, 1500.0f);
            object[] instantiationData = { force, torque };

            PhotonNetwork.InstantiateSceneObject(eggName, position,
                Quaternion.Euler(Random.value * 360.0f, Random.value * 360.0f, Random.value * 360.0f),
                0, instantiationData);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("PlayerLive"))
        {
            CheckEndofGame();
            return;
        }

        if (!targetPlayer.IsLocal)
        {
            return;
        }
        nickNameText.text = targetPlayer.NickName;
        scoreText.text = "Score : " + targetPlayer.GetScore();
    }


    private void CheckEndofGame()
    {
        foreach(Player p in PhotonNetwork.PlayerList)
        {
            object playerLive;
            if(p.CustomProperties.TryGetValue("PlayerLive", out playerLive))
            {
                if ((bool)playerLive)
                    return;
            }
            else
            {
                return;
            }

            if (PhotonNetwork.IsMasterClient)
            {
                StopAllCoroutines();
            }
            string winner = "";
            int score = -1;

            foreach(Player player in PhotonNetwork.PlayerList)
            {
                if(player.GetScore() > score)
                {
                    winner = player.NickName;
                    score = player.GetScore();
                }
            }

            StartCoroutine(EndOfGame(winner, score));
        }
    }

    private IEnumerator EndOfGame(string winner, int score)
    {
        leaderboard.AddScore(score);
        leaderboard.gameObject.SetActive(true);

        float timer = 5.0f;

        while(timer > 0.0f)
        {
            nickNameText.text = string.Format("Winner : {0} ({1})", winner, score);
            scoreText.text = string.Format("Returning to login screen in {0} seconds.", timer.ToString("n2"));

            yield return new WaitForEndOfFrame();

            timer -= Time.deltaTime;
        }

        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Lobby");
    }
}
