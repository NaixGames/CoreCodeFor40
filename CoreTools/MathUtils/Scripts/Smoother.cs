using Godot;
using System;

namespace CoreCode.MathUtils
{
	public class Smoother<T,TOp>
		where TOp : struct, IOperations<T>
	{
		// -------------------------------Information
		/* This is a class to provide smoothing/averaging operations on a window for generics objects
		that allow arithmetic operations. Useful in the AI system, but also I am sure it will be useful
		in other places */

		//------------------------------------------------------------------

		private T mActualValue;

		private T[] mRecordedValues;

		private int mIndexRecorded;

		private int mWindowLength;

		private bool mFirstPass=true;

		//Anoying but needed 

		private TOp Operations = new TOp();

		//------------------------------------------------------------------

		public Smoother(int smoothingWindow){
			mRecordedValues = new T[smoothingWindow]; 
			mIndexRecorded = 0;
			mWindowLength = smoothingWindow;
		}

		public T Smooth(T newValue){
			if (mFirstPass){
				T partialResult = Operations.Div(newValue, mWindowLength);
				mActualValue = Operations.Add(mActualValue, partialResult);
				mRecordedValues[mIndexRecorded] = newValue;
				mIndexRecorded = (mIndexRecorded + 1)%mWindowLength;
				mFirstPass = mIndexRecorded != 0;
			}
			else{
				T partialResultOne = Operations.Div(mRecordedValues[mIndexRecorded], mWindowLength);
				T partialResultTwo = Operations.Div(newValue,mWindowLength);
				partialResultOne = Operations.Sub(mActualValue, partialResultOne);
				mActualValue = Operations.Add(partialResultOne, partialResultTwo);
				mRecordedValues[mIndexRecorded] = newValue;
				mIndexRecorded = (mIndexRecorded + 1)%mWindowLength;
			}
			return mActualValue;
		}
	}
}