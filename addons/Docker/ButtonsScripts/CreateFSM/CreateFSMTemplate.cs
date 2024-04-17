#if TOOLS
using Godot;
using CoreCode.FSM;
using CoreCode.Scripts;

namespace  CoreCode.Docker
{
    [Tool]
    public partial class CreateFSMTemplate : Button
    {
        private LineEdit mPathStringInputNode;
        private LineEdit mFSMNameStringInputNode;
        private LineEdit mInitialStateNameStringInputNode;
        private ItemList mFSMCaseNode;

        private const string ProjectName = "CoreCode";
        private const string PathScriptFSMActor="res://CoreTools/FSM/Scripts/StateMachineActor.cs";
        private const string PathScriptFSMAI="res://CoreTools/FSM/Scripts/StateMachineAIInput.cs";
        private const string PathStringGameActorReference2D="res://CoreTools/GameActorReferenceHandler/Script/GameActorReferenceHandler2D.cs";
        private const string PathStringGameActorReference3D="res://CoreTools/GameActorReferenceHandler/Script/GameActorReferenceHandler3D.cs";
        private const string PathScriptStateTemplate="res://addons/ScriptTemplates/Node/StateTemplate.cs";
        private const string PathScriptStateManagerTemplate="res://addons/ScriptTemplates/Node/StateManagerTemplate.cs";

        public override void _EnterTree()
        {
            Pressed += Clicked;
            mPathStringInputNode = this.GetParent<Node>().GetNode<LineEdit>("FSMPathInputBox");
            mFSMNameStringInputNode = this.GetParent<Node>().GetNode<LineEdit>("FSMNameInputBox");
            mInitialStateNameStringInputNode = this.GetParent<Node>().GetNode<LineEdit>("InitialStateNameInputBox");
            mFSMCaseNode = this.GetParent<Node>().GetNode<ItemList>("FSMCase");
        }

        public void Clicked()
        {
            string mPath = mPathStringInputNode.Text;
            string mName = mFSMNameStringInputNode.Text;
            string mInitialStateName = mInitialStateNameStringInputNode.Text;

            string NameCorrected;
            if (mFSMCaseNode.IsSelected(2)){
                NameCorrected = "AI"+mName;
            }
            else{
                NameCorrected="Actor"+mName;
            }

            //Should create the paths I am using here
            DirAccess.MakeDirAbsolute(mPath+"/"+NameCorrected+"/");
            DirAccess.MakeDirAbsolute(mPath+"/"+NameCorrected+"/StateManager/");
            DirAccess.MakeDirAbsolute(mPath+"/"+NameCorrected+"/States/");

            if (mFSMCaseNode.IsSelected(0)){
                CreateFSMActor(mPath, NameCorrected, mInitialStateName, true);
            }
            else if (mFSMCaseNode.IsSelected(1)){
                CreateFSMActor(mPath, NameCorrected, mInitialStateName, false);
            }
            else if (mFSMCaseNode.IsSelected(2)){
                CreateFSMAI(mPath, NameCorrected, mInitialStateName);
            }

        }

        private void CreateFSMActor(string path, string FSMName, string stateName, bool IsTwoDimension){
            PackedScene Packer = new PackedScene();

            string stateNameUpper = stateName[0].ToString().ToUpper() + stateName.Substr(1, stateName.Length-1);
            string stateNameLower = stateName[0].ToString().ToLower() + stateName.Substr(1, stateName.Length-1);

            string newNamespace = ProjectName+".Actor"+FSMName;

            //Create the initial state script here
            FileAccess file = FileAccess.Open(PathScriptStateTemplate, FileAccess.ModeFlags.Read);
            string content = file.GetAsText();
            content = EraseFirstTwoLines(content);
            content=content.Replace("_STATENAMESPACE_", newNamespace);
            content=content.Replace("_CLASS_", stateNameUpper);
            file = FileAccess.Open(path+"/"+FSMName+"/States/" + stateName + ".cs", FileAccess.ModeFlags.Write);
            file.StoreLine(content);
            file.Close();


            //First Create the state manager
            file = FileAccess.Open(PathScriptStateManagerTemplate, FileAccess.ModeFlags.Read);
            content = file.GetAsText();
            file.Close();
            content = EraseFirstTwoLines(content);
            content=content.Replace("_STATEMANAGERNAMESPACE_", newNamespace);
            content=content.Replace("_CLASS_",FSMName+"StateManager");
            content=content.Replace("StateAbstract dummyState", stateNameUpper + " " + stateNameLower + " = new " + stateNameUpper + "()");
            content=content.Replace("return dummyState;", "return "+ stateNameLower +";");
            content=content.Replace("dummyState.InitializeState", stateNameLower+".InitializeState");
            string pathForStateManager= path+ FSMName + "/StateManager/"+FSMName+"StateManager.cs";
            file = FileAccess.Open(pathForStateManager, FileAccess.ModeFlags.Write);
            file.StoreLine(content);
            file.Close();


            //Create state manager pointer
            StateManagerPointer stateManagerPointer = new StateManagerPointer();
            stateManagerPointer.StateManagerClassPath = newNamespace+"."+FSMName+"StateManager";
            ResourceSaver.Save(stateManagerPointer, path + FSMName + "/StateManager/"+FSMName+"ManagerPointer.tres");


            //Make the actor base node
            Node ActorNode;

            //This avoid Godot losing reference to FSMNode due to Casting shenanigans.
            Node dummyNode = new Node();

            if (IsTwoDimension){
                ActorNode = new CharacterBody2D();
                dummyNode.AddChild(ActorNode);
                ActorNode.SetScript(ResourceLoader.Load<Script>(PathStringGameActorReference2D));
            }
            else{
                ActorNode = new CharacterBody3D();
                dummyNode.AddChild(ActorNode);
                ActorNode.SetScript(ResourceLoader.Load<Script>(PathStringGameActorReference3D));
            }
            ActorNode = dummyNode.GetChild(0);
            ActorNode.Name = FSMName;

            //Add the the State Machine node and component
            Node FSMNode = new Node();
            FSMNode.Name = FSMName + "FSM";
            ActorNode.AddChild(FSMNode);
            FSMNode.Owner = ActorNode;
            FSMNode.SetScript(ResourceLoader.Load<Script>(PathScriptFSMActor));
            //Assign the state manager pointer
            StateManagerPointer savedPointer = ResourceLoader.Load<StateManagerPointer>(path + FSMName + "/StateManager/"+FSMName+"ManagerPointer.tres");
            StateMachineActor FSMActor = ActorNode.GetChild<StateMachineActor>(0);
            FSMActor.StateManagerResource = savedPointer;
            
            //Assign FSM to reference handler
            if (IsTwoDimension){
                GameActorReferenceHandler2D refHandler = ActorNode as GameActorReferenceHandler2D;
                refHandler.StateMachine = FSMActor;
                refHandler.TagObject = FSMName;
            }
            else{
                GameActorReferenceHandler3D refHandler = ActorNode as GameActorReferenceHandler3D;
                refHandler.StateMachine = FSMActor;
                refHandler.TagObject = FSMName;
            }

            Packer.Pack(ActorNode);
            ResourceSaver.Save(Packer, path +"/" + FSMName +"/" + FSMName + ".tscn");

            file.Close();
            file.Dispose();
        }

