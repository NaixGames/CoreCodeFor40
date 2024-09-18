using Godot;
using System;

namespace CoreCode.Scripts{
	public partial class AudioPlayer : AudioStreamPlayer, IAudioPlayer
	{
		[Export] private string mTag;
		public string Tag() => mTag;

		public override void _EnterTree()
		{
			base._EnterTree();
			AudioPlayersRegister.Instance.RegisterAudioPlayer(this);
		}

		public override void _ExitTree()
		{
			base._EnterTree();
			AudioPlayersRegister.Instance.UnregisterAudioPlayer(this);
		}
	}
}
