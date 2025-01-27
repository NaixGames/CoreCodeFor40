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

			//For some reason the testing suite will, in some cases, try to free the object pooler
			//before this is processed. This is horrible behaviour imo, but for avoiding this breaking
			//test I will allow only to call this if the object pooler is not null.
			if (GameObjectPooler.Instance == null){
				GD.PushWarning("GameObjectPooler does not exists. This should happen outside testing");
				//Well it shouldn't happen ever, but I cant fix the testing suite. I will try to report when I can
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
