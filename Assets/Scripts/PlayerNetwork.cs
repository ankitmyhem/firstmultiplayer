using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField]
    private Transform spawnObject;
    private Transform spawnedObjectTransform;
    private readonly NetworkVariable<PlayerNetworkData> _netData = new(writePerm: NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        //_netData.OnValueChanged += ShowData;
    }

    private void ShowData(PlayerData previous, PlayerData current)
    {
        //Debug.Log(OwnerClientId + ": Has a Value" + current._int + ": " + current._bool);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            /*if (Input.GetKeyDown(KeyCode.A))
            {
                spawnedObjectTransform = Instantiate(spawnObject);
                spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);


                _netData.Value = new PlayerData
                {
                    _int = Random.Range(0, 100),
                    _bool = false
                };
                TestClientRpc(new ClientRpcParams());
                //_netData.Value = Random.Range(0, 100);
            }*/
            _netData.Value = new PlayerNetworkData() 
            {
                PlayerPosition = transform.position,
                PlayerRotation = transform.eulerAngles 
            };
            if (Input.GetKeyDown(KeyCode.D))
            {
                if(spawnedObjectTransform != null)
                {
                    spawnedObjectTransform.GetComponent<NetworkObject>().Despawn(true);
                }
            }
        }
        else
        {
            transform.position = _netData.Value.PlayerPosition;
            transform.eulerAngles = _netData.Value.PlayerRotation;
        }
        
    }

    [ServerRpc]
    private void TestServerRpc(ServerRpcParams serverRpcParams)
    {
        Debug.Log(serverRpcParams.Receive.SenderClientId);
    }

    [ClientRpc]
    private void TestClientRpc(ClientRpcParams clientRpcParams)
    {
        Debug.Log("TestClientRpc");
    }
    public struct PlayerData : INetworkSerializable
    {
        public int _int;
        public bool _bool;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
        }
    }

    struct PlayerNetworkData : INetworkSerializable
    {
        private float _x, _z;
        private short _yRot;
        internal Vector3 PlayerPosition
        {
            get => new Vector3(_x, 0, _z);
            set
            {
                _x = value.x;
                _z = value.z;
            }
        }

        internal Vector3 PlayerRotation
        {
            get => new Vector3(0, _yRot, 0);
            set
            {
                _yRot = (short)value.y;
            }
        }
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _x);
            serializer.SerializeValue(ref _z);
            serializer.SerializeValue(ref _yRot);
        }
    }

    
}

