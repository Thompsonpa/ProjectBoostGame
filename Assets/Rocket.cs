using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] bool collisionsDisabled = false;
    [SerializeField] AudioClip engine;
    [SerializeField] AudioClip sucess;
    [SerializeField] AudioClip death;
    [SerializeField] ParticleSystem engineParticles;
    [SerializeField] ParticleSystem sucessParticles;
    [SerializeField] ParticleSystem deathParticles;

    Rigidbody RocketRB;
    AudioSource RocketAS;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        RocketRB = GetComponent<Rigidbody>();
        RocketAS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }

        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }      
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || !collisionsDisabled) //Ignore Collisions
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                {

                    break;
                }
            case "Finish":
                {
                    RocketAS.Stop();
                    sucessParticles.Play();
                    RocketAS.PlayOneShot(sucess);
                    state = State.Transcending;
                    Invoke("LoadNextLevel", levelLoadDelay);
                    break;
                }
            default:
                {
                    RocketAS.Stop();
                    deathParticles.Play();
                    RocketAS.PlayOneShot(death);
                    state = State.Dying;
                    Invoke("LoadFirstLevel", levelLoadDelay);
                    break;
                }
        }
    }

    private void LoadNextLevel()
    {
        RocketAS.Stop();
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel()
    {
        RocketAS.Stop();
        SceneManager.LoadScene(0);
    }

    private void LoadSecoundLevel()
    {
        RocketAS.Stop();
        SceneManager.LoadScene(1);
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
                RocketAS.PlayOneShot(engine);
                engineParticles.Play();
            }
        }
        else
        {
            RocketAS.Stop();
            engineParticles.Stop();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKey(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene(1);
        }
    }
}
