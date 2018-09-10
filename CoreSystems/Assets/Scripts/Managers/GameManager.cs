using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Xeo;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    protected EventManager eventManager = new EventManager();

    public BaseCharacter p1;
    public BaseCharacter p2;

    private MatchManager mm;

    public GameObject medal_UI_Prefab;
    public GameObject p1_medal_text;
    public GameObject p2_medal_text;
    public GameObject scoreboard;
    public GameObject countdowntext;

    //Map Data
    public Vector2 p1_spawnpoint;
    public Vector2 p2_spawnpoint;

    

    public enum Mode
    {
        Title,
        PlayMode
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            GameManager.Destroy(this.gameObject);
        }

        Services.Init();
    }

    // Use this for initialization
    void Start () {

        mm = new MatchManager(this, 5, medal_UI_Prefab, p1_medal_text, p2_medal_text, scoreboard, countdowntext);
	}
	

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(mm.RoundEnd());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void OnRoundEnd()
    {
        StartCoroutine(mm.RoundEnd());
    }

    public void InitServices()
    {
        Services.AnimationLibray = new AnimationLibrary();
    }
}
