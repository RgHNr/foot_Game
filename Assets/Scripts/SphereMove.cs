using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;



public class SphereMove : MonoBehaviour, IResetPos
{
    public static bool IsInGoal;
    public static bool isPaused;
    // Start is called before the first frame update
    public float speed;
    public float force;

    public GameObject[] MyPlayers= new GameObject[4];

    public GameObject goal;

    public GameObject CurrentPlayer;
    
    [SerializeField]
    private Camera cam;
    public float shotPower;
    [SerializeField]
    private LayerMask layer;
    private Vector3 direction;

    private float BallDistance;

    public Rigidbody rb;

    float Power;

    public capsuleMove capsule;

    float targetAngle;

    public FixedJoystick joyStick;

    public Plane plane;

    Vector2 firstPostion, currentPos;

    bool shouldMove=true;

    Vector3 shootingDirrection;

    float dragCounter;
    float clampedDrag;

    public LineRenderer lineRenderer;
    public GameObject line;

    private GameObject dragLine;
    public Material SelectedMaterial;
    public Material defultMat;

    public bool InpassingState;

    public bool freeball;

    public Volume MyVolume;

    private Vignette vignette;
    private PaniniProjection PaniniProjection;
    private ChromaticAberration chromatic;
    private ChannelMixer channelMixer;
    private FilmGrain filmGrain;
    private float volumeTime = 5f;
    public bool changeVolume;

    bool shooting;

    TrailRenderer trail;

    public int numberOfplayerFollowing;
    public GameObject camState;
    public CameraBlend camBlend;

    public float curvePower;
    public float curveDirection;

    public UIManager uIManager;

    bool curveChanigng;

    Vector3 startPos ;
    bool timeToNormal;
    public bool isButtonClick;
    public void GetCurveTouching() {
        curveChanigng = true;
    }

    public void resetPos() {

        if (InpassingState)
            transform.position = startPos;
        else
            transform.position = CurrentPlayer.GetComponent<capsuleMove>().startPos+new Vector3(0,0,1);
    }
   
    void Start()
    {
        isPaused = false;
        GetcurrentPlayer();
        transform.position= CurrentPlayer.GetComponent<capsuleMove>().transform.position + new Vector3(0, 0, 1);
        startPos = transform.position;

        
        trail = GetComponent<TrailRenderer>();
        
        GetcurrentPlayer();

        MyVolume.profile.TryGet<PaniniProjection>(out PaniniProjection);

        camBlend = camState.GetComponent<CameraBlend>();

        MyVolume.profile.TryGet<Vignette>(out vignette);
    
        MyVolume.profile.TryGet<ChromaticAberration>(out chromatic);
        
        MyVolume.profile.TryGet<ChannelMixer>(out channelMixer);
     
        MyVolume.profile.TryGet<FilmGrain>(out filmGrain);
      

        
    }

