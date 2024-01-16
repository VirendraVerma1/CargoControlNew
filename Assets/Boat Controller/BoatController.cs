using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody boatRb;
    [SerializeField]
    private float Rotspeed;
    [SerializeField]
    private float boatspeed;
    [SerializeField]
    private float deacclartion;

    private float currentspeed = 0f;

    static Vector3 playerposition;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        Vector3 movedir = new Vector3(0, 0, 0);


        float horizontalAxis = Input.GetAxis("Horizontal");        
        float verticaAxis = Input.GetAxis("Vertical");        
        Vector3 inputdir = new Vector3(horizontalAxis, 0, verticaAxis);
        movedir = transform.forward * inputdir.z;

        transform.position += movedir *Time.deltaTime*boatspeed;
        transform.eulerAngles += new Vector3(0, horizontalAxis * Time.deltaTime * Rotspeed, 0);       
       
    }
}
