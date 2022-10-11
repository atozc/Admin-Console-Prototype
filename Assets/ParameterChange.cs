using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParameterChange : MonoBehaviour
{
    public InputField packetSpeedText;
    public InputField badPacketRatioText;
    public Button submit;

    GameObject packetSpeed = GameObject.Find("packetSpeed");
    GameObject badPacketRatio = GameObject.Find("BadPacketRatio");

    string newPacketSpeed;
    string newBadPacketRatio;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(packetSpeedText.text);
        Debug.Log(badPacketRatioText.text);

        Button btn = submit.GetComponent<Button>();
        btn.onClick.AddListener(changeParameters);

    }

    void changeParameters()
    {
        newPacketSpeed = packetSpeedText.text;
        newBadPacketRatio = badPacketRatioText.text;
    }
}
