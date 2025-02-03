using Godot;
using System.Threading.Tasks;

namespace CoreCode.Scripts{
	[Tool]
	public partial class SceneTransitionManager : SingleNode<SceneTransitionManager>
	{
		// Information
		/*This is a script to allow Scene transition in godot in a nice way. That is, preserving singletons
		and persistant elements betweens scene (like the player), while also not needed to have scene be 
		"stripped down in the editor"*/
		
		// Use 
		/* This class give methods to load scenes given a path to the scene. This node should eventually
		support shaders for scene transition to make this transition (duh) more smooth*/

		// Variables
		[Export] private SceneDatabase mIdToPackedSceneAsset;
		[Export] private SceneTransitionAnimator mSceneTransitionAnimator;
		public SceneTransitionAnimator SceneTransitionAnimator => mSceneTransitionAnimator;
		private SceneTransitionReferenceHelper mReferenceHelper;
		private bool mIsLoading;
		public bool IsLoading => mIsLoading;
		

		// Methods 

		public async void TransitionToNewScene(PackedScene sceneToLoad, bool isHeavyLoad = true, int fadeDuration = -1, bool cleanPooler = false){
			EmitSignal(SignalName.OnSceneLoadingStarted);
			mIsLoading = true;

			if (mSceneTransitionAnimator != null){
				await mSceneTransitionAnimator.DoFadeOutAnimation(fadeDuration);
				EmitSignal(SignalName.OnSceneFadeOutEnded);
			}

			GameObjectPooler.Instance.PoolAllObjects();

			if (cleanPooler)
			{
				GameObjectPooler.Instance.CleanObjectPooler();
			}

			//We load the other scene
			Node newActualScene = sceneToLoad.Instantiate(); 
			SceneTransitionReferenceHelper loadedReferenceHelper = (SceneTransitionReferenceHelper) newActualScene;
			if (loadedReferenceHelper == null)
			{
				mLogObject.Err("Tried to load a scene without the correct format. This wont work!");
				return;
			}
			loadedReferenceHelper.GetNodesFromPaths();

			SetupNewScene(loadedReferenceHelper, isHeavyLoad);

			if (mSceneTransitionAnimator != null){
				await mSceneTransitionAnimator.DoFadeInAnimation(fadeDuration);
				EmitSignal(SignalName.OnSceneFadeInEnded);
			}

			mIsLoading = false;
			EmitSignal(SignalName.OnSceneLoadingEnded);
		}


		public void TransitionToNewScene(string sceneIdToLoad, bool isHeavyLoad = true, int fadeDuration = -1, bool cleanPooler = false){
			if (!mIdToPackedSceneAsset.SceneIdToPackedScene.ContainsKey(sceneIdToLoad)){
				mLogObject.Err("Trying to load scene id that does not exist!" + sceneIdToLoad);
			}

			TransitionToNewScene(mIdToPackedSceneAsset.SceneIdToPackedScene[sceneIdToLoad], isHeavyLoad, fadeDuration, cleanPooler);
		}



		private void SetupNewScene(SceneTransitionReferenceHelper newScene, bool isHeavyLoad)
		{
			if (!isHeavyLoad){
				newScene.PersistentElements.QueueFree();
				Node pastPersistentElements = mReferenceHelper.PersistentElements;
				newScene.PersistentElements = pastPersistentElements;
				mReferenceHelper.RemoveChild(mReferenceHelper.PersistentElements);
				newScene.AddChild(pastPersistentElements);
			}	

			mReferenceHelper.QueueFree();
			mReferenceHelper = newScene;
			GetTree().Root.AddChild(mReferenceHelper);
		}



		public override void _Ready(){
			if (Engine.IsEditorHint()){
				return;
			}
			Initialize();
		}


		private void Initialize(){
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
		}


		//Signals
		[Signal]
		public delegate void OnSceneLoadingStartedEventHandler();
		[Signal]
		public delegate void OnSceneFadeOutEndedEventHandler();

		[Signal]
		public delegate void OnSceneFadeInEndedEventHandler();

		[Signal]
		public delegate void OnSceneLoadingEndedEventHandler();

	}
}
