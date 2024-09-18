using Godot;

namespace CoreCode.Scripts{
	[Tool]
	public abstract partial class SingleNode<T> : Node
	where T: SingleNode<T>
	{

		// Information
		/* Base class for having singletons that are nodes. Useful for making sure Managers all have the
		same interace*/
			
		// Use 
		/* This class give methods to load scenes given a path to the scene. This node should eventually
		support shaders for scene transition to make this transition (duh) more smooth*/

		// Singleton instantiation 
		private static T instance;
		public static T Instance{
			get {return TryToReturnInstance();}
		}

		public static T TryToReturnInstance(){
			if (Engine.IsEditorHint()){
				return null;
			}
			if (instance == null){
				GD.PushWarning("Instance of" + typeof(T).ToString() + "SingleNode called before the instance was ready!");
				return null;
			}
			return instance;
		}

		// Variable for logging
		[Export] protected bool mShouldLog;
		[Export] protected string mLogChannel;
		protected ILogObject mLogObject; 


		// Dealing with creation and destruction
		public override void _EnterTree()
		{
			if (Engine.IsEditorHint()){
				return;
			}
			if (instance == null){
				instance = this as T; 
			}
			else{
				GD.PushWarning("Instance of" + typeof(T).ToString() + "created when there is an existing instance!");
			}

			mLogObject = LogManager.Instance.RequestLog(mLogChannel, mShouldLog);
		}

	
		public override void _ExitTree()
		{
			instance = null;
		}
	}
}