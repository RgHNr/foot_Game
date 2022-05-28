using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    private CharacterController controller;
    public float speed;
    public float force;
    private float time,timeEnd;

    public GameObject goal;
    public GameObject ball;
    [SerializeField]
    private Camera cam;
    public float shotPower;
    [SerializeField]
    private LayerMask layer;
    private Vector3 direction;

    private float BallDistance;

    float Power;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
      
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            time += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            //if (time == 0) {
            //    time = 0.5f;
            //}
            
            
        }

        
    }
    private void FixedUpdate()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        direction = new Vector3(horizontal, 0, vertical).normalized;
        controller.Move(direction * Time.fixedDeltaTime * speed);

        float distance = Vector3.Distance(goal.transform.position,ball.transform.position);
        //Debug.DrawLine(ball.transform.position, goal.transform.position,Color.red);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ball.transform.position, ray.direction * distance, Color.blue);

        BallDistance = Vector3.Distance(transform.position, ball.transform.position);

        //ball.gameObject.GetComponent<Rigidbody>().velocity = ray.direction * shotPower;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance+100f,layer))
        {
            //draw invisible ray cast/vector
            Debug.DrawLine(ball.transform.position, hit.point,Color.yellow);
            //log hit area to the console

            if (Input.GetMouseButtonDown(0))
            {
                time += Time.deltaTime;
            }
            if (Input.GetMouseButtonUp(0)) {

                if (time == 0) return;
                Debug.Log("Time= " + time);
                Power = 1 / time;
                //ball.gameObject.GetComponent<Rigidbody>().velocity = (hit.point-ball.transform.position).normalized * shotPower+new Vector3(0,10,0);
                ball.gameObject.GetComponent<Rigidbody>().velocity = (hit.point-ball.transform.position).normalized*Power* shotPower;
                Debug.Log("Point: " + (hit.point - ball.transform.position).normalized);
                
                
                Debug.Log("Shot power: "+Power);
                time = 0;
            }
            
        }

        //if (BallDistance <= 1.5f ) {
        //    ball.GetComponent<Rigidbody>().velocity = direction * force*0.5f;
        //}
        
        
    }
   
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb != null) 
        {
            rb.velocity=direction * force;

        }
        
    }

    
}
