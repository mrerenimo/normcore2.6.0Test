using UnityEngine;

namespace DefaultNamespace
{
	[RealtimeModel]
	public partial class SpawnPointModel
	{
		[RealtimeProperty(1, true)]
		private int _playerId;

		[RealtimeProperty(2, true)]
		private int _spawnPointIdx;
		
		[RealtimeProperty(3, true)]
		private bool _isLocked;
	}
}