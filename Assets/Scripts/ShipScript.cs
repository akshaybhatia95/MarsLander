using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipScript : MonoBehaviour {
    private Rigidbody rb;
    public Vector3 forcesY = new Vector3(0,14,0);
    public Vector3 forcesX = new Vector3(12, 0, 0);
    public Vector3 forcesNegX = new Vector3(-12, 0, 0);
    public Vector3 rotValue = new Vector3(0,0,1);
    public GameObject deathParticle;
    public bool shipMoveFlag = true;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        //Physics.gravity = new Vector3(0, -4.0f, 0);
        
	}
	private void thurst()
    {
        if (shipMoveFlag == false)
        {
            return;
        }
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(forcesY);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(forcesX);
           // rb.AddTorque(-rotValue);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(forcesNegX);
            //rb.AddTorque(rotValue);
        }
    }
    private void rotate()
    {
        if (shipMoveFlag == false)
        {
            return;
        }

        if (Input.GetKey(KeyCode.D))
        {
           
             rb.AddTorque(-rotValue);
        }
        if (Input.GetKey(KeyCode.A))
        {
            
            rb.AddTorque(rotValue);
        }

    }
    private void limit()
    {
        Vector3 myVelocity = rb.velocity;
        int maxSpeed = 10;
        if (myVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            float myVelocityMagnitude = myVelocity.magnitude;
            rb.AddForce(myVelocity.normalized * (maxSpeed - myVelocityMagnitude), ForceMode.Impulse);
        }
    }
    // Update is called once per frame
    void Update () {

        thurst();
        rotate();
        limit();

        if (Input.GetKey(KeyCode.N) && Debug.isDebugBuild) {
            Utils.loadNextScene();
        }
        if (Input.GetKey(KeyCode.R) && Debug.isDebugBuild)
        {
            Utils.loadCurrentScene();
        }


    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("Ground Collision");
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Debug.Log("Collided with obstacle, restart level");
            StartCoroutine(killPlayer(3));
            //Utils.loadCurrentScene();
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("EndPad"))
        {
            Debug.Log("Reached end,load next level");
            Utils.loadNextScene();
            

        }
        



    }
    IEnumerator killPlayer(float seconds)
    {
        float currentTime = 0;
        shipMoveFlag = false;
        var dp = deathParticle.GetComponent<ParticleSystem>();
        rb.AddForce(0, 0, 0);
        

        dp.Play();
        Debug.Log("HERE IN KILL");
        while (currentTime < seconds)
        {
            Debug.Log("CO ROUTINE LOOP");
            currentTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("EXITING LOOP");
        dp.Stop();
        Utils.loadCurrentScene();



    }



}
