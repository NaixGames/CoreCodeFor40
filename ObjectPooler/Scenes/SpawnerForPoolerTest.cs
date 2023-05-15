using Godot;
using System;
using System.Collections.Generic;
using CoreCode.Scripts;

namespace CoreCode.Example{
	public partial class SpawnerForPoolerTest : Node2D
	{
		[Export]
		private InputReaderAbstract mInputReference;

		[Export]
		private Vector2 mPositionSpawn;

		[Export]
		private float mXAxisSpawnOffset;
		private float mAxisTotalOffsetRed=0;
		private float mAxisTotalOffsetGreen=0;

		[Export]
		private float mYAxisSpawnOffset;

		private Stack<Node2D> mGreenObjectStack = new Stack<Node2D>();
		private Stack<Node2D> mRedObjectStack = new Stack<Node2D>();

		private LogObject mLog;

		[Export] bool mShouldLog = true;

		public override void _Ready(){
			if (mShouldLog){
				mLog = LogManager.Instance.RequestLogReference("GameObjectPooler", 0);
			}
		}
		

		public override void _Process(double delta)
		{
			if (mInputReference.IsButtonJustPressedInput("Right")){
				Vector2 RealOffsetPosition = mPositionSpawn + new Vector2(mAxisTotalOffsetRed,0);
				Node2D mNewObject = (GameObjectPooler.Instance as GameObjectPooler2D).InstantiateGameObjectIn2D("RedObject", RealOffsetPosition,0);
				mAxisTotalOffsetRed += mXAxisSpawnOffset;
				mRedObjectStack.Push(mNewObject);
			}
			if (mInputReference.IsButtonJustPressedInput("Left")){
				if (mRedObjectStack.Count == 0){
					mLog.AddToLogString("Emtpy red object stack!"); 
					return;
				}
				Node2D mLastRed = mRedObjectStack.Pop();
				GameObjectPooler.Instance.ReturnObjectToPool(mLastRed);
				mAxisTotalOffsetRed -= mXAxisSpawnOffset;
			}
			if (mInputReference.IsButtonJustPressedInput("Up")){
				Vector2 RealOffsetPosition = mPositionSpawn + new Vector2(mAxisTotalOffsetGreen,mYAxisSpawnOffset);
				Node2D mNewObject = (GameObjectPooler.Instance as GameObjectPooler2D).InstantiateGameObjectIn2D("GreenObject", RealOffsetPosition,0);
				mAxisTotalOffsetGreen += mXAxisSpawnOffset;
				mGreenObjectStack.Push(mNewObject);
			}
			if (mInputReference.IsButtonJustPressedInput("Down")){
				if (mGreenObjectStack.Count == 0){
					mLog.AddToLogString("Emtpy green object stack!"); 
					return;
				}
				Node2D mLastGreen = mGreenObjectStack.Pop();
				mAxisTotalOffsetGreen -= mXAxisSpawnOffset;
				GameObjectPooler.Instance.ReturnObjectToPool(mLastGreen);
			}
		}
	}
}
