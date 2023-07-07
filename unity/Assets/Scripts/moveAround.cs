using System;
using UnityEngine;
using UnityEngine.Rendering;

public class moveAround : MonoBehaviour, ICroquetDriven
{
    // configured on the prefab
    public float speed = 1.0f;
    public float boostSpeedFactor = 2.0f;
    public float rotationSpeed = 10.0f;

    private bool positionHasBeenInitialized;
    private Terrain _terrain;
    // private float boostSpeed;
    // private float computedSpeed;

    private string croquetHandle;
    private CroquetAvatarComponent avatarComponent;
    private GameState gameState;

    private float lastShootTime = 0;
    private float waitShootTime = 0.1f; // 100ms

    private AudioSource shotSound;

    void Start()
    {
        _terrain = FindObjectOfType<Terrain>();
        // boostSpeed = boostSpeedFactor * speed;
        // computedSpeed = speed;

        shotSound = GetComponent<AudioSource>();
    }

    public void PawnInitializationComplete() {
        croquetHandle = gameObject.GetComponent<CroquetEntityComponent>().croquetHandle;
        avatarComponent = gameObject.GetComponent<CroquetAvatarComponent>();
    }

    void Update()
    {
        if (avatarComponent == null) return;

        if (gameState == null)
        {
            GameObject gameStateGO = GameObject.FindWithTag("GameController");
            if (gameStateGO != null)
            {
                gameState = gameStateGO.GetComponent<GameState>();
            }

            if (gameState == null) return;
        }

        if (CroquetAvatarSystem.Instance.GetActiveAvatarComponent() == avatarComponent && !gameState.gameEnded)
        {
            // it's the active avatar, and we're live in a game - so perhaps moving, perhaps shooting
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            float speedNow = 0;

            if (!positionHasBeenInitialized || Mathf.Abs(horizontal) > 0.01 || Mathf.Abs(vertical) > 0.01)
            {
                speedNow = speed * vertical;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    speedNow *= boostSpeedFactor;
                }

                transform.Translate(transform.forward * (speedNow * Time.deltaTime), Space.World);
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime * horizontal);

                AlignWithTerrain();

                CroquetSpatialSystem.Instance.SnapObjectTo(croquetHandle, transform.position, transform.rotation);
                CroquetSpatialSystem.Instance.SnapObjectInCroquet(croquetHandle, transform.position, transform.rotation);

                positionHasBeenInitialized = true;
            }

            // after moving, see if we want to shoot from here
            CheckForMissileLaunch(speedNow);
        }
        else
        {
            AlignWithTerrain();
        }
    }

    void AlignWithTerrain() {
        Vector3 tPos = _terrain.gameObject.transform.position;
        transform.position = new Vector3(transform.position.x,
            _terrain.terrainData.GetInterpolatedHeight((transform.position.x-tPos.x)/(_terrain.terrainData.size.x), (transform.position.z-tPos.z)/(_terrain.terrainData.size.z)),
            transform.position.z);

        var slopeRotation = Quaternion.FromToRotation(transform.up, _terrain.terrainData.GetInterpolatedNormal((transform.position.x-tPos.x)/(_terrain.terrainData.size.x), (transform.position.z-tPos.z)/(_terrain.terrainData.size.z)));
        float slerpFactor = positionHasBeenInitialized ? 10.0f * Time.deltaTime : 1.0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation * transform.rotation, slerpFactor);
    }

    void CheckForMissileLaunch(float speed)
    {
        float now = Time.realtimeSinceStartup;
        if (now - lastShootTime < waitShootTime) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            lastShootTime = now;

            Quaternion q = transform.rotation;
            // yaw is in radians
            float yaw = Mathf.Atan2(2 * q.y * q.w - 2 * q.x * q.z, 1 - 2 * q.y * q.y - 2 * q.z * q.z);
            // send a message with four numbers: launch position x, y, z and yaw
            Vector3 pos = (speed * 0.05f + 2.0f) * transform.forward + transform.position; // position in 50ms' time
            float[] args = { pos.x, pos.y, pos.z, yaw };
            Croquet.Say(gameObject, "shoot", args);
            
            shotSound.PlayOneShot(shotSound.clip);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        transform.Translate((transform.position-other.transform.position).normalized*1.52f, Space.World);
    }
}
