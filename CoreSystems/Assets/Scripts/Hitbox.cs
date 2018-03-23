using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

    private Player parent_player;
    private CollisionBoxData properties;
    private float attack_damage;
    private float knockback_growth;
    private float base_knockback;
    private float launch_angle;
    private float launch_direction;
    private int i = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void AttachHitbox(GameObject obj, CollisionBoxData hitboxProperties, float launch_direction)
    {
        Hitbox instance = obj.AddComponent<Hitbox>();
        instance.properties = hitboxProperties;
        instance.attack_damage = hitboxProperties.attack_damage;
        instance.knockback_growth = hitboxProperties.knockback_growth;
        instance.base_knockback = hitboxProperties.base_knockback;
        instance.launch_angle = hitboxProperties.launch_angle;
        instance.launch_direction = launch_direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject != transform.parent)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Hurt") && i == 0)
            {
                    Debug.Log(this.name);
                    Player player_hit = collision.gameObject.GetComponentInParent<Player>();
                    player_hit.mState.FireCustomEvent(new EventList.HitEvent(player_hit, properties, launch_direction));
                    i++;
                    Debug.Log(i);
            }
            
        }
    }
}
