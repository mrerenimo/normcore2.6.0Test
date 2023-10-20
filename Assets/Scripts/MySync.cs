using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Normal.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class MySync : RealtimeComponent<MyModel>
{
	[SerializeField]
	private List<int> playerSlotIdxList = new List<int>();

	private void Start()
	{
		StartCoroutine(WaitForPlayers());
	}

	protected override void OnRealtimeModelReplaced(MyModel previousModel, MyModel currentModel)
	{
		if(previousModel != null)
		{
			previousModel.myStateDidChange -= HandleState;
		}

		if(currentModel != null)
		{
			if (currentModel.isFreshModel)
			{
				currentModel.myState = MyState.Prepare;
				for(int i = 0; i < 5; i++)
				{
					playerSlotIdxList.Add(i);
					var spawnPointModel = new SpawnPointModel
					{
						spawnPointIdx = i,
						playerId      = -1,
						isLocked      = false
					};

					currentModel.spawnPoints.Add(spawnPointModel);
				}
			}
			
			currentModel.myStateDidChange += HandleState;
		}
	}

	private void HandleState(MyModel myModel, MyState value)
	{
		Debug.Log("state changed : " + value);
		switch (value)
		{
			case MyState.Start:
				Debug.Log(model.spawnPoints.Count);
				break;
		}
	}
	
	private void SetSpawnPoint(MyModel myModel, MyState value)
	{
		
		switch (value)
		{
			case MyState.Start:
				foreach (var spawnPointModel in model.spawnPoints)
				{
					Debug.Log(spawnPointModel.playerId + " " + spawnPointModel.spawnPointIdx);
				}
				break;
		}
	}

	public void SetState(MyState newState) => model.myState = newState;
	
	public void SetPlayersSpawnPoint()
	{
	
		var tempIndexList = new List<int>(playerSlotIdxList);
			
		foreach(var player in RealtimeManager.Instance.Players)
		{
			if(tempIndexList.Count == 0)
				throw new Exception("Oyuncu sayısı, spawn point sayısından fazla");

			var index = Random.Range(0, tempIndexList.Count);
			var idx   = tempIndexList[index];
			SetSlotPlayerId(idx, player.ID);
			tempIndexList.RemoveAt(index);
		}
	}

	public void SetSlotPlayerId(int idx, int playerId)
	{
		foreach(var spawnPointModel in model.spawnPoints)
		{
			if(spawnPointModel.spawnPointIdx == idx)
			{
				spawnPointModel.playerId = playerId;
				break;
			}
		}
	}

	private bool AmITheLowestClientID(int id)
	{
		var lowestID = int.MaxValue;
		foreach(var player in RealtimeManager.Instance.Players)
			if(player.ID < lowestID)
				lowestID = player.ID;

		return lowestID == id;
	}

	IEnumerator WaitForPlayers()
	{
		while (RealtimeManager.Instance.Players.Count < 2) //Wait for other player's connection
			yield return null;


		yield return new WaitForSeconds(5); //to let players set their ids of their playermodel id value
		
		if(!AmITheLowestClientID(RealtimeManager.GetClientID)) //Just lowest client can set the values
			yield break;

		SetPlayersSpawnPoint();
		
		yield return new WaitForSeconds(1);

		SetState(MyState.Start);
	}
}
