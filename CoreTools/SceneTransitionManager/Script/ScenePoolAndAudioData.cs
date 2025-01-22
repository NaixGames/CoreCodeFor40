using Godot;
using System;
using System.Collections.Generic;

namespace CoreCode.Scripts{
	[GlobalClass][Tool]
	public partial class ScenePoolAndAudioData : Resource
	{
		[Export] private PoolSceneData mPoolableObjectsData;
		public PoolSceneData PoolableObjectsData => mPoolableObjectsData;

		//Algo parecido pal audio
	}
}
