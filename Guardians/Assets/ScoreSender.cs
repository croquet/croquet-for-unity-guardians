using UnityEngine;
using System.Runtime.InteropServices;

public class ScoreSender : MonoBehaviour
{
    // [DllImport("__Internal")]
    // private static extern void RequestServerTime(string walletAddress);

    // [DllImport("__Internal")]
    // private static extern void SubmitHighScore(string message);

    // private string walletAddress;
    // private float gameDuration; // Duration in seconds
    // private int highScore;

    // public void Initialize(string wallet)
    // {
    //     walletAddress = wallet;
    // }

    // public void SendHighScore(int score)
    // {
    //     highScore = score;
    //     RequestServerTime(walletAddress);
    // }

    // public void ReceiveServerTime(string serverTime)
    // {
    //     string message = EncodeMessage(highScore, serverTime, walletAddress, gameDuration);
    //     SubmitHighScore(message);
    // }

    // private string EncodeMessage(int highScore, string serverTime, string walletAddress, float gameDuration)
    // {
    //     string rawMessage = $"{highScore}|{serverTime}|{walletAddress}|{gameDuration}";
    //     byte[] rawMessageBytes = System.Text.Encoding.UTF8.GetBytes(rawMessage);
    //     return System.Convert.ToBase64String(rawMessageBytes);
    // }
}
