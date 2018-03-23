using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(BoxCollider2D))]
public class CollisionBox : MonoBehaviour {

    //State
    public bool isInUse = false;
    public bool isDebug = true;
    private CollisionBoxData.BoxType boxType;

    //Refernces
    public Player owner;
    private BoxCollider2D _collider;
    private SpriteRenderer _spriteRenderer;


	// Use this for initialization
	void Start () {
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Activate(string name, Transform parent, Player owner, CollisionBoxData properties, float facing, bool drawDebug = true)
    {
        isInUse = true;
        isDebug = drawDebug;

        gameObject.name = name;
        gameObject.layer = LayerMask.NameToLayer(properties.type.ToString());

        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.position += new Vector3(
            properties._dimensions.position.x * facing,
            properties._dimensions.position.y);
        transform.localScale = new Vector3(
            properties._dimensions.size.x ,
            properties._dimensions.size.y);

        _collider.enabled = true;
        _collider.isTrigger = properties.isTrigger;

        this.owner = owner;
        boxType = properties.type;
    }

    public void Deactivate()
    {
        owner = null;
        gameObject.name = "Unused CollisionBox";
        isInUse = false;
        _collider.enabled = false;
        transform.parent = null;
        transform.position = new Vector3(-10000, -10000, 0);

    }

    public void DrawDebugBoxes()
    {
        if (isDebug)
        {
            _spriteRenderer.enabled = true;

            switch (boxType)
            {
                case CollisionBoxData.BoxType.Hit:
                    _spriteRenderer.color = new Color(1, 0, 0, 0.5f);
                    break;

                case CollisionBoxData.BoxType.Hurt:
                    _spriteRenderer.color = new Color(0, 1, 0, 0.5f);
                    break;

                default:
                    _spriteRenderer.color = new Color(1, 1, 1, 0.5f);
                    break;
            }
        }
        else
        {
            _spriteRenderer.enabled = false;
        }
    }
}
