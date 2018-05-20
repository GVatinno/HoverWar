using System;
using UnityEngine;

public class MathUtils
{
	// compute solution for quadratic quations ax^2 + bx + c = 0
	// which could give up to two solutions t = -b +- SqrRoot( b^2 -4ac ) / 2a
	// with b^2 - 4ac there no solutions
	public static bool computeQuadraticSolution(float a, float b, float c, out float t0, out float t1) 
	{ 
		float discriminant = b * b - 4.0f * a * c; 
		// no solutions
		if (discriminant < 0.0f) {
			t0 = t1 = 0.0f;
			return false; 
		}
		// one solution
		if (discriminant == 0.0f) {
			t0 = t1 = -b * 0.5f / a ;
			return true;
		}
		// two solutions
		float sqrtDiscriminant = Mathf.Sqrt(discriminant);
		t0 = (-b + sqrtDiscriminant) * 0.5f / a;
		t1 = (-b - sqrtDiscriminant) * 0.5f / a;

		if (t0 > t1) {
			float temp = t0;
			t1 = t0;
			t0 = temp;
		} 
		return true; 
	} 
}


