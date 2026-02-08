using UnityEngine;

public abstract class NetworkedObject : MonoBehaviour
{
   public abstract void Initialize();

   public abstract string GetNetworkId();
}
