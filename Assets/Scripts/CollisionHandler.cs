using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 0.4f;
    [SerializeField] AudioClip crashAudio;
    [SerializeField] AudioClip successAudio;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audioSource;

    bool collisionEnabled;
    bool isTransitioning = false;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        RespondToDebugKeys();
    }
    
    void OnCollisionEnter(Collision other) 
    {
        if (isTransitioning || collisionEnabled) { return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;

            case "Finish":
                freezeMotion();
                PlaySuccessEffects();
                Invoke("NextLevelDelay", levelLoadDelay);
                break;

            default:
                freezeMotion();
                PlayCrashEffects();
                Invoke("ReloadDelay", levelLoadDelay);
                break;
        }
    }

    void PlaySuccessEffects()
    {
        audioSource.PlayOneShot(successAudio);
        successParticles.Play();
    }

    void PlayCrashEffects()
    {
        audioSource.PlayOneShot(crashAudio);
        crashParticles.Play();
    }

    // Scene loading management
    // yes this invoking is terrible code, but the tutorial requires it
    void ReloadDelay()
    {
        SceneLoad(0);
    }

    void NextLevelDelay()
    {
        SceneLoad(1);
    }

    // back to semi-decent code
    void freezeMotion()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        GetComponent<Movement>().enabled = false;

        isTransitioning = true;
        rigidbody.useGravity = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        rigidbody.velocity = Vector3.zero;
        audioSource.Stop();
    }

    void SceneLoad(int sceneRequestModifier)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int requestedSceneIndex = currentSceneIndex + sceneRequestModifier;
        
        if (requestedSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            requestedSceneIndex = 0;
        }

        SceneManager.LoadScene(requestedSceneIndex);
    } 


    // Debug commands
    void RespondToDebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            SceneLoad(1);
        }

        else if (Input.GetKey(KeyCode.C))
        {
            toggleBoxCollider();
        }
    }

    void toggleBoxCollider()
    {
        collisionEnabled = !collisionEnabled;
    }
}
