﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuButtonController : MonoBehaviour {

	// Use this for initialization
	public int index;
    public bool keyDown;
    public int maxIndex;
	public AudioSource audioSource;
    public bool canMove;

	void Start () 
    {
		audioSource = GetComponent<AudioSource>();
        canMove = true;
    }

    void Update()
    {
        if (canMove)
        {
            if (Input.GetAxis("Vertical") != 0)
            {
                if (!keyDown)
                {
                    if (Input.GetAxis("Vertical") < 0)
                    {
                        if (index < maxIndex)
                        {
                            index++;
                        }

                        else
                        {
                            index = 0;
                        }

                    }

                    else if (Input.GetAxis("Vertical") > 0)
                    {
                        if (index > 0)
                        {
                            index--;
                        }
                        else
                        {
                            index = maxIndex;
                        }
                    }

                    keyDown = true;
                }
            }

            else
            {
                keyDown = false;
            }
        }
    }
}
