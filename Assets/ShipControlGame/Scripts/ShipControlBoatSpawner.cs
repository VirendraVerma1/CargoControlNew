using System;
using System.Collections;
using System.Collections.Generic;
using ShipControl;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShipControlBoatSpawner : MonoBehaviour
{
    private float minxrange = 30;
    private float maxxrange = 35;
    
    private float minyrange = 20;
    private float maxyrange = 25;

    public float spawnRate = 1;

    private ShipControlGameManager _shipControlGameManager;
    public GameObject[] Boats;

    public int MaxSpawnedBoatAtTime = 5;

    List<GameObject> allSpawnedBoats = new List<GameObject>();

    public PropsManager propsManager;

    private void Start()
    {
        _shipControlGameManager = gameObject.GetComponent<ShipControlGameManager>();
        StartCoroutine(SpawnObject());
    }

    IEnumerator SpawnObject()
    {
        while (_shipControlGameManager.isGameOver==false)
        {
            if(CheckIfMaxBoatReached() < MaxSpawnedBoatAtTime)
                SpawnBoat();
            yield return new WaitForSeconds(spawnRate);
            
        }
    }

    int CheckIfMaxBoatReached()
    {
        for (int i=0;i<allSpawnedBoats.Count;i++)
        {
            if(!allSpawnedBoats[i])
            {
                allSpawnedBoats.RemoveAt(i);
            }
        }
        return allSpawnedBoats.Count;
    }

    public void SpawnBoat()
    {
        int randomBoat = Random.Range(0, Boats.Length);
        GameObject g = Instantiate(Boats[randomBoat], SpawnPoint(), gameObject.transform.rotation);
        g.transform.LookAt(gameObject.transform);
        allSpawnedBoats.Add(g);

        GameObject objectTOPlace=propsManager.GetProp();
        ShipBotController shipBotController=g.GetComponent<ShipBotController>();
        shipBotController.ObjectCarior = Instantiate( objectTOPlace, shipBotController.carriorPlacer.position,Quaternion.identity);
        shipBotController.ObjectCarior.transform.SetParent(g.transform);
        shipBotController.ObjectId = objectTOPlace.GetInstanceID();
        shipBotController.ObjectCarior.SetActive(true);
        Destroy(shipBotController.ObjectCarior.GetComponent<MeshCollider>());
        shipBotController.ObjectCarior.transform.localScale = shipBotController.ObjectCarior.transform.localScale*2;
    }
    
    
    Vector3 SpawnPoint()
    {
        var px = Random.Range(minxrange*-1, minxrange);
        var mx = Random.Range(maxxrange*-1, maxxrange);
        var py = Random.Range(minyrange*-1, minyrange);
        var my = Random.Range(maxyrange*-1, maxyrange);
        Vector3 pos = new Vector3(mx+(mx-px),0, my+(my-py));
        return pos;
    }
}
