using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowQRForSession : MonoBehaviour
{
    public string urlPrefix = "https://croquet.io/demolition-multi/";
    public void Start()
    {
        ShowQRCode qrShower = GameObject.FindObjectOfType<ShowQRCode>();
        if (qrShower != null)
        {
            CroquetRunner runner = CroquetBridge.Instance.GetComponent<CroquetRunner>();
            string localReflector = PlayerPrefs.GetString("sessionIP", "");
            string sessionNameValue = CroquetBridge.Instance.defaultSessionName;
            string url;
            if (localReflector == "")
            {
                Debug.Log("local reflector session ip setting empty, using live croquet network");
                url = $"{urlPrefix}?room={sessionNameValue}";
            }
            else
            {
                Debug.Log("local reflector session ip setting found, using set ip");
                url = $"http://{localReflector}/demolition-multi?q={sessionNameValue}&reflector=ws://{localReflector}/reflector&files=http://{localReflector}/files";
            }
            
            Debug.Log($"Displaying QR: Session Name Value Loaded: {sessionNameValue} with reflector: {url}");
            qrShower.DisplayQR(url);
        }
    }
}
