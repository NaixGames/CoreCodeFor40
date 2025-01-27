using Godot;
using System;
using System.Collections.Generic;

namespace CoreCode.Scripts{
	[GlobalClass][Tool]
	public partial class ScenePoolAndAudioData : Resource
	{
		[Export] public PoolSceneData PoolableObjectsData;

		//Algo parecido pal audio
	}
}
