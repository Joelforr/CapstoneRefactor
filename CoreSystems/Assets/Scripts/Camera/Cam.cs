using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {

    public float lerpAmnt;
    public Transform player;
    public Transform cursor;
    public float cursorOffsetScale;
    Vector2 truePos;

    public float shakeTimer;
    public float shakeAmount;
    private Vector3 preShakePos;
    public Vector3 originalPos;
    public Vector3[] values;

    // Use this for initialization
    void Start()
    {
        preShakePos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*transform.position = Vector2.Lerp(transform.position, player.position + ((cursor.position - player.position) * cursorOffsetScale), lerpAmnt);
        //transform.position = truePos + (Vector2)((cursor.position - player.position) * cursorOffsetScale);


        transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
        */
        
        

        if (shakeTimer >= 0)
        {
            Vector2 shakePos = Random.insideUnitCircle * shakeAmount;

            transform.position = new Vector3(preShakePos.x + shakePos.x, preShakePos.y + shakePos.y, transform.position.z);

            shakeTimer --;

        }
        else
        {

            transform.position = Vector3.Lerp(transform.position, preShakePos, .2f);

        }

    }

    public void ShakeCamera(float _shakePower, float _shakeDuration)
    {
        preShakePos = transform.position;
        shakeAmount = _shakePower;
        shakeTimer = _shakeDuration;
    }

}
