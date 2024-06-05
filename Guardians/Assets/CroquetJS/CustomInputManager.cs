using UnityEngine;

public class CustomInputManager : MonoBehaviour
{
    public static CustomInputManager Instance { get; private set; }

    private float horizontal;
    private float vertical;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetAxis(string axisName, float value)
    {
        if (axisName == "Horizontal")
        {
            horizontal = value;
        }
        else if (axisName == "Vertical")
        {
            vertical = value;
        }
    }

    public float GetAxis(string axisName)
    {
        if (axisName == "Horizontal")
        {
            return horizontal;
        }
        else if (axisName == "Vertical")
        {
            return vertical;
        }
        return 0;
    }
}
