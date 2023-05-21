using Godot;
using System;

namespace CoreCode.Scripts{
	public partial class AchievementAPIPCDefault : AchievementAPIAbstract
	{
		public override void UnlockAchivement(string AchievementName){
			GD.Print("Achivement " + AchievementName + "event send by the test API");
		}
	}

}