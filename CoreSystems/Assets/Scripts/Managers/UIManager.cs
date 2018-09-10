using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public BaseCharacter player1;
    public BaseCharacter player2;


    public Image p1_bar;
    public Image p2_bar;
    public Image divider;

    public Text P1_stamina;
    public Text P2_stamina;

    private float p1_init;
    private float p2_init;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        P1_stamina.text = "P1: " + (player1._stamina.GetPercentage()).ToString("#%");
        P2_stamina.text = "P2: " + (player2._stamina.GetPercentage()).ToString("#%");

        p1_bar.rectTransform.localScale = new Vector3(player1._stamina.GetPercentage(),1,1);
        p2_bar.rectTransform.localScale = new Vector3(player2._stamina.GetPercentage(),1,1);

        p1_bar.rectTransform.sizeDelta = new Vector2(300f + (player1._stamina.GetBasePercentageChange() * 300),p1_bar.rectTransform.sizeDelta.y);
        p2_bar.rectTransform.sizeDelta = new Vector2(300f + (player2._stamina.GetBasePercentageChange() * 300), p2_bar.rectTransform.sizeDelta.y);

        divider.rectTransform.localPosition = new Vector3((p1_bar.rectTransform.sizeDelta.x - p2_bar.rectTransform.sizeDelta.x)/2, -10, 0);


    }
}
