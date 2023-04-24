using Godot;
using System;

namespace CoreCode.Scripts{
	public partial class SceneTransitionReferenceHelper : Node
	{
		// ----------------------------------- Information ------------------------------------------------
		/*This is a script to the scene transition manager easy reference for elements in the scene. 
		For any scene we transition into this should be the script of the root node of the subscene
		(ie, the part not containing the singletons)."*/
		
		// ------------------------------------ Use -------------------------------------------------------
		/* Assign reference to whatever persistant elements you have/whatever the scene actually is. 
		SceneTransitionManager should make the heavy work*/


		// ------------------------------------Variables 
		
		[Export]public Node NonPersistentElements;
		[Export] public Node PersistentElements;
		[Export] public Node ObjectPoolerNode;
		[Export] public Node AudioBankContainerNode;

		// ------------------------------------ Method to queue without anoying references staying arround

		public void SceneFinishedLoading(){
			NonPersistentElements=null;
			PersistentElements=null;
			ObjectPoolerNode=null;
			AudioBankContainerNode=null;
			this.QueueFree();
		}
	}
}
