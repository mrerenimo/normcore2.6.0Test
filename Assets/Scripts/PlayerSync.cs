using Normal.Realtime;

namespace DefaultNamespace
{
	public class PlayerSync : RealtimeComponent<PlayerModel>
	{
		public int ID => model.id;
		
		protected override void OnRealtimeModelReplaced(PlayerModel previousModel, PlayerModel currentModel)
		{
			if(previousModel != null)
			{
				// Unregister from events
			}

			if(currentModel != null)
			{
				if(currentModel.isFreshModel)
				{
					model.id = currentModel.room.clientID;
				}
			}

			RealtimeManager.Instance.PlayerAdded?.Invoke(this);
		}
	}
}