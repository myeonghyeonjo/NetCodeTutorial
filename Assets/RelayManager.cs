using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayManager : MonoBehaviour
{
    public struct RelayHostData
    {
        public string JoinCode;

        public string IPv4Address;

        public ushort Port;

        public Guid AllocationID;
        public byte[] HostConnectionData;
        public byte[] AllocationIDBytes;

        public byte[] ConnectionData;



        public byte[] Key;
    }


    public struct RelayJoinData
    {

        public string IPv4Address;

        public ushort Port;

        public Guid AllocationID;

        public byte[] AllocationIDBytes;

        public byte[] ConnectionData;

        public byte[] HostConnectionData;

        public byte[] Key;

    }

    public static async Task<RelayHostData> SetupRelay(int maxConn, string ecviroment)
    {

        InitializationOptions options = new InitializationOptions()
        .SetEnvironmentName(ecviroment);

        await UnityServices.InitializeAsync(options);

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        Allocation allocation = await Unity.Services.Relay.RelayService.Instance.CreateAllocationAsync(maxConn);

        RelayHostData data = new RelayHostData
        {
            IPv4Address = allocation.RelayServer.IpV4,
            Port = (ushort)allocation.RelayServer.Port,

            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            ConnectionData = allocation.ConnectionData,
            Key = allocation.Key,
           
        };
        data.JoinCode = await Unity.Services.Relay.RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);



        return data;
    }




    public static async Task<RelayJoinData> JoinRelay(string joinCode, string environment)
    {
        //Debug.LogError($"Start Join by {joinCode}");

        InitializationOptions options = new InitializationOptions()
        .SetEnvironmentName(environment);


        await UnityServices.InitializeAsync(options);

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        JoinAllocation allocation = await Unity.Services.Relay.RelayService.Instance.JoinAllocationAsync(joinCode);

        RelayJoinData data = new RelayJoinData
        {
            IPv4Address = allocation.RelayServer.IpV4,
            Port = (ushort)allocation.RelayServer.Port,

            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            ConnectionData = allocation.ConnectionData,
            HostConnectionData = allocation.HostConnectionData,
            Key = allocation.Key,

        };
        return data;

    }
}
