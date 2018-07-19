using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartSystem : MonoBehaviour {

    private int maxHeartAmount = 10;
    public int startHearts = 3;
    public int currentHP;
    private int maxHealth;
    private int healthPerHeart = 1;

    public Image[] healthImages;
    public Sprite[] healthSprites;

	void Start () {
        currentHP = startHearts * healthPerHeart;
        maxHealth = maxHeartAmount * healthPerHeart;
		
	}
	
	// Update is called once per frame
	void checkHealthAmount () {
        for (int i = 0; i < maxHeartAmount; i++)
        {
            if(startHearts <= i)
            {
                healthImages[i].enabled = false;
            }
            else
            {
                healthImages[i].enabled = true;
            }
        }
		
	}
}
