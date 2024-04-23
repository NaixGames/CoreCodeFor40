using Godot;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace CoreCode.FSM
{
    [GlobalClass, Tool]
    public  partial class StateManagerPointer : Resource
    {
        [Export] public string StateManagerClassPath;

        private Dictionary<string, string> mClassToNameMapping = new Dictionary<string, string>();

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
            GD.Print("Generating data");
            mClassToNameMapping.Clear();
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

            GetStateTransitions(StatesFolder, fileName.Substring(0,fileName.IndexOf(".")));

            /*
            GD.Print("TEST");
            foreach (string value in mClassToNameMapping.Keys){
                GD.Print(value + " " + mClassToNameMapping[value]);
            }
            GD.Print("TEST2");

            foreach (string value in mStatesConnections.Keys){
                GD.Print(value + " " + mStatesConnections[value].Count);
            }*/

            PrintAndSaveInformation(ContainerFolder);
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
                    mClassToNameMapping.Add(words[i-3], words[i-2]);
                    mNameToClassMapping.Add(words[i-2], words[i-3]);
                }
            }
        }

        private void GetStateTransitions(string StateFolder, string StateManagerName){
            foreach (string className in mClassToNameMapping.Keys){
                mStatesConnections.Add(className, new List<string>());
                FileAccess file = FileAccess.Open(ProjectSettings.LocalizePath(StateFolder+"/"+className+".cs"), FileAccess.ModeFlags.Read);
                string content = file.GetAsText();
                file.Close();

                content = content.Replace("\n", " ");
                content = content.Replace("\t", " ");
                string[] words = content.Split(" ");
                for(int i=0 ; i< words.Length; i++){
                    int pointIndex = words[i].IndexOf(".");

                    if (pointIndex == -1){
                        continue;
                    }

                    int endIndex = words[i].IndexOf(";");

                    string stateName = "";

                    if (endIndex == -1){
                        stateName = (words[i].Substring(pointIndex+1));
                    }
                    else{
                        stateName = (words[i].Substring(pointIndex+1, endIndex-pointIndex));
                    }


                    //If it is a state we need to remove the ";" and the start of the next line 
                    if (stateName.Length < 2){
                        continue;
                    }
                    stateName = stateName.Remove(stateName.Length - 1);

                    if (!mClassToNameMapping.Values.Contains(stateName)){
                        continue;
                    }
                    
                    if (mStatesConnections[className].Contains(stateName)){
                        continue;
                    }

                    mStatesConnections[className].Add(stateName);
                }
            }
        }

        private void PrintAndSaveInformation(string PathToSave){
            string preamble = "";
            preamble += "@startuml" + "\n";
            preamble += "hide circle" + "\n";
            preamble += "skinparam linetype ortho" + "\n";
            string content = "";
            foreach (string className in mStatesConnections.Keys){
                content += "class " + PutQuotes(className) + "\n";
                foreach (string destinationState in mStatesConnections[className]){
                    content += PutQuotes(className) + " --> " + PutQuotes(mNameToClassMapping[destinationState]) + "\n";
                }
            }
            GD.Print(content);
    
            content = preamble + content;
            content += "@enduml";
            FileAccess file = FileAccess.Open(PathToSave+"/" + "Graph.puml", FileAccess.ModeFlags.Write);
            file.StoreLine(content);

            file.Close();
        }

        private string PutQuotes(string state){
            return "\"" + state + "\"";
        }
#endif


    }
}
