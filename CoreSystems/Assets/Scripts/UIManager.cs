using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Player player1;
    public Player player2;

    public Text P1_stamina;
    public Text P2_stamina;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        P1_stamina.text = "P1: " + (player1.GetPercentStamina()).ToString("#%");
        P2_stamina.text = "P2: " + (player2.GetPercentStamina()).ToString("#%");
    }
}
