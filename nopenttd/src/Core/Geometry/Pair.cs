//original geometry_type.cs
/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file geometry_type.hpp All geometry types in OpenTTD. */


//#if defined(__AMIGA__)
//	/* AmigaOS already has a Point declared */
//#define Point OTTD_Point
//#endif /* __AMIGA__ */

//#if defined(__APPLE__)
//	/* Mac OS X already has both Rect and Point declared */
//#define Rect OTTD_Rect
//#define Point OTTD_Point
//#endif /* __APPLE__ */

namespace Nopenttd.Core.Geometry
{
    /** A pair of two integers */
    struct Pair {
        int a;
        int b;
    };
}