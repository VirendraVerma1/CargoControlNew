using System.Collections.Generic;
using UnityEngine;

public class PropsManager : MonoBehaviour
{
    public Transform allProps;

    List<GameObject> propsList = new List<GameObject>();
    List<int> propsListnotReceived = new List<int>();

    private int index = 0;


    private void Awake()
    {
        index=0;
        propsListnotReceived.Clear();
        propsList.Clear();
        if (allProps != null)
        {
            foreach (Transform t in allProps)
            {
                propsList.Add(t.gameObject);
            }
        }

        foreach (GameObject g in propsList)
        {
            g.SetActive(false);
        }
    }

    public GameObject GetProp()
    {
        if(propsListnotReceived.Count>0)
        {
            int id = 0;
            foreach(GameObject g in propsList)
            {
                if(g.GetInstanceID()== propsListnotReceived[0])
                {
                    propsListnotReceived.RemoveAt(0);
                    return propsList[id];
                }
                id++;
            }
            return (propsList.Count > 0) ? propsList[index++] : null;
        }
        else
        {
            return (propsList.Count > 0) ? propsList[index++] : null;
        }
        
    }

    public void EnableObject(int guid)
    {
        foreach(GameObject t in propsList)
        {
            if(t.GetInstanceID()==guid)
            {
                t.gameObject.SetActive(true);
            }
        }
    }


    public void NotReceivedObject(int guid)
    {
        propsListnotReceived.Add(guid);
    }
}
