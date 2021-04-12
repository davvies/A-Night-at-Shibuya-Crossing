using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LocalCarNavigation : MonoBehaviour
{
    #region Inspector values
    [SerializeField] GlobalCarSystem GlobalCarSystem = default; //reference to the car manager
    [SerializeField] RoundStatTracker currentRoundStats = default; //cars have a direct influence on stats (car count)
    [SerializeField] GameState activeState = default; //track active state
    [SerializeField] int waypoint; //waypointIndex
    [SerializeField] float debugDistance;
    [SerializeField] GameObject particleSystem = default; //flames
    public GameObject[] activeRoute;
    [Range(0, -1000)] public int speed = -900;
    [SerializeField] float rotationSpeed = 4.15f;
    #endregion

    bool journeyActive = false; //track the journey
    const float turningMargin = 0.4f; //margin for turning before point (to head to next one)
    public bool shouldStop = false; //used for light data
    public bool carCrashImmenent = false; //used at traffic lights to prevent pile up
    bool hasCrash = false; //track velocity should continue at a constant rate
    Rigidbody rb;
    const float carRadius = 8f;

    void Start()
    {
        //Configure default states
        rb = GetComponent<Rigidbody>();
        if (particleSystem.GetComponent<ParticleSystem>().isPlaying) particleSystem.GetComponent<ParticleSystem>().Pause();
        waypoint = 0;
    }

    /// <summary>Method <c>FixedUpdate</c> Handle physics and navigation</summary>
    void FixedUpdate() 
    {
        //when the end is reached, enable a trigger to stop
        if (waypoint == activeRoute.Length)
        {
            journeyActive = false;
        }

        if (journeyActive)
        {
            //rotation lerp to waypoint
            var targetPoint = activeRoute[waypoint].transform.position;
            var targetRotation = Quaternion.LookRotation(targetPoint - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            if (carCrashImmenent)
            {
                rb.velocity = Vector3.zero; //apply emergency break
            }
            if (ShouldStop(3f) == false && carCrashImmenent == false && hasCrash == false)
           {
                Vector3 dir = transform.position - activeRoute[waypoint].transform.position;
                dir.Normalize(); //direction is normalised to allow scalar product
                rb.velocity = dir * -900f * Time.fixedDeltaTime; //gradual breaking causes traffic jams, this is not a simulator
            } else
            {
                rb.velocity = Vector3.zero; 
            }

            if(Vector3.Distance(transform.position,activeRoute[waypoint].transform.position) < turningMargin && journeyActive)
            {
                
                waypoint += 1; //when at the destination go to next
            } else
            {
                debugDistance = Vector3.Distance(transform.position, activeRoute[waypoint].transform.position); //for debug purposes output distance to next point
            }
        } else
        {
            currentRoundStats.IncrementCarCount(); //when car has gone to its destination, increment the global count of cars
            GlobalCarSystem.AddCarBackToQueue(int.Parse(name)); //**ASSUMPTION** - its assumed cars are named by index
            gameObject.SetActive(false); //disable car
        }

        carCrashImmenent = false;

        //Find if there are cars in front at the lights to apply breaks
        //arbitrary values found in raycasting are used as an offset to fire the ray at an appropriate height and distance
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z), transform.TransformDirection(Vector3.forward * 3), out RaycastHit hit, 4.25f) && waypoint == 0 && hit.transform.CompareTag("Hazard"))
        {
            carCrashImmenent = true; //trigger a stop if a car is front

        }

        Collider[] localObjects = Physics.OverlapSphere(transform.position, carRadius); //get local radious
        
        foreach (Collider collider in localObjects)
        {
            //Stop at traffic lights
            if (collider.name.Contains("Light"))
            {
                shouldStop = collider.GetComponent<TrafficStates>().IsTrafficLightRed();
            } 
        }
    }

    /// <summary>Method <c>ShouldStop</c> Ability to stop at lights</summary>
    bool ShouldStop(float distance) => shouldStop == true && Vector3.Distance(transform.position, activeRoute[0].transform.position) < distance&&waypoint==1;

    void OnEnable()
    {
        //starting configure
        waypoint = 0; 
        journeyActive = true;
    }

    const float velocityDecelerationConstant = 0.5f;

    void OnCollisionEnter(Collision collision)
    {
        activeState.EndGame();
        particleSystem.SetActive(true);
        particleSystem.GetComponent<ParticleSystem>().Play(); //engulf car in flames
        hasCrash = true; 
        rb.velocity *= velocityDecelerationConstant; //begin smooth de-acceleration 
    }
}
