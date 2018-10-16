using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Xeo;
using UnityEngine.SceneManagement;

public class MatchManager {

    public BaseCharacter p1;
    public BaseCharacter p2;

    public int p1_wins = 0;
    public int p2_wins = 0;

    public GameObject scoreboard;

    public Image[] p1_medals;
    public Image[] p2_medals;

    public int goal;

    public GameObject countdown;

    public MapPicker mapPicker;

    private bool ignoreCalls = false;
 
    public MatchManager(GameManager gm, int goal, GameObject medalPrefab, GameObject p1_medaltext, GameObject p2_medaltext, GameObject scoreboard, GameObject countdownText)
    {
        this.p1 = gm.p1;
        this.p2 = gm.p2;

        this.goal = goal;
        this.scoreboard = scoreboard;
        this.countdown = countdownText;

        this.mapPicker = gm.mapPicker;

        p1_medals = new Image[goal];
        p2_medals = new Image[goal];

        for (int i = 0; i < goal; i++)
        {
            double offset = (i) * (medalPrefab.GetComponent<RectTransform>().sizeDelta.x * 1.5f);

            GameObject temp_p1_medal = GameObject.Instantiate(medalPrefab, p1_medaltext.transform);
            temp_p1_medal.GetComponent<RectTransform>().anchoredPosition += Vector2.right * (float)offset;
            p1_medals[i] = temp_p1_medal.GetComponent<Image>();

            GameObject temp_p2_medal = GameObject.Instantiate(medalPrefab, p2_medaltext.transform);
            temp_p2_medal.GetComponent<RectTransform>().anchoredPosition += Vector2.right * (float)offset;
            p2_medals[i] = temp_p2_medal.GetComponent<Image>();
        }
    }


    public IEnumerator RoundEnd()
    {
        if (ignoreCalls)
        {
            yield break;
        }

        ignoreCalls = true;
        yield return new WaitForSeconds(1f);

        //Step 1 Check the status of the players
        if(p1.status == BaseCharacter.PlayerStatus.Dead && p2.status == BaseCharacter.PlayerStatus.Dead)
        {
            p1.status = BaseCharacter.PlayerStatus.Frozen;
            p2.status = BaseCharacter.PlayerStatus.Frozen;
        }
        else if (p1.status == BaseCharacter.PlayerStatus.Dead)
        {
            p2_wins++;
            p1.status = BaseCharacter.PlayerStatus.Frozen;
            p2.status = BaseCharacter.PlayerStatus.Frozen;
        }
        else if (p2.status == BaseCharacter.PlayerStatus.Dead)
        {
            p1_wins++;
            p1.status = BaseCharacter.PlayerStatus.Frozen;
            p2.status = BaseCharacter.PlayerStatus.Frozen;
        }
        else
        {
            p2_wins++;
            p1.status = BaseCharacter.PlayerStatus.Frozen;
            p2.status = BaseCharacter.PlayerStatus.Frozen;
        }


       

        //Step 3 Show Scoreboard
        scoreboard.SetActive(true);

        for(int i  = 0; i < p1_wins; i++)
        {
            p1_medals[i].color = Color.white;
        }

        for (int i = 0; i < p2_wins; i++)
        {
            p2_medals[i].color = Color.white;
        }

        yield return new WaitForSeconds(4f);

        //Step 2 Check if there is a winner
        if (p1_wins == goal)
        {
            SceneManager.LoadScene(0);
            yield break;
        }
        else if (p2_wins == goal)
        {
            SceneManager.LoadScene(0);
            yield break;
        }
        else
        {
            scoreboard.SetActive(false);
        }

        //Step 4 Reset Map and PLayers Players
        Map newMap = mapPicker.SelectMap();
        p1.Reset(newMap.spawnA);
        p2.Reset(newMap.spawnB);

        //Step 5 Start New Round
        countdown.SetActive(true);
        Text cdT = countdown.GetComponent<Text>();
        cdT.text = "3";
        yield return new WaitForSeconds(1f);
        cdT.text = "2";
        yield return new WaitForSeconds(1f);
        cdT.text = "1";
        yield return new WaitForSeconds(1f);
        ignoreCalls = false;
        countdown.SetActive(false);
        p1.status = BaseCharacter.PlayerStatus.Alive;
        p2.status = BaseCharacter.PlayerStatus.Alive;

    }
}
