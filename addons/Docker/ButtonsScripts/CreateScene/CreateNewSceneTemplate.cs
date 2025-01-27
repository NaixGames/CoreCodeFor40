#if TOOLS
using Godot;
using System;
using CoreCode.Scripts;

namespace  CoreCode.Docker
{
    [Tool]
    public partial class CreateNewSceneTemplate : Button
    {
        private LineEdit mPathStringInputNode;
        private LineEdit mNameStringInputNode;
        private ItemList mDimensionCaseNode;
        private CheckBox mShouldCreatePoolDataNode;

        private const string PathScriptSceneManagerTransitionHelper="res://CoreTools/SceneTransitionManager/Script/SceneTransitionReferenceHelper.cs";
        public override void _EnterTree()
        {
            Pressed += Clicked;
            mPathStringInputNode = this.GetParent<Node>().GetNode<LineEdit>("ScenePathInputBox");
            mNameStringInputNode = this.GetParent<Node>().GetNode<LineEdit>("SceneNameInputBox");
            mDimensionCaseNode = this.GetParent<Node>().GetNode<ItemList>("DimensionCase");
            mShouldCreatePoolDataNode = this.GetParent<Node>().GetNode<CheckBox>("CreatePoolData");
        }

        public void Clicked()
        {
            string name = mNameStringInputNode.Text;
            string path = mPathStringInputNode.Text + "/" +name+"/";
            string absolutePath = ProjectSettings.GlobalizePath(path);
            DirAccess.MakeDirAbsolute(absolutePath); //Save this in a different folder for convenience.


            CreateSceneTemplate(path, name);
        }

        private void CreateSceneTemplate(string scenePath, string sceneName){
            PackedScene Packer = new PackedScene();           

            //Now we create the actual scene.
            //Instantiate the ActualScene node
            Node parentScene = new Node();
            parentScene.Name = sceneName;
            

            Node persistentElements = new Node();
            persistentElements.Name = "PE"+sceneName;
            parentScene.AddChild(persistentElements);
            persistentElements.Owner = parentScene;

            Node NPEpersistentElements = new Node();
            NPEpersistentElements.Name = "NPE"+sceneName;
            parentScene.AddChild(NPEpersistentElements);
            NPEpersistentElements.Owner = parentScene;
            

            //Put the reference of the script in the actual scene
            //Adding a script dispose of the original memory address for some really weird reason.
            parentScene.SetScript(GD.Load<Script>(PathScriptSceneManagerTransitionHelper));
            SceneTransitionReferenceHelper referenceHelper = persistentElements.GetParent() as SceneTransitionReferenceHelper;
            referenceHelper.NonPersistentElementsPath = referenceHelper.GetPathTo(NPEpersistentElements);
            referenceHelper.PersistentElementsPath = referenceHelper.GetPathTo(persistentElements);


            //HERE I SHOULD CREATE THE SCENE RESOURCE AND SAVE IT
            ScenePoolAndAudioData sceneData = new ScenePoolAndAudioData();
            sceneData.ResourceName = sceneName + "SceneData";

            if (mShouldCreatePoolDataNode.ButtonPressed){
                PoolSceneData poolData = new PoolSceneData();
                poolData.ResourceName = sceneName + "PoolData";
                sceneData.PoolableObjectsData = poolData;
                ResourceSaver.Save(poolData, scenePath + sceneName + "PoolData.tres" );
            }
            referenceHelper.SceneData = sceneData;
            ResourceSaver.Save(sceneData, scenePath + sceneName + "SceneData.tres");


            //HERE I SHOULD DO THE SCENE WITH AUDIO BANK IF I EVER GO BACK TO THAT

            
            //Save the actual scene
            Packer.Pack(referenceHelper);
            ResourceSaver.Save(Packer, scenePath + sceneName + ".tscn" );

        }
    }
}
#endif

