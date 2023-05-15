// meta-name: Action Template
// meta-description: Template for creating an Action class

using Godot;

namespace CoreCode.ActorCommons.Actions{
	public partial class _CLASS_ : ActionAbstract 
	{
		//This is an abstract interface to create action that can be common between different actors.
		//This is to create to repeting code as much as posible.
		protected override void DoBaseAction(Node mNode, Godot.Collections.Dictionary mMemoryBlackboardCache){
			return;
		}
	}
}