    // Update is called once per frame
    void Update()
    {
        
        //if (Input.GetMouseButtonDown(0))
        //{
        //    time += Time.deltaTime;
        //}

        //for (int i = 0; i <Input.touchCount ; i++)
        //{
        //    Ray TouchPos = Camera.main.ScreenPointToRay(Input.touches[i].position);

        //    if (plane.Raycast(TouchPos, out float hit))
        //    {
        //        //Find the world pos of our screen point on the plane
        //        Vector3 hitPos = TouchPos.GetPoint(hit);
        //        Debug.DrawLine(transform.position, hitPos, Color.red);
        //        Debug.Log(hitPos);
        //    }

        //}
        //vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 15, 0.05f * Time.deltaTime);
        checkFreeBall();
        GetcurrentPlayer();

        if (changeVolume)
        {
            editVolume();
        }
        else if (changeVolume == false) {
            UneditVolume();
        }

        if (!InpassingState) {

            var followingPlayers = MyPlayers.Where(t => t.GetComponent<capsuleMove>().follow == true).ToArray();
            //Debug.Log("number of following players= " + followingPlayers.Length);
            if (followingPlayers.Length > 1) {
                followingPlayers[0].GetComponent<capsuleMove>().follow = false;
            }
        }

        if (InpassingState) {
            //trail.enabled = true;
            //trail.time = 1f;
            StartCoroutine(closePlayerFollow());
            CamShake.Instance.StopCameraShake();
            if (rb.velocity.magnitude <= 1 && rb.position.y<1) {
                
                rb.velocity = Vector3.zero;
            }

            foreach (var player in MyPlayers.Where(t => t != CurrentPlayer)) {
                float dist = Vector3.Distance(transform.position, player.transform.position);
                if (dist < 3) {
                    //rb.velocity = Vector3.zero;
                    rb.velocity=new Vector3(0,rb.velocity.y,0);

                    player.GetComponent<capsuleMove>().follow = true;
                    shouldMove = true;
                    InpassingState = false;
                    shooting = false;
                }
            }
        }
        
        if (Input.touchCount > 0) {

            //Touch touch2 = Input.GetTouch(1);
            //Debug.Log("touch 2 postion: "+touch2.position);
            
            Touch touch = Input.GetTouch(0);
            
            if (Input.touchCount > 1 && direction!= Vector3.zero) {

                touch = Input.GetTouch(1);
                //Debug.Log("there is 2 fingers touching");
                if (Input.GetTouch(1).phase == TouchPhase.Began && direction != Vector3.zero)
                {
                    firstPostion = Input.GetTouch(1).position;
                }
            }


            //Debug.Log("input count: " + Input.touchCount);
            if (touch.phase == TouchPhase.Began && direction==Vector3.zero && !curveChanigng) {
                firstPostion = touch.position;
                //Debug.Log("First Pos: " + firstPostion);
                

            }
            

            if (touch.phase == TouchPhase.Moved) {
                //Debug.Log("fingerId: "+ touch.fingerId);
                isButtonClick = false;

                if ( !InpassingState && !shooting && !curveChanigng && !isPaused) {

                    if (joyStick.Direction != Vector2.zero && touch.fingerId == 0)
                        return;

                    
                    if (dragLine != null) {
                        Destroy(dragLine);
                    }
                    changeVolume = true;
                    
                    currentPos = touch.position;
                    //Debug.Log("currentPos: " + currentPos);



                    Vector2 lastpos = currentPos-touch.deltaPosition;
                    //Debug.Log("last pos: " + lastpos);

                    float dist1 = Vector2.Distance(firstPostion, currentPos);
                    float dist2 = Vector2.Distance(firstPostion, lastpos);

                    if (dist1 > dist2)
                    {
                        dragCounter += 0.5f;
                    }
                    else if (dist2 > dist1) {

                        dragCounter -= 0.5f;
                    }

                    //Debug.Log("dragCounter: " + dragCounter);
                    
                    clampedDrag = Mathf.Clamp(dragCounter, 0f, 18f);
                    shootingDirrection = new Vector3(currentPos.x - firstPostion.x, 0, currentPos.y - firstPostion.y).normalized;
                    //Debug.Log(shootingDirrection);
                    
                    //Debug.DrawLine(transform.position, shootingDirrection * 1,Color.red);
                    Debug.DrawRay(transform.position, -shootingDirrection * dragCounter, Color.red);
                    dragLine = Instantiate(line, transform.position, Quaternion.identity);
                    //if (clampedDrag > 7 && clampedDrag < 12)
                    //{
                    //    dragLine.GetComponent<LineRenderer>().material.SetColor("Color_9dc96495698e4784863b966ec1bb8e28", Color.yellow);
                    //}
                    //else if (clampedDrag > 12) {

                    //    dragLine.GetComponent<LineRenderer>().material.SetColor("Color_9dc96495698e4784863b966ec1bb8e28", Color.red);
                    //}else
                    //    dragLine.GetComponent<LineRenderer>().material.SetColor("Color_9dc96495698e4784863b966ec1bb8e28", Color.blue);
                    //dragLine.GetComponent<LineRenderer>().SetPosition(0, -shootingDirrection * clampedDrag+new Vector3(0,0.5f,0));
                    dragLine.GetComponent<LineRenderer>().SetPosition(0, Vector3.Lerp(dragLine.GetComponent<LineRenderer>().GetPosition(0), -shootingDirrection * clampedDrag + new Vector3(0, 0.5f, 0), 100* Time.unscaledDeltaTime));
                    ChangeColor(getTargetPlayer(-shootingDirrection));

                }

                


            }
            if (touch.phase == TouchPhase.Ended && !isButtonClick && !isPaused ) {

                Debug.Log("touch id= " + touch.fingerId);
                Debug.Log("Direction= " + direction);
                Debug.Log("joystic direction= " + joyStick.Direction);
                

                if (InpassingState == false && !curveChanigng)
                {
                    float angleToGoal = Vector3.SignedAngle(-shootingDirrection, goal.transform.position - transform.position,Vector3.up);
                    Debug.Log("the angle is " + angleToGoal);
                    checkCurveDirection();    
                    if (direction == Vector3.zero)
                    {
                        //shootBall();
                        //CurrentPlayer.transform.LookAt(Vector3.Lerp(CurrentPlayer.transform.forward, -shootingDirrection, 0.5f * Time.deltaTime));

                        //StartCoroutine(shootDelay(CurrentPlayer));

                        startShootAnim(CurrentPlayer);

                        changeVolume = false;

                        Debug.Log("shooooot");
                        noSelection();
                        //dragLine.GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
                        Destroy(dragLine);

                    }

                    if (touch.fingerId == 1)
                    {

                        direction = Vector3.zero;
                        //CurrentPlayer.transform.LookAt(Vector3.Lerp(CurrentPlayer.transform.forward, -shootingDirrection, 0.5f * Time.deltaTime));

                        //StartCoroutine(shootDelay(CurrentPlayer));
                        startShootAnim(CurrentPlayer);

                        changeVolume = false;

                        Debug.Log("shooooot");
                        noSelection();
                        //dragLine.GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
                        Destroy(dragLine);
                    }
                }


                if (line != null) {
                    Destroy(dragLine);
                    noSelection();
                }
                changeVolume = false;
                curveChanigng = false;
            }

        }
        
        //Debug.Log(rb.velocity.magnitude);
    }

    
    private void FixedUpdate()
    {

        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");
        float horizontal = joyStick.Horizontal;
        float vertical = joyStick.Vertical;
        direction = new Vector3(horizontal, 0, vertical).normalized;
        //rb.AddForce(direction * speed * Time.deltaTime,ForceMode.VelocityChange);
        if (shouldMove) {
            //trail.enabled = false;
            //trail.time = 0.05f;
            if (CurrentPlayer != null) {

                if (CurrentPlayer.GetComponent<capsuleMove>().getHealth() > 2)
                {
                    speed = 600;
                    CurrentPlayer.GetComponent<capsuleMove>().anim.speed = 1f;
                }


                else
                {
                    speed = 300;
                    CurrentPlayer.GetComponent<capsuleMove>().anim.speed = 0.7f;

                }
            }
            
            rb.velocity = direction * speed * Time.deltaTime;
            
            if (rb.velocity.magnitude > 0.1)
            {

                CamShake.Instance.shakeCamera();

            }else
                CamShake.Instance.StopCameraShake();

            if (transform.position.y >= 0.33)
            {
                rb.velocity += new Vector3(0, -6, 0);
            }
        }
        targetAngle = Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg;
        
        float distance = Vector3.Distance(goal.transform.position, transform.position);
        //Debug.DrawLine(ball.transform.position, goal.transform.position,Color.red);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ball.transform.position, ray.direction * distance, Color.blue);

        BallDistance = Vector3.Distance(transform.position, transform.position);

        //ball.gameObject.GetComponent<Rigidbody>().velocity = ray.direction * shotPower;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance + 100f, layer))
        {
            //draw invisible ray cast/vector
            Debug.DrawLine(transform.position, hit.point, Color.yellow);
            //log hit area to the console


        }


    }

