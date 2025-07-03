using Fusion;

public class NetworkDictionaryTest : NetworkBehaviour
{
    [System.Serializable]
    struct NetworkStruct : INetworkStruct
    {
        [Networked, Capacity(16)]
        public NetworkDictionary<int, int> NestedDict => default;

        // NetworkString is a normal struct, so it doesn't require any Fusion attributes
        public NetworkString<_16> NestedString;

        public static NetworkStruct Defaults
        {
            get
            {
                var result = new NetworkStruct();
                result.NestedDict.Add(0, 0);
                result.NestedString = "Initialized";
                return result;
            }
        }
    }

    // Property declared as ref type, allowing direct modification of values
    [Networked]
    [UnitySerializeField]
    private ref NetworkStruct NestedStruct => ref MakeRef(NetworkStruct.Defaults);
    

        
    public void ModifyValues()
    {
        // NestedStruct was declared as a ref above, so modifications
        // may be made directly to the reference, without need for copies.
        NestedStruct.NestedDict.Add(NestedStruct.NestedDict.Count, default);
        NestedStruct.NestedString = System.IO.Path.GetRandomFileName();
    }
}