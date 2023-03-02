using Godot;
using System;

public interface IControlableByInput
{
	//Interface for giving an interface for dealing with inputs
	public InputReaderAbstract ReturnInputReader();

	public void ClearInputReader();

	public void RecieveInputReader(InputReaderAbstract inputReaderPath);
}