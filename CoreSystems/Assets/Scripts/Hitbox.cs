using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

    private HitboxProperties properties;
    private float attack_damage;
    private float knockback_growth;
    private float base_knockback;
    private float launch_angle;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void AttachHitbox(GameObject obj, HitboxProperties hitboxProperties)
    {
        Hitbox instance = obj.AddComponent<Hitbox>();
        instance.properties = hitboxProperties;
        instance.attack_damage = hitboxProperties.attack_damage;
        instance.knockback_growth = hitboxProperties.knockback_growth;
        instance.base_knockback = hitboxProperties.base_knockback;
        instance.launch_angle = hitboxProperties.launch_angle;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject != transform.parent)
        {
            collision.gameObject.SendMessage("SetState", new HurtState(GetComponentInParent<Player>(), properties), SendMessageOptions.DontRequireReceiver);
        }
    }
}
