using Godot;
using System;
using System.Collections.Generic;

namespace CoreCode.Scripts{
	public partial class BoidActorsManager2D : Area2D
	{
		private List<Node2D> mActorsForBoid = new List<Node2D>();

		public List<Node2D> ActorsForBoid{
			get{return mActorsForBoid;}
		}
		
		private void OnBodyTrigerEnter(Node2D body){
			if (body.IsInGroup("BoidActor") && body != this.GetParent<Node2D>()){
				mActorsForBoid.Add(body);
			}
		}

		private void OnBodyTrigerExit(Node2D body){
			if (body.IsInGroup("BoidActor") && mActorsForBoid.Contains(body)  && body != this.GetParent<Node2D>()){
				mActorsForBoid.Remove(body);
			}
		}
		
	}
}