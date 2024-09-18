using CoreCode.FSM;
using Godot;

namespace CoreCode.Example.DummyPlayerFSM{
    [Tool]
    public partial class TestStateManagerPointer : StateManagerPointer
    {
        public override StateManagerAbstract GiveStateManagerInstance(){
            return new PlayerStateManagerExample();
        }

        public override string GiveNamespaceString()
        {
            return "CoreCode.Example.DummyPlayerFSM";
        }

    }
}
