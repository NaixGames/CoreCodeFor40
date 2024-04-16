using Godot;
using System;
using CoreCode.Scripts;
using System.Reflection;

namespace CoreCode.FSM
{
    [GlobalClass, Tool]
    public  partial class StateManagerPointer : Resource
    {
        [Export] public string StateManagerClassPath;
        public StateManagerAbstract GiveStateManagerInstance(){
            try{
                return (StateManagerAbstract)Assembly.GetAssembly(typeof(StateManagerAbstract)).CreateInstance(StateManagerClassPath);
            }
            catch(Exception e){
                GD.PrintErr("State manager casting failed", e);
                return null;
            }
        }
    }
}
