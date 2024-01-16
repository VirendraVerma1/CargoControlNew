using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShipControl
{
    public class ShipControlGameManager : MonoBehaviour
    {
        public bool isGameOver = false;
        public GameObject YellowPoint;
        public GameObject GreenPoint;
        private GameObject selectedPoint;

        private GameObject selectedObject;
        private bool startmakingWaypoints = false;
        private ShipBotController _shipBotController;

        public float minDistanceToSpawnWaypoints = 0.2f;

        private GameObject previousSpawnPoint;
        private float timer = 0.5f;
        bool isholdingForWaypoint = false;

        private void Start()
        {
            previousSpawnPoint=gameObject;
        }

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
                if (Physics.Raycast(ray, out hit)) 
                {
                    if (hit.collider.tag == "Boat" && !isholdingForWaypoint)
                    {
                        isholdingForWaypoint = true;
                        selectedObject = hit.collider.gameObject;
                        startmakingWaypoints = true;
                        _shipBotController = selectedObject.GetComponent<ShipBotController>();
                        _shipBotController.ClearWayPoint();
                        if (_shipBotController.myTag == "Yellow")
                            selectedPoint = YellowPoint;
                        else if (_shipBotController.myTag == "Green")
                            selectedPoint = GreenPoint;
                    }
                    else if (hit.collider.tag == "Yellow"||hit.collider.tag == "Green"&&selectedObject!=null)
                    {
                        //_shipBotController.myWaypoints.Add(hit.collider.gameObject);
                        _shipBotController.isLoopCompleted = true;
                        selectedObject = null;
                        startmakingWaypoints = false;
                    }
                    else if (startmakingWaypoints )
                    {
                        if (Vector3.Distance(hit.point, previousSpawnPoint.transform.position) > minDistanceToSpawnWaypoints)
                        {
                            GameObject g = Instantiate(selectedPoint, hit.point+new Vector3(0,0.8f,0), Quaternion.identity);
                            _shipBotController.myWaypoints.Add(g);
                            _shipBotController.SetPoint();
                            previousSpawnPoint = g;
                        }
                    }
                    else
                    {
                        startmakingWaypoints = false;
                    }
                }
            }

            if(Input.GetMouseButtonUp(0))
            {
                isholdingForWaypoint = false;
                previousSpawnPoint = gameObject;
            }
        }
    }
}