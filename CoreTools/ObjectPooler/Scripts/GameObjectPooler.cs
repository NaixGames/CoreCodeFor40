using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CoreCode.Scripts{
	public abstract partial class GameObjectPooler : SingleNode<GameObjectPooler>
	{
		// ----------------------------------- Information ------------------------------------------------
		/*This is a script to have a game object poooler. This is a more efficient and memory safe way to manage instantiation of game objects. 
		The game object pooler should be the waypoint of all instantion during gameplay. 
		
		UPDATE: This was changed to be an abstract class, to allow seperation to 2D and 3D, which allows better compatibility with godot's systems*/
		
		// ------------------------------------ Use -------------------------------------------------------
		/*  Every instantiable game object should inherit from a PoolableObject Node. Once this is done, one can add
		the game objects to the array PoolableObjectsStartUp so they are instantiated at the start. The number of instantiated
		copies will be the corresponding element in the NumberOfCopiesStarUp array. To request any of them use
		GameObjectPooler.Instance.GiveObject(tag), where tag it the tag given by the IPoolableObject component. To put them
		into the pool, use IPoolableObject.ReturnToPool();.

		If an object that did not have its tag in the GameObjectPooler at start up is pooled, You also need to add a number of copies
		to instantiate in the pool, or 1 will be used by default. A warning message will be sent to the Logging system.

		If a copy of a game object is requested while there are not any copy in the pool, one more copy will be instantiated in
		the pool before giving a copy. A warnig message will be sent to the Logging system. 

		In practice, the parameters should be set such that the last two cases never occur.
		*/

		// Variables
		[Export] protected int mNumberForPoolExpandUponFilling=10;
		
		//I think the next two dictionaries can be simplified into a tupple.  
		protected Dictionary<string, IPoolableObject[]> mObjectPoolerMap = new Dictionary<string, IPoolableObject[]>(); 
		protected Dictionary<string, PackedScene> mTagToPackedSceneMap = new Dictionary<string, PackedScene>();
		protected Dictionary<string, int> mIndexForObjectPoolerMap = new Dictionary<string, int>(); //This should now split "used" versus "unused" objects

        // -----------------------------------------------------------------------------


        // ------------------------- Initialization
		
		//We use EnterTree because it comes before _Ready() on the execution order.
        public override void _EnterTree()
        {
            base._EnterTree();
			ProcessMode = ProcessModeEnum.Disabled;
		}


        public override void _ExitTree()
        {
			CleanObjectPooler();
            base._ExitTree();
        }

        // ------------------------- Methods

        // -----------Methods that return a game object from the pool

        public Node GiveObject(string tag, Node Parent = null){
			mLogObject.Print("Attepmting to get object with tag " + tag + " from the index" + mIndexForObjectPoolerMap[tag]);
			mLogObject.Print("There are " + mObjectPoolerMap[tag].Length + " total object in that pool");
			
			if (!mObjectPoolerMap.ContainsKey(tag)){
				mLogObject.Err("Trying to obtain from pool tag " + tag + " which is not in pool!");
				return null;
			}
			if (mIndexForObjectPoolerMap[tag] == mObjectPoolerMap[tag].Length){ //If we arrive at the end of the Pool, we need to expand.
				mLogObject.Warn(tag + " object requested but non active. Instantiating new copies"); 
				ExpandPoolAndGenerateObjects(tag);
			}
			IPoolableObject mObjectPooled = mObjectPoolerMap[tag][mIndexForObjectPoolerMap[tag]];
			Node realParent = Parent != null? Parent : this;
			realParent.AddChild(mObjectPooled as Node);
			mIndexForObjectPoolerMap[tag] += 1;
			return mObjectPooled as Node; 
		}

		public Node GiveObject(IPoolableObject ObjectToGive, Node Parent = null){
			return GiveObject(ObjectToGive.TagObject);
		}

		// -----------Methods to manage the pool
		

		//Method to reset the object pooler when loading new scene
		public void PoolAllObjects(){
			foreach (string tag in mObjectPoolerMap.Keys){
				for(int i=0; i< mObjectPoolerMap[tag].Length;i++){
					ReturnObjectToPool(mObjectPoolerMap[tag][i] as Node);
				}
			}
		}

		//Method to erase the object pooler. Usefull to replace it with anotherone on level changes

		public void CleanObjectPooler(){
			foreach (string tag in mObjectPoolerMap.Keys){
				for(int i=0; i< mObjectPoolerMap[tag].Length;i++){
					(mObjectPoolerMap[tag][i] as Node).Free(); // This makes me afraid it will create bugs in the future, but wont think about
				}
			}

			mObjectPoolerMap.Clear();
			mTagToPackedSceneMap.Clear();
			mIndexForObjectPoolerMap.Clear();
		}

		// -----------Methods that return a game object to the pool

		public void ReturnObjectToPool(Node ObjectToPool){
			//This method is to return active objects to the pool
			IPoolableObject PoolableVersion = (IPoolableObject)ObjectToPool;
			string tag = PoolableVersion.TagObject;
			PoolableVersion.ReturnToPool();
			//Make sure to mantain all inactive objects at "the right" of the index.
			mIndexForObjectPoolerMap[tag] = Mathf.Max(mIndexForObjectPoolerMap[tag]-1,0);
			IPoolableObject mTemporalForObject = mObjectPoolerMap[tag][mIndexForObjectPoolerMap[tag]];
			int IndexOfObjectToPool = Array.IndexOf(mObjectPoolerMap[tag], PoolableVersion);
			mObjectPoolerMap[tag][IndexOfObjectToPool] = mTemporalForObject;
			mObjectPoolerMap[tag][mIndexForObjectPoolerMap[tag]] = PoolableVersion;
			ObjectToPool.GetParent<Node>()?.RemoveChild(ObjectToPool);
		}

		public void AddObjectReferenceToPool(IPoolableObject ObjectToAdd){
			//This method is to put in the pool a give object that was not referenced before
			if (mObjectPoolerMap.ContainsKey(ObjectToAdd.TagObject)){
				mLogObject.Warn("Object with tag" + ObjectToAdd.TagObject + " returned to pool, but had no reference in pool. Expanding pool to adjust.");
				ObjectToAdd.HasPoolReference = true;
				ExpandPoolAndAddObject(ObjectToAdd.TagObject, ObjectToAdd);
			}
			else{
				mLogObject.Warn("Object with tag" + ObjectToAdd.TagObject + " returned to pool, but no pool existed. Freeing the node instead");
				(ObjectToAdd as Node).QueueFree();
			}
		}


		// ---------------------- Auxiliar method

		public void ExpandPoolAndGenerateObjects(string tagOfPool){
			ExpandPoolAndGenerateObjects(tagOfPool, mNumberForPoolExpandUponFilling);
		}


		public void ExpandPoolAndGenerateObjects(string tagOfPool, int ammountToAdd){
			IPoolableObject[] oldPool = mObjectPoolerMap[tagOfPool]; 
			IPoolableObject[] newPool = new IPoolableObject[oldPool.Length+ammountToAdd];
			oldPool.CopyTo(newPool, 0);
			mObjectPoolerMap[tagOfPool] = newPool;
			for (int i=oldPool.Length; i < newPool.Length; i++){
				PackedScene sceneReference = mTagToPackedSceneMap[tagOfPool];
				Node CopyObject = sceneReference.Instantiate();
				if (!(CopyObject is IPoolableObject)){
					mLogObject.Err("Trying to use object pooler for an object that is not poolable " + tagOfPool);
					return;
				}
				newPool[i] = CopyObject as IPoolableObject;
				newPool[i].HasPoolReference=true;
				newPool[i].ReturnToPool();
			}
		}


		public void ExpandPoolAndAddObject(string tagOfPool, IPoolableObject objectToAdd){
			IPoolableObject[] oldPool = mObjectPoolerMap[tagOfPool]; 
			IPoolableObject[] newPool = new IPoolableObject[oldPool.Length+1];
			oldPool.CopyTo(newPool, 0);
			newPool[newPool.Length-1] = objectToAdd;
			mObjectPoolerMap[tagOfPool] = newPool;
		}



		public void AddObjectFromPoolData(PoolSceneData poolSceneData)
		{
			if (poolSceneData == null){
				mLogObject.Warn("Given object pool information is empty.");
				return;
			}

			Godot.Collections.Dictionary<PoolableObjectReference, int> poolObjectData = poolSceneData.PoolableObjectsData;

			//If pool is empty, just return. Shouldn't happen in games, mostly during testing
			if (poolObjectData == null || poolObjectData.Count == 0){
				mLogObject.Warn("Given object pool information is empty.");
				return;
			}

			//Initiate the game object pooler.
			foreach (PoolableObjectReference objectReference in poolObjectData.Keys){
				if (!mObjectPoolerMap.ContainsKey(objectReference.Tag)){
					IPoolableObject[] poolOfAGameObject = new IPoolableObject[0];
					mObjectPoolerMap.Add(objectReference.Tag, poolOfAGameObject); 
					mIndexForObjectPoolerMap.Add(objectReference.Tag, 0);
					mTagToPackedSceneMap.Add(objectReference.Tag, objectReference.Object);
				}

				int numberOfCopies = Math.Max(poolObjectData[objectReference]-mObjectPoolerMap[objectReference.Tag].Length, 0);
				ExpandPoolAndGenerateObjects(objectReference.Tag, numberOfCopies);
			}
		}

	}
}