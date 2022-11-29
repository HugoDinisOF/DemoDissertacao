using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Dissertation.Multiplayer 
{ 
    public abstract class AbstractNetworkObject : NetworkBehaviour
    {
		protected Transform parent;

		[ServerRpc(RequireOwnership = false)]
		public void DebugServerRpc(string debugString)
		{
			Debug.Log(debugString);
		}

		virtual protected void Start()
		{
			Spawn();
		}

		IEnumerator ChangeParentFailed()
		{
			int counter = 0;
			bool success = false;

			while (counter < 2)
			{
				if (parent.GetComponent<NetworkObject>().IsSpawned)
				{
					transform.parent = parent;
					counter = 2;
					success = true;
				}
				else
				{
					yield return new WaitForSeconds(0.10f);
					counter++;
				}
			}

			if (!success)
			{
				Debug.Log($"FAILED TO SPAWN {gameObject.name}");
			}

			yield return null;
		}

		void Spawn()
		{
			if (NetworkManager.Singleton.IsServer && !IsSpawned)
			{
				parent = transform.parent;
				gameObject.SetActive(true);
				NetworkObject.Spawn(true);
				if (!(parent is null))
				{
					if (parent.GetComponent<NetworkObject>().IsSpawned)
					{
						transform.parent = parent;
					}
					else
					{
						StartCoroutine(ChangeParentFailed());
					}
				}
			}
		}
    }
}
