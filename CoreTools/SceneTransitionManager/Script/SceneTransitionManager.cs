using Godot;
using CoreCode.AudioSystem;


namespace CoreCode.Scripts{
	[Tool]
	public partial class SceneTransitionManager : Node
	{
		// Information
		/*This is a script to allow Scene transition in godot in a nice way. That is, preserving singletons
		and persistant elements betweens scene (like the player), while also not needed to have scene be 
		"stripped down in the editor"*/
		
		// Use 
		/* This class give methods to load scenes given a path to the scene. This node should eventually
		support shaders for scene transition to make this transition (duh) more smooth*/

		// Singleton instantiation 
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
		// Variables
		[Export] private SceneDatabase mSceneDatabase;
		[Export] private SceneTransitionAnimator mSceneTransitionAnimator;
		public SceneTransitionAnimator SceneTransitionAnimator => mSceneTransitionAnimator;
		private SceneTransitionReferenceHelper mReferenceHelper;

		private bool mIsLoading;
		public bool IsLoading => mIsLoading;

		// Variable for logging
		[Export] protected bool mShouldLog;
		protected ILogObject mLogObject; 

		// Methods 

		public async void HeavyTransitionToNewScene(string sceneName, int fadeDuration = -1){
			if (!mSceneDatabase.mSceneNameToPathMapping.ContainsKey(sceneName)){
				mLogObject.Err("trying to load scene not loaded in Scene database named: " + sceneName);
				return;
			}

			EmitSignal(SignalName.OnSceneLoadingStarted);
			mIsLoading = true;

			if (mSceneTransitionAnimator != null){
				await mSceneTransitionAnimator.DoFadeOutAnimation(fadeDuration);
			}

			//First we remove the object pooler, since it is a singleton also present on the other scene.
			mReferenceHelper.ObjectPoolerNode=null;
			if (GameObjectPooler.Instance != null)
			{
				GameObjectPooler.Instance.PoolAllObjects();
				GameObjectPooler.Instance.Free();
			}
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
			ProcessNewElements(loadedReferenceHelper.ObjectPoolerNode);
			mReferenceHelper.ObjectPoolerNode = loadedReferenceHelper.ObjectPoolerNode;

			//Change the persistent elements for the new ones.
			loadedReferenceHelper.PersistentElements.GetParent<Node>().RemoveChild(loadedReferenceHelper.PersistentElements);
			ProcessNewElements(loadedReferenceHelper.PersistentElements);
			mReferenceHelper.PersistentElements = loadedReferenceHelper.PersistentElements;
			
			//Should update the audio bank and make the song transition.
			AudioManager.Instance.UpdateMusicBanks(loadedReferenceHelper.AudioBankContainerNode);

			//Change the non persistent elements for the new ones.
			loadedReferenceHelper.NonPersistentElements.GetParent<Node>().RemoveChild(loadedReferenceHelper.NonPersistentElements);
			ProcessNewElements(loadedReferenceHelper.NonPersistentElements);
			mReferenceHelper.NonPersistentElements = loadedReferenceHelper.NonPersistentElements;
			
			//Free the loaded scene from memory
			loadedReferenceHelper.SceneFinishedLoading();	

			if (mSceneTransitionAnimator != null){
				await mSceneTransitionAnimator.DoFadeInAnimation(fadeDuration);
			}		

			mIsLoading = false;
			EmitSignal(SignalName.OnSceneLoadingEnded);
		}

		public async void LightTransitionToNewScene(string sceneName, int fadeDuration = -1){
			if (!mSceneDatabase.mNonPersistantSceneNameToPathMapping.ContainsKey(sceneName)){
				mLogObject.Err("trying to load scene not loaded in Scene database named: " + sceneName);
				return;
			}
			
			EmitSignal(SignalName.OnSceneLoadingStarted);
			mIsLoading = true;

			if (mSceneTransitionAnimator != null){
				await mSceneTransitionAnimator.DoFadeOutAnimation(fadeDuration);
			}

			if (GameObjectPooler.Instance != null)
			{
				GameObjectPooler.Instance.PoolAllObjects();
			}
			
			mReferenceHelper.NonPersistentElements.QueueFree();

			Node newNonPersitanceScene = ResourceLoader.Load<PackedScene>(mSceneDatabase.mNonPersistantSceneNameToPathMapping[sceneName]).Instantiate(); 

			//Change the non persistent elements for the new ones.
			ProcessNewElements(newNonPersitanceScene);
			mReferenceHelper.NonPersistentElements = newNonPersitanceScene;

			if (mSceneTransitionAnimator != null){
				await mSceneTransitionAnimator.DoFadeInAnimation(fadeDuration);
			}	

			mIsLoading = false;
			EmitSignal(SignalName.OnSceneLoadingEnded);
		}


		private void ProcessNewElements(Node newElementsNode){
			newElementsNode.Owner = null;
			mReferenceHelper.AddChild(newElementsNode);
			newElementsNode.Owner = mReferenceHelper.Owner;
		}



		public override void _Ready(){
			if (Engine.IsEditorHint()){
				return;
			}
			Initialize();
		}


		private void Initialize(){
			mLogObject = LogManager.Instance.RequestLog("SceneTransitions", mShouldLog);
			
			//If we dont have a SceneDatabase give a warning.
			if (mSceneDatabase == null){
				mLogObject.Warn("No scene database provided. No scene transition will be posible!");
			}	
			
			foreach (Node childNode in GetTree().Root.GetChildren()){
				mReferenceHelper = childNode as SceneTransitionReferenceHelper;
				if (mReferenceHelper != null){
					break;
				}
			}

			if (mReferenceHelper == null){
				mLogObject.Warn("No reference SceneTransitionReferenceHelper as base. Audio and Scene Transitions will not work correclty");
				return;		
			}
	
			mReferenceHelper.GetNodesFromPaths();
			AudioManager.Instance.UpdateMusicBanks(mReferenceHelper.AudioBankContainerNode);	
		}


		//Signals
		[Signal]
		public delegate void OnSceneLoadingStartedEventHandler();

		[Signal]
		public delegate void OnSceneLoadingEndedEventHandler();

	}
}
