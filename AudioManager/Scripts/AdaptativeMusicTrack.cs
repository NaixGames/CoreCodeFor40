using Godot;
using System;

public partial class AdaptativeMusicTrack : AudioStreamPlayer
{
	// ----------------------------------- Information ------------------------------------------------
	/*This is a script to execute "complex" logic on music tracks, such a go to another track on finishing or
	phasing volume in or out.*/
	
	// ------------------------------------ Use -------------------------------------------------------
	/* The methods are pretty much self explanatory. Upon finishing the FollowUpTrack will play. 
	Note it can be itself, and so in that case the track will loop. Prefer this to standard "looping" settings in Godot, 
	due to those never trigerring the "OnFinished" status. (And hence cutting flexibility). */

	// ------------------------------------ Variables ------------------------------------------------

	private enum PhasingOutMode{
		None,
		PhasingIn,
		PhasingOut
	}

	private PhasingOutMode mPhaseMode=PhasingOutMode.None;

	[Export] private AdaptativeMusicTrack mFollowUpTrack;


	private const float mSpeedPhasing = 0.5f;


	//Variables for changing track;

	private double mDelayTime;

	private double mAudioLength;

	private double mExactTimeInTrack=0;

	// ------------------------------------ Methods ------------------------------------------------

	public void PhaseOut(){
		mPhaseMode = PhasingOutMode.PhasingOut;
	}

	public void PhaseIn(float startime = 0f, float StartingLinearDB=1){
		PlayTrack(startime);
		if (StartingLinearDB==1){
			return;
		}
		this.VolumeDb=Godot.Mathf.LinearToDb(StartingLinearDB);
		mPhaseMode = PhasingOutMode.PhasingIn;
	}


	public void PlayTrack(float startime = 0f){
		if(mFollowUpTrack==null){
			mFollowUpTrack = this; //By default tracks will loop into themselves.
		}
		mDelayTime = AudioServer.GetTimeToNextMix() + AudioServer.GetOutputLatency();
		mAudioLength = this.Stream.GetLength();
		this.Play(startime);
		this.SetProcess(true);
	}

	public void StopTrack(){
		this.Stop();
		this.SetProcess(false);
	}

	//This executes when the track finishes!
	private void SetNextTrack(){ 
		this.SetProcess(false);
		mFollowUpTrack.PlayTrack();
		mFollowUpTrack.VolumeDb = this.VolumeDb;
		mFollowUpTrack.mPhaseMode=this.mPhaseMode;
		if (this.mPhaseMode!=PhasingOutMode.PhasingOut){
			AudioManager.Instance.MusicManager.ActualMusicTrack = mFollowUpTrack;
		}
	}


	// ------------------------------------ Basic overrides ------------------------------------------------
	public override void _Ready(){
		this.StopTrack();
	}
	
	public override void _Process(double delta)
	{
		if (!this.Playing){
			this.SetProcess(false);
			if (mFollowUpTrack.Playing){
				return;
			}
			SetNextTrack();
		}
		if (mPhaseMode==PhasingOutMode.PhasingIn){
			this.VolumeDb=Godot.Mathf.LinearToDb(Mathf.Min(1f,Godot.Mathf.DbToLinear(this.VolumeDb)+mSpeedPhasing*(float)delta));
			if (Godot.Mathf.DbToLinear(this.VolumeDb)==1){
				mPhaseMode=PhasingOutMode.None;
			}
		}
		if (mPhaseMode==PhasingOutMode.PhasingOut){
			this.VolumeDb=Godot.Mathf.LinearToDb(Mathf.Max(0f,Godot.Mathf.DbToLinear(this.VolumeDb)-mSpeedPhasing*(float)delta));
			if (Godot.Mathf.DbToLinear(this.VolumeDb)<0.001f){
				mPhaseMode=PhasingOutMode.None;
				this.StopTrack();
				return;
			}
		}
		mExactTimeInTrack = this.GetPlaybackPosition() + AudioServer.GetTimeSinceLastMix() + AudioServer.GetTimeToNextMix()+AudioServer.GetOutputLatency();
		if (mExactTimeInTrack > this.mAudioLength){
			SetNextTrack();
		}
		
	}

}
