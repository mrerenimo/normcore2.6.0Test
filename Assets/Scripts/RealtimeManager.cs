using System;
using System.Collections.Generic;
using Normal.Realtime;
using UnityEngine;

namespace DefaultNamespace
{
	public class RealtimeManager : MonoBehaviour
	{
		public static RealtimeManager Instance;

		[SerializeField] private MySync mySync;

		[SerializeField] private Realtime realtime;

		[SerializeField] private GameObject playerPrefab;

		public static int GetClientID => Instance.realtime.clientID;

		public Action<PlayerSync> PlayerAdded;

		public List<PlayerSync> Players;

		private void Awake()
		{
			Instance = this;
			realtime.didConnectToRoom += ConnectedToRoom;
			PlayerAdded += HandlePlayer;
		}

		private void HandlePlayer(PlayerSync obj)
		{
			Debug.Log("player added");
			Players.Add(obj);
		}

		private void ConnectedToRoom(Realtime realtime1)
		{
			var options = new Realtime.InstantiateOptions
			{
				ownedByClient               = false,    // Make sure the RealtimeView on this prefab is owned by this client.
				preventOwnershipTakeover    = false,    // Prevent other clients from calling RequestOwnership() on the root RealtimeView.
				useInstance                 = realtime, // Use the instance of Realtime that fired the didConnectToRoom event.
				destroyWhenOwnerLeaves      = true,
				destroyWhenLastClientLeaves = true
			};

			Realtime.Instantiate(playerPrefab.name, options);
		}
	}
}