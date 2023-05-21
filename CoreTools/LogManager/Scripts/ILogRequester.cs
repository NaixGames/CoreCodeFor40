using Godot;
using CoreCode.FSM;
using CoreCode.Scripts;

//This should be PROJECTNAME.ACTORNAME
namespace CoreCode.Scripts{
	public partial interface ILogRequester 
	{
		
		// -------------------------- Abstract overrides -------------------------------------

		public void SendMessageToLog(string message);

	}
}