        private void CreateFSMAI(string path, string FSMName, string stateName){
            PackedScene Packer = new PackedScene();

            string stateNameUpper = stateName[0].ToString().ToUpper() + stateName.Substr(1, stateName.Length-1);
            string stateNameLower = stateName[0].ToString().ToLower() + stateName.Substr(1, stateName.Length-1);

            string newNamespace = ProjectName+".AI"+FSMName;

            //Create the initial state script here
            FileAccess file = FileAccess.Open(PathScriptStateTemplate, FileAccess.ModeFlags.Read);
            string content = file.GetAsText();
            content = EraseFirstTwoLines(content);
            content=content.Replace("_STATENAMESPACE_", newNamespace);
            content=content.Replace("_CLASS_", stateNameUpper);
            file = FileAccess.Open(path+"/"+FSMName+"/States/" + stateName + ".cs", FileAccess.ModeFlags.Write);
            file.StoreLine(content);
            file.Close();


            //First Create the state manager class
            file = FileAccess.Open(PathScriptStateManagerTemplate, FileAccess.ModeFlags.Read);
            content = file.GetAsText();
            file.Close();
            content = EraseFirstTwoLines(content);
            content=content.Replace("_STATEMANAGERNAMESPACE_", newNamespace);
            content=content.Replace("_CLASS_",FSMName+"StateManager");
            content=content.Replace("StateAbstract dummyState", stateNameUpper + " " + stateNameLower + " = new " + stateNameUpper + "()");
            content=content.Replace("return dummyState;", "return "+ stateNameLower +";");
            content=content.Replace("dummyState.InitializeState", stateNameLower+".InitializeState");
            string pathForStateManager= path+ FSMName + "/StateManager/"+FSMName+"StateManager.cs";
            file = FileAccess.Open(pathForStateManager, FileAccess.ModeFlags.Write);
            file.StoreLine(content);

            file.Close();

            //Make the StateMachinePointer
            StateManagerPointer stateManagerPointer = new StateManagerPointer();
            stateManagerPointer.StateManagerClassPath = newNamespace+"."+FSMName+"StateManager";
            ResourceSaver.Save(stateManagerPointer, path + FSMName + "/StateManager/"+FSMName+"ManagerPointer.tres");

            //Make the State Machine
            Node FSMNode = new Node();
            FSMNode.Name = FSMName + "FSM";

            //This avoid Godot losing reference to FSMNode due to Casting shenanigans.
            Node dummyNode = new Node();
            dummyNode.AddChild(FSMNode);

            //Assign the script and recover the reference from dummyNode.
            FSMNode.SetScript(ResourceLoader.Load<Script>(PathScriptFSMAI));
            FSMNode = dummyNode.GetChild(0);

            //Assign the state manager pointer
            StateManagerPointer savedPointer = ResourceLoader.Load<StateManagerPointer>(path + FSMName + "/StateManager/"+FSMName+"ManagerPointer.tres");
            StateMachineAIInput FSMAI = FSMNode as StateMachineAIInput;
            FSMAI.mStateManagerPointer = savedPointer;

            Packer.Pack(FSMNode);
            ResourceSaver.Save(Packer, path +"/" + FSMName +"/" + FSMName + ".tscn");

            file.Close();
            file.Dispose();
        }


        private string EraseFirstTwoLines(string content){
            content = EraseFirstLine(content);
            content = EraseFirstLine(content);
            return content;
        }

        private string EraseFirstLine(string content){
            int index = content.IndexOf("\n");
            return content.Substring(index+2);
        }
    }
}
#endif