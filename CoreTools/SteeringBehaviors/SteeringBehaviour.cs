using System;
using Godot;

namespace CoreCode.Scripts{
    public static class SteeringBehaviour{

        //------------------------ Seek behaviour
        public static Vector2 SeekDirectionForce2D(Vector2 agentPos, Vector2 objetivePos, Vector2 velocity){
            //We try to seek the target, but also cancel the perpendicular velocity we got.
            Vector2 TargetDirection = (objetivePos-agentPos).Normalized();
            if (velocity.Length()<0.01f){
                return TargetDirection;
            }
            Vector2 desiredVelocity =  TargetDirection*velocity.Length();
            return (desiredVelocity+(desiredVelocity-velocity)).Normalized();
        }

        public static Vector3 SeekDirectionForce3D(Vector3 agentPos, Vector3 objetivePos, Vector3 velocity){
            //We try to seek the target, but also cancel the perpendicular velocity we got.
            Vector3 TargetDirection = (objetivePos-agentPos).Normalized();
            if (velocity.Length()<0.01f){
                return TargetDirection;
            }
            Vector3 desiredVelocity =  TargetDirection*velocity.Length();
            return (desiredVelocity+(desiredVelocity-velocity)).Normalized();
        }

        //------------------------ Arrive behaviour

        public static Vector2 ArriveDirectionForce2D(Vector2 agentPos, Vector2 objetivePos, Vector2 velocity, float desacelerationRange, float velocityTolerance){
            Vector2 distVector = objetivePos-agentPos;
            if ((objetivePos-agentPos).Length()>desacelerationRange){
                return SeekDirectionForce2D(agentPos, objetivePos, velocity);
            }
            if (velocity.Length()>velocityTolerance){
                return -velocity.Normalized();
            }
            return Vector2.Zero;
        }

        public static Vector3 ArriveDirectionForce3D(Vector3 agentPos, Vector3 objetivePos, Vector3 velocity, float desacelerationRange, float velocityTolerance){
            Vector3 distVector = objetivePos-agentPos;
            if ((objetivePos-agentPos).Length()>desacelerationRange){
                return SeekDirectionForce3D(agentPos, objetivePos, velocity);
            }
            if (velocity.Length()>velocityTolerance){
                return -velocity.Normalized();
            }
            return Vector3.Zero;
        }

         //------------------------ Anticipate behaviour

        public static Vector2 AnticipateDirectionForce2D(Vector2 agentPos, Vector2 objectivePos, Vector2 agentVelocity, Vector2 objectiveVelocity){
            Vector2 objectiveVector = objectivePos-agentPos;
            float totalVelocity = agentVelocity.Length()+objectiveVelocity.Length();
            if (Mathf.IsZeroApprox(totalVelocity)){
                return (objectiveVector).Normalized();
            }
            float lookAheadTime = objectiveVector.Length()/(totalVelocity);
            Vector2 realObjectivePoint = objectivePos + objectiveVelocity*lookAheadTime;
            return SeekDirectionForce2D(agentPos, realObjectivePoint, agentVelocity);
        }

        public static Vector3 AnticipateDirectionForce3D(Vector3 agentPos, Vector3 objectivePos, Vector3 agentVelocity, Vector3 objectiveVelocity){
            Vector3 objectiveVector = objectivePos-agentPos;
            float totalVelocity = agentVelocity.Length()+objectiveVelocity.Length();
            if (Mathf.IsZeroApprox(totalVelocity)){
                return (objectiveVector).Normalized();
            }
            float lookAheadTime = objectiveVector.Length()/(totalVelocity);
            Vector3 realObjectivePoint = objectivePos + objectiveVelocity*lookAheadTime;
            return SeekDirectionForce3D(agentPos, realObjectivePoint, agentVelocity);
        }

        //------------------------ Flee behaviour

        public static Vector2 FleeInRangeDirectionForce2D(Vector2 agentPos, Vector2 objetivePos, Vector2 velocity, float threshold){
            if ((agentPos-objetivePos).Length()>threshold){
                return Vector2.Zero;
            }
            return -SeekDirectionForce2D(agentPos, objetivePos, velocity);
        }

         public static Vector3 FleeInRangeDirectionForce3D(Vector3 agentPos, Vector3 objetivePos, Vector3 velocity, float threshold){
            if ((agentPos-objetivePos).Length()>threshold){
                return Vector3.Zero;
            }
            return -SeekDirectionForce3D(agentPos, objetivePos, velocity);
        }

        //------------------------ Evade behaviour

        public static Vector2 EvadeInRangeDirectionForce2D(Vector2 agentPos, Vector2 objetivePos, Vector2 agentVelocity, Vector2 objectiveVelocity, float threshold){
            if ((agentPos-objetivePos).Length()>threshold){
                return Vector2.Zero;
            }
            return -AnticipateDirectionForce2D(agentPos, objetivePos, agentVelocity, objectiveVelocity);
        }

         public static Vector3 EvadeInRangeDirectionForce3D(Vector3 agentPos, Vector3 objetivePos, Vector3 agentVelocity, Vector3 objectiveVelocity, float threshold){
            if ((agentPos-objetivePos).Length()>threshold){
                return Vector3.Zero;
            }
            return -AnticipateDirectionForce3D(agentPos, objetivePos, agentVelocity, objectiveVelocity);
        }

         //------------------------ Wander behaviour

        public static Vector2 WanderForce2D(Vector2 forward, float radius, float distance, float jitter, Vector2 oldWanderTarget, out Vector2 newWanderTarget){
            Vector2 randomOffset = new Vector2(GD.RandRange(-1,1),GD.RandRange(-1,1));
            newWanderTarget = (oldWanderTarget + jitter*randomOffset).Normalized();
            return (distance*forward + newWanderTarget*radius).Normalized();
        }

        public static Vector3 WanderForce3D(Vector3 forward, float radius, float distance, float jitter, Vector3 oldWanderTarget, out Vector3 newWanderTarget){
            Vector3 randomOffset = new Vector3(GD.RandRange(-1,1),GD.RandRange(-1,1), GD.RandRange(-1,1));
            newWanderTarget = (oldWanderTarget + jitter*randomOffset).Normalized();
            return (distance*forward + newWanderTarget*radius).Normalized();
        }
    }
}
