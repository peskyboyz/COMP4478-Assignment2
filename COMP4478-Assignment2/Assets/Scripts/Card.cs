using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    Main main;
    public int cardValue;
    public bool isFlipped;
    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.Find("Scripts").GetComponent<Main>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
