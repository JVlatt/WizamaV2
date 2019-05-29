using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wizama.NFC;

public class APIController : MonoBehaviour
{
    public GameObject textContainer;
    Text textDisplay;
    NFC_DEVICE_ID[] devicesIds;
    List<NFCTag> cardTags;
    bool isPolling = false;
    void Start()
    {
        NFC_DEVICE_ID[] devicesIds = new NFC_DEVICE_ID[1];
        cardTags = new List<NFCTag>();
        devicesIds[0] = NFC_DEVICE_ID.ANTENNA_2;
        textDisplay = textContainer.GetComponent<Text>();
    }

    // Update is called once per frame
    public void CheckTags()
    {
        cardTags = NFCController.GetTags(devicesIds);
    }

    public void TestCards()
    {
        NFCController.CanReadNFC = true;
        NFCController.StartPolling(devicesIds);
        isPolling = true;
    }
    public void StopTest()
    {
        NFCController.StopPolling();
        NFCController.CanReadNFC = false;
        isPolling = false;
        foreach(NFCTag item in cardTags)
        {
            Debug.Log("Data is : " + item.Data + "\n");
            textDisplay.text = item.Data;
        }
    }
    
    void GetCardTags()
    {
        cardTags = NFCController.GetTags(devicesIds);
        textDisplay.text = cardTags[0].Data;
    }
}
