using Godot;
using System;
using CoreCode.AudioSystem;


namespace CoreCode.Scripts{
	[Tool]
	public partial class SceneTransitionManager : Node
	{
		// ----------------------------------- Information ------------------------------------------------
		/*This is a script to allow Scene transition in godot in a nice way. That is, preserving singletons
		and persistant elements betweens scene (like the player), while also not needed to have scene be 
		"stripped down in the editor"*/
		
		// ------------------------------------ Use -------------------------------------------------------
		/* This class give methods to load scenes given a path to the scene. This node should eventually
		support shaders for scene transition to make this transition (duh) more smooth*/

		// ------------------------------------- Singleton instantiation -------------------------------
		private static SceneTransitionManager instance;
		public static SceneTransitionManager Instance{
			get {return TryToReturnInstance();}
		}

		public static SceneTransitionManager TryToReturnInstance(){
			if (Engine.IsEditorHint()){
				return new SceneTransitionManager();
			}
			if (instance == null){
				GD.PushWarning("Instance of SceneTransitionManager called before the instance was ready!");
				instance = new SceneTransitionManager();
			}
			return instance;
		}


		public SceneTransitionManager(){
			if (Engine.IsEditorHint()){
				return;
			}
			if (instance==null){
				instance=this; 
			}
			else{
				GD.PushWarning("Instance of SceneTransitionManager created when there is an existing instance!");
			}
		}
		// ------------------------------------ Variables-----------------------------------------

		[Export] private SceneDatabase mSceneDatabase;
		
		private SceneTransitionReferenceHelper mReferenceHelper;

		// ------------------------------------ Variable for logging-----------------------------------------

		[Export] protected bool mShouldLog;

		protected ILogObject mLogObject; 

		// ------------------------------------ Methods -----------------------------------------

		public void HeavyTransitionToNewScene(string sceneName){
			if (!mSceneDatabase.mSceneNameToPathMapping.ContainsKey(sceneName)){
				mLogObject.Err("trying to load scene not loaded in Scene database named: " + sceneName);
				return;
			}
			//First we remove the object pooler, since it is a singleton also present on the other scene.
			mReferenceHelper.ObjectPoolerNode=null;
			GameObjectPooler.Instance.PoolAllObjects();
			GameObjectPooler.Instance.EraseObjectPooler();
			//For a similar reason we erase the other elements. Dont want two of the same thing of a scene. But we keep the parent for adding nodes later.
			mReferenceHelper.PersistentElements.QueueFree();
			mReferenceHelper.PersistentElements=null;
			mReferenceHelper.NonPersistentElements.QueueFree();
			mReferenceHelper.NonPersistentElements=null;

			//We load the other scene
			Node newActualScene = ResourceLoader.Load<PackedScene>(mSceneDatabase.mSceneNameToPathMapping[sceneName]).Instantiate(); 
			SceneTransitionReferenceHelper loadedReferenceHelper = (SceneTransitionReferenceHelper) newActualScene;
			loadedReferenceHelper.GetNodesFromPaths();

			//Add the new object pooler.
			loadedReferenceHelper.RemoveChild(loadedReferenceHelper.ObjectPoolerNode);
			loadedReferenceHelper.ObjectPoolerNode.Owner = null;
			mReferenceHelper.AddChild(loadedReferenceHelper.ObjectPoolerNode);
			loadedReferenceHelper.ObjectPoolerNode.Owner = mReferenceHelper.Owner;
			mReferenceHelper.ObjectPoolerNode = loadedReferenceHelper.ObjectPoolerNode;

			//Change the non persistent elements for the new ones.
			mReferenceHelper.PersistentElements = loadedReferenceHelper.PersistentElements;
			loadedReferenceHelper.PersistentElements.GetParent<Node>().RemoveChild(loadedReferenceHelper.PersistentElements);
			loadedReferenceHelper.PersistentElements.Owner = null;
			mReferenceHelper.AddChild(loadedReferenceHelper.PersistentElements);
			loadedReferenceHelper.PersistentElements.Owner = mReferenceHelper.Owner;
			
			//Should update the audio bank and make the song transition.
			AudioManager.Instance.UpdateMusicBanks(loadedReferenceHelper.AudioBankContainerNode);

			//Change the non persistent elements for the new ones.
			mReferenceHelper.NonPersistentElements = loadedReferenceHelper.NonPersistentElements;
			loadedReferenceHelper.NonPersistentElements.GetParent<Node>().RemoveChild(loadedReferenceHelper.NonPersistentElements);
			loadedReferenceHelper.NonPersistentElements.Owner = null;
			mReferenceHelper.AddChild(loadedReferenceHelper.NonPersistentElements);
			loadedReferenceHelper.NonPersistentElements.Owner = mReferenceHelper.Owner;
			
			//Free the loaded scene from memory
			loadedReferenceHelper.SceneFinishedLoading();			
		}

		public void LightTransitionToNewScene(string sceneName){
			if (!mSceneDatabase.mNonPersistantSceneNameToPathMapping.ContainsKey(sceneName)){
				mLogObject.Err("trying to load scene not loaded in Scene database named: " + sceneName);
				return;
			}
			
			GameObjectPooler.Instance.PoolAllObjects();

			Node newNonPersitanceScene = ResourceLoader.Load<PackedScene>(mSceneDatabase.mNonPersistantSceneNameToPathMapping[sceneName]).Instantiate(); 

			//Change the non persistent elements for the new ones.
			mReferenceHelper.NonPersistentElements.QueueFree();
			mReferenceHelper.NonPersistentElements = newNonPersitanceScene;
			newNonPersitanceScene.Owner = null;
			mReferenceHelper.AddChild(newNonPersitanceScene);
			newNonPersitanceScene.Owner = mReferenceHelper.Owner;
		}

		public override void _Ready(){
			if (Engine.IsEditorHint()){
				return;
			}
			
			mLogObject = LogManager.Instance.RequestLog("SceneTransitions", mShouldLog);
			
			//If we dont have a SceneDatabase give a warning.
			if (mSceneDatabase == null){
				mLogObject.Warn("No scene database provided. No scene transition will be posible!");
			}	
			
			Node baseNode = GetTree().Root.GetChild(GetTree().Root.GetChildCount()-1);

			if (!(baseNode is SceneTransitionReferenceHelper)){
				mLogObject.Warn("No reference SceneTransitionReferenceHelper as base. Audio and Scene Transitions will not work correclty");
			}

			mReferenceHelper = baseNode as SceneTransitionReferenceHelper;
			mReferenceHelper.GetNodesFromPaths();
			AudioManager.Instance.UpdateMusicBanks(mReferenceHelper.AudioBankContainerNode);	

		}


		//--------------------------------------
	}
}
