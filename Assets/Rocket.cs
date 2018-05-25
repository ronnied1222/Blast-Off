using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    private Rigidbody rigidBody;
    private AudioSource audio;
    [SerializeField] float rcsThrust;
    [SerializeField] float mainThrust;

    private void Start() {
        rigidBody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        rcsThrust = 100f;
        mainThrust = 1200f;
    }

    // Update is called once per frame
    void Update () {
        Thrust();
        Rotate();
	}

    private void OnCollisionEnter(Collision collision) {
        switch(collision.gameObject.tag){
            case "Friendly":
                //do nothing
                break;
            case "Fuel":
                print("you got fuel");
                break;
            default:
                //die
                print("you died");
                break;
        }
    }

    private void Rotate() {
        rigidBody.freezeRotation = true; //take manual control of rotation

        float rotationThisFrame = Time.deltaTime * rcsThrust;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) { 
            transform.Rotate(Vector3.forward * rotationThisFrame);
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            transform.Rotate(Vector3.back * rotationThisFrame);
        } else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            transform.Rotate(Vector3.left * rotationThisFrame);
        } else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            transform.Rotate(Vector3.right * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //resume physics control 
    }

    private void Thrust() {
        float thrustThisFrame = mainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space)) {
            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
            if (!audio.isPlaying) {
                audio.Play();
            }
        }else {
            audio.Stop();
        }
    }
}
