using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SendToGoogle : MonoBehaviour
{
    public GameObject packetspeed;
    public GameObject badpacketratio;

    private string PacketSpeed;
    private string BadPacketRatio;

    [SerializeField] private string url = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSd3ivUjaxJ_4g20Yv4beifTnayy6yua3J6gfCFbQwInAYnJeA/formResponse";

    IEnumerator Post(string packetspeed, string badpacketratio)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1445729372", packetspeed);
        form.AddField("entry.1059381353", badpacketratio);
 
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("ERROR: " + www.error);
        }
        else
        {
            Debug.Log(packetspeed);
            Debug.Log(badpacketratio);

            Debug.Log("No Errors! File is uploading...");
            if(www.uploadProgress == 1 || www.isDone)
            {
                yield return new WaitForSeconds(5);
                Debug.Log("Form Upload Complete!");
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
    public void Send()
    {
        PacketSpeed = packetspeed.GetComponent<InputField>().text;
        BadPacketRatio = badpacketratio.GetComponent<InputField>().text;

        StartCoroutine(Post(PacketSpeed, BadPacketRatio));

    }
}