    public Vector3 GetDirection() {

        return direction;
    }

    public float getAngle() {
        return targetAngle;
    }

    public void shootBall(Vector3 direction) {
        if (CurrentPlayer != null) {
            CurrentPlayer.GetComponent<capsuleMove>().DoNotFollow();
            CurrentPlayer.GetComponent<capsuleMove>().shootRotation(direction);
        }
            

        shouldMove = false;
        rb.velocity = direction;
        InpassingState = true;
        
        //rb.AddForce(direction);
    }

    private Vector3 DirectionTo(GameObject Player) {
        return Vector3.Normalize(Player.transform.position - transform.position);
    }

    public GameObject getTargetPlayer(Vector3 direction) {

        GameObject selectedPlayer = null;
        float angle = Mathf.Infinity;
        foreach (var player in MyPlayers) {

            float PlayerAngle = Vector3.Angle(direction, DirectionTo(player));

            if (PlayerAngle < angle) {
                selectedPlayer = player;
                angle = PlayerAngle;
            }

        }
        return selectedPlayer;
    }

    public void ChangeColor(GameObject player) {

        if (player == CurrentPlayer)
            return;
        player.GetComponent<capsuleMove>().selctedPlane.SetActive(true);
        foreach (var other in MyPlayers.Where(t  => t != player)) {

            other.GetComponent<capsuleMove>().selctedPlane.SetActive(false);

        }
    }

    public void noSelection()
    {

        foreach (var player in MyPlayers) {
            player.GetComponent<capsuleMove>().selctedPlane.SetActive(false);
        }
    }
    public void GetcurrentPlayer() {

        foreach (var player in MyPlayers) {

            if (player.GetComponent<capsuleMove>().follow) {

                CurrentPlayer = player;
            }
        }
    }

    public void checkFreeBall() {

        
        if(InpassingState )
        {
            if ( rb.velocity.magnitude <=2f) {

                CurrentPlayer = null;

                ClosestPlayer(transform.position).GetComponent<capsuleMove>().goToBall(transform.position);
                if (CurrentPlayer == ClosestPlayer(transform.position)) {
                    CurrentPlayer = null;
                }
            }
        }
    }

