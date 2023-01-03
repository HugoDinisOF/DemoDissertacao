using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Dissertation.Multiplayer
{
	public abstract class AbstractOwnershipAction : AbstractNetworkObject
	{
		public bool isRemoving = false;
		public NetworkVariable<int> ownerID = new NetworkVariable<int>(1);
		public static bool isAllowedToChangeOwnership = true;

		public bool ChangeOwnership()
		{
			// If it already is the owner or another player is the owner don't do anything
			if (IsOwner || !IsOwnedByServer) return false;

			// FIXME: maybe look at joining the two bool
			if (!isAllowedToChangeOwnership && ownerID.Value != -1) return false;

			StopAllCoroutines();
			isRemoving = false;
			ChangeOwnershipServerRpc(NetworkManager.Singleton.LocalClientId);
			return true;
		}

		public bool RemoveOwnership(float delay = 0.08f)
		{
			// If it isn't the owner don't do anything
			if (!IsOwner) return false;

			//FIXME: maybe look at joining the two bool
			if (!isAllowedToChangeOwnership && ownerID.Value != -1) return false;

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

		

	}
}
