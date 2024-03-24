using CoreCode.Scripts;
using Godot;
using System;
using System.Diagnostics;


namespace CoreCode.Scripts{
	public partial class EditorLogObject : ILogObject
	{
		// ----------------------------------- Information ------------------------------------------------
		/*This is a script to have a Log for different part of the game and print the relevant messages to editor when needed.
		
		This version is based on ChickenSoft GDLog.cs*/
		
		// ------------------------------------ Use -------------------------------------------------------
		/* Different instances of this object are dedicated to different part of the game (input, FSM, object pooler, etc.)
		There is a Log Manager script, that works as a Service Locator (plus manager) to this instances.
		*/


		// ------------------------------------- Variables -----------------------------------------------

		private string mPrefix;

		// ------------------------------------- Methods -----------------------------------------------

		public EditorLogObject(string prefix) {
			mPrefix = prefix;
		}

		// Base class actions to delegate to Godot

		public static readonly Action<string> DefaultPrint	= GD.Print;
		public static Action<string> PrintAction { get; set; } = DefaultPrint;
		public static readonly Action<string> DefaultPushWarning = GD.PushWarning;
  		public static Action<string> PushWarningAction { get; set; } = DefaultPushWarning;
		public static readonly Action<string> DefaultPushError = GD.PushError;
  		public static Action<string> PushErrorAction { get; set; } = DefaultPushError;


		// ------------------------------------- Interface Methods -----------------------------------------------

		public void Print(string message) => PrintAction(mPrefix + ": " + message);

		public void Print(StackTrace stackTrace) {
			foreach (StackFrame frame in stackTrace.GetFrames()) {
				string fileName = frame.GetFileName() ?? "**";
				int lineNumber = frame.GetFileLineNumber();
				int colNumber = frame.GetFileColumnNumber();
				System.Reflection.MethodBase method = frame.GetMethod();
				string className = method?.DeclaringType?.Name ?? "UnknownClass";
				string methodName = method?.Name ?? "UnknownMethod";
				Print(
					$"{className}.{methodName} in " +
					$"{fileName}({lineNumber},{colNumber})"
				);
			}
		}

		// -------------------------------------

		public void Print(Exception e) {
			Err("An error ocurred.");
			Err(e.ToString());
		}

		// -------------------------------------

		public void Warn(string message) {
			PushWarningAction(mPrefix + ": " + message);
		}

		// -------------------------------------

		public void Err(string message) {
			PushErrorAction(mPrefix + ": " + message);
		}

		// -------------------------------------

		public void Assert(bool condition, string message) {
			if (!condition) {
			Err(message);
			throw new AssertionException(message);
			}
		}

		// -------------------------------------

	}
}