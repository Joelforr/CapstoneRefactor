using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionGroupManager : MonoBehaviour {

    //References
    private BaseCharacter owner;

    //State
    private CollisionBoxData.BoxType groupType;
    public bool collided = false;

    //General Collision Data
    private CollisionBoxData properties;

    //Hitbox-Only Data
    private float attack_damage = 0;
    private float knockback_growth = 0;
    private float base_knockback = 0;
    private float launch_angle = 0;
    private float launch_direction = 0;

    //Storage
    private delegate void CollisionDelegate();
    CollisionDelegate del;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        try{
            CollisionBox cb = collision.GetComponent<CollisionBox>();
            if(cb.owner != owner && collision.GetComponentInParent<BaseCharacter>() != null)
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("Hurt") && collided == false)
                {
                    collided = true;
                    if (del == null)
                    {
                        del = () =>
                        {
                            Debug.Log(this.name + " collided with: " + collision.name);
                            BaseCharacter player_hit = collision.gameObject.GetComponentInParent<BaseCharacter>();
                            player_hit._sm.currentState.FireCustomEvent(new EventList.HitEvent(player_hit, properties, launch_direction));
                            owner._stamina.Siphon(player_hit, CalculateDamage());

                            player_hit.sfxPlayer.Play(Services.SFXLibrary.GetSFX(SFXLibrary.SFXTags.Impact));
                            Camera.main.GetComponent<Cam>().ShakeCamera(.65f, 15);
                            if (player_hit.staminaSystem.GetCurrentMaxStamina() > 2)
                            {
                                owner.staminaSystem.Add();
                                player_hit.staminaSystem.Remove();
                            }
                            else
                            {
                                player_hit.staminaSystem.SpendOrbs(1);
                            }
                        };
                    }
                    del();
                }
            }
        }
        catch
        {

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
   
    }

    /*******************************************************************************************************/
    /// <summary>
    /// Utility method to set data for this collision group manager based on the active frame
    /// </summary>
    /// <param name="collisionBox_data"></param>
    /// <param name="launch_direction"></param>
    public void SetData(BaseCharacter owner, CollisionBoxData collisionBox_data, float launch_direction)
    {
        this.owner = owner;
        this.properties = collisionBox_data;
        gameObject.layer = LayerMask.NameToLayer(collisionBox_data.type.ToString());
        this.attack_damage = collisionBox_data.attack_damage;
        this.knockback_growth = collisionBox_data.knockback_growth;
        this.base_knockback = collisionBox_data.base_knockback;
        this.launch_angle = collisionBox_data.launch_angle;
        this.launch_direction = launch_direction;
    }

    public void Clear()
    {

    }

    private float CalculateDamage()
    {
        return this.attack_damage * 2;
    }
}
