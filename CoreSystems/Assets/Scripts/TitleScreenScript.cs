using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

public class TitleScreenScript : MonoBehaviour {

    public Player player;

    public Texture2D fadeOutTexture;
    public float fadeSpeed = .8f;

    private int drawDepth = -100;
    private float alpha = 1.0f;
    private int fadeDir = -1;                    //1 out -1 in

    private void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);

    }

    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }

    void Awake()
    {
        player = ReInput.players.GetPlayer(0);
    }

    // Use this for initialization
    void Start () {
        player = ReInput.players.GetPlayer(0);
    }
	
	// Update is called once per frame
	void Update () {
        if (player.GetButton("Action5"))
        {
            StartCoroutine(FadeToBlack());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
	}

    IEnumerator FadeToBlack()
    {
        float fadeTime = BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(1);
    }
}
