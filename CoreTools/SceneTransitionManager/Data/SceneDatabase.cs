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

        [Export] public Godot.Collections.Dictionary<string, PackedScene> SceneIdToPackedScene = new Godot.Collections.Dictionary<string, PackedScene>();


    }
}