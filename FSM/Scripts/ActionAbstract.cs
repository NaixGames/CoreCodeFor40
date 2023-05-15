using Godot;

namespace CoreCode.ActorCommons.Actions{
	public abstract partial class ActionAbstract 
	{
		//This is an abstract interface to create action that can be common between different actors.
		//This is to create to repeting code as much as posible.
		protected abstract void DoBaseAction(Node mNode, Godot.Collections.Dictionary mMemoryBlackboardCache);
	}
}