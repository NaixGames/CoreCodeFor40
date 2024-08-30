using Godot;
using System.Threading.Tasks;

namespace CoreCode.Scripts{
	[Tool]
	public partial class SceneTransitionAnimator : Node
	{
		[Export] private int mFadeTime=250;
		[Export] private ColorRect mBlackScreen;
		private Tween mTween;

		public async Task DoFadeOutAnimation(int fadeInput = -1){
			int fadeTime = fadeInput > 0 ? fadeInput : mFadeTime;
			GetTree().Paused = true;
			mTween = CreateTween();
			mTween.TweenProperty(mBlackScreen,"color", new Color(0,0,0,1), fadeTime/1000f).SetTrans(Tween.TransitionType.Circ).SetEase(Tween.EaseType.In);
			await ToSignal(mTween, "finished");
			EmitSignal(SignalName.OnFadeOutAnimationEnded);
		}

		public async Task DoFadeInAnimation(int fadeInput = -1){
			int fadeTime = fadeInput > 0 ? fadeInput : mFadeTime;
			GetTree().Paused = false;
			mTween = CreateTween();
			mTween.TweenProperty(mBlackScreen,"color", new Color(0,0,0,0), fadeTime/1000f).SetTrans(Tween.TransitionType.Circ).SetEase(Tween.EaseType.Out);
			await ToSignal(mTween, "finished");
			EmitSignal(SignalName.OnFadeInAnimationEnded);
		}


		[Signal]
		public delegate void OnFadeOutAnimationEndedEventHandler();

		[Signal]
		public delegate void OnFadeInAnimationEndedEventHandler();

	}
}