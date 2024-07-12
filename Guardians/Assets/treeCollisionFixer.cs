using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treeCollisionFixer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Awake()
    {
        GetComponent<TerrainCollider>().enabled = false;
        GetComponent<TerrainCollider>().enabled = true; 
    }
}
