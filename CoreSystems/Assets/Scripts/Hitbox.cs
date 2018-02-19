using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

    private Player parent_player;
    private HitboxProperties properties;
    private float attack_damage;
    private float knockback_growth;
    private float base_knockback;
    private float launch_angle;
    private float launch_direction;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void AttachHitbox(GameObject obj, HitboxProperties hitboxProperties, float launch_direction)
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
            if (collision.gameObject.layer == LayerMask.NameToLayer("Hurt"))
            {
                try
                {
                    Player player_hit = collision.gameObject.GetComponentInParent<Player>();
                    player_hit.SetState(new HurtState(player_hit, properties, launch_direction));
                }
                catch
                {
                    Debug.Log(collision.gameObject.name);
                    Debug.Log("failed to get player component");
                }
            }
            
        }
    }
}
