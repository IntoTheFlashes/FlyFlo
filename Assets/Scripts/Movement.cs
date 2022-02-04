using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float relativeUpThrust = 2f;
    [SerializeField] float rotationThrust = 1f;
    [SerializeField] AudioClip mainThrusterAudio;

    [SerializeField] ParticleSystem mainThrusterParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;

    AudioSource audioSource;
    new Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessThrustInput();
        ProcessRotationInput();
    }

    void ProcessThrustInput ()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))
        {
            StartThrusting();
        }

        else
        {
            StopThrusting();
        }
    }

    void ProcessRotationInput()
    {
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            RotateLeft();
        }

        else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            RotateRight();
        }

        else
        {
            StopSideThrusterParticles();
        }
    }

    void StartThrusting()
    {
        rigidbody.AddRelativeForce(Vector3.up * relativeUpThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainThrusterAudio);
        }

        if (!mainThrusterParticles.isPlaying)
        {
            mainThrusterParticles.Play();
        }
    }

    void StopThrusting()
    {
        audioSource.Stop();
        mainThrusterParticles.Stop();
    }

    void RotateLeft()
    {
        ApplyRotation(rotationThrust);
        if (!rightThrusterParticles.isPlaying)
        {
            rightThrusterParticles.Play();
        }
    }

    void RotateRight()
    {
        ApplyRotation(-rotationThrust);
        if (!leftThrusterParticles.isPlaying)
        {
            leftThrusterParticles.Play();
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rigidbody.freezeRotation = true;    // freeze physics system to allow priority to manual input
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationThisFrame);
        rigidbody.freezeRotation = false;   // reactivate physics system
    }

        void StopSideThrusterParticles()
    {
        rightThrusterParticles.Stop();
        leftThrusterParticles.Stop();
    }
}
