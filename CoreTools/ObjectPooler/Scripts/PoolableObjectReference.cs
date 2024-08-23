using Godot;
using System;

namespace CoreCode.Scripts{
	[GlobalClass]
	public partial class PoolableObjectReference: Resource
	{
		[Export]
		public string Tag;
		[Export]
		public PackedScene Object;
	}
}
