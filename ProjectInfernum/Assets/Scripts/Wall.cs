using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    public Sprite dmgSprite;
    public int hp = 3;

    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public void DamageWall(int loss)
    {

        spriteRenderer.sprite = dmgSprite;
        hp -= loss;
        if (hp <= 0)
            gameObject.SetActive(false);

    }
}
