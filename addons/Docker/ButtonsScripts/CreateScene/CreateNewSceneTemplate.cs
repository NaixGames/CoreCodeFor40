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

        private const string ManagerNodePath="res://CoreTools/SceneTemplate/Managers.tscn";
        private const string PathScriptSceneManagerTransitionHelper="res://CoreTools/SceneTransitionManager/Script/SceneTransitionReferenceHelper.cs";
        private const string PathScriptGameObjectPooler2D="res://CoreTools/ObjectPooler/Scripts/GameObjectPooler2D.cs";
        private const string PathScriptGameObjectPooler3D="res://CoreTools/ObjectPooler/Scripts/GameObjectPooler3D.cs";
        public override void _EnterTree()
        {
            Pressed += Clicked;
            mPathStringInputNode = this.GetParent<Node>().GetNode<LineEdit>("ScenePathInputBox");
            mNameStringInputNode = this.GetParent<Node>().GetNode<LineEdit>("SceneNameInputBox");
            mDimensionCaseNode = this.GetParent<Node>().GetNode<ItemList>("DimensionCase");
        }

        public void Clicked()
        {
            string name = mNameStringInputNode.Text;
            string path = mPathStringInputNode.Text + "/" +name+"/";
            string absolutePath = ProjectSettings.GlobalizePath(path);
            DirAccess.MakeDirAbsolute(absolutePath); //Save this in a different folder for convenience.

            //Decide dimension case for object pooler.
            string dimString;
            if (mDimensionCaseNode.IsSelected(0)){
                dimString = "2D";
            }
            else{
                dimString = "3D";
            }
            CreateSceneTemplate(path, name, dimString);
        }

        private void CreateSceneTemplate(string scenePath, string sceneName, string dimensionCase){
            PackedScene Packer = new PackedScene();

            //Create the scene system from the down to top appoach.


            //First Non persistent elements.
            Node NPElements = new Node();
            NPElements.Name = "NPE"+sceneName;
            Packer.Pack(NPElements);
            ResourceSaver.Save(Packer, scenePath + "NPE" + sceneName + ".tscn");


            //Now we create the actual scene.
            //Instantiate the ActualScene node
            Node parentScene = new Node();
            parentScene.Name = "Actual"+sceneName;
            

            Node persistentElements = new Node();
            persistentElements.Name = "PE"+sceneName;
            parentScene.AddChild(persistentElements);
            persistentElements.Owner = parentScene;
            Node NPElementsInstance = (Node)GD.Load<PackedScene>(scenePath + "NPE" + sceneName + ".tscn").Instantiate();
            parentScene.AddChild(NPElementsInstance);
            NPElementsInstance.Owner = parentScene;

            //Add the object pooler and audio bank node.
            Node objectPooler = new Node();
            objectPooler.Name="ObjectPooler";
            if (dimensionCase == "2D"){
                objectPooler.SetScript(GD.Load<Script>(PathScriptGameObjectPooler2D));
            }
            else if(dimensionCase=="3D"){
                objectPooler.SetScript(GD.Load<Script>(PathScriptGameObjectPooler3D));
            }
            Node audioBanks = new Node();
            audioBanks.Name = "AudioBank";
            parentScene.AddChild(objectPooler);
            parentScene.AddChild(audioBanks);
            objectPooler.Owner=parentScene;
            audioBanks.Owner=parentScene;


            //Put the reference of the script in the actual scene
            parentScene.SetScript(GD.Load<Script>(PathScriptSceneManagerTransitionHelper));
            SceneTransitionReferenceHelper referenceHelper = parentScene as SceneTransitionReferenceHelper;
            referenceHelper.NonPersistentElementsPath = referenceHelper.GetPathTo(NPElementsInstance);
            referenceHelper.PersistentElementsPath = referenceHelper.GetPathTo(persistentElements);
            referenceHelper.ObjectPoolerNodePath = referenceHelper.GetPathTo(objectPooler);
            referenceHelper.AudioBankContainerNodePath = referenceHelper.GetPathTo(audioBanks);

            //Save the actual scene
            Packer.Pack(referenceHelper);
            ResourceSaver.Save(Packer, scenePath + sceneName + ".tscn");

        }
    }
}
#endif

