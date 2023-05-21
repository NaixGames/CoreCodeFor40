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
            NPElements.Name = "NPActual"+sceneName;
            Packer.Pack(NPElements);
            ResourceSaver.Save(Packer, scenePath + "NPActual" + sceneName + ".tscn");


            //Now we create the actual scene.
            //Instantiate the ActualScene node
            Node actualScene = new Node();
            actualScene.Name = "Actual"+sceneName;
            //Add the basic element structure
            Node elements = new Node();
            elements.Name="Elements";
            actualScene.AddChild(elements);
            elements.Owner=actualScene;
            Node persistentElements = new Node();
            persistentElements.Name = "PE"+sceneName;
            elements.AddChild(persistentElements);
            persistentElements.Owner = actualScene;
            Node NPElementsInstance = (Node)GD.Load<PackedScene>(scenePath + "NPActual" + sceneName + ".tscn").Instantiate();
            elements.AddChild(NPElementsInstance);
            NPElementsInstance.Owner = actualScene;

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
            actualScene.AddChild(objectPooler);
            actualScene.AddChild(audioBanks);
            objectPooler.Owner=actualScene;
            audioBanks.Owner=actualScene;


            //Put the reference of the script in the actual scene
            actualScene.SetScript(GD.Load<Script>(PathScriptSceneManagerTransitionHelper));
            SceneTransitionReferenceHelper referenceHelper = elements.Owner as SceneTransitionReferenceHelper;
            referenceHelper.NonPersistentElementsPath = referenceHelper.GetPathTo(NPElementsInstance);
            referenceHelper.PersistentElementsPath = referenceHelper.GetPathTo(persistentElements);
            referenceHelper.ObjectPoolerNodePath = referenceHelper.GetPathTo(objectPooler);
            referenceHelper.AudioBankContainerNodePath = referenceHelper.GetPathTo(audioBanks);

            //Save the actual scene
            Packer.Pack(referenceHelper);
            ResourceSaver.Save(Packer, scenePath + "Actual" + sceneName + ".tscn");

            //Create the whole scene node
            Node wholeScene = new Node();
            wholeScene.Name = "Whole"+sceneName;

            //Add Managers to the scene.
            Node Managers = (Node)GD.Load<PackedScene>(ManagerNodePath).Instantiate();
            wholeScene.AddChild(Managers);
            Managers.Owner = wholeScene;

            //hook up the reference helper
            wholeScene.SetEditableInstance(Managers,true);
            //Put the actual scene in the whole scene
            Node actualSceneInstance = (Node)GD.Load<PackedScene>(scenePath + "Actual" + sceneName + ".tscn").Instantiate();    
            wholeScene.AddChild(actualSceneInstance);
            actualSceneInstance.Owner = wholeScene;
            
            //Hook up the manager reference to the actualSceneInstance. Note we need to add "../" to get back to root (ie, absolute path)
            (Managers.GetNode<SceneTransitionManager>("SceneTransitionManager")).ReferenceHelperPath="../" + Managers.GetPathTo(actualSceneInstance);
            
            Packer.Pack(wholeScene);
            ResourceSaver.Save(Packer, scenePath + "Whole" + sceneName + ".tscn");
        }
    }
}
#endif

