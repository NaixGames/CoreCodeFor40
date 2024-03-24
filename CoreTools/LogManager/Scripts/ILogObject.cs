using Godot;
using System;
using System.Diagnostics;

namespace CoreCode.Scripts{
	public partial interface ILogObject
	{
		//Interface for having logs we can easily reditect as we want.
		//Heavily based on ChickenSoft's GodotLot.


		/// <summary>
		/// Prints the specified message to the log.
		/// </summary>
		/// <param name="message">Message to output.</param>
		void Print(string message);

		/// <summary>
		/// Displays a stack trace in a convenient format.
		/// </summary>
		/// <param name="stackTrace">Stack trace to output.</param>
		void Print(StackTrace stackTrace);

		/// <summary>
		/// Prints an exception.
		/// </summary>
		/// <param name="e">Exception to print.</param>
		void Print(Exception e);

		/// <summary>
		/// Adds a warning message to the log.
		/// </summary>
		/// <param name="message">Message to output.</param>
		void Warn(string message);

		/// <summary>
		/// Adds an error message to the log.
		/// </summary>
		/// <param name="message">Message to output.</param>
		void Err(string message);

		/// <summary>
		/// Asserts that condition is true, or else logs and throws an exception.
		/// </summary>
		/// <param name="condition">Condition to assert.</param>
		/// <param name="message">Message to use for error logs and
		/// exception.</param>
		void Assert(bool condition, string message);

	}
}