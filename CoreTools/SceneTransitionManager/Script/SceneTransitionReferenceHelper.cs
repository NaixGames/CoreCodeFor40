using Godot;
using System;

namespace CoreCode.Scripts{
	[Tool]
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
		
		[Export] public ScenePoolAndAudioData SceneData;
		[Export] public NodePath NonPersistentElementsPath;
		[Export] public NodePath PersistentElementsPath;
		public Node NonPersistentElements;
		public Node PersistentElements;



		// ------------------------------------ Method to queue without anoying references staying arround

		public void SceneFinishedLoading(){
			NonPersistentElements=null;
			PersistentElements=null;
			this.QueueFree();
		}


		//---------------------------------------------------------------------------

		public override void _Ready(){
			if (Engine.IsEditorHint()){
				return;
			}

			if (SceneData == null)
			{
				GD.PushWarning("Scene without scene data!");
				return;
			}

			GameObjectPooler.Instance.AddObjectFromPoolData(SceneData.PoolableObjectsData);
		}


		public void GetNodesFromPaths(){
			NonPersistentElements = this.GetNode(NonPersistentElementsPath);
			PersistentElements = this.GetNode(PersistentElementsPath);
		} 
	}
}
