using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    Main main;
    Transform tr;
    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.Find("Scripts").GetComponent<Main>();
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

}
