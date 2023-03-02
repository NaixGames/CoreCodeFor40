using Godot;
using System;

public partial class AchievementManager : Node
{

	// ----------------------------------- Information ------------------------------------------------
	/*This is a script to manage achievements for Steam/Console. This is an adapter between the game and a API for the system.
	It also stores which achievements have been unlocked.*/
	
	// ------------------------------------ Use -------------------------------------------------------
	/* Achievements are unlocked using a call to this class, which done also communicates to the API.
	The API then sees how to deal with whatever achievements system in needs to deal with.
	*/


	// ------------------------------------- Singleton instantiation -------------------------------

	private static AchievementManager instance;
	public static AchievementManager Instance{
		get {return TryToReturnInstance();}
	}

	public static AchievementManager TryToReturnInstance(){
		if (instance == null){
			GD.PushWarning("instance of LogManager called before the instance was ready!");
			instance = new AchievementManager();
		}
		return instance;
	}

	public AchievementManager(){
		instance=this; 
	}

	// ------------------------------------- Variables -------------------------------
	private AchievementAPIAbstract mAPIReference;

	private Godot.Collections.Dictionary<string,bool> mAchievementsDictionary = new Godot.Collections.Dictionary<string,bool>();




	//---------------------Variables for loging
	
	[Export]
	private bool mShouldLog;
	private LogObject mLogObject;


	// ------------------------------------- Methods -------------------------------
	public override void _Ready()
	{
		//Set mAPIReference according to platform target. 
		#if GODOT_WINDOWS
			mAPIReference = new AchievementAPISteam();		
		#elif GODOT_PC
			mAPIReference = new AchievementAPIPCDefault();
		#endif

		if (mShouldLog){
			mLogObject = LogManager.Instance.RequestLogReference("Achievements", 0); //This should be use to send logs in certain events if needed.
		}

		LoadAchievementDatabase();
	}

	public void UnlockAchivement(string AchievementName){
		if (IsAchivementUnlocked(AchievementName)){
			return;
		}
		mAchievementsDictionary[AchievementName] = true;
		mAPIReference.UnlockAchivement(AchievementName);

		if (mShouldLog){
			mLogObject.AddToLogString(AchievementName + " Achivement has been unlocked");
		}
	}

	public bool IsAchivementUnlocked(string AchievementName){
		if (!mAchievementsDictionary.ContainsKey(AchievementName)){
			if (mShouldLog){
				mLogObject.AddToLogString(AchievementName + " has been called from the achivement dictionary, but does not exist!");
			}
			return false;
		}
		return mAchievementsDictionary[AchievementName];
	}


	public void LoadAchievementDatabase(){
		// Initiliaze posible Achievements here. May load them from a .csv in the future (so it is robust for translation)
		mAchievementsDictionary.Add("MyFirstAchivement", false);
	}
	//TO DO: Load data from save file.

}
