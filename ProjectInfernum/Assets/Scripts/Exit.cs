using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {

    GameObject[] enems;
    private Animator animator;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        enems = GameObject.FindGameObjectsWithTag("Enemy");
        if (enems.Length == 0)
        {
            animator.SetBool("exitOpen", true);
        }
        else
        {
            animator.SetBool("exitOpen", false);
        }


    }
}
