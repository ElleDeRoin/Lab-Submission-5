using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//Require component NavMeshAgent, make a reference to it
//Require the object I need to avoid (avoidee)
//Require the range that I do not allow the avoidee to approach
//Require if I need to visualize

/* GENERAL LOOP */
//Do this forever
//Can the avoidee see me?
//No: Wait a bit and check again
//Yes: Is there a place to run? (is the candidate list empty?)  
//No: Wait a bit and check again
//Yes: Tell the agent to move there (which point in the candidate list is closest?)

/* FIND A SPOT */
//Create a PoissonDiscSampler
//Create a collection to store candidate hiding spots
//Foreach point visualize a line to it
//Foreach point in the PoissonDiscSampler, can the avoidee see it? (check visibility to point)
//Yes: ignore that point
//No: add the point to the candidate list

/* CHECK VISIBILITY TO POINT */
//Create a ray from one point to another
//Check if the ray hits something that is not the player (avoidee)
//NO: The point is visible
//YES: The point is not visible

public class Avoider : MonoBehaviour
{
    public GameObject avoidee;
    public NavMeshAgent navMeshAgent;
    [SerializeField]
    private float _range = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
