using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace ShipControl
{

    [RequireComponent(typeof(NavMeshAgent))]
    public class ShipBotController : MonoBehaviour
    {


        public GameObject ObjectCarior;
        public int ObjectId;

        public Transform carriorPlacer;

        //move in the forward direction if there is no any other waypoint travel
        //if there is any waypoint travel then travel to it
        //if the loop is completed then the ship got minimized and destroyed
        //if the loop is completed then the ship autoadjust its own shipyard
        //if not then it will continuous move forward

        private NavMeshAgent navMeshAgent;
        public List<GameObject> myWaypoints=new List<GameObject>();
        public bool isLoopCompleted = false;
        public bool isReached = false;
        public float reachedAtWaypoint = 0.2f;
        public float movingSpeed = 0.3f;
        public string myTag = "Yellow";

        PropsManager propsManager;
        void Start()
        {
            navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            navMeshAgent.speed = movingSpeed*2;
            isReached = false;
            FindNewPoint();
            navMeshAgent.SetDestination(tempPoint);
            propsManager=GameObject.FindGameObjectWithTag("GameController").GetComponent<PropsManager>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == myTag)
            {
                isReached = true;
                ClearWayPoint();
                propsManager.EnableObject(ObjectId);
                Destroy(gameObject);
            }
            else if(collision.collider.tag!="Untagged")
            {
                //Destroy the ship and send the instance id that not received
                ClearWayPoint();
                Destroy(gameObject);
            }
        }


        private Vector3 tempPoint;
        void Update()
        {
            if (isReached)
                return;
            if (myWaypoints.Count > 0)
            {
                if (Vector3.Distance(gameObject.transform.position, myWaypoints[0].transform.position) > reachedAtWaypoint)
                {
                    return;
                }
                else
                {
                    //set the new Destination
                    Destroy(myWaypoints[0]);
                    myWaypoints.RemoveAt(0);
                    if(myWaypoints.Count > 0)
                        navMeshAgent.SetDestination(myWaypoints[0].transform.position);
                }
            }
            else
            {
                //move forward
                FindNewPoint();

                //gameObject.transform.position += gameObject.transform.forward * movingSpeed/10;
            }
        }

        void FindNewPoint()
        {
            if (Vector3.Distance(gameObject.transform.position, tempPoint) < reachedAtWaypoint)
            {
                tempPoint=GetRandomPoint(gameObject.transform.position, 20);
                navMeshAgent.SetDestination(tempPoint);
            }
        }

        public void ClearWayPoint()
        {
            foreach (var point in myWaypoints)
            {
                Destroy(point);
            }
            myWaypoints.Clear();
        }

        public void SetPoint()
        {
            navMeshAgent.SetDestination(myWaypoints[0].transform.position);
        }
        
        // Get Random Point on a Navmesh surface
        public static Vector3 GetRandomPoint(Vector3 center, float maxDistance) {
            // Get Random Point inside Sphere which position is center, radius is maxDistance
            Vector3 randomPos = Random.insideUnitSphere * maxDistance + center;

            NavMeshHit hit; // NavMesh Sampling Info Container

            // from randomPos find a nearest point on NavMesh surface in range of maxDistance
            NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas);

            return hit.position;
        }
    }
}