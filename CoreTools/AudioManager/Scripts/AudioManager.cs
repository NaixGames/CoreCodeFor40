using Godot;
using System;
using CoreCode.Scripts;

namespace CoreCode.AudioSystem{
	public partial class AudioManager : Node
	{
		// ----------------------------------- Information ------------------------------------------------
		/*This is a script to allow playing music and sounds and loading sound banks correctly when needed*/
		
		// ------------------------------------ Use -------------------------------------------------------
		/* The node of this class should have three offsprings: one AdapativeMusic class node,
		one node that has as offsprings the StreamPlayer for global SFX and a node that contains SFXV that can be played at certain positions.
		Note that in this version audio banks for SFX are always loaded. 
		
		Use the methods that make request to MusicManager to change music, for SFX this class plenty of methods.
		MAKE SURE TO LOAD THE SFX BANKS ON THE EDITOR!*/

		// ------------------------------------- Singleton instantiation -------------------------------
		private static AudioManager instance;
		public static AudioManager Instance{
			get {return TryToReturnInstance();}
		}

		public static AudioManager TryToReturnInstance(){
			if (instance == null){
				GD.PushWarning("Instance of AudioManager called before the instance was ready!");
				instance = new AudioManager();
			}
			return instance;
		}


		public AudioManager(){
			if (instance==null){
				instance=this; 
			}
			else{ //This may happen naturally when loading scenes additively. 
				GD.PushWarning("Instance of AudioManager created when there is an existing instance!");
			}
		}


		// ------------------------------------ Variable for logging-----------------------------------------

		[Export] protected bool mShouldLog;

		protected ILogObject mLogObject;


		// ------------------------------------ Variables-----------------------------------------

		[Export] private NodePath mMusicManagerPath;
		private AdaptativeMusicManager mMusicManager;

		public AdaptativeMusicManager MusicManager{
			get{return mMusicManager;}
		}

		// ------------------------------------ Variable for maping scene names to Music folder-----------------------------------------

		[Export] private bool mPlayMusicAtStart;


		[Export] private NodePath mGlobalSFXNodeParentPath;
		private Node mGlobalSFXNodeParent;

		private Godot.Collections.Dictionary<string, AudioStreamPlayer> mGlobalSFXMap = new Godot.Collections.Dictionary<string,AudioStreamPlayer>();

		[Export] private NodePath mPositionalSFXNodeParentPath;
		private Node mPositionalSFXNodeParent;

		private Godot.Collections.Dictionary<string, AudioStreamPlayer2D> mPositionalSFXMap = new Godot.Collections.Dictionary<string,AudioStreamPlayer2D>();



		// ------------------------------------ Methods -----------------------------------------

		//This should be loaded in the editor to save time!
		public void PopulateMusicAndSFXDictionaries(){
			GetMusicManagerAndSFXReferences();
			
			//This now gets called from the SceneTransitionReferenceHelper when it loads the audio banks
			//UpdateMusicTracks();

			mGlobalSFXMap.Clear();
			foreach (AudioStreamPlayer mStreamPlayer in mGlobalSFXNodeParent.GetChildren()){
				mGlobalSFXMap.Add(mStreamPlayer.Name, mStreamPlayer);
			}
			mGlobalSFXMap.Clear();
			foreach (AudioStreamPlayer2D mStreamPlayer2D in mPositionalSFXNodeParent.GetChildren()){
				mPositionalSFXMap.Add(mStreamPlayer2D.Name, mStreamPlayer2D);
			}
		}

		public void PlayMusic(){
			if (mShouldLog){
				mLogObject.Print("Playing music");
			}
			mMusicManager.Play();
		}

		public void StopMusic(){
			if (mShouldLog){
				mLogObject.Print("Stopping music");
			}
			mMusicManager.Stop();
		}

		public void TransitionToOtherTrack(int TrackID){
			if (mShouldLog){
				mLogObject.Print("Transitioning music to track " + TrackID);
			}
			mMusicManager.TransitionToOtherTrack(TrackID);
		}

		public void UpdateMusicTracks(){
			if (mShouldLog){
				mLogObject.Print("Loading music tracks ");
			}
			mMusicManager.UpdateMusicTracks();
		}

		public void PlayGlobalSFX(string SfxIdentifier){
			if (mShouldLog){
				mLogObject.Print("Playing SFX " + SfxIdentifier);
			}
			mGlobalSFXMap[SfxIdentifier].Play();
		}

		public void PlaySFXAtPosition(string SfxIdentifier, Vector2 position){
			if (mShouldLog){
				mLogObject.Print("Playing SFX " + SfxIdentifier + " at position " + position);
			}
			mPositionalSFXMap[SfxIdentifier].Position = position;
			mPositionalSFXMap[SfxIdentifier].Play();
		}


		private void ChangeMasterBusLevel(float level){
			if (mShouldLog){
				mLogObject.Print("Putting master level volume at level " + level);
			}
			AudioServer.SetBusVolumeDb(0, level);
		}

		private void GetMusicManagerAndSFXReferences(){
			mMusicManager = GetNode<AdaptativeMusicManager>(mMusicManagerPath);
			mGlobalSFXNodeParent = GetNode<Node>(mGlobalSFXNodeParentPath);
			mPositionalSFXNodeParent = GetNode<Node>(mPositionalSFXNodeParentPath);
		}


		public void UpdateMusicBanks(Node AudioBankContainer){
			PopulateMusicAndSFXDictionaries();
			mMusicManager.UpdateMusicBanks(AudioBankContainer);
		}

		//Need to trigger an event on pause so all audio that is NOT pause SFX gets cut by half.
		// So OnPause -> ChangeMasterBusLevel(-3f). OffPause ->ChangeMasterBusLevel(0f). (Remember DB scale!)

		// ---------------------------------- Basic godot overrides ---------------------------

		public override void _Ready(){
			base._Ready();
			if (mMusicManager==null){
				PopulateMusicAndSFXDictionaries();
			}
			if (mShouldLog){
				mLogObject = LogManager.Instance.RequestLog("Audio");
			}
			if (!mMusicManager.HasMusic()){
				return;
			}
			if (mPlayMusicAtStart){
				PlayMusic();
			}
			else{
				StopMusic();
			}
		}		
	}
}