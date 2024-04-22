using Godot;
using System;

namespace CoreCode.Scripts{
    [Tool]
    public partial class SceneDatabase : Resource
    {
        // ----------------------------------- Information ------------------------------------------------
        // Resource that contains and generates the information of the Scenes in the file system.*/
        
        // ------------------------------------ Use -------------------------------------------------------
        /* Complete the mSceneNameToUIDMapping with the name and UID mapping of the ACTUAL scene. Then press
        the "Generate whole database" to complete all the information for the other elements that may be needed.*/
        
        
        // ----------------------------------- Variables

        [Export] public Godot.Collections.Dictionary<string, string> mSceneNameToUIDMapping = new Godot.Collections.Dictionary<string,string>();

        [Export] public bool IsSceneDatabaseUpdated{
            get{ return mSceneNameToUIDMapping.Count == mSceneNameToPathMapping.Count && mSceneNameToUIDMapping.Count == mNonPersistantSceneNameToPathMapping.Count;}
            set{ return; } 
        }

        //This MUST be exported to be serialized. Sadly.
        [Export] public Godot.Collections.Dictionary<string, string> mSceneNameToPathMapping = new Godot.Collections.Dictionary<string,string>();

        [Export] public Godot.Collections.Dictionary<string, string> mNonPersistantSceneNameToPathMapping = new Godot.Collections.Dictionary<string,string>();

    
        // ----------------- Methods

        public void LoadSceneDatabase(){
            if (!Engine.IsEditorHint()){
                return;
            }
            if (IsSceneDatabaseUpdated){
                GD.PushWarning("SceneDatabase seems updated, avoiding recreating database. If this is wrong, clear the mappings and try again.");
                return;
            }
            GD.Print("The scene database is being updated");
            mSceneNameToPathMapping.Clear();
            mNonPersistantSceneNameToPathMapping.Clear();

            foreach (string keyString in mSceneNameToUIDMapping.Keys){
                string wholeScenePath = ResourceUid.GetIdPath(ResourceUid.TextToId(mSceneNameToUIDMapping[keyString]));
                mSceneNameToPathMapping.Add(keyString, wholeScenePath);
                int lastSeparator = wholeScenePath.LastIndexOf("/");
                string nonPersistantScenePath = wholeScenePath.Insert(lastSeparator+1, "NPE");
                mNonPersistantSceneNameToPathMapping.Add(keyString, nonPersistantScenePath);
            }
            GD.Print("The scene database was updated!");
        }
    }
}