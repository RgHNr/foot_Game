using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Opponent : MonoBehaviour, IResetPos
{
    // Start is called before the first frame update

    public enum OpponentState
    {
        Patrolling,
        chasing,
        Attacking

    }
    public Animator anim;

    public NavMeshAgent agent;

    public GameObject ball;

    public Rigidbody rb;

    bool shouldMove;

    public OpponentState currentState;

    public float ChaseRange,attackRange;

    float NextTackle;

    float tackleRate = 3f;


    float patrolRate = 0f;
    float nextPatrol;
    public bool isRunning ;
    

    public bool ballInchaseRange, ballInattackRange;
    public LayerMask whatIsBall;

    public Opponent[] teamOpponent;
    capsuleMove[] players;

    public bool tackling;

    public GameObject currentPatrollingPlayer;

    GameObject playerWithBall;   

    public float remainingDist;

    PositionZone positionZone;
    public GameObject MyZone;

    Vector3 startPos;

    public void resetPos()
    {
        transform.position = startPos;
    }
    void Start()
    {
        startPos = transform.position;
        teamOpponent = FindObjectsOfType<Opponent>().Where(t => t != this).ToArray();
        players = FindObjectsOfType<capsuleMove>().ToArray();
        shouldMove = true;
        positionZone = GetComponent<PositionZone>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!positionZone.checkZone() == MyZone) {

        //    agent.SetDestination(MyZone.transform.position);

        //    if (agent.remainingDistance <= agent.stoppingDistance)
        //    {
        //        isRunning = false;
        //    }
        //    else
        //        isRunning = true;


        //    anim.SetBool("run", isRunning);
            
        //}   

        ballInattackRange = Physics.CheckSphere(transform.position, attackRange,whatIsBall);
        ballInchaseRange = Physics.CheckSphere(transform.position, ChaseRange, whatIsBall);


        if (!ballInattackRange && !ballInchaseRange) currentState = OpponentState.Patrolling;
        if (!ballInattackRange && ballInchaseRange ) {

            if (teamOpponent.Any(t => t.currentState == OpponentState.chasing))
            {

                currentState = OpponentState.Patrolling;
            }
            else {
                currentState = OpponentState.chasing;
            }              

        }

        if (ballInattackRange && ballInchaseRange) currentState = OpponentState.Attacking;



        switch (currentState) {

            case OpponentState.Patrolling:
                patrolling();
                //Debug.Log("Patrolling");
                break;
            case OpponentState.chasing:
                chase();
                //Debug.Log("chasing");
                break;
            case OpponentState.Attacking:
                //Debug.Log("Attacking");
                
                Tackle();
                break;
             default:

                break;


        }

        
        //if(shouldMove)
        //    agent.SetDestination(ball.position);

        //if (agent.velocity != Vector3.zero) {

        //    anim.SetBool("run", true);
        //}

        //float dist = Vector3.Distance(transform.position, ball.position);
        //if (dist < 8)
        //{
        //    Debug.Log("distance: " + dist);
        //    if (dist < 5)
        //    {

        //        anim.SetTrigger("shoot");
        //        ball.GetComponent<SphereMove>().shootBall(Vector3.forward * 500);
        //        ball.GetComponent<SphereMove>().CurrentPlayer = null;
        //        ball.GetComponent<SphereMove>().InpassingState = true;
        //        agent.velocity = Vector3.zero;
        //        shouldMove = false;
        //        Debug.Log("shoot");

        //    }


        //}
        //else
        //    shouldMove = true;

        remainingDist = agent.remainingDistance;
    }


    public void Tackle() {



        if (Time.time > NextTackle) {

            NextTackle = Time.time + tackleRate;
            transform.LookAt(ball.transform);

            StartCoroutine(tackleCooldown());
            if (ball.GetComponent<SphereMove>().InpassingState) {

                anim.SetTrigger("shoot");

            }else   
                anim.SetTrigger("tackle");
        }
    }

    public void chase() {

        if (!shouldMove)
            return;

        agent.SetDestination(ball.transform.position);
        playerWithBall = ball.GetComponent<SphereMove>().CurrentPlayer;
        currentPatrollingPlayer = playerWithBall;
        //Debug.Log("remaingDistance: " + agent.remainingDistance);
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            isRunning = false;
        }
        else
            isRunning = true;
        anim.SetBool("run", isRunning );
    }


    public void patrolling() {
        if (!shouldMove)
            return;
        currentPatrollingPlayer = null;
        if (Time.time > nextPatrol) {
            nextPatrol = Time.time + patrolRate;
            //GameObject closestPlayer = ball.gameObject.GetComponent<SphereMove>().ClosestPlayer(transform.position);
           
            currentPatrollingPlayer = players[Random.Range(0,players.Length)].gameObject;
            currentPatrollingPlayer = GetPatrollingPlayer(teamOpponent, players);
            //Debug.Log(name + " pattrolling player is " + currentPatrollingPlayer.name);
            //if (teamOpponent.Any(t => t.currentPatrollingPlayer == currentPatrollingPlayer))
            //{

            //    closestPlayer = findNextPLayer(players.Where(t => t.gameObject != currentPatrollingPlayer).ToArray());
            //    if (agent.remainingDistance <= agent.stoppingDistance)
            //    {
            //        isRunning = false;
            //    }
            //    else
            //        isRunning = true;

            //}

            agent.SetDestination(currentPatrollingPlayer.transform.position + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f)));
           
        }
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            isRunning = false;
        }
        else
            isRunning = true;


        anim.SetBool("run", isRunning );
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name== "Sphere (1)") {

            //Debug.Log("opponent hit the ball");
            Vector3 direction = (ball.transform.position - transform.position).normalized;
            ball.gameObject.GetComponent<SphereMove>().shootBall(direction *45);

            //ball.GetComponent<SphereMove>().CurrentPlayer = null;
            ball.GetComponent<SphereMove>().InpassingState = true;

        }
    }


    IEnumerator tackleCooldown() {
        shouldMove = false;
        isRunning = false;
        yield return new WaitForSeconds(1.5f);
        shouldMove = true;
        isRunning = true;
    }

    public GameObject findNextPLayer(capsuleMove[] players) {
        GameObject neededPlayer = null;
        foreach (var p in players) {
            if (!teamOpponent.Any(t => t.GetComponent<Opponent>().currentPatrollingPlayer == p.gameObject)  || !teamOpponent.Any(t => t.GetComponent<Opponent>().playerWithBall==p.gameObject))
            {
                neededPlayer= p.gameObject;
                Debug.Log(name + " next player is " + neededPlayer.name);
            }
            
        }
        return neededPlayer ;

    }

    public GameObject GetPatrollingPlayer(Opponent[] teamMates,capsuleMove[] players) {
        GameObject NeededPlayer = null;
        foreach (var player in players) {
            if (!teamMates.Any(t => t.currentPatrollingPlayer == player.gameObject)) {

                NeededPlayer = player.gameObject;
                //Debug.Log(name + " next player is " + NeededPlayer.name);
            }
        }

        

        return NeededPlayer;
    
    }
    public void shootEventTest() {

        //Vector3 direction = (ball.transform.position - transform.position).normalized;
        //ball.gameObject.GetComponent<SphereMove>().shootBall(direction * 50);
        ////ball.GetComponent<SphereMove>().CurrentPlayer = null;
        //ball.GetComponent<SphereMove>().InpassingState = true;
    }
}
