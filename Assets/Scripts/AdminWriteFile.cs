using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

public class AdminWriteFile : MonoBehaviour
{
    [Header("Input Fields")]
    public InputField userName;
    public InputField packetSpeed;
    public InputField badPacketRatio;
    [Header("Data Strings")]
    public string usernameText;
    public string packetSpeedText;
    public string badPacketRatioText;

    public void Send()
    {
        usernameText = userName.text;
        packetSpeedText = packetSpeed.text;
        badPacketRatioText = badPacketRatio.text;

        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();

        form.AddField("name", usernameText);
        form.AddField("data", packetSpeedText + badPacketRatioText);

        //form.AddField("PacketSpeed", packetSpeedText);
        //form.AddField("BadPacketRatio", badPacketRatioText);

        UnityWebRequest www = UnityWebRequest.Post("https://www.cse.unr.edu/~crystala/taiser/test/data/dataUploader.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("ERROR: " + www.error);
        }
        else
        {
            Debug.Log("No Errors! File is uploading...");
            if (www.uploadProgress == 1 || www.isDone)
            {
                yield return new WaitForSeconds(5);
                Debug.Log("Form Upload Complete!");
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
}