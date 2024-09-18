using Godot;
using System;

namespace CoreCode.Scripts{

	public partial class AchievementManager : SingleNode<AchievementManager> 
	{

		// Information
		/*This is a script to manage achievements for Steam/Console. This is an adapter between the game and a API for the system.
		It also stores which achievements have been unlocked.*/
		
		// Use 
		/* Achievements are unlocked using a call to this class, which done also communicates to the API.
		The API then sees how to deal with whatever achievements system in needs to deal with.
		*/

		// Variables
		private AchievementAPIAbstract mAPIReference;

		private Godot.Collections.Dictionary<string,bool> mAchievementsDictionary = new Godot.Collections.Dictionary<string,bool>();



		// Methods
		public override void _Ready()
		{
			//Set mAPIReference according to platform target. 
			#if GODOT_WINDOWS
				mAPIReference = new AchievementAPISteam();		
			#elif GODOT_PC
				mAPIReference = new AchievementAPIPCDefault();
			#endif

			LoadAchievementDatabase();
		}

		public void UnlockAchivement(string AchievementName){
			if (IsAchivementUnlocked(AchievementName)){
				return;
			}
			mAchievementsDictionary[AchievementName] = true;
			mAPIReference.UnlockAchivement(AchievementName);

			if (mShouldLog){
				mLogObject.Print(AchievementName + " Achivement has been unlocked");
			}
		}

		public bool IsAchivementUnlocked(string AchievementName){
			if (!mAchievementsDictionary.ContainsKey(AchievementName)){
				if (mShouldLog){
					mLogObject.Print(AchievementName + " has been called from the achivement dictionary, but does not exist!");
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
}