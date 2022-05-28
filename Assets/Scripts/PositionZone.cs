using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionZone : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask layer;
    public GameObject MyZone;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkZone();
    }

    public GameObject checkZone() {
        RaycastHit hit;
        GameObject zone = null;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down),out hit, 10f,layer)) {

            //Debug.Log(name+" current zone is "+ hit.collider.name);
            zone = hit.collider.gameObject;
        }
        return zone;
    }

    public void getToZone() {


    }
}
