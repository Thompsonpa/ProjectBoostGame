using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    Rigidbody RocketRB;
    AudioSource RocketAS;

    // Start is called before the first frame update
    void Start()
    {
        RocketRB = GetComponent<Rigidbody>();
        RocketAS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }
    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                {
                    print("Friendly");
                    break;
                }
            case "Fuel":
                {
                    print("Fuel");
                    break;
                }
            default:
                {
                    print("Hurts");
                    break;
                }
        }
    }

    private void Rotate()
    {
        RocketRB.freezeRotation = true; //take control of the rockets physics rotation
        
        float rotationSpeed = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {           
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }

        RocketRB.freezeRotation = false; //resume the physics control of the rockets rotation
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))
        {
            RocketRB.AddRelativeForce(Vector3.up * mainThrust);
            if (!RocketAS.isPlaying)
            {
                RocketAS.Play();
            }
        }
        else
        {
            RocketAS.Stop();
        }
    }
}
