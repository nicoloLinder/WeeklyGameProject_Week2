using UnityEngine;

namespace MeshUtilities
{
    public class Intersections
    {
        //Is a point p inside a triangle p1-p2-p3?
        //From http://totologic.blogspot.se/2014/01/accurate-point-in-triangle-test.html
        public static bool IsPointInTriangle(Vector3 p, Vector3 p1, Vector3 p2, Vector3 p3)
        {
        	bool isWithinTriangle = false;
        
        	float denominator = ((p2.z - p3.z) * (p1.x - p3.x) + (p3.x - p2.x) * (p1.z - p3.z));
        
        	float a = ((p2.z - p3.z) * (p.x - p3.x) + (p3.x - p2.x) * (p.z - p3.z)) / denominator;
        	float b = ((p3.z - p1.z) * (p.x - p3.x) + (p1.x - p3.x) * (p.z - p3.z)) / denominator;
        	float c = 1 - a - b;
        
        	//The point is within the triangle if 0 <= a <= 1 and 0 <= b <= 1 and 0 <= c <= 1
        	if (a >= 0f && a <= 1f && b >= 0f && b <= 1f && c >= 0f && c <= 1f)
        	{
        		isWithinTriangle = true;
        	}
        
        	return isWithinTriangle;
        }
        
        public static bool AreLinesIntersecting(Vector2 l1_p1, Vector2 l1_p2, Vector2 l2_p1, Vector2 l2_p2, bool shouldIncludeEndPoints)
        {
	        bool isIntersecting = false;

	        float denominator = (l2_p2.y - l2_p1.y) * (l1_p2.x - l1_p1.x) - (l2_p2.x - l2_p1.x) * (l1_p2.y - l1_p1.y);

	        //Make sure the denominator is > 0, if not the lines are parallel
	        if (denominator != 0f)
	        {
		        float u_a = ((l2_p2.x - l2_p1.x) * (l1_p1.y - l2_p1.y) - (l2_p2.y - l2_p1.y) * (l1_p1.x - l2_p1.x)) / denominator;
		        float u_b = ((l1_p2.x - l1_p1.x) * (l1_p1.y - l2_p1.y) - (l1_p2.y - l1_p1.y) * (l1_p1.x - l2_p1.x)) / denominator;

		        //Are the line segments intersecting if the end points are the same
		        if (shouldIncludeEndPoints)
		        {
			        //Is intersecting if u_a and u_b are between 0 and 1 or exactly 0 or 1
			        if (u_a >= 0f && u_a <= 1f && u_b >= 0f && u_b <= 1f)
			        {
				        isIntersecting = true;
			        }
		        }
		        else
		        {
			        //Is intersecting if u_a and u_b are between 0 and 1
			        if (u_a > 0f && u_a < 1f && u_b > 0f && u_b < 1f)
			        {
				        isIntersecting = true;
			        }
		        }
		
	        }

	        return isIntersecting;
        }

    }

}