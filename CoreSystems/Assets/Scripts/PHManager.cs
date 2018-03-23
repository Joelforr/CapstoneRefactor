using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PHManager : MonoBehaviour {

    public Text restart_text;

	// Use this for initialization
	void Start () {
        restart_text.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        restart_text.gameObject.SetActive(true);
    }
}
