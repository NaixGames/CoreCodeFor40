using Godot;
using System;
using System.Diagnostics;

namespace CoreCode.Scripts{
	public partial class VoidLogObject : ILogObject
	{
		// ----------------------------------- Information ------------------------------------------------
		/*This is a script to avoid logging at all if a certain log channel is turn off in LogManager. */
		
		// ------------------------------------- Interface Methods -----------------------------------------------

		public void Print(string message){
			return;
		}

		public void Print(StackTrace stackTrace) {
			return;
		}

		// -------------------------------------

		public void Print(Exception e) {
			return;
		}

		// -------------------------------------

		public void Warn(string message) {
			return;
		}

		// -------------------------------------

		public void Err(string message) {
			GD.PrintErr(message);
		}

		// -------------------------------------

		public void Assert(bool condition, string message) {
			if (!condition)
			{
				GD.PrintErr(message);
			}
		}

		// -------------------------------------
	}
}

