using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Positions
{
    public int x,y,z;
}
public class LineArrays : MonoBehaviour
{
    LineRenderer lineRenderer;
    [SerializeField] Positions[] positions;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
