using Godot;
using System;
using System.Collections.Generic;

namespace CoreCode.Scripts{
	[GlobalClass]
	public partial class ScenePoolAndAudioData : Resource
	{
		[Export] private PackedScene mSceneReference;
		public PackedScene SceneReference => mSceneReference;
		[Export] private PoolSceneData mPoolableObjectsData;
		public PoolSceneData PoolableObjectsData => mPoolableObjectsData;

		//Algo parecido pal audio
	}
}
