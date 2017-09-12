#pragma once

#include <math.h>

namespace deft
{
	typedef struct
	{
		float x, y;
		int w, h;
	} Rect;

	typedef struct
	{
		float x, y;
		int radius;
	} Circle;

	namespace geometry
	{
		//
		// Methods with explicit Shapes
		//
		static bool rects_collide(Rect r1, Rect r2)
		{
			if (r1.x < r2.x + r2.w && r2.x < r1.x + r1.w && r1.y < r2.y + r2.h)
				return r2.y < r1.y + r1.h;
			return false;
		}

		static bool circles_collide(Circle c1, Circle c2)
		{
			return fabs((c1.x - c2.x) * (c1.x - c2.x) + (c1.y - c2.y) * (c1.y - c2.y)) <
				(c1.radius + c2.radius) * (c1.radius + c2.radius);
		}

		//
		// Methods with generic Shapes
		//
		static bool shapes_collide(Rect r1, Rect r2)
			{return rects_collide(r1, r2);}

		static bool shapes_collide(Circle c1, Circle c2)
			{return circles_collide(c1, c2);}
	}
}