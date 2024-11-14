using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipControler : MonoBehaviour
{
    public CinemachineCamera Cam;
    private CinemachineBasicMultiChannelPerlin noise;
    public float forwardSpeed = 10f;
    public float strafeSpeed = 5f;
    public float howerSpeed = 4f;

    private float modifier;
    public float boost = 1.5f;
    public bool FOV;
    public float normalFOV = 60f;
    public float changedFOV = 80f;
    public ParticleSystem speedEffect;

    public float forwardAcceleration = 2.5f;
    public float strafeAcceleration = 2f;
    public float howerAcceleration = 2f;

    public float RotationSpeed = 90f;
    private Vector2 CursorInput;
    private Vector2 Center;
    private Vector2 CursorDistance;

    private float rollInput;
    public float rollSpeed = 90f;
    public float rollAcceleration = 3.5f;

    // To store the player's input
    private float horizontalInput;
    private float verticalInput;
    private float anotherInput;

    void Start()
    {
        Center.x = Screen.width * 0.5f;
        Center.y = Screen.height * 0.5f;

        modifier = 1f;

        Cam.Lens.FieldOfView = normalFOV;
        FOV = false;

        noise = Cam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise != null)
        noise.FrequencyGain = 0;  // Shake

        Cursor.lockState = CursorLockMode.Confined;
    }
    // Update is called once per frame
    void Update()
    {
        CursorInput.x = Input.mousePosition.x;
        CursorInput.y = Input.mousePosition.y;

        CursorDistance.x = (CursorInput.x - Center.x) / Center.y;
        CursorDistance.y = (CursorInput.y - Center.y) / Center.y;

        CursorDistance = Vector2.ClampMagnitude(CursorDistance, 1f);

        rollInput = Mathf.Lerp(rollInput, Input.GetAxis("Roll"), rollAcceleration * Time.deltaTime);

        transform.Rotate(-CursorDistance.y * RotationSpeed * Time.deltaTime, CursorDistance.x * RotationSpeed * Time.deltaTime, rollInput * rollSpeed * Time.deltaTime, Space.Self);

        horizontalInput = Mathf.Lerp(horizontalInput, Input.GetAxis("Horizontal") * strafeSpeed, strafeAcceleration * Time.deltaTime); // A and D keys
        verticalInput = Mathf.Lerp(verticalInput, Input.GetAxis("Vertical") * forwardSpeed, forwardAcceleration * Time.deltaTime); // W and S keys
        anotherInput = Mathf.Lerp(anotherInput, Input.GetAxis("Hover") * howerSpeed, howerAcceleration * Time.deltaTime); // Space and left alt keys

        if (Input.GetKeyDown(KeyCode.LeftShift) || (Input.GetKeyDown(KeyCode.Space)) && Input.GetAxis("Vertical") > 0)
        {
            modifier = boost;
            AudioManager.PlayLoopSound(SoundType.Boost, 0.6f);

            FOV = true;
            
            if (noise != null)
                noise.FrequencyGain = 1.8f;  // Shake
            

            if (speedEffect != null)
                speedEffect.Play();

        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || (Input.GetKeyUp(KeyCode.Space)))
        {
            modifier = 1;
            AudioManager.StopLoopSoundGradually(0.7f);

            FOV = false;
            
            if (noise != null)
                noise.FrequencyGain = 0f;  // Shake
            

            if (speedEffect != null)
                speedEffect.Stop();

        }

        if (FOV)
        {
            Cam.Lens.FieldOfView = Mathf.Lerp(Cam.Lens.FieldOfView, changedFOV, 10f * Time.deltaTime);
        }
        else
        {
            Cam.Lens.FieldOfView = Mathf.Lerp(Cam.Lens.FieldOfView, normalFOV, 10f * Time.deltaTime);
        }
        

        Move();
    }
    void Move()
    {
        transform.position += transform.forward * verticalInput * modifier * Time.deltaTime;
        transform.position += transform.right * horizontalInput * Time.deltaTime;
        transform.position += transform.up * anotherInput * Time.deltaTime;
    }
}
