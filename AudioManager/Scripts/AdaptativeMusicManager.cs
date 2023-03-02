using Godot;
using System;
using System.Collections.Generic;

public partial class AdaptativeMusicManager : Node
{
	// ----------------------------------- Information ------------------------------------------------
	/*This is a script to allow playing adapatative music and dealing with complex music logic.*/
	
	// ------------------------------------ Use -------------------------------------------------------
	/* Your assets for certain scene should be in Asset/Sound/Music and then separated by folder.
	use  ChangeMusicConfiguration(string NewMusicConfigurationPath) to Load the new music files from that folder.
	By default the first file will be consider intro, and then go into the second file. All the other ones will loop*/


	// ------------------------------------ Variable for logging-----------------------------------------

	[Export] protected bool mShouldLog;

	protected LogObject mLogObject;


	// ------------------------------------ Variables-----------------------------------------

	public AdaptativeMusicTrack ActualMusicTrack;

	private List<AdaptativeMusicTrack> mMusicTrackSceneList = new List<AdaptativeMusicTrack>();


	// ------------------------------------ Methods -----------------------------------------

	public void Play(){
		if (ActualMusicTrack==null){
			ActualMusicTrack = mMusicTrackSceneList[0];
		}
		ActualMusicTrack.PlayTrack();
	}

	public void Stop(){
		if (ActualMusicTrack==null){
			ActualMusicTrack = mMusicTrackSceneList[0];
		}
		ActualMusicTrack.Stop();
	}

	public void TransitionToOtherTrack(int TrackID){
		//For TrackID 0-> introduction, 1-> MainLoopingtrack, 2-> Alternativetrack and so on.
		GD.Print("TrackID: " + TrackID + " " + ActualMusicTrack.Name + " " + mMusicTrackSceneList[TrackID].Name);

		if (ActualMusicTrack == mMusicTrackSceneList[TrackID]){
			return;
		}
		ActualMusicTrack.PhaseOut();
		mMusicTrackSceneList[TrackID].PhaseIn(ActualMusicTrack.GetPlaybackPosition()%((float)mMusicTrackSceneList[TrackID].Stream.GetLength()),0);

		ActualMusicTrack = mMusicTrackSceneList[TrackID];		
	}


	public void UpdateMusicTracks()
	{
		mMusicTrackSceneList.Clear();
		foreach (AdaptativeMusicTrack mMusicTrack in this.GetChildren()){
			mMusicTrackSceneList.Add(mMusicTrack);
		}
	}

	public bool HasMusic(){
		return (mMusicTrackSceneList.Count>0);
	}

}
