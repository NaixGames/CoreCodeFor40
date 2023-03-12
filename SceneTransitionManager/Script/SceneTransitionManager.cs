using Godot;
using System;

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
		if (instance == null){
			GD.PushWarning("Instance of SceneTransitionManager called before the instance was ready!");
			instance = new SceneTransitionManager();
		}
		return instance;
	}


	public SceneTransitionManager(){
		if (instance==null){
			instance=this; 
		}
		else{
			GD.PushWarning("Instance of SceneTransitionManager created when there is an existing instance!");
		}
	}
	// ------------------------------------ Variables-----------------------------------------

	//This dictionary should be updated to keep the correct database of scene list	
	//The UID mapping is not working on C#, need to update later.
	[Export] Godot.Collections.Dictionary<string, string> mSceneNameToUIDMapping = new Godot.Collections.Dictionary<string,string>();

	[Export] Godot.Collections.Dictionary<string, string> mSceneNameToPathMapping = new Godot.Collections.Dictionary<string,string>();

	[Export] Godot.Collections.Dictionary<string, string> mNonPersistantSceneNameToPathMapping = new Godot.Collections.Dictionary<string,string>();

	//This should point to the actual SceneTransitionReferenceHelper of the actual scene
	
	[Export] private SceneTransitionReferenceHelper mReferenceHelper;

	// ------------------------------------ Variable for logging-----------------------------------------

	[Export] protected bool mShouldLog;

	protected LogObject mLogObject; 

	// ------------------------------------ Methods -----------------------------------------

	public void HeavyTransitionToNewScene(string sceneName){
		if (!mSceneNameToPathMapping.ContainsKey(sceneName)){
			mLogObject.AddToLogString("trying to load scene not loaded in Scene database named: " + sceneName);
			return;
		}
		//First we remove the object pooler, since it is a singleton also present on the other scene.
		mReferenceHelper.ObjectPoolerNode=null;
		GameObjectPooler.Instance.PoolAllObjects();
		GameObjectPooler.Instance.EraseObjectPooler();
		//For a similar reason we erase the other elements. Dont want two of the same thing of a scene. But we keep the parent for adding nodes later.
		Node PersistentElementsParent = mReferenceHelper.PersistentElements.GetParent<Node>();
		Node nonPersistentElementsParent = mReferenceHelper.NonPersistentElements.GetParent<Node>();
		PersistentElementsParent.RemoveChild(mReferenceHelper.PersistentElements);
		nonPersistentElementsParent.RemoveChild(mReferenceHelper.NonPersistentElements);
		mReferenceHelper.PersistentElements.QueueFree();
		mReferenceHelper.PersistentElements=null;
		mReferenceHelper.NonPersistentElements.QueueFree();
		mReferenceHelper.NonPersistentElements=null;

		//We load the other scene
		Node newActualScene = ResourceLoader.Load<PackedScene>(mSceneNameToPathMapping[sceneName]).Instantiate(); 
		SceneTransitionReferenceHelper loadedReferenceHelper = (SceneTransitionReferenceHelper) newActualScene;

		//Add the new object pooler.
		loadedReferenceHelper.RemoveChild(loadedReferenceHelper.ObjectPoolerNode);
		mReferenceHelper.AddChild(loadedReferenceHelper.ObjectPoolerNode);
		mReferenceHelper.ObjectPoolerNode = loadedReferenceHelper.ObjectPoolerNode;

		//Change the non persistent elements for the new ones.
		mReferenceHelper.PersistentElements = loadedReferenceHelper.PersistentElements;
		loadedReferenceHelper.PersistentElements.GetParent<Node>().RemoveChild(loadedReferenceHelper.PersistentElements);
		PersistentElementsParent.AddChild(loadedReferenceHelper.PersistentElements);
	
		
		//Should update the audio bank and make the song transition.
		AudioManager.Instance.UpdateMusicBanks(loadedReferenceHelper.AudioBankContainerNode);

		//Change the non persistent elements for the new ones.
		mReferenceHelper.NonPersistentElements = loadedReferenceHelper.NonPersistentElements;
		loadedReferenceHelper.NonPersistentElements.GetParent<Node>().RemoveChild(loadedReferenceHelper.NonPersistentElements);
		nonPersistentElementsParent.AddChild(loadedReferenceHelper.NonPersistentElements);
	
        
		//Free the loaded scene from memory
		loadedReferenceHelper.SceneFinishedLoading();
		loadedReferenceHelper=null; newActualScene=null;
		
	}

	public void LightTransitionToNewScene(string sceneName){
		if (!mSceneNameToPathMapping.ContainsKey(sceneName)){
			mLogObject.AddToLogString("trying to load scene not loaded in Scene database named: " + sceneName);
			return;
		}
		
		GameObjectPooler.Instance.PoolAllObjects();

		Node newNonPersitanceScene = ResourceLoader.Load<PackedScene>(mNonPersistantSceneNameToPathMapping[sceneName]).Instantiate(); 

		//Change the non persistent elements for the new ones.
		Node nonPersistentElementsParent = mReferenceHelper.NonPersistentElements.GetParent<Node>();
		mReferenceHelper.NonPersistentElements.QueueFree();
		mReferenceHelper.NonPersistentElements = newNonPersitanceScene;
		nonPersistentElementsParent.AddChild(newNonPersitanceScene);
	}

	// Some nice methods for shaders when transition scene? Some animation? 

	
	//On ready complain if we dont have a reference to the SceneTransitionReferenceHelper.

	//On ready load

	public override void _Ready(){
		//If not any reference for SceneTransitionReferenceHelper give a warning
		if (mReferenceHelper == null){
			GD.PushWarning("No reference SceneTransitionReferenceHelper on Scene Transition Manager");
		}
		
		//The UID mapping is not working on C#, need to update later.
		//mSceneNameToPathMapping.Clear();

		//foreach (string keyString in mSceneNameToUIDMapping.Keys){
		//	mSceneNameToPathMapping.Add(keyString, ResourceUid.GetIdPath(ResourceUid.TextToId(mSceneNameToUIDMapping[keyString])));
		//}
		//Give the AudioManager the audio banks of the actual scene.
		AudioManager.Instance.UpdateMusicBanks(mReferenceHelper.AudioBankContainerNode);

	}

}
