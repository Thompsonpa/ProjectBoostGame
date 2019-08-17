using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    Rigidbody RocketRB;
    AudioSource RocketAS;

    enum State { Alive, Dying, Transcending};
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
        if(state == State.Alive)
        {
            Thrust();
            Rotate();
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive)
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
                    state = State.Transcending;
                    Invoke("LoadNextLevel", 1f);
                    break;
                }
            default:
                {
                    state = State.Dying;
                    Invoke("LoadFirstLevel", 1f);
                    break;
                }
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
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
