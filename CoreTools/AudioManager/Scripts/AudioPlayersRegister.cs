using System.Collections.Generic;

namespace CoreCode.Scripts{
	public partial class AudioPlayersRegister : SingleNode<AudioPlayersRegister>
	{
		// ----------------------------------- Information ------------------------------------------------
		/*This is a scripts is a register for audio elements, so other scripts can request to access them.
		It basically works as a service locator, using Tags for getting the audio elements*/
	

		// Variables
		private Dictionary<string, IAudioPlayer> mRegisteredPlayers = new Dictionary<string, IAudioPlayer>();


        public override void _EnterTree()
        {
            base._EnterTree();
			mLogObject = LogManager.Instance.RequestLog("Audio", mShouldLog);
        }


        public void RegisterAudioPlayer(IAudioPlayer player)
		{
			if (mRegisteredPlayers.ContainsKey(player.Tag())){
				mLogObject.Err("Trying to register Audio player that was already registered: " + player.Tag());
				return;
			}
			mRegisteredPlayers.Add(player.Tag(), player);
		}
		

		public void UnregisterAudioPlayer(IAudioPlayer player){
			if (!mRegisteredPlayers.ContainsKey(player.Tag())){
				mLogObject.Err("Trying to get Audio player that was not registered: " + player.Tag());
				return;
			}
			mRegisteredPlayers.Remove(player.Tag());
		}


		public IAudioPlayer GetAudioPlayer(string tag){
			//Could potentially use random or something here to get variations on the same SVFX.
			//Or, better, I could have ANOTHER class to delegate for that! That is better, doing the
			//change later.
			return mRegisteredPlayers[tag];
		}

	}
}