    public GameObject ClosestPlayer(Vector3 Mypos) {
        GameObject neededPlayer = null;
        float minDist=Mathf.Infinity;
        foreach (var player in MyPlayers) {
            float dist = Vector3.Distance(Mypos, player.transform.position);

            if (dist < minDist) {
                neededPlayer = player;
                minDist = dist;
            }
        }
        return neededPlayer;
    }

    //IEnumerator shootDelay(GameObject player) {

    //    player.GetComponent<capsuleMove>().shootAnim();
    //    shouldMove = false;
    //    shooting = true;
    //    yield return new WaitForSeconds(0.3f);
    //    shootBall(-shootingDirrection * shotPower * 4 * Mathf.Clamp(dragCounter,0f,15f)-new Vector3(0,10,0));
    //    //Debug.Log("power: " + shotPower * 5 * dragCounter);
    //    InpassingState = true;
    //    dragCounter = 0;
    //    StartCoroutine(closePlayerFollow());
    //}

    public void startShootAnim(GameObject player) {
        player.GetComponent<capsuleMove>().shootAnim();
        shouldMove = false;
        shooting = true;
    }

    public void shootBall() {

        if (InpassingState) {
            return;
        }
        shootBall(-shootingDirrection * shotPower * 4 * Mathf.Clamp(dragCounter, 0f, 15f) - new Vector3(0, clampedDrag*1f, 0));
        StartCoroutine(curve());
        //Debug.Log("power: " + shotPower * 5 * dragCounter);
        InpassingState = true;
        //camBlend.switchState();
        dragCounter = 0;
        StartCoroutine(closePlayerFollow());
    }


    public void secondTouch(Touch touch)
    {

       

    }

    IEnumerator closePlayerFollow() {
        yield return new WaitForSeconds(0.1f);
        capsuleMove closePlayer = ClosestPlayer(transform.position).GetComponent<capsuleMove>();
        closePlayer.state = capsuleMove.playerState.getToBall;
        closePlayer.goToBall(transform.position);

    }

    IEnumerator curve()
    {

        float timePassed = 0;
        while (timePassed <= 1.5f)
        {
            curvePower = uIManager.getCurvePower();
            rb.AddForce(curvePower * Vector3.Cross(rb.velocity, Vector3.down*curveDirection), ForceMode.Force);
            timePassed += Time.deltaTime;

            yield return null;
        }
    }

    public void checkCurveDirection() {

        float angleToGoal = Vector3.SignedAngle(-shootingDirrection, goal.transform.position - transform.position, Vector3.up);
        if (Mathf.Abs(angleToGoal) <= 5)
        {

            curveDirection = 0;
        }
        else if (angleToGoal > 5)
        {

            curveDirection = 1;

        }
        else if (angleToGoal < -5) {
            curveDirection = -1;
        }

    }
    public void editVolume() {
        TimeManager.doSlowmotion();
        timeToNormal = true;
        PaniniProjection.distance.value = Mathf.Lerp(PaniniProjection.distance.value, 0.2f, volumeTime*Time.unscaledDeltaTime);
        
        vignette.intensity.value= Mathf.Lerp(vignette.intensity.value, 0.30f, volumeTime * Time.unscaledDeltaTime);
       
        chromatic.intensity.value= Mathf.Lerp(chromatic.intensity.value, 1f, volumeTime * Time.unscaledDeltaTime);
        
        channelMixer.redOutGreenIn.value= Mathf.Lerp(channelMixer.redOutGreenIn.value, 25f, volumeTime * Time.unscaledDeltaTime);
        
        filmGrain.intensity.value= Mathf.Lerp(filmGrain.intensity.value, 0.3f, volumeTime * Time.unscaledDeltaTime);

    }

    public void UneditVolume() {
        if(timeToNormal)
            TimeManager.stopSlowmotion();

        timeToNormal = false;
        PaniniProjection.distance.value = Mathf.Lerp(PaniniProjection.distance.value, 0f, volumeTime * Time.unscaledDeltaTime);

        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0f, volumeTime* Time.unscaledDeltaTime);

        chromatic.intensity.value = Mathf.Lerp(chromatic.intensity.value, 0f, volumeTime * Time.unscaledDeltaTime);

        channelMixer.redOutGreenIn.value = Mathf.Lerp(channelMixer.redOutGreenIn.value, 0, volumeTime * Time.unscaledDeltaTime);

        filmGrain.intensity.value = Mathf.Lerp(filmGrain.intensity.value, 0f, volumeTime * Time.unscaledDeltaTime);
    }
}
