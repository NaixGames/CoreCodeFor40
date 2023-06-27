using Godot;
using System;

namespace CoreCode.MathUtils
{
	public interface IOperations<T>
    {
        T Add(T op1,T op2);
        T Sub(T op1,T op2);
        T Mul(T op1, float op2);
        T Div(T op1, float op2);
    }

	public struct FloatOperations : IOperations<float>
    {
        public float Add(float op1,float op2) => op1 + op2;
        public float Sub(float op1,float op2) => op1 - op2;
        public float Mul(float op1,float op2) => op1 * op2;
        public float Div(float op1,float op2) => op1 / op2;
    }
 
    public struct Vector3Operations : IOperations<Vector3>
    {
        public Vector3 Add(Vector3 op1,Vector3 op2) => op1 + op2;
        public Vector3 Sub(Vector3 op1,Vector3 op2) => op1 - op2;
        public Vector3 Mul(Vector3 op1,float op2) => op1 * op2;
        public Vector3 Div(Vector3 op1,float op2) => op1 / op2;
    }

    public struct Vector2Operations : IOperations<Vector2>
    {
        public Vector2 Add(Vector2 op1,Vector2 op2) => op1 + op2;
        public Vector2 Sub(Vector2 op1,Vector2 op2) => op1 - op2;
        public Vector2 Mul(Vector2 op1,float op2) => op1 * op2;
        public Vector2 Div(Vector2 op1,float op2) => op1 / op2;
    }
}