using System;
using Godot;
using System.Collections.Generic;

namespace CoreCode.Scripts{
    public static class BoidSteeringBehaviour{

        //------------------------ Separation behaviour
        public static Vector2 SeparationForce2D(Vector2 agentPos, List<Node2D> flockList, float magnitudeTolerance){
            Vector2 input = Vector2.Zero;
			foreach (Node2D otherAgent in flockList){
				float InverseMangitude = magnitudeTolerance/((agentPos-otherAgent.GlobalPosition).Length());
				input += InverseMangitude > magnitudeTolerance? magnitudeTolerance*(agentPos-otherAgent.GlobalPosition).Normalized() : InverseMangitude*(agentPos-otherAgent.GlobalPosition).Normalized();
			}
			if (input.Length() > 1){
				input = input.Normalized();
			}
			return input; 
        }

        public static Vector3 SeparationForce3D(Vector3 agentPos, List<Node3D> flockList, float magnitudeTolerance){
            Vector3 input = Vector3.Zero;
			foreach (Node3D otherAgent in flockList){
				float InverseMangitude = 1/((agentPos-otherAgent.GlobalPosition).Length());
				input += InverseMangitude > magnitudeTolerance? magnitudeTolerance*(agentPos-otherAgent.GlobalPosition).Normalized() : InverseMangitude*(agentPos-otherAgent.GlobalPosition).Normalized();
			}
			if (input.Length() > 1){
				input = input.Normalized();
			}
			return input; 
        }

		//------------------------ Alignment behaviour

		public static Vector2 AlignmentForce2D(Vector2 agentForward, List<Node2D> flockList){
            Vector2 input = Vector2.Zero; int bodiesCounted=0;
			foreach (Node2D otherAgent in flockList){
				input += -otherAgent.Transform.Y;
				bodiesCounted ++;
			}
			input /= bodiesCounted;
			input = input-agentForward;
			if (input.Length() > 1){
				input = input.Normalized();
			}
			return input; 
        }

        public static Vector3 AlignmentForce3D(Vector3 agentForward, List<Node3D> flockList){
            Vector3 input = Vector3.Zero; int bodiesCounted=0;
			foreach (Node3D otherAgent in flockList){
				input += -otherAgent.GlobalTransform.Basis.Y;
				bodiesCounted ++;
			}
			input /= bodiesCounted;
			input = input-agentForward;
			if (input.Length() > 1){
				input = input.Normalized();
			}
			return input; 
        }

		//------------------------ Cohesion behaviour

		public static Vector2 CohesionForce2D(Vector2 agentPos, List<Node2D> flockList, Vector2 velocity, float normalizingRange){
            Vector2 centerOfMass=Vector2.Zero; int bodiesCounted=0;
			foreach (Node2D otherAgent in flockList){
				centerOfMass+=otherAgent.GlobalPosition;
				bodiesCounted++;
			}
			centerOfMass/=bodiesCounted;
			Vector2 difVector = centerOfMass-agentPos;
			return Mathf.Min(1,difVector.Length()/normalizingRange)*SteeringBehaviour.SeekDirectionForce2D(agentPos, centerOfMass, velocity);
        }

        public static Vector3 CohesionForce3D(Vector3 agentPos, List<Node3D> flockList, Vector3 velocity, float normalizingRange){
            Vector3 centerOfMass=Vector3.Zero; int bodiesCounted=0;
			foreach (Node3D otherAgent in flockList){
				centerOfMass+=otherAgent.GlobalPosition;
				bodiesCounted++;
			}
			centerOfMass/=bodiesCounted;
			Vector3 difVector = centerOfMass-agentPos;
			return Mathf.Min(1,difVector.Length()/normalizingRange)*SteeringBehaviour.SeekDirectionForce3D(agentPos, centerOfMass, velocity);
        }
       
    }
}
