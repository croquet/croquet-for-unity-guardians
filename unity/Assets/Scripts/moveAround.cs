using UnityEngine;

public class moveAround : MonoBehaviour
{
    public float speed = 1.0f;
    public float boostSpeedFactor = 2.0f;
    public float rotationSpeed = 10.0f;

    private Terrain _terrain;
    private float boostSpeed;
    private float computedSpeed;
    private string croquetHandle;
    
    void Start()
    {
        _terrain = FindObjectOfType<Terrain>();
        boostSpeed = boostSpeedFactor * speed;
        computedSpeed = speed;

        croquetHandle = gameObject.GetComponent<CroquetEntityComponent>().croquetHandle;
    }

  
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal == 0 && vertical == 0) return;
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            computedSpeed = boostSpeed;
        }
        else
        {
            computedSpeed = speed;
        }
        
        transform.Translate(transform.forward * (computedSpeed * Time.deltaTime * vertical), Space.World);
        Vector3 tPos = _terrain.gameObject.transform.position;
        transform.position = new Vector3(transform.position.x,
            _terrain.terrainData.GetInterpolatedHeight((transform.position.x-tPos.x)/(_terrain.terrainData.size.x), (transform.position.z-tPos.z)/(_terrain.terrainData.size.z)),
            transform.position.z);
        
        
        transform.Rotate(Vector3.up, rotationSpeed*Time.deltaTime*horizontal);

        var slopeRotation = Quaternion.FromToRotation(transform.up, _terrain.terrainData.GetInterpolatedNormal((transform.position.x+640)/(_terrain.terrainData.size.x), (transform.position.z+640)/(_terrain.terrainData.size.z)));
        transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation * transform.rotation, 10.0f* Time.deltaTime);
        
        CroquetSpatialSystem.Instance.SnapObjectTo(croquetHandle, transform.position, transform.rotation);
        CroquetSpatialSystem.Instance.SnapObjectInCroquet(croquetHandle, transform.position, transform.rotation);
    }
}
