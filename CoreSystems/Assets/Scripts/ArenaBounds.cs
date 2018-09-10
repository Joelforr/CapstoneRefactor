﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaBounds : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<BaseCharacter>() != null)
        {
            collision.GetComponent<BaseCharacter>().status = BaseCharacter.PlayerStatus.Dead;
        }

        GameManager.instance.OnRoundEnd();
    }

}
