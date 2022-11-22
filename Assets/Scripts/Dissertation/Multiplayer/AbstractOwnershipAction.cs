using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Dissertation.Multiplayer
{
	public abstract class AbstractOwnershipAction : NetworkBehaviour
	{
		public bool isRemoving = false;
		private Transform parent;

		public bool ChangeOwnership()
		{
			// If it already is the owner or another player is the owner don't do anything
			if (IsOwner || !IsOwnedByServer) return false;

			StopAllCoroutines();
			isRemoving = false;
			ChangeOwnershipServerRpc(NetworkManager.Singleton.LocalClientId);
			return true;
		}

		public bool RemoveOwnership(float delay = 0.08f)
		{
			// If it isn't the owner don't do anything
			if (!IsOwner) return false;

			StartCoroutine(DelayRemoveOwnership(delay));
			Debug.Log("RemoveOwnership");
			return true;
		}

		IEnumerator DelayRemoveOwnership(float delay)
		{
			isRemoving = true;
			yield return new WaitForSeconds(delay);
			RemoveOwnershipServerRpc();
			Debug.Log("RemoveOwnership Coroutine");
		}

		[ServerRpc(RequireOwnership = false)]
		public void ChangeOwnershipServerRpc(ulong localClientId)
		{
			GetComponent<NetworkObject>().ChangeOwnership(localClientId);
			Debug.Log("changed ownership");
		}

		[ServerRpc(RequireOwnership = false)]
		public void RemoveOwnershipServerRpc()
		{
			Debug.Log("removed ownership");
			GetComponent<NetworkObject>().RemoveOwnership();
		}

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

			while (counter < 2) {
				if (parent.GetComponent<NetworkObject>().IsSpawned)
				{
					transform.parent = parent;
					counter = 2;
					success = true;
				}
				else { 
					yield return new WaitForSeconds(0.10f);
					counter++;
				}
			}

			if (!success) {
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
