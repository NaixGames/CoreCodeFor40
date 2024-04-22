using Godot;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Net.Mime;

namespace CoreCode.FSM
{
    [GlobalClass, Tool]
    public  partial class StateManagerPointer : Resource
    {
        [Export] public string StateManagerClassPath;

        private Dictionary<string, string> mNameToClassMapping = new Dictionary<string, string>();

        private Dictionary<string, List<string>> mStatesConnections = new Dictionary<string, List<string>>();

        public StateManagerAbstract GiveStateManagerInstance(){
            try{
                return (StateManagerAbstract)Assembly.GetAssembly(typeof(StateManagerAbstract)).CreateInstance(StateManagerClassPath);
            }
            catch(Exception e){
                GD.PrintErr("State manager casting failed", e);
                return null;
            }
        }

        // ----------------------------------------------------

        public void PrintStateManagerGraph(){
#if TOOLS
            GD.Print("TEST");
            mNameToClassMapping.Clear();
            mStatesConnections.Clear();

            //Get StateManager folder and States folder
            string FilePath = ProjectSettings.GlobalizePath(this.ResourcePath);
            int lastSeparator = FilePath.LastIndexOf("/");
            string ContainerFolder = FilePath.Substring(0, lastSeparator);
            int secondLastSeparator = ContainerFolder.LastIndexOf("/");
            string StatesFolder = ContainerFolder.Substring(0,secondLastSeparator).Insert(secondLastSeparator, "/States");

            //Run through StateManager folder and get StateManager file and print all states.
            DirAccess dir = DirAccess.Open(ContainerFolder);
            dir.ListDirBegin();
            string fileName ="a";
            while (fileName != ""){
                fileName = dir.GetNext();
                if (IsAStateManagerClass(fileName)){
                    ProcessStateManagerClass(ContainerFolder+"/"+fileName);
                    break;
                }
            }
            if (fileName == ""){
                GD.PrintErr("No state manager found on the folder of the pointer");
                return;
            }

            GiveStateTransitions(StatesFolder, fileName.Substring(0,fileName.IndexOf(".")));
#endif
        }

#if TOOLS

        private bool IsAStateManagerClass(string fileName){
            int lastSeparator = fileName.LastIndexOf(".");
            if (lastSeparator == -1){
                return false;
            }
            string typeString = fileName.Substring(lastSeparator);
            if (typeString != ".cs"){
                return false;
            }
            return Type.GetType(StateManagerClassPath).IsSubclassOf(typeof(StateManagerAbstract));
        }

        private void ProcessStateManagerClass(string stateManagerPath){
            FileAccess file = FileAccess.Open(ProjectSettings.LocalizePath(stateManagerPath), FileAccess.ModeFlags.Read);
            string content = file.GetAsText();
            file.Close();

            string[] words = content.Split(" ");
            for(int i=0 ; i< words.Length; i++){
                if (words[i]=="new"){
                    mNameToClassMapping.Add(words[i-3], words[i-2]);
                }
            }
        }

        private void GiveStateTransitions(string StateFolder, string StateManagerName){
            foreach (string className in mNameToClassMapping.Keys){
                mStatesConnections.Add(className, new List<string>());
                FileAccess file = FileAccess.Open(ProjectSettings.LocalizePath(StateFolder+"/"+className+".cs"), FileAccess.ModeFlags.Read);
                string content = file.GetAsText();
                file.Close();

                content = content.Replace("\n", " ");
                content = content.Replace("\t", " ");
                string[] words = content.Split(" ");
                for(int i=0 ; i< words.Length; i++){
                    if (words[i]!="return"){
                        continue;
                    }

                    string nextString = words[i+1];
                    int pointIndex = nextString.IndexOf(".");
                    if (pointIndex==-1){
                        continue;
                    }

                    string stateName = nextString.Substring(pointIndex+1);

                    if (mStatesConnections[className].Contains(stateName)){
                        continue;
                    }

                    mStatesConnections[className].Add(nextString.Substring(pointIndex+1));
                }
            }
            
            foreach (string className in mStatesConnections.Keys){
                foreach (string destinationState in mStatesConnections[className]){
                    GD.Print(mNameToClassMapping[className] + " -> " + destinationState);
                }
            }
        }
#endif


    }
}
