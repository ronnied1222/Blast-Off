using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    private Rigidbody rigidBody;
    private AudioSource audioSource;
    [SerializeField] float rcsThrust;
    [SerializeField] float mainThrust;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip levelCompleteSound;
    [SerializeField] ParticleSystem thrustParticle;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem successParticle;
    [SerializeField] float levelLoadDelay = 2f;


    enum State { Alive, Dying, Transceding}
    State state = State.Alive;

    private void Start() {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        rcsThrust = 100f;
        mainThrust = 1200f;
    }

    // Update is called once per frame
    void Update () {
        if(state != State.Dying){
            Thrust();
            Rotate();
        }
	}

    private void OnCollisionEnter(Collision collision) {
        if(state != State.Alive && state != State.Transceding){
            return;
        }
        switch(collision.gameObject.tag){
            case "Friendly":
                //do nothing
                break;
            case "Finish":
                audioSource.Stop();
                audioSource.PlayOneShot(levelCompleteSound);
                successParticle.Play();
                state = State.Transceding;
                Invoke("LoadNextLevel", levelLoadDelay);
                break;
            default:
                //die
                audioSource.Stop();
                audioSource.PlayOneShot(deathSound);
                deathParticle.Play();
                state = State.Dying;
                Invoke("LoadFirstLevel", levelLoadDelay);

                break;
        }
    }

    private void LoadFirstLevel() {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel() {
        SceneManager.LoadScene(1);
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
            if (!audioSource.isPlaying) {
                audioSource.PlayOneShot(mainEngine);
            }
            thrustParticle.Play();
        }else {
            audioSource.Stop();
            thrustParticle.Stop();
        }
    }
}
