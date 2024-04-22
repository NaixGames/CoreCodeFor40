#if TOOLS
using Godot;
using CoreCode.Scripts;
using CoreCode.FSM;


namespace CoreCode.Inspector{
	public partial class StateManagerPointerInspectorPluging : EditorProperty
	{
		// The main control for editing the property.
		private Button _propertyControl = new Button();
		
		// A guard against internal changes when the property is updated.
		private bool _updating = false;

		public StateManagerPointerInspectorPluging()
		{
			// Add the control as a direct child of EditorProperty node.
			AddChild(_propertyControl);
			// Make sure the control is able to retain the focus.
			AddFocusable(_propertyControl);
			// Setup the initial state and connect to the signal to track changes.
			RefreshControlText();
			_propertyControl.Pressed += OnButtonPressed;
		}

		private void OnButtonPressed()
		{
			// Ignore the signal if the property is currently being updated.
			if (_updating)
			{
				return;
			}
			// Update the input maps
			_updating = true;
			StateManagerPointer managerPointer = (StateManagerPointer)GetEditedObject();
			managerPointer.PrintStateManagerGraph();
			//Call the object, get the method from there.
			_updating = false;

		}


		private void RefreshControlText()
		{
			_propertyControl.Text = $"Print graph";
		}
	}
}
#endif