using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keeper : MonoBehaviour
{
    public enum KeeperState
    {

        patrolling,
        chasing,
        keeping
    }
    public GameObject ball;
    public Rigidbody rb;
    public LayerMask ballLayer;
    public bool ballinAttackRange, ballinChaseRange;

    bool startAnimation;

    public GameObject goal;
    public float attackRange, chaseRange, animationRange;


    public KeeperState currentState;
    // Start is called before the first frame update

    float keepRate = 5f;

    float nextkeep;

    public float speed;

    public float keepSpeed;

    public Animator anim;

    [SerializeField]
    bool canMove;


    void Start()
    {
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        ballinAttackRange = Physics.CheckSphere(goal.transform.position, attackRange, ballLayer);
        ballinChaseRange = Physics.CheckSphere(goal.transform.position, chaseRange, ballLayer);
        startAnimation = Physics.CheckSphere(goal.transform.position, animationRange, ballLayer);

        if (!ballinAttackRange && !ballinChaseRange) currentState = KeeperState.patrolling;
        if (!ballinAttackRange && ballinChaseRange && !ball.GetComponent<SphereMove>().InpassingState) currentState = KeeperState.chasing;
        if (ballinAttackRange && ballinChaseRange && ball.GetComponent<SphereMove>().InpassingState) currentState = KeeperState.keeping;





    }
    private void FixedUpdate()
    {
        switch (currentState)
        {

            case KeeperState.patrolling:
                patrolling();
                //Debug.Log("keeper patrolling");
                break;
            case KeeperState.chasing:
                //Debug.Log("keeper chasing");
                if (Vector3.Distance(goal.transform.position, transform.position) > 20)
                    return;
                chasing();
                break;
            case KeeperState.keeping:
                //Debug.Log("keeper keeping");


                keeping();
                break;



        }
    }

    public void getToBall()
    {

        Vector3 ballTransform = ball.transform.position;

        rb.velocity = (new Vector3(ballTransform.x, transform.position.y + ballTransform.y, transform.position.z));
    }

    public void chasing()
    {
        if (!canMove)
            return;

        if (!ball.GetComponent<SphereMove>().InpassingState)
        {

            Vector3 diection = (ball.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + diection * Time.deltaTime * speed);
            transform.LookAt(ball.transform);
            anim.SetBool("run", true);
        }

    }

    public void patrolling()
    {

        if (Vector3.Distance(transform.position, goal.transform.position) < 2)
        {
            anim.SetBool("run", false);
            transform.LookAt(ball.transform);
            return;
        }
        if (!canMove)
            return;


        rb.MovePosition(transform.position + new Vector3(-transform.position.x, 0, 57.3f - transform.position.z).normalized * Time.deltaTime * speed);
        transform.LookAt(goal.transform);
        anim.SetBool("run", true);
    }

    public void keeping()
    {

        //if (Mathf.Abs(transform.position.x) > 7f)
        //    return;

        //Debug.Log("he is keeping");
        if (!canMove)
           return;

        Vector3 direction = new Vector3(ball.transform.position.x - transform.position.x, 0, 0);

        //Debug.Log(direction);

        //if (startAnimation)
        //    if (!canMove)
        //        return;

        if(startAnimation)
            rb.MovePosition(transform.position + direction * Time.deltaTime * keepSpeed);



        if (Time.time > nextkeep)
        {
            nextkeep = Time.time + keepRate;

            if (ball.transform.position.x > 0)
            {

                anim.SetTrigger("ground_left");

            }
            else
            {
                anim.SetTrigger("ground_right");
            }
        }









        //if (Mathf.Abs( transform.position.x) > 7f)
        //    return;

        //Vector3 direction = new Vector3(ball.transform.position.x-transform.position.x, 0, 0);

        //Debug.Log(direction);

        //rb.MovePosition(transform.position + direction * Time.deltaTime * keepSpeed);


        //rb.MovePosition(new Vector3(Mathf.Clamp(ball.transform.position.x,-7f,7f), transform.position.y, transform.position.z));

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Sphere (1)")
        {
            canMove = false;
            Debug.Log("keeper hit the ball");
            Vector3 direction = (ball.transform.position - transform.position).normalized;
            ball.gameObject.GetComponent<SphereMove>().shootBall(direction * 25);
            //ball.GetComponent<SphereMove>().CurrentPlayer = null;
            ball.GetComponent<SphereMove>().InpassingState = true;
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Jog Forward"))
                canMove = true;
            

        }

    }

    public void moveToBall() {
        Vector3 direction = new Vector3(ball.transform.position.x - transform.position.x, 0, 0);
        rb.MovePosition(transform.position + direction * Time.deltaTime * keepSpeed);

    }
    public void SetCanMove() {
        canMove = true;
    }
    public void PreventMove() {
        canMove = false;
    }
}
