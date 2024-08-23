using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CoreCode.Scripts{
	public abstract partial class GameObjectPooler : Node
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


		// ------------------------------------- Singleton instantiation -------------------------------

		[Export] protected static GameObjectPooler instance;
		public static GameObjectPooler Instance{
			get {return TryToReturnInstance();}
		}

		protected static GameObjectPooler TryToReturnInstance(){
			if (instance == null){
				GD.PushWarning("Instance of GameObjectPooler called before the instance was ready!");
				instance = new GameObjectPooler2D();
			}
			return instance;
		}

		public GameObjectPooler(){
			if (instance==null){
				instance =this;
			}
		}
		// ------------------------------------- Variables

		protected IPoolableObject[] PoolableObjectsStartUp;

		[Export] protected NodePath[] PoolableObjectStartUpManualSetUp = new NodePath[0]; //This could be extended to actuall contain folders of different type of objects

		[Export]
		protected Godot.Collections.Array<int> NumberOfCopiesStartUp = new Godot.Collections.Array<int>();
		
		[Export] protected int mNumberForPoolExpandUponFilling=10;
		protected Dictionary<string,IPoolableObject[]> mObjectPoolerMap = new Dictionary<string, IPoolableObject[]>(); 
		

		protected Dictionary<string,int> mIndexForObjectPoolerMap = new Dictionary<string, int>(); //This should now split "used" versus "unused" objects

		[Export] protected Vector3 mPoolPosition; 
		public Vector3 PoolPosition {
			get{return mPoolPosition;}
		}

		//---------------------Variables for loging
		
		[Export]
		protected bool mShouldLog;

		protected ILogObject mLogObject;

		// -----------------------------------------------------------------------------


		// ------------------------- Initialization
		public override void _Ready()
		{
			//Hook up to the loging system.
			mLogObject = LogManager.Instance.RequestLog("GameObjectPooler", mShouldLog);

			PoolableObjectsStartUp = new IPoolableObject[PoolableObjectStartUpManualSetUp.Length];
			for (int index=0;index<PoolableObjectsStartUp.Length;index++){
				PoolableObjectsStartUp[index] = (IPoolableObject)GetNode(PoolableObjectStartUpManualSetUp[index]);
			}
			
			//If pool is empty, just return. Shouldn't happen in games, mostly during testing
			if (PoolableObjectsStartUp == null || PoolableObjectsStartUp.Length == 0){
				mLogObject.Print("Object pool is empty.");
				return;
			}

			//Make sure the input given to the pooler makes sense.
			if (PoolableObjectsStartUp.Length!= NumberOfCopiesStartUp.Count){
				GD.PushError("Number of objects in the pool at the start should be the same as the number of the pool sizes in the game object pooler. Fix this!");
				return;
			}
			
			//Initiate the game object pooler.
			for (int i=0; i < PoolableObjectsStartUp.Length; i++){
				IPoolableObject[] mPoolOfAGameObject = new IPoolableObject[NumberOfCopiesStartUp[i]+1]; //Should initialize array of the given size!
				mObjectPoolerMap.Add(PoolableObjectsStartUp[i].TagObject, mPoolOfAGameObject); 
				mIndexForObjectPoolerMap.Add(PoolableObjectsStartUp[i].TagObject, 1);
				//The prototypical object is put in the pool but never used!
				mPoolOfAGameObject[0] = PoolableObjectsStartUp[i];
				mPoolOfAGameObject[0].HasPoolReference=true;
				AddPrototipicalInstanceToPool((Node)PoolableObjectsStartUp[i]); //This does not work with index 0!
				for (int j=1; j < NumberOfCopiesStartUp[i]+1; j++){
					Node CopyObject = ((Node)mPoolOfAGameObject[0]).Duplicate();
					AddChild(CopyObject);
					mPoolOfAGameObject[j]=(IPoolableObject)CopyObject;
					mPoolOfAGameObject[j].HasPoolReference=true;
					ReturnObjectToPool(CopyObject);
				}
				mLogObject.Print(PoolableObjectsStartUp[i].TagObject + " instatiated correctly in the object pooler with " + NumberOfCopiesStartUp[i] + " copies");
			}
			mLogObject.Print("Game object pooler instantiated correctly");
		}

		// ------------------------- Methods

		// -----------Methods that return a game object from the pool

		public Node GiveObject(string tag){
			mLogObject.Print("Attepmting to get object with tag " + tag + " from the index" + mIndexForObjectPoolerMap[tag]);
			mLogObject.Print("There are " + mObjectPoolerMap[tag].Length + " total object in that pool");
			
			if (!mObjectPoolerMap.ContainsKey(tag)){
				mLogObject.Err("Trying to obtain from pool tag " + tag + " which is not in pool!");
			}
			if (mIndexForObjectPoolerMap[tag] == mObjectPoolerMap[tag].Length){ //If we arrive at the end of the Pool, we need to expand.
				mLogObject.Warn(tag + " object requested but non active. Instantiating new copies"); 
				ExpandPool(tag, mNumberForPoolExpandUponFilling);
			}
			IPoolableObject mObjectPooled = mObjectPoolerMap[tag][mIndexForObjectPoolerMap[tag]];
			mIndexForObjectPoolerMap[tag]= (mIndexForObjectPoolerMap[tag]+1);
			mObjectPooled.ActivatePooledObject();
			return (Node)mObjectPooled; 
		}

		public Node GiveObject(IPoolableObject ObjectToGive){
			return GiveObject(ObjectToGive.TagObject);
		}

		// -----------Methods to manage the pool
		

		//Method to reset the object pooler when loading new scene
		public void PoolAllObjects(){
			foreach (string tag in mObjectPoolerMap.Keys){
				for(int i=1; i< mObjectPoolerMap[tag].Length;i++){
					mObjectPoolerMap[tag][i].ReturnToPool();
				}
			}
		}

		//Method to erase the object pooler. Usefull to replace it with anotherone on level changes

		public void EraseObjectPooler(){
			instance=null;
			this.QueueFree();
		}

		// -----------Methods that return a game object to the pool

		public void ReturnObjectToPool(Node ObjectToPool){
			//This method is to return active objects to the pool
			IPoolableObject PoolableVersion = (IPoolableObject)ObjectToPool;
			string tag = PoolableVersion.TagObject;
			PoolableVersion.ReturnToPool();
			//Make sure to mantain all inactive objects at "the left" of the index.
			mIndexForObjectPoolerMap[tag] = Mathf.Max(mIndexForObjectPoolerMap[tag]-1,1);
			IPoolableObject mTemporalForObject = mObjectPoolerMap[tag][mIndexForObjectPoolerMap[tag]];
			int IndexOfObjectToPool = Array.IndexOf(mObjectPoolerMap[tag], PoolableVersion);
			mObjectPoolerMap[tag][IndexOfObjectToPool] = mTemporalForObject;
			mObjectPoolerMap[tag][mIndexForObjectPoolerMap[tag]] = PoolableVersion;
			//Put object in position of pool.
			if (ObjectToPool.IsClass("Node2D")){
				((Node2D)ObjectToPool).Position = new Vector2(PoolPosition.X, PoolPosition.Y);
			}
			if (ObjectToPool.IsClass("Node3D")){
				((Node3D)ObjectToPool).Position = PoolPosition;
			}
			if (ObjectToPool.GetParent<Node>()!=null){
				ObjectToPool.GetParent<Node>().RemoveChild(ObjectToPool);
			}
			this.AddChild(ObjectToPool);
		}

		public void AddObjectReferenceToPool(IPoolableObject ObjectToAdd){
			//This method is to put in the pool a give object that was not referenced before
			if (mObjectPoolerMap.ContainsKey(ObjectToAdd.TagObject)){
				mLogObject.Warn("Object with tag" + ObjectToAdd.TagObject + " returned to pool, but had no reference in pool. Expanding pool to adjust.");
				ObjectToAdd.HasPoolReference = true;
				ExpandPool(ObjectToAdd.TagObject, 1);
			}
			else{
				mLogObject.Warn("Object with tag" + ObjectToAdd.TagObject + " returned to pool, but no pool existed. Creating a new pool");
				IPoolableObject[] mPoolOfAGameObject = new PoolableObject2D[2];
				//Need to add the two following objects correctly.
				ObjectToAdd.HasPoolReference=true;
				mPoolOfAGameObject[0] = ObjectToAdd; //Prototypical instance added to initiate the pool
				AddPrototipicalInstanceToPool((Node)ObjectToAdd);

				Node CopyObject = ((Node)ObjectToAdd).Duplicate(); //The instance we will actually spawn
				AddChild(CopyObject);
				mPoolOfAGameObject[1]=(IPoolableObject)CopyObject;
				mPoolOfAGameObject[1].HasPoolReference=true;
				ReturnObjectToPool(CopyObject);

				mObjectPoolerMap.Add(ObjectToAdd.TagObject, mPoolOfAGameObject);
				mIndexForObjectPoolerMap.Add(ObjectToAdd.TagObject, 1);
			}
		}

		private void AddPrototipicalInstanceToPool(Node ObjectToPool){
			//This method is to return protypical instances to the pool (ie to expand the pool for example)
			IPoolableObject PoolableVersion = (IPoolableObject)ObjectToPool;
			string tag = PoolableVersion.TagObject;
			PoolableVersion.ReturnToPool();
			//Put object in position of pool.
			if (ObjectToPool.IsClass("Node2D")){
				((Node2D)ObjectToPool).Position = new Vector2(PoolPosition.X, PoolPosition.Y);
			}
			if (ObjectToPool.IsClass("Node3D")){
				((Node3D)ObjectToPool).Position = PoolPosition;
			}
		}

		// ---------------------- Auxiliar method

		public void ExpandPool(string tagOfPool, int lengthToExpand){
			IPoolableObject[] oldPool = mObjectPoolerMap[tagOfPool]; 
			IPoolableObject[] newPool = new IPoolableObject[oldPool.Length+mNumberForPoolExpandUponFilling];
			oldPool.CopyTo(newPool, 0);
			IPoolableObject prototypeObject = newPool[0];
			for (int i=oldPool.Length; i < newPool.Length; i++){
				Node CopyObject = ((Node)prototypeObject).Duplicate();
				AddChild(CopyObject);
				newPool[i] =(IPoolableObject)CopyObject;
				newPool[i].HasPoolReference = true;
				AddPrototipicalInstanceToPool(CopyObject);
			}
			mObjectPoolerMap[tagOfPool] = newPool;
		}

	}
}