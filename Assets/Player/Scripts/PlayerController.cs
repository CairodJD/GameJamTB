using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class PlayerController : MonoBehaviour {

    public Transform Cam;
    public float movespeed = 3.5f;
    public float turnSmoothTime = 0.1f;
    public float rotationSpeed = 0.5f;
    float turnSmoothVelocity;
    CharacterController controller;
    NavMeshAgent agent;
    int defaultLayer;
    Transform skin;
    Rigidbody rigidbody;
    private static readonly string hiddenLayer = "Dig";
    private static readonly string endLayer = "Ending";

    AudioSource source;
    public AudioClip[] clips;
    // 1 dril
    // 2 dril out

    private void Awake() {
        //rigidbody = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        defaultLayer = gameObject.layer;
        skin = transform.GetChild(0);
    }

    private void Start() {
        //agent.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update() {     
        //HIDING or na
        if (Input.GetKey(KeyCode.Mouse0)) {

            if (agent.enabled == true) {
                if (IsOn(hiddenLayer)) {
                    //Debug.Log(" je me cache / animation de creusage tu sais deja");
                    Hidding();
                } else if (IsOn(endLayer)) {
                    GameManager.instance.NextLevel();
                }
               
            } 
            
        } else {
            //Debug.Log("JETAIS CACHE");
            NotHidding();
        }

     
        Moovement();
       
       
    }


    void Moovement() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertital = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(horizontal, 0f, vertital).normalized;
        Walk(dir.magnitude);

        if (dir.magnitude >= 0.1f && !isInState(SPROUT)) {
           
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + Cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0, angle, 0);
            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * movespeed * Time.deltaTime);
        }
        controller.Move(Physics.gravity);
    }

    public void setAgent(bool active = true) {
        agent.enabled = active;
    }
    
    // Is mouse currently on "area"
    public bool IsOn(string area) {
        return getCurrentArea() == (1 << NavMesh.GetAreaFromName(area));
    }

    //return current mouse Area
    public int getCurrentArea() {
        NavMeshHit hit;
        agent.SamplePathPosition(agent.areaMask, 0.01f, out hit);
        return hit.mask;
    }

    void Hidding() {
        Dig();
        controller.detectCollisions = false;
        gameObject.layer = (1 << NavMesh.GetAreaFromName(hiddenLayer));
    }
    void NotHidding() {
        Dig(false);
        controller.detectCollisions = true;
        gameObject.layer = defaultLayer;
    }

    //remove renderer at the end of anim
    public void DigRendererEvent(int tkt) {
        skin.gameObject.SetActive(Convert.ToBoolean(tkt));
    }

    #region AnimationControler

    Animator animator;
    static readonly string WALK  = "WALK";
    static readonly string DIG   = "DIG";
    static readonly string SPROUT = "SPROUT";
    public float trans = 3f;

  
  

    void Walk(float speed) {
        animator.SetFloat(WALK, speed);
    }

    void Dig(bool dig = true) {
        animator.SetBool(DIG,dig);
       
    }

    public void playDrill(bool drillin = true) {
        AudioClip clip = drillin == true ? clips[0] : clips[1];
        source.loop = false;
        source.PlayOneShot(clip);
    }

    public void playDirt(bool drillin = true) {
        AudioClip clip = drillin == true ? clips[2] : clips[3];
        source.loop = false;
        source.PlayOneShot(clip);
    }

    public bool isInState(string state) {

        if (animator.GetCurrentAnimatorStateInfo(0).IsName(state)) {
            return true;
        }

        return false;
    }

    #endregion
}

