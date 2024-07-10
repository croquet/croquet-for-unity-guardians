using System.Collections;
using UnityEngine;

public class ExplodeOnEnding : MonoBehaviour
{
    public GameObject explosionPrefab;
    public GameObject spaceshipPrefab;
    public GameObject jetPrefab;
    public float explosionDuration = 3.0f; // Adjust based on your particle system duration
    public float horizontalDuration = 2.5f; // Duration for horizontal movement
    public float verticalDuration = 2.5f; // Duration for vertical movement
    public float landingDuration = 5.0f; // Adjust based on your landing animation duration
    public HUDController hudController;
    private Vector3 originalPosition;
    private bool sequenceTriggeredAtZeroHealth = false;
    private bool inFlight = false;

    // Start is called before the first frame update
    void Start()
    {
        // originalPosition = spaceshipPrefab.transform.position;
        hudController = FindObjectOfType<HUDController>();
        jetPrefab.SetActive(false);
        StartCoroutine(waitAMoment());
    }
    IEnumerator waitAMoment()
    {
        yield return new WaitForSeconds(5);
        // originalPosition = spaceshipPrefab.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space) && !inFlight) // Trigger once when spacebar is pressed
        // {
        //     StartCoroutine(ExecuteTakeoffSequence());
        // }

        if (!sequenceTriggeredAtZeroHealth && hudController.GetHealth() == 0) // Trigger once at zero health
        {
            sequenceTriggeredAtZeroHealth = true;
            StartCoroutine(ExecuteTakeoffSequence());
        }
        if (hudController.GetHealth() != 0)
        {
            sequenceTriggeredAtZeroHealth = false;
        }
    }

    IEnumerator ExecuteTakeoffSequence()
    {
        if (inFlight)
        {
            yield break; // Exit the coroutine if already in flight
        }
        // Set inFlight to true
        inFlight = true;

        spaceshipPrefab.SetActive(false); // Turn off the spaceship (optional)
        // Instantiate explosion at the local position (0, 0, 0)
        Vector3 explosionPosition = transform.position;
        explosionPosition.y = transform.position.y + 25.0f; // Adjust the Y position based on your spaceship
        GameObject explosion = Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);
        
        // Wait for the explosion to complete
        yield return new WaitForSeconds(explosionDuration);

        // Destroy the explosion effect
        Destroy(explosion);
        
        originalPosition = spaceshipPrefab.transform.position;
        // Instantly move the spaceship to the new position
        spaceshipPrefab.transform.position = originalPosition + new Vector3(200, 100, 0);
        spaceshipPrefab.SetActive(true); // Turn on the spaceship (optional)
        jetPrefab.SetActive(true);

        // Move the spaceship back to the original position horizontally
        yield return MoveSpaceship(originalPosition + new Vector3(0, 100, 0), horizontalDuration);

        // Move the spaceship back to the original position vertically
        yield return MoveSpaceship(originalPosition, verticalDuration);

        spaceshipPrefab.transform.position = originalPosition;

        // Turn off the jet
        inFlight = false;
        jetPrefab.SetActive(false);
    }

    IEnumerator MoveSpaceship(Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0;
        Vector3 startPosition = spaceshipPrefab.transform.position;

        while (elapsedTime < duration)
        {
            spaceshipPrefab.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the spaceship reaches the target position at the end
        spaceshipPrefab.transform.position = targetPosition;
    }
}
