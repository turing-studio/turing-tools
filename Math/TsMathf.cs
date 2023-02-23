using UnityEngine;

namespace Turing.Tools.Math
{
	public static class TsMathf 
	{
		public static Vector3 AbsVector(Vector3 vector) {
            vector.x = System.Math.Abs(vector.x);
            vector.y = System.Math.Abs(vector.y);
            vector.z = System.Math.Abs(vector.z);
            return vector;
        }

        public static Vector2 AbsVector(Vector2 vector)
        {
            vector.x = System.Math.Abs(vector.x);
            vector.y = System.Math.Abs(vector.y);
            return vector;
        }
        
		public static int Sum(params int[] array)
		{
			int result=0;
			for (int i = 0; i < array.Length; i++)
				result += array[i];
			return result;
		}
		
		public static float Approach(float from, float to, float amount)
		{
			if (from < to)
			{
				from += amount;
				if (from > to)
					return to;
			}
			else
			{
				from -= amount;
				if (from < to)
					return to;
			}
			return from;
		} 

        public static bool FloatEqual(float a, float b)
        {
            return Mathf.Abs(a - b) < Mathf.Epsilon;
        }

        public static bool VectorsEqual(Vector3 a, Vector3 b)
        {
            return FloatEqual(a.x, b.x) && FloatEqual(a.y, b.y) && FloatEqual(a.z, b.z);
        }

        public static float SinusoidalDamp(float from, float to, float t)
        {
            float tSin = Mathf.Cos(t * Mathf.PI);
            return (from + to) / 2 - tSin * (to - from) / 2;
        }
    }
}