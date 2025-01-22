using Godot;
using System;

namespace CoreCode.Scripts{
	[GlobalClass][Tool]
	public partial class PoolSceneData : Resource
	{
		[Export] public Godot.Collections.Dictionary<PoolableObjectReference, int> PoolableObjectsData = new Godot.Collections.Dictionary<PoolableObjectReference, int>();
	}
}
