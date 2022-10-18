using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using Unity.Netcode.Transports.UTP;

public class UIManager : MonoBehaviour
{
    public Button hostButton;

    public Button clientClient;

    public TMP_InputField inputFiled;


    private void Start()
    {

        hostButton.onClick.AddListener(() => HostButtonClick());
        clientClient.onClick.AddListener(() => ClientButtonClick());
    }

    public async void HostButtonClick()
    {
        var data =   await RelayManager.SetupRelay(10, "production");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(data.IPv4Address, data.Port, data.AllocationIDBytes, data.Key, data.ConnectionData);

        inputFiled.text = data.JoinCode;

        NetworkManager.Singleton.StartHost();
    }

    public async void ClientButtonClick()
    {
        var data = await RelayManager.JoinRelay(inputFiled.text, "production");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(data.IPv4Address, data.Port, data.AllocationIDBytes, data.Key, data.ConnectionData, data.HostConnectionData);

        NetworkManager.Singleton.StartClient();
    }

}
