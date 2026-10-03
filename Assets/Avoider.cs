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
    public Transform avoider;

    private float size_x;
    private float size_y;

    // The amount of space between sample points in the PoissonDiscSampler
    [SerializeField]
    private float _radius = 1f;
    // The range of the PoissonDiscSampler
    [SerializeField]
    private float _range = 15f;


    private void OnEnable()
    {
        avoider = this.gameObject.GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        size_x = _range;
        size_y = _range;
        StartCoroutine(Avoid()); 
    }

    IEnumerator Avoid()
    {
        // Do this forever
        while (true)
        {
            
            if(!isVisible(avoider.position))
            {
                // check again in 0.5 seconds if not seen
                Debug.Log("avoidee cannot see me");
                CreatePoissonDisc();
                yield return new WaitForSeconds(0.5f);
                continue;
            }
            else
            {
                Debug.Log("avoidee can see me");
                List<Vector3> candidates = CreatePoissonDisc();

                //if (candidates.Count > 0)
                //{
                //    // is there a place to run? (is candidate list empty?)
                //    // candidate list would be all points in the sampler that are out of player line of sight

                //    // don't pause for seconds if avoider can be seen
                //    yield return null;
                //}
            }
        }
    }

    // creates and visualizes PoissonDiscSampler regardless of whether avoidee can see the avoider or not
    private List<Vector3> CreatePoissonDisc()
    {
        var sampler = new PoissonDiscSampler(size_x, size_y, _radius);
        List<Vector3> candidates = new List<Vector3>();
        foreach (var point in sampler.Samples())
        {
            // add raycast to see if a point is visible (in avoidee line of sight)
            // if yes then ignore
            // if no then add to candidate list
            Vector3 samplePosition = new Vector3(transform.position.x + point.x - size_x / 2f, transform.position.y, transform.position.z + point.y - size_y / 2f);

            if (!isVisible(samplePosition))
            {
                candidates.Add(samplePosition);
                Debug.DrawLine(avoider.transform.position, samplePosition, Color.blue, 0.5f);
            }
            else
            {
                // turn line red if visible
                Debug.DrawLine(avoider.transform.position, samplePosition, Color.red, 0.5f);
            }
        }
        return candidates;
    }

    // checks if a point is visible to the avoidee
    private bool isVisible(Vector3 point)
    {
        Vector3 direction = (avoidee.transform.position - point);
        float distance = direction.magnitude;
        Vector3 normalizedDirection = direction.normalized;
        if (Physics.Raycast(point, normalizedDirection, out RaycastHit hit, distance))
        {
            if(hit.collider.gameObject == avoidee)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
}
