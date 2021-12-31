using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class GunGuy : Agent
{
    [Header("Other")]
    public Rigidbody rb;
    public GameManager gameManager;

    [Header("Movin' and Groovin'")]
    [SerializeField]Transform orientation;
    [SerializeField]Transform groundCheck;
    [SerializeField]LayerMask groundMask;
    [SerializeField]float speed;
    [SerializeField]float movementMultiplier = 10;
    [SerializeField]float airMultiplier = 6;
    [SerializeField]Vector3 moveDirection;
    public float rotationY;
    public float rotationX;

    [Header("Jumpin' Jazz")]
    [SerializeField]float groundDistance = 1.2f;
    [SerializeField]float jumpForce = 500;
    [SerializeField]float gravity = 5;
    public bool isGrounded { get; private set; }
    [SerializeField]bool jumpNow;
    
    [Header("Rootin' and Shootin'")]
    public float damage = 1;
    public float range;
    public float fireRate;
    public float impactForce;
    public Camera cam;
    [SerializeField]bool shootNow;
    [SerializeField]float nextTimeToFire = 0;
    [SerializeField]ParticleSystem bang;
    [SerializeField]GameObject impactEffect;

    [Header("Dyin' and Cryin'")]
    [SerializeField]float health = 10;
    [SerializeField]int index;
    


    
    
    public override void OnEpisodeBegin()
    {
        index = Random.Range(0, gameManager.spawns.Length);
        transform.position = gameManager.spawns[index].position;
        health = 10;
    } 

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Awake()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f); 
    }
    
    
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position.x);
        sensor.AddObservation(transform.position.y);
        sensor.AddObservation(transform.position.z);
        sensor.AddObservation(isGrounded);
        sensor.AddObservation(cam.transform.rotation.x);
        sensor.AddObservation(transform.rotation.y);
        sensor.AddObservation(speed);
        
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float horizontalMovement = actions.ContinuousActions[0];
        float verticalMovement = actions.ContinuousActions[1];
        float myAss = actions.DiscreteActions[2];
        speed = actions.DiscreteActions[0];
        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
        if (myAss == 0 && isGrounded)
        {
            jumpNow = true;
        }
        if (actions.DiscreteActions[3] == 1)
        {
            shootNow = true;
        }
        rotationY = actions.DiscreteActions[1];
        rotationX = actions.ContinuousActions[2];
    }

    void FixedUpdate()
    {
        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * speed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * speed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
            rb.AddForce(Vector3.down * speed * gravity * rb.mass);
        }
        
    }

    void Update()
    {
          ControlDrag();
          isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
          transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x, Mathf.Lerp(transform.rotation.y, rotationY, Time.deltaTime * speed * 10), transform.rotation.z), Time.deltaTime * speed * 10);
          cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.Euler(Mathf.Lerp(cam.transform.rotation.x, rotationX * 90, Time.deltaTime * speed * 20), 180, cam.transform.rotation.z), Time.deltaTime * speed * 20);
          if (jumpNow)
          {
              Jump();
              jumpNow = false;
          }
          if (shootNow)
          {
              Shoot();
              shootNow = false;
          }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            
        }
    }

    void Shoot()
    {
        bang.Play();
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
        RaycastHit hit;
        Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.red, 1);
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            
            if (hit.collider.tag == "Guy")
            {
                Debug.Log(hit.transform.name);
                GunGuy gotcha = hit.transform.GetComponent<GunGuy>();
                if (gotcha.health > 0)
                {
                    gotcha.Hit();
                }
                else
                {
                    AddReward(2f);
                }
                AddReward(1f);
            }
            else
            {
                AddReward(-0.5f);
            }
        GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impactGO, 2f);
        }
        }
        
    }

    void ControlDrag()
    {
         if (isGrounded)
        {
            rb.drag = 6;
            rb.mass = 1;
        }
        else if (!isGrounded)
        {
            rb.drag = 0;
            rb.mass = 5;
        }
    }

    void Hit()
    {
        AddReward(-1f);
        health -= damage;
        if (health <= 0)
          {
              AddReward(-2f);
              EndEpisode();
              Destroy(gameObject);
          }
    }
}
