using System;
using UnityEngine;

public class moveAround : MonoBehaviour
{
    public float speed = 1.0f;
    public float boostSpeedFactor = 2.0f;
    public float rotationSpeed = 10.0f;

    private bool positionHasBeenInitialized;
    private Terrain _terrain;
    private float boostSpeed;
    private float computedSpeed;
    private string croquetHandle;
    private CroquetAvatarComponent avatarComponent;
    
    void Start()
    {
        _terrain = FindObjectOfType<Terrain>();
        boostSpeed = boostSpeedFactor * speed;
        computedSpeed = speed;

        croquetHandle = gameObject.GetComponent<CroquetEntityComponent>().croquetHandle;
        avatarComponent = gameObject.GetComponent<CroquetAvatarComponent>();
    }


    void Update()
    {
        if (avatarComponent.isActiveAvatar)
        {
            CheckForMissileLaunch();

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (positionHasBeenInitialized && Mathf.Abs(horizontal) < 0.01 && Mathf.Abs(vertical) < 0.01) return;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                computedSpeed = boostSpeed;
            }
            else
            {
                computedSpeed = speed;
            }

            transform.Translate(transform.forward * (computedSpeed * Time.deltaTime * vertical), Space.World);
            transform.Rotate(Vector3.up, rotationSpeed*Time.deltaTime*horizontal);
            
            AlignWithTerrain();
            
            CroquetSpatialSystem.Instance.SnapObjectTo(croquetHandle, transform.position, transform.rotation);
            CroquetSpatialSystem.Instance.SnapObjectInCroquet(croquetHandle, transform.position, transform.rotation);

            positionHasBeenInitialized = true;
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

    void CheckForMissileLaunch()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Quaternion q = transform.rotation;
            float yaw = Mathf.Atan2(2 * q.y * q.w - 2 * q.x * q.z, 1 - 2 * q.y * q.y - 2 * q.z * q.z);
            Croquet.Say(gameObject, "shoot", yaw);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        {
            transform.Translate((transform.position-other.transform.position).normalized*1.52f, Space.World);
        }
    }
}
