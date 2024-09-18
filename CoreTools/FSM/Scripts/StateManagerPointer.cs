using Godot;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace CoreCode.FSM
{
    [Tool]
    public abstract partial class StateManagerPointer : Resource
    {

        private Dictionary<string, string> mClassToNameMapping = new Dictionary<string, string>();

        private Dictionary<string, string> mNameToClassMapping = new Dictionary<string, string>();

        private Dictionary<string, List<string>> mStatesConnections = new Dictionary<string, List<string>>();

        public abstract StateManagerAbstract GiveStateManagerInstance();

        public abstract string GiveNamespaceString();
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

            GetStateTransitions(StatesFolder);

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
            string className  = fileName.Substring(0, lastSeparator);
            className = GiveNamespaceString() + "." + className;
            if (Type.GetType(className) == null)
            {
                return false;
            }
            return Type.GetType(className).IsSubclassOf(typeof(StateManagerAbstract));
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

        private void GetStateTransitions(string StateFolder){
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

            //Try to call PUML to generate the graph image automatically.
            string pathToPUML = ProjectSettings.GlobalizePath("res://ExternalLibraries/PUML/") + "plantuml.jar";

            ProcessStartInfo processInfo = new ProcessStartInfo("java.exe", "-jar " + pathToPUML + " " +PathToSave+"/" + "Graph.puml")
                      {
                          CreateNoWindow = true,
                          UseShellExecute = false
                      };
            Process proc;

            if ((proc = Process.Start(processInfo)) == null)
            {
                throw new InvalidOperationException("Couldnt find PUML!");
            }

            proc.WaitForExit();
            int exitCode = proc.ExitCode;
            proc.Close();

        }

        private string PutQuotes(string state){
            return "\"" + state + "\"";
        }
#endif


    }
}
