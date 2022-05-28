using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class capsuleMove : MonoBehaviour , IResetPos
{
    // Start is called before the first frame update

    public enum playerState{

        following,
        moving,
        getToBall,
        moveToPlayer,
        moveToGoal,
        moveAwayFromPlayers

    }

    public PlayerStats PlayerStat;
    public GameObject ball,goal;

    public  bool follow=true;

    [SerializeField]
    float[] playersDistances = new float[3];

    public SphereMove sphere;

    public Vector3 offset;

    float smoothAngle;
    Vector3 pos;

    public Animator anim;

    public GameObject selctedPlane,currentPlane;

    public GameObject[] players;

    public bool ballInRange , goalInRange;

    public float ballRange, goalRange;

    public LayerMask whatIsBall, whatIsGoal;

    public bool movingToBall;

    SphereMove myball;

    public playerState state;

    public capsuleMove[] myTeam;

    public bool moveToBallInRange;
    public float moveToballRange;

    public GameObject closePlayer=null;

    NavMeshAgent agent;
    bool closeToAPlayer;
    public float remainingDist;
    UIManager uIManager;

    Renderer render;
    Transform cam;

    public int index;
    float health, maxHealth=100f;

    public Vector3 startPos;
    public TextMesh backNumber;
    public OwnedPlayers ownedPlayers;
    public Material Player_Mat;
    public void resetPos()
    {
        //follow = false;

        //if (index == 0) {
        //    follow = true;
        //}
       
        
        transform.position = ownedPlayers.myFormation.PlayersPositions[index];
        startPos = transform.position;
    }

    void Start()
    {
        Player_Mat.SetTexture("_BaseMap", ownedPlayers.OwnedTextures[ownedPlayers.currentTextureID]);
        transform.position = ownedPlayers.myFormation.PlayersPositions[index];
        backNumber.text = PlayerStat.number.ToString();
        backNumber.color = ownedPlayers.NumberColor;
        startPos =ownedPlayers.myFormation.PlayersPositions[index];
        myTeam = FindObjectsOfType<capsuleMove>().Where( t => t != this).ToArray();
        myball = ball.GetComponent<SphereMove>();
        agent = GetComponent<NavMeshAgent>();
        uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        render = GetComponent<Renderer>();
        health = maxHealth;
        cam = Camera.main.transform;

        
    }

    

    // Update is called once per frame
    void Update()
    {
        checkClosestPlayer(myTeam);
        
        
        //if (follow)
        //{

        //    movingToBall = false;
        //    currentPlane.SetActive(true);
        //    Vector3 playerToBall = (ball.transform.position - transform.position).normalized;
        //    transform.position = Vector3.Lerp(transform.position, ball.transform.position + (playerToBall*-1f), 0.2f);
        //    if (sphere.GetDirection() != Vector3.zero)
        //    {
        //        anim.SetBool("run", true);
        //        pos = ball.transform.position - sphere.GetDirection();
        //        transform.position = Vector3.Lerp(transform.position, pos+new Vector3(0,0,0.5f), 0.5f);
        //        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, sphere.getAngle(), ref smoothAngle, 0.1f);
        //        transform.rotation = Quaternion.Euler(0, angle, 0);
        //    }
        //    else
        //    {
        //        anim.SetBool("run", false);
        //    }
        //}
        //else
        //{
        //    checkballPos(ball, goal);
        //    currentPlane.SetActive(false);
        //    //anim.SetBool("run", false);
        //}

        moveToBallInRange = Physics.CheckSphere(transform.position, moveToballRange, whatIsBall);
        ballInRange = Physics.CheckSphere(transform.position, ballRange, whatIsBall);
        goalInRange = Physics.CheckSphere(transform.position, goalRange, whatIsGoal);

        
        if (follow) state = playerState.following;
        if (!follow && !movingToBall && !myball.InpassingState) state = playerState.moving;
        if (!follow  && myball.InpassingState) {


            state = playerState.getToBall;

        }
        if (!moveToBallInRange && !follow && !ballInRange && !myball.InpassingState) state = playerState.moveToPlayer;
        if (!follow && closePlayer != null && !myball.InpassingState)
        {

            state = playerState.moveAwayFromPlayers;
        }


        switch (state) {

            case playerState.following:
                StopAllCoroutines();
                agent.enabled = false;
                currentPlane.SetActive(true);
                movingToBall = false;
                Vector3 playerToBall = (ball.transform.position - transform.position).normalized;
                //transform.position = Vector3.Lerp(transform.position, ball.transform.position + (playerToBall * -1f), 10f*Time.deltaTime);
                if (sphere.GetDirection() != Vector3.zero)
                {
                    anim.SetBool("run", true);
                    pos = ball.transform.position - sphere.GetDirection();
                    transform.position = Vector3.Lerp(transform.position, pos + new Vector3(0, 0, 0.5f), 0.5f);
                    
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, sphere.getAngle(), ref smoothAngle, 0.1f);
                    transform.rotation = Quaternion.Euler(0, angle, 0);
                    //runDamage();
                }
                else
                {
                    anim.SetBool("run", false);
                    //fillRun();
                }

                uIManager.runHealth(health, maxHealth);
                break;
            case playerState.moving:
                fillRun();
               
                if (closeToAPlayer)
                    return;
                agent.enabled = true;

                

                if (ballInRange &&  !myball.InpassingState) {
                    //transform.position = Vector3.Lerp(transform.position, moveToPostion(ball,-1), 2f*Time.deltaTime);
                    //agent.SetDestination(moveToPostion(ball, -1));
                    //transform.LookAt(moveToPostion(ball,-1));
                    //checkAnimation();
                    StartCoroutine( movePlayerTowards(ball.transform.position, -1));
                }

             
                else
                    anim.SetBool("run", false);

                checkAnimation();
                break;
            case playerState.getToBall:
                //Debug.Log(this.name + " is going to the ball");
                agent.enabled = true;
                fillRun();
              

                //goToBall(ball.transform.position);
                checkAnimation();

                

                break;

            case playerState.moveToPlayer:
                fillRun();
                
                //if (closeToAPlayer)
                //    return;

                //agent.SetDestination(moveToPostion(ball, 1));
                //transform.LookAt(moveToPostion(ball, 1));
                //checkAnimation();
                agent.enabled = true;
                StartCoroutine( movePlayerTowards(ball.transform.position+new Vector3(Random.Range(7,12),0, Random.Range(7, 12)), 1));
                break;

            case playerState.moveAwayFromPlayers:
                
               fillRun();
                //agent.SetDestination(moveToPostion(closePlayer, -1));
                //transform.LookAt(moveToPostion(closePlayer,-1));
                //checkAnimation();
                agent.enabled = true;
                if (closePlayer != null) {
                    StartCoroutine(movePlayerTowards(closePlayer.transform.position + new Vector3(Random.Range(7, 12), 0, Random.Range(7, 12)), -1));
                }
                checkAnimation();
                
                break;
                
        }
        

        if (transform.position.y != 0) {

            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }

        //Debug.Log(name + "current state is " + state);
        if (agent.isActiveAndEnabled) {
            remainingDist = agent.remainingDistance;
        }
    }
    private void FixedUpdate()
    {
        if (state == playerState.following) {
            if (sphere.GetDirection() != Vector3.zero)
            {
                runDamage();
            }
            else
                fillRun();
        }
    }
    public void DoNotFollow() {
        follow = false;
    }

    public void goToBall(Vector3 destination) {
        if (agent.enabled == false)
            return;
        movingToBall = true;
        //transform.LookAt(destination);
        //transform.position = Vector3.MoveTowards(transform.position, destination, 0.3f);
        agent.SetDestination(destination);
        anim.SetBool("run", true);

    }
    public void shootAnim() {
        anim.SetTrigger("shoot");
    }

    public void passAnim() {
        anim.SetTrigger("pass");
    }

    public void shootRotation(Vector3 dir) {

        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float targetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref smoothAngle, 0.05f);
        transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        


    }

    //public void checkballPos(GameObject ball, GameObject goal) {
        

    //    if (movingToBall)
    //        return;

    //    if (ballInRange && !goalInRange) {
    //        Vector3 dir = (goal.transform.position - transform.position).normalized;
    //        transform.LookAt(transform.position + dir);
    //        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir * Random.Range(1f, 10f), 5f*Time.deltaTime);
    //        anim.SetBool("run", true);
    //    }
    //    if (!ballInRange && goalInRange)
    //    {      
    //        Vector3 dir = (ball.transform.position - transform.position).normalized;
    //        transform.LookAt(transform.position + dir);
    //        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir * Random.Range(1f, 10f), 5f*Time.deltaTime);
    //        anim.SetBool("run", true);
    //    }
        
        
    //}

    public Vector3 moveToPostion(Vector3 obj, int multi) {

       
        float rand = Random.Range(1f, 5f);
        Vector3 goalDir = (goal.transform.position - transform.position).normalized;
        Vector3 dir = (obj - transform.position).normalized;
        Vector3 NewPos = transform.position + multi*dir * rand  ;
        return (NewPos+goalDir);
    }

    public void checkClosestPlayer(capsuleMove[] myTeam) {


        for (int i = 0; i < myTeam.Length; i++) {

            float distance = Vector3.Distance(transform.position, myTeam[i].transform.position);

            playersDistances[i] = distance;

            if (distance < 20)
            {
                closeToAPlayer = true;
                //Debug.Log(name + " closest player is " + myTeam[i].gameObject.name);
                closePlayer = myTeam[i].gameObject;
                return;
            }
            else
            {
                closeToAPlayer = false;
                closePlayer = null;
            
            }
                

        }

    }

    public void checkAnimation() {
        if (agent.remainingDistance != 0) {

            anim.SetBool("run", true);
        }
        else
            anim.SetBool("run", false);
    }


    IEnumerator movePlayerTowards(Vector3 obj, int multi) {

        
        yield return new WaitForSeconds(0.2f);
        Vector3 newDir = moveToPostion(obj, multi);
        agent.SetDestination(newDir);
        //transform.LookAt(newDir);
        checkAnimation();

        
    }

    public void runDamage()
    {

        if (health > 2)
        {

            health -= 0.2f;

        }
    }

    public void fillRun()
    {
        if (health < maxHealth)
        {

            health += 0.2f;
        }

    }

    public float getHealth() => health;

    public void shootEventTest() {

        myball.shootBall();
    }

    //public void visibility() {

    //    if (render.isVisible)
    //    {
    //        //Debug.Log(name + " is visible. ");
    //        uIManager.disbaleImage(index);
    //    }
    //    else {
    //        //Debug.Log(name + " is not visible. ");

    //        if (transform.position.x > cam.position.x)
    //        {
    //            uIManager.playerImageVisual(index, projectRay().x, projectRay().y, 1);
    //        }
    //        else if (transform.position.x < cam.position.x) {
    //            uIManager.playerImageVisual(index, projectRay().x, projectRay().y, -1);
    //        }
            
    //    }
    //}

    public Vector2 projectRay() {
        Vector2 dir = new Vector2(cam.position.x - transform.position.x, cam.position.z - transform.position.z).normalized;

        return dir;
    }

    public Vector3 GetRoamingDirection() {
        Vector3 CurrentDirection=sphere.GetDirection();
        return Vector3.zero;
    
    }
}
