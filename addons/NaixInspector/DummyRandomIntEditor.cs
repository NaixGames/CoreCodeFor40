#if TOOLS
using Godot;

namespace CoreCode.Docker{
    public partial class DummyRandomIntEditor : EditorProperty
    {
        // The main control for editing the property.
        private Button _propertyControl = new Button();
        // An internal value of the property.
        private int _currentValue = 0;
        // A guard against internal changes when the property is updated.
        private bool _updating = false;

        public DummyRandomIntEditor()
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

            // Generate a new random integer between 0 and 99.
            _currentValue = (int)GD.Randi() % 100;
            RefreshControlText();
            EmitChanged(GetEditedProperty(), _currentValue);
            GD.Print("Here " + _currentValue);
        }

        public override void _UpdateProperty()
        {
            // Read the current value from the property.
            var newValue = (int)GetEditedObject().Get(GetEditedProperty());
            GD.Print("Here again " + _currentValue + " " + newValue);
            if (newValue == _currentValue)
            {
                return;
            }

            // Update the control with the new value.
            _updating = true;
            _currentValue = newValue;
            RefreshControlText();
            _updating = false;
        }

        private void RefreshControlText()
        {
            _propertyControl.Text = $"Value: {_currentValue}";
        }
    }
}
#endif
