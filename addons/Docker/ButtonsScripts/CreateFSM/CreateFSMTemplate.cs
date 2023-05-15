#if TOOLS
using Godot;
using CoreCode.FSM;

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
        private const string PathScriptFSMActor="res://FSM/Scripts/StateMachineActor.cs";
        private const string PathScriptFSMAI="res://FSM/Scripts/StateMachineAIInput.cs";
        private const string PathStringGameActorReference2D="res://GameActorReferenceHandler/Script/GameActorReferenceHandler2D.cs";
        private const string PathStringGameActorReference3D="res://GameActorReferenceHandler/Script/GameActorReferenceHandler3D.cs";
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
            Node ActorStateManager = new Node();
            ActorStateManager.Name = FSMName + "StateManager";
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
            ActorStateManager.SetScript(GD.Load<Script>(pathForStateManager));

            //Make the State Machine
            Node FSMNode = new Node();
            FSMNode.Name = FSMName + "FSM";
            FSMNode.SetScript(GD.Load<Script>(PathScriptFSMActor));
            FSMNode.AddChild(ActorStateManager);
            
            //Add all of this to an actor
            Node ActorNode;
            if (IsTwoDimension){
                ActorNode = new CharacterBody2D();
                ActorNode.SetScript(GD.Load<Script>(PathStringGameActorReference2D));
            }
            else{
                ActorNode = new CharacterBody3D();
                ActorNode.SetScript(GD.Load<Script>(PathStringGameActorReference3D));
            }
            ActorNode.Name = FSMName;
            ActorNode.AddChild(FSMNode);
            FSMNode.Owner = ActorNode;
            ActorStateManager.Owner = ActorNode;

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

            //Make the State Machine
            Node ActorStateManager = new Node();
            ActorStateManager.Name = FSMName + "StateManager";

            Node FSMNode = new Node();
            FSMNode.Name = FSMName + "FSM";
            FSMNode.AddChild(ActorStateManager);
            
            ActorStateManager.Owner = FSMNode;
            
            //This is due to Godot losing base clases in multiple inheritance. Maybe could fix it later.
            Node dummyNode = new Node();
            dummyNode.AddChild(FSMNode);
            
            ActorStateManager.SetScript(GD.Load<Script>(pathForStateManager));
            FSMNode.SetScript(GD.Load<Script>(PathScriptFSMAI));

            FSMNode = dummyNode.GetChild(0);

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