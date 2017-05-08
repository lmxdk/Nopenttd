/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file rail.h Rail specific functions. */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Nopenttd.Core;
using Nopenttd.src;
using Nopenttd.src.Settings;
using Nopenttd.Slopes;
using Nopenttd.Tiles;

namespace Nopenttd
{
    
    /** Railtype flags. */
    [Flags]
public enum RailTypeFlags {                              /// Bit number for drawing a catenary.
	RTF_CATENARY          = 0,                    /// Bit number for disallowing level crossings.       
	RTF_NO_LEVEL_CROSSING = 1,                           
                                                  /// All flags cleared.
	RTFB_NONE              = 0,                   /// Value for drawing a catenary.       
	RTFB_CATENARY          = 1 << RTF_CATENARY,   /// Value for disallowing level crossings.       
        RTFB_NO_LEVEL_CROSSING = 1 << RTF_NO_LEVEL_CROSSING, 
};



/** Sprite groups for a railtype. */
public enum RailTypeSpriteGroup {/// Cursor and toolbar icon images
        RTSG_CURSORS,         /// Images for overlaying track
	RTSG_OVERLAY,         /// Main group of ground images
	RTSG_GROUND,          /// Main group of ground images for snow or desert
	RTSG_TUNNEL,          /// Catenary wires
	RTSG_WIRES,           /// Catenary pylons
	RTSG_PYLONS,          /// Bridge surface images
	RTSG_BRIDGE,          /// Level crossing overlay images
	RTSG_CROSSING,        /// Depot images
	RTSG_DEPOT,           /// Fence images
	RTSG_FENCES,          /// Tunnel portal overlay
	RTSG_TUNNEL_PORTAL,   /// Signal images
	RTSG_SIGNALS,       
	RTSG_END,
};

/**
 * Offsets for sprites within an overlay/underlay set.
 * These are the same for overlay and underlay sprites.
 */
public enum RailTrackOffset {/// Piece of rail in X direction
        RTO_X,                   /// Piece of rail in Y direction
	RTO_Y,                   /// Piece of rail in northern corner
	RTO_N,                   /// Piece of rail in southern corner
	RTO_S,                   /// Piece of rail in eastern corner
	RTO_E,                   /// Piece of rail in western corner
	RTO_W,                   /// Piece of rail on slope with north-east raised
	RTO_SLOPE_NE,            /// Piece of rail on slope with south-east raised
	RTO_SLOPE_SE,            /// Piece of rail on slope with south-west raised
	RTO_SLOPE_SW,            /// Piece of rail on slope with north-west raised
	RTO_SLOPE_NW,            /// Crossing of X and Y rail, with ballast
	RTO_CROSSING_XY,         /// Ballast for junction 'pointing' SW
	RTO_JUNCTION_SW,         /// Ballast for junction 'pointing' NE
	RTO_JUNCTION_NE,         /// Ballast for junction 'pointing' SE
	RTO_JUNCTION_SE,         /// Ballast for junction 'pointing' NW
	RTO_JUNCTION_NW,         /// Ballast for full junction
	RTO_JUNCTION_NSEW,
};

/**
 * Offsets for sprites within a bridge surface overlay set.
 */
enum RailTrackBridgeOffset {/// Piece of rail in X direction
        RTBO_X,                 /// Piece of rail in Y direction
	RTBO_Y,                 /// Sloped rail pieces, in order NE, SE, SW, NW
	RTBO_SLOPE, 
};

/**
 * Offsets from base sprite for fence sprites. These are in the order of
 *  the sprites in the original data files.
 */
public enum RailFenceOffset
{// Slope FLAT, Track X,     Fence NW
        RFO_FLAT_X_NW,    // Slope FLAT, Track Y,     Fence NE 
	RFO_FLAT_Y_NE,    // Slope FLAT, Track LEFT,  Fence E 
	RFO_FLAT_LEFT,    // Slope FLAT, Track UPPER, Fence S 
	RFO_FLAT_UPPER,   // Slope SW,   Track X,     Fence NW 
	RFO_SLOPE_SW_NW,  // Slope SE,   Track Y,     Fence NE 
	RFO_SLOPE_SE_NE,  // Slope NE,   Track X,     Fence NW 
	RFO_SLOPE_NE_NW,  // Slope NW,   Track Y,     Fence NE 
	RFO_SLOPE_NW_NE,  // Slope FLAT, Track X,     Fence SE 
	RFO_FLAT_X_SE,    // Slope FLAT, Track Y,     Fence SW 
	RFO_FLAT_Y_SW,    // Slope FLAT, Track RIGHT, Fence W 
	RFO_FLAT_RIGHT,   // Slope FLAT, Track LOWER, Fence N 
	RFO_FLAT_LOWER,   // Slope SW,   Track X,     Fence SE 
	RFO_SLOPE_SW_SE,  // Slope SE,   Track Y,     Fence SW 
	RFO_SLOPE_SE_SW,  // Slope NE,   Track X,     Fence SE 
	RFO_SLOPE_NE_SE,  // Slope NW,   Track Y,     Fence SW 
	RFO_SLOPE_NW_SW,   
};

    /**
	 * Struct containing the main sprites. @note not all sprites are listed, but only
	 *  the ones used directly in the code
	 */

    public class RailBaseSprites {/// single piece of rail in Y direction, with ground
        SpriteID track_y;           /// two pieces of rail in North and South corner (East-West direction)
        SpriteID track_ns;          /// ground sprite for a 3-way switch
        SpriteID ground;            /// single piece of rail in X direction, without ground
        SpriteID single_x;          /// single piece of rail in Y direction, without ground
        SpriteID single_y;          /// single piece of rail in the northern corner
        SpriteID single_n;          /// single piece of rail in the southern corner
        SpriteID single_s;          /// single piece of rail in the eastern corner
        SpriteID single_e;          /// single piece of rail in the western corner
        SpriteID single_w;          /// single piece of rail for slopes
        SpriteID single_sloped;     /// level crossing, rail in X direction
        SpriteID crossing;          /// tunnel sprites base
        SpriteID tunnel;       
    }


    /**
	 * struct containing the sprites for the rail GUI. @note only sprites referred to
	 * directly in the code are listed
	 */
    public class RailGuiSprites {/// button for building single rail in N-S direction
        SpriteID build_ns_rail;  /// button for building single rail in X direction    
    SpriteID build_x_rail;   /// button for building single rail in E-W direction    
    SpriteID build_ew_rail;  /// button for building single rail in Y direction    
    SpriteID build_y_rail;   /// button for the autorail construction    
    SpriteID auto_rail;      /// button for building depots    
    SpriteID build_depot;    /// button for building a tunnel    
    SpriteID build_tunnel;   /// button for converting rail    
    SpriteID convert_rail;       /// signal GUI sprites (type, variant, state)
        SpriteID[][][] signals = new SpriteID[SIGTYPE_END][2][2]; 
    }

    /// Cursors associated with the rail type.
    public class RailCursors
    {
        /// Cursor for building rail in N-S direction
        CursorID rail_ns;

        /// Cursor for building rail in X direction 
        CursorID rail_swne;

        /// Cursor for building rail in E-W direction 
        CursorID rail_ew;

        /// Cursor for building rail in Y direction 
        CursorID rail_nwse;

        /// Cursor for autorail tool 
        CursorID autorail;

        /// Cursor for building a depot 
        CursorID depot;

        /// Cursor for building a tunnel 
        CursorID tunnel;

        /// Cursor for converting track 
        CursorID convert;
    }


    public class RailStrings
    {
            /// Name of this rail type.
            StringID name;

            /// Caption in the construction toolbar GUI for this rail type.
            StringID toolbar_caption;

            /// Name of this rail type in the main toolbar dropdown.
            StringID menu_text;

            /// Caption of the build vehicle GUI for this rail type.
            StringID build_caption;

            /// Text used in the autoreplace GUI.
            StringID replace_text;

            /// Name of an engine for this type of rail in the engine preview GUI.
            StringID new_loco; /// Strings associated with the rail type.
        }

        /**
         * This struct contains all the info that is needed to draw and construct tracks.
         */
        public class RailtypeInfo
        {

            public RailBaseSprites base_sprites = new RailBaseSprites();
            public RailGuiSprites gui_sprites = new RailGuiSprites();
            public RailCursors cursors = new RailCursors();
            public RailStrings strings = new RailStrings();

            /** sprite number difference between a piece of track on a snowy ground and the corresponding one on normal ground */
            public SpriteID snow_offset;

            /** bitmask to the OTHER railtypes on which an engine of THIS railtype generates power */
            public RailTypes powered_railtypes;

            /** bitmask to the OTHER railtypes on which an engine of THIS railtype can physically travel */
            public RailTypes compatible_railtypes;

            /**
             * Bridge offset
             */
            public SpriteID bridge_offset;

            /**
             * Original railtype number to use when drawing non-newgrf railtypes, or when drawing stations.
             */
            public byte fallback_railtype;

            /**
             * Multiplier for curve maximum speed advantage
             */
            public byte curve_speed;

            /**
             * Bit mask of rail type flags
             */
            public RailTypeFlags flags;

            /**
             * Cost multiplier for building this rail type
             */
            public ushort cost_multiplier;

            /**
             * Cost multiplier for maintenance of this rail type
             */
            public ushort maintenance_multiplier;

            /**
             * Acceleration type of this rail type
             */
            public byte acceleration_type;

            /**
             * Maximum speed for vehicles travelling on this rail type
             */
            public ushort max_speed;

            /**
             * Unique 32 bit rail type identifier
             */
            public RailTypeLabel label;

            /**
             * Rail type labels this type provides in addition to the main label.
             */
            public List<RailTypeLabel> alternate_labels;

            /**
             * Colour on mini-map
             */
            public byte map_colour;

            /**
             * Introduction date.
             * When #INVALID_DATE or a vehicle using this railtype gets introduced earlier,
             * the vehicle's introduction date will be used instead for this railtype.
             * The introduction at this date is furthermore limited by the
             * #introduction_required_types.
             */
            public Date introduction_date;

            /**
             * Bitmask of railtypes that are required for this railtype to be introduced
             * at a given #introduction_date.
             */
            public RailTypes introduction_required_railtypes;

            /**
             * Bitmask of which other railtypes are introduced when this railtype is introduced.
             */
            public RailTypes introduces_railtypes;

            /**
             * The sorting order of this railtype for the toolbar dropdown.
             */
            public byte sorting_order;

            /**
             * NewGRF providing the Action3 for the railtype. NULL if not available.
             */
            public GRFFile[] grffile = new GRFFile[(int) RailTypeSpriteGroup.RTSG_END];

            /**
             * Sprite groups for resolving sprites
             */
            public SpriteGroup[] group = new SpriteGroup[(int) RailTypeSpriteGroup.RTSG_END];

            public bool UsesOverlay()
            {
                return this.group[(int) RailTypeSpriteGroup.RTSG_GROUND] != null;
            }

            /**
             * Offset between the current railtype and normal rail. This means that:<p>
             * 1) All the sprites in a railset MUST be in the same order. This order
             *    is determined by normal rail. Check sprites 1005 and following for this order<p>
             * 2) The position where the railtype is loaded must always be the same, otherwise
             *    the offset will fail.
             */
            public uint GetRailtypeSpriteOffset()
            {
                return 82 * this.fallback_railtype;
            }
        };

        public static class RailTypeExtensions
        {

/**
 * Returns a pointer to the Railtype information for a given railtype
 * @param railtype the rail type which the information is requested for
 * @return The pointer to the RailtypeInfo
 */
            public static RailtypeInfo GetRailTypeInfo(this RailType railtype)
            {

                //extern RailtypeInfo _railtypes[RailType.RAILTYPE_END];
                Debug.Assert(railtype < RailType.RAILTYPE_END);
                return _railtypes[railtype];
            }

/**
 * Checks if an engine of the given RailType can drive on a tile with a given
 * RailType. This would normally just be an equality check, but for electric
 * rails (which also support non-electric engines).
 * @return Whether the engine can drive on this tile.
 * @param  enginetype The RailType of the engine we are considering.
 * @param  tiletype   The RailType of the tile we are considering.
 */
            public static bool IsCompatibleRail(this RailType enginetype, RailType tiletype)
            {
                return BitMath.HasBit(GetRailTypeInfo(enginetype).compatible_railtypes, tiletype);
            }

/**
 * Checks if an engine of the given RailType got power on a tile with a given
 * RailType. This would normally just be an equality check, but for electric
 * rails (which also support non-electric engines).
 * @return Whether the engine got power on this tile.
 * @param  enginetype The RailType of the engine we are considering.
 * @param  tiletype   The RailType of the tile we are considering.
 */
            public static bool HasPowerOnRail(this RailType enginetype, RailType tiletype)
            {
                return BitMath.HasBit(GetRailTypeInfo(enginetype).powered_railtypes, tiletype);
            }

/**
 * Test if a RailType disallows build of level crossings.
 * @param rt The RailType to check.
 * @return Whether level crossings are not allowed.
 */
            public static bool RailNoLevelCrossings(this RailType rt)
            {
                return BitMath.HasBit(GetRailTypeInfo(rt).flags, RailTypeFlags.RTF_NO_LEVEL_CROSSING);
            }

/**
 * Returns the cost of building the specified railtype.
 * @param railtype The railtype being built.
 * @return The cost multiplier.
 */
            public static Money RailBuildCost(this RailType railtype)
            {
                Debug.Assert(railtype < RailType.RAILTYPE_END);
                return (_price[PR_BUILD_RAIL] * GetRailTypeInfo(railtype).cost_multiplier) >> 3;
            }

/**
 * Returns the 'cost' of clearing the specified railtype.
 * @param railtype The railtype being removed.
 * @return The cost.
 */
            public static Money RailClearCost(RailType railtype)
            {
                /* Clearing rail in fact earns money, but if the build cost is set
                 * very low then a loophole exists where money can be made.
                 * In this case we limit the removal earnings to 3/4s of the build
                 * cost.
                 */
                Debug.Assert(railtype < RailType.RAILTYPE_END);
                return Math.Max(_price[PR_CLEAR_RAIL], -RailBuildCost(railtype) * 3 / 4);
            }

/**
 * Calculates the cost of rail conversion
 * @param from The railtype we are converting from
 * @param to   The railtype we are converting to
 * @return Cost per TrackBit
 */
            public static Money RailConvertCost(RailType from, RailType to)
            {
                /* Get the costs for removing and building anew
                 * A conversion can never be more costly */
                Money rebuildcost = RailBuildCost(to) + RailClearCost(from);

                /* Conversion between somewhat compatible railtypes:
                 * Pay 1/8 of the target rail cost (labour costs) and additionally any difference in the
                 * build costs, if the target type is more expensive (material upgrade costs).
                 * Upgrade can never be more expensive than re-building. */
                if (HasPowerOnRail(from, to) || HasPowerOnRail(to, from))
                {
                    Money upgradecost = RailBuildCost(to) / 8 +
                                        Math.Max((Money) 0, RailBuildCost(to) - RailBuildCost(from));
                    return Math.Min(upgradecost, rebuildcost);
                }

                /* make the price the same as remove + build new type for rail types
                 * which are not compatible in any way */
                return rebuildcost;
            }

/**
 * Calculates the maintenance cost of a number of track bits.
 * @param railtype The railtype to get the cost of.
 * @param num Number of track bits of this railtype.
 * @param total_num Total number of track bits of all railtypes.
 * @return Total cost.
 */
            public static Money RailMaintenanceCost(this RailType railtype, uint num, uint total_num)
            {
                Debug.Assert(railtype < RailType.RAILTYPE_END);
                return (_price[PR_INFRASTRUCTURE_RAIL] * GetRailTypeInfo(railtype).maintenance_multiplier * num *
                        (1 + IntSqrt(total_num))) >> 11; // 4 bits fraction for the multiplier and 7 bits scaling.
            }
        }

        public class Rail
        {

            /* XXX: Below 3 tables store duplicate data. Maybe remove some? */
            /* Maps a trackdir to the bit that stores its status in the map arrays, in the
             * direction along with the trackdir */
            public static readonly byte[] _signal_along_trackdir = //new [(int)Trackdir.TRACKDIR_END] = 
            {
                0x8, 0x8, 0x8, 0x2, 0x4, 0x1, 0, 0,
                0x4, 0x4, 0x4, 0x1, 0x8, 0x2
            };

            /* Maps a trackdir to the bit that stores its status in the map arrays, in the
             * direction against the trackdir */
            public static readonly byte[] _signal_against_trackdir = //[(int)Trackdir.TRACKDIR_END] = 
            {
                0x4, 0x4, 0x4, 0x1, 0x8, 0x2, 0, 0,
                0x8, 0x8, 0x8, 0x2, 0x4, 0x1
            };

            /* Maps a Track to the bits that store the status of the two signals that can
             * be present on the given track */
            public static readonly byte[] _signal_on_track =
            {
                0xC, 0xC, 0xC, 0x3, 0xC, 0x3
            };

            /* Maps a diagonal direction to the all trackdirs that are connected to any
             * track entering in this direction (including those making 90 degree turns)
             */
            public static readonly TrackdirBits[] _exitdir_reaches_trackdirs =
            {
                TrackdirBits.TRACKDIR_BIT_X_NE | TrackdirBits.TRACKDIR_BIT_LOWER_E |
                TrackdirBits.TRACKDIR_BIT_LEFT_N, // DIAGDIR_NE
                TrackdirBits.TRACKDIR_BIT_Y_SE | TrackdirBits.TRACKDIR_BIT_LEFT_S |
                TrackdirBits.TRACKDIR_BIT_UPPER_E, // DIAGDIR_SE
                TrackdirBits.TRACKDIR_BIT_X_SW | TrackdirBits.TRACKDIR_BIT_UPPER_W |
                TrackdirBits.TRACKDIR_BIT_RIGHT_S, // DIAGDIR_SW
                TrackdirBits.TRACKDIR_BIT_Y_NW | TrackdirBits.TRACKDIR_BIT_RIGHT_N |
                TrackdirBits.TRACKDIR_BIT_LOWER_W // DIAGDIR_NW
            };

            public static readonly Trackdir[] _next_trackdir = //[TRACKDIR_END] = 
            {
                Trackdir.TRACKDIR_X_NE, Trackdir.TRACKDIR_Y_SE, Trackdir.TRACKDIR_LOWER_E, Trackdir.TRACKDIR_UPPER_E,
                Trackdir.TRACKDIR_RIGHT_S, Trackdir.TRACKDIR_LEFT_S, Trackdir.INVALID_TRACKDIR,
                Trackdir.INVALID_TRACKDIR,
                Trackdir.TRACKDIR_X_SW, Trackdir.TRACKDIR_Y_NW, Trackdir.TRACKDIR_LOWER_W, Trackdir.TRACKDIR_UPPER_W,
                Trackdir.TRACKDIR_RIGHT_N, Trackdir.TRACKDIR_LEFT_N
            };

            /* Maps a trackdir to all trackdirs that make 90 deg turns with it. */
            public static readonly TrackdirBits[] _track_crosses_trackdirs = //[TRACKDIR_END] = 
            {
                TrackdirBits.TRACKDIR_BIT_Y_SE | TrackdirBits.TRACKDIR_BIT_Y_NW, // TRACK_X
                TrackdirBits.TRACKDIR_BIT_X_NE | TrackdirBits.TRACKDIR_BIT_X_SW, // TRACK_Y
                TrackdirBits.TRACKDIR_BIT_RIGHT_N | TrackdirBits.TRACKDIR_BIT_RIGHT_S |
                TrackdirBits.TRACKDIR_BIT_LEFT_N | TrackdirBits.TRACKDIR_BIT_LEFT_S, // TRACK_UPPER
                TrackdirBits.TRACKDIR_BIT_RIGHT_N | TrackdirBits.TRACKDIR_BIT_RIGHT_S |
                TrackdirBits.TRACKDIR_BIT_LEFT_N | TrackdirBits.TRACKDIR_BIT_LEFT_S, // TRACK_LOWER
                TrackdirBits.TRACKDIR_BIT_UPPER_W | TrackdirBits.TRACKDIR_BIT_UPPER_E |
                TrackdirBits.TRACKDIR_BIT_LOWER_W | TrackdirBits.TRACKDIR_BIT_LOWER_E, // TRACK_LEFT
                TrackdirBits.TRACKDIR_BIT_UPPER_W | TrackdirBits.TRACKDIR_BIT_UPPER_E |
                TrackdirBits.TRACKDIR_BIT_LOWER_W | TrackdirBits.TRACKDIR_BIT_LOWER_E // TRACK_RIGHT
            };

/* Maps a track to all tracks that make 90 deg turns with it. */
            public static readonly TrackBits[] _track_crosses_tracks =
            {
                TrackBits.TRACK_BIT_Y, // TRACK_X
                TrackBits.TRACK_BIT_X, // TRACK_Y
                TrackBits.TRACK_BIT_VERT, // TRACK_UPPER
                TrackBits.TRACK_BIT_VERT, // TRACK_LOWER
                TrackBits.TRACK_BIT_HORZ, // TRACK_LEFT
                TrackBits.TRACK_BIT_HORZ // TRACK_RIGHT
            };

/* Maps a trackdir to the (4-way) direction the tile is exited when following
 * that trackdir */
            public static readonly DiagDirection[] _trackdir_to_exitdir = //[TRACKDIR_END] = 
            {
                DiagDirection.DIAGDIR_NE, DiagDirection.DIAGDIR_SE, DiagDirection.DIAGDIR_NE, DiagDirection.DIAGDIR_SE,
                DiagDirection.DIAGDIR_SW, DiagDirection.DIAGDIR_SE, DiagDirection.DIAGDIR_NE, DiagDirection.DIAGDIR_NE,
                DiagDirection.DIAGDIR_SW, DiagDirection.DIAGDIR_NW, DiagDirection.DIAGDIR_NW, DiagDirection.DIAGDIR_SW,
                DiagDirection.DIAGDIR_NW, DiagDirection.DIAGDIR_NE,
            };

            public static readonly Trackdir[][] _track_exitdir_to_trackdir = //[][DIAGDIR_END] = 
            {
                new Trackdir[]
                {
                    Trackdir.TRACKDIR_X_NE, Trackdir.INVALID_TRACKDIR, Trackdir.TRACKDIR_X_SW, Trackdir.INVALID_TRACKDIR
                },
                new Trackdir[]
                {
                    Trackdir.INVALID_TRACKDIR, Trackdir.TRACKDIR_Y_SE, Trackdir.INVALID_TRACKDIR, Trackdir.TRACKDIR_Y_NW
                },
                new Trackdir[]
                {
                    Trackdir.TRACKDIR_UPPER_E, Trackdir.INVALID_TRACKDIR, Trackdir.INVALID_TRACKDIR,
                    Trackdir.TRACKDIR_UPPER_W
                },
                new Trackdir[]
                {
                    Trackdir.INVALID_TRACKDIR, Trackdir.TRACKDIR_LOWER_E, Trackdir.TRACKDIR_LOWER_W,
                    Trackdir.INVALID_TRACKDIR
                },
                new Trackdir[]
                {
                    Trackdir.INVALID_TRACKDIR, Trackdir.INVALID_TRACKDIR, Trackdir.TRACKDIR_LEFT_S,
                    Trackdir.TRACKDIR_LEFT_N
                },
                new Trackdir[]
                {
                    Trackdir.TRACKDIR_RIGHT_N, Trackdir.TRACKDIR_RIGHT_S, Trackdir.INVALID_TRACKDIR,
                    Trackdir.INVALID_TRACKDIR
                }
            };

            public static readonly Trackdir[][] _track_enterdir_to_trackdir = //[][DIAGDIR_END] = 
            {
                new Trackdir[]
                {
                    Trackdir.TRACKDIR_X_NE, Trackdir.INVALID_TRACKDIR, Trackdir.TRACKDIR_X_SW, Trackdir.INVALID_TRACKDIR
                },
                new Trackdir[]
                {
                    Trackdir.INVALID_TRACKDIR, Trackdir.TRACKDIR_Y_SE, Trackdir.INVALID_TRACKDIR, Trackdir.TRACKDIR_Y_NW
                },
                new Trackdir[]
                {
                    Trackdir.INVALID_TRACKDIR, Trackdir.TRACKDIR_UPPER_E, Trackdir.TRACKDIR_UPPER_W,
                    Trackdir.INVALID_TRACKDIR
                },
                new Trackdir[]
                {
                    Trackdir.TRACKDIR_LOWER_E, Trackdir.INVALID_TRACKDIR, Trackdir.INVALID_TRACKDIR,
                    Trackdir.TRACKDIR_LOWER_W
                },
                new Trackdir[]
                {
                    Trackdir.TRACKDIR_LEFT_N, Trackdir.TRACKDIR_LEFT_S, Trackdir.INVALID_TRACKDIR,
                    Trackdir.INVALID_TRACKDIR
                },
                new Trackdir[]
                {
                    Trackdir.INVALID_TRACKDIR, Trackdir.INVALID_TRACKDIR, Trackdir.TRACKDIR_RIGHT_S,
                    Trackdir.TRACKDIR_RIGHT_N
                }
            };

            public static readonly Trackdir[][] _track_direction_to_trackdir = //[][DIR_END] = 
            {
                new Trackdir[]
                {
                    Trackdir.INVALID_TRACKDIR, Trackdir.TRACKDIR_X_NE, Trackdir.INVALID_TRACKDIR,
                    Trackdir.INVALID_TRACKDIR, Trackdir.INVALID_TRACKDIR, Trackdir.TRACKDIR_X_SW,
                    Trackdir.INVALID_TRACKDIR, Trackdir.INVALID_TRACKDIR
                },
                new Trackdir[]
                {
                    Trackdir.INVALID_TRACKDIR, Trackdir.INVALID_TRACKDIR, Trackdir.INVALID_TRACKDIR,
                    Trackdir.TRACKDIR_Y_SE, Trackdir.INVALID_TRACKDIR, Trackdir.INVALID_TRACKDIR,
                    Trackdir.INVALID_TRACKDIR, Trackdir.TRACKDIR_Y_NW
                },
                new Trackdir[]
                {
                    Trackdir.INVALID_TRACKDIR, Trackdir.INVALID_TRACKDIR, Trackdir.TRACKDIR_UPPER_E,
                    Trackdir.INVALID_TRACKDIR, Trackdir.INVALID_TRACKDIR, Trackdir.INVALID_TRACKDIR,
                    Trackdir.TRACKDIR_UPPER_W, Trackdir.INVALID_TRACKDIR
                },
                new Trackdir[]
                {
                    Trackdir.INVALID_TRACKDIR, Trackdir.INVALID_TRACKDIR, Trackdir.TRACKDIR_LOWER_E,
                    Trackdir.INVALID_TRACKDIR, Trackdir.INVALID_TRACKDIR, Trackdir.INVALID_TRACKDIR,
                    Trackdir.TRACKDIR_LOWER_W, Trackdir.INVALID_TRACKDIR
                },
                new Trackdir[]
                {
                    Trackdir.TRACKDIR_LEFT_N, Trackdir.INVALID_TRACKDIR, Trackdir.INVALID_TRACKDIR,
                    Trackdir.INVALID_TRACKDIR, Trackdir.TRACKDIR_LEFT_S, Trackdir.INVALID_TRACKDIR,
                    Trackdir.INVALID_TRACKDIR, Trackdir.INVALID_TRACKDIR
                },
                new Trackdir[]
                {
                    Trackdir.TRACKDIR_RIGHT_N, Trackdir.INVALID_TRACKDIR, Trackdir.INVALID_TRACKDIR,
                    Trackdir.INVALID_TRACKDIR, Trackdir.TRACKDIR_RIGHT_S, Trackdir.INVALID_TRACKDIR,
                    Trackdir.INVALID_TRACKDIR, Trackdir.INVALID_TRACKDIR
                }
            };

            public static readonly Trackdir[] _dir_to_diag_trackdir =
            {
                Trackdir.TRACKDIR_X_NE, Trackdir.TRACKDIR_Y_SE, Trackdir.TRACKDIR_X_SW, Trackdir.TRACKDIR_Y_NW,
            };

            public static readonly TrackBits[] _corner_to_trackbits =
            {
                TrackBits.TRACK_BIT_LEFT, TrackBits.TRACK_BIT_LOWER, TrackBits.TRACK_BIT_RIGHT,
                TrackBits.TRACK_BIT_UPPER,
            };

            public static readonly TrackdirBits[] _uphill_trackdirs =
            {
                ///  0 SLOPE_FLAT
                TrackdirBits.TRACKDIR_BIT_NONE, ///  1 SLOPE_W   . inclined for diagonal track
                TrackdirBits.TRACKDIR_BIT_X_SW |
                TrackdirBits.TRACKDIR_BIT_Y_NW, ///  2 SLOPE_S   . inclined for diagonal track
                TrackdirBits.TRACKDIR_BIT_X_SW | TrackdirBits.TRACKDIR_BIT_Y_SE, ///  3 SLOPE_SW
                TrackdirBits.TRACKDIR_BIT_X_SW, ///  4 SLOPE_E   . inclined for diagonal track
                TrackdirBits.TRACKDIR_BIT_X_NE | TrackdirBits.TRACKDIR_BIT_Y_SE, ///  5 SLOPE_EW
                TrackdirBits.TRACKDIR_BIT_NONE, ///  6 SLOPE_SE
                TrackdirBits.TRACKDIR_BIT_Y_SE, ///  7 SLOPE_WSE . leveled
                TrackdirBits.TRACKDIR_BIT_NONE, ///  8 SLOPE_N   . inclined for diagonal track
                TrackdirBits.TRACKDIR_BIT_X_NE | TrackdirBits.TRACKDIR_BIT_Y_NW, ///  9 SLOPE_NW
                TrackdirBits.TRACKDIR_BIT_Y_NW, /// 10 SLOPE_NS
                TrackdirBits.TRACKDIR_BIT_NONE, /// 11 SLOPE_NWS . leveled
                TrackdirBits.TRACKDIR_BIT_NONE, /// 12 SLOPE_NE
                TrackdirBits.TRACKDIR_BIT_X_NE, /// 13 SLOPE_ENW . leveled
                TrackdirBits.TRACKDIR_BIT_NONE, /// 14 SLOPE_SEN . leveled
                TrackdirBits.TRACKDIR_BIT_NONE, /// 15 invalid
                TrackdirBits.TRACKDIR_BIT_NONE, /// 16 invalid
                TrackdirBits.TRACKDIR_BIT_NONE, /// 17 invalid
                TrackdirBits.TRACKDIR_BIT_NONE, /// 18 invalid
                TrackdirBits.TRACKDIR_BIT_NONE, /// 19 invalid
                TrackdirBits.TRACKDIR_BIT_NONE, /// 20 invalid
                TrackdirBits.TRACKDIR_BIT_NONE, /// 21 invalid
                TrackdirBits.TRACKDIR_BIT_NONE, /// 22 invalid
                TrackdirBits.TRACKDIR_BIT_NONE, /// 23 SLOPE_STEEP_S . inclined for diagonal track
                TrackdirBits.TRACKDIR_BIT_X_SW | TrackdirBits.TRACKDIR_BIT_Y_SE, /// 24 invalid
                TrackdirBits.TRACKDIR_BIT_NONE, /// 25 invalid
                TrackdirBits.TRACKDIR_BIT_NONE, /// 26 invalid
                TrackdirBits.TRACKDIR_BIT_NONE, /// 27 SLOPE_STEEP_W . inclined for diagonal track
                TrackdirBits.TRACKDIR_BIT_X_SW | TrackdirBits.TRACKDIR_BIT_Y_NW, /// 28 invalid
                TrackdirBits.TRACKDIR_BIT_NONE, /// 29 SLOPE_STEEP_N . inclined for diagonal track
                TrackdirBits.TRACKDIR_BIT_X_NE |
                TrackdirBits.TRACKDIR_BIT_Y_NW, /// 30 SLOPE_STEEP_E . inclined for diagonal track
                TrackdirBits.TRACKDIR_BIT_X_NE | TrackdirBits.TRACKDIR_BIT_Y_SE,
            };


/**
 * Calculates the maintenance cost of a number of signals.
 * @param num Number of signals.
 * @return Total cost.
 */
            public static Money SignalMaintenanceCost(uint num)
            {
                return (_price[PR_INFRASTRUCTURE_RAIL] * 15 * num * (1 + MathFuncs.IntSqrt(num))) >>
                       8; // 1 bit fraction for the multiplier and 7 bits scaling.
            }



            /**
             * Return the rail type of tile, or INVALID_RAILTYPE if this is no rail tile.
             */
            RailType GetTileRailType(TileIndex tile)
            {
                switch (TileMap.GetTileType(tile))
                {
                    case TileType.MP_RAILWAY:
                        return GetRailType(tile);

                    case TileType.MP_ROAD:
                        /* rail/road crossing */
                        if (IsLevelCrossing(tile)) return GetRailType(tile);
                        break;

                    case TileType.MP_STATION:
                        if (HasStationRail(tile)) return GetRailType(tile);
                        break;

                    case TileType.MP_TUNNELBRIDGE:
                        if (GetTunnelBridgeTransportType(tile) == TRANSPORT_RAIL) return GetRailType(tile);
                        break;

                    default:
                        break;
                }
                return RailType.INVALID_RAILTYPE;
            }

            /**
             * Finds out if a company has a certain railtype available
             * @param company the company in question
             * @param railtype requested RailType
             * @return true if company has requested RailType available
             */
            public bool HasRailtypeAvail(CompanyID company, RailType railtype)
            {
                return BitMath.HasBit(Company.Get(company).avail_railtypes, railtype);
            }

            /**
             * Validate functions for rail building.
             * @param rail the railtype to check.
             * @return true if the current company may build the rail.
             */
            public bool ValParamRailtype(RailType rail)
            {
                return rail < RailType.RAILTYPE_END && HasRailtypeAvail(_current_company, rail);
            }

            /**
             * Returns the "best" railtype a company can build.
             * As the AI doesn't know what the BEST one is, we have our own priority list
             * here. When adding new railtypes, modify this function
             * @param company the company "in action"
             * @return The "best" railtype a company has available
             */
            public RailType GetBestRailtype(CompanyID company)
            {
                if (HasRailtypeAvail(company, RailType.RAILTYPE_MAGLEV)) return RailType.RAILTYPE_MAGLEV;
                if (HasRailtypeAvail(company, RailType.RAILTYPE_MONO)) return RailType.RAILTYPE_MONO;
                if (HasRailtypeAvail(company, RailType.RAILTYPE_ELECTRIC)) return RailType.RAILTYPE_ELECTRIC;
                return RailType.RAILTYPE_RAIL;
            }

            /**
             * Add the rail types that are to be introduced at the given date.
             * @param current The currently available railtypes.
             * @param date    The date for the introduction comparisons.
             * @return The rail types that should be available when date
             *         introduced rail types are taken into account as well.
             */
            public RailTypes AddDateIntroducedRailTypes(RailTypes current, Date date)
            {
                RailTypes rts = current;

                for (RailType rt = RailType.RAILTYPE_BEGIN; rt != RailType.RAILTYPE_END; rt++)
                {
                    RailtypeInfo rti = GetRailTypeInfo(rt);
                    /* Unused rail type. */
                    if (rti.label == 0) continue;

                    /* Not date introduced. */
                    if (!MathFuncs.IsInsideMM(rti.introduction_date, 0, MAX_DAY)) continue;

                    /* Not yet introduced at this date. */
                    if (rti.introduction_date > date) continue;

                    /* Have we introduced all required railtypes? */
                    RailTypes required = rti.introduction_required_railtypes;
                    if ((rts & required) != required) continue;

                    rts |= rti.introduces_railtypes;
                }

                /* When we added railtypes we need to run this method again; the added
                 * railtypes might enable more rail types to become introduced. */
                return rts == current ? rts : AddDateIntroducedRailTypes(rts, date);
            }

            /**
             * Get the rail types the given company can build.
             * @param c the company to get the rail types for.
             * @return the rail types.
             */
            RailTypes GetCompanyRailtypes(CompanyID company)
            {
                var rts = RailTypes.RAILTYPES_NONE;

                Engine e;
                FOR_ALL_ENGINES_OF_TYPE(e, VEH_TRAIN) {
                    const EngineInfo* ei = &e.info;

                    if (HasBit(ei.climates, _settings_game.game_creation.landscape) &&
                        (HasBit(e.company_avail, company) || _date >= e.intro_date + DAYS_IN_YEAR))
                    {
                        const RailVehicleInfo* rvi = &e.u.rail;

                        if (rvi.railveh_type != RAILVEH_WAGON)
                        {
                            assert(rvi.railtype < RAILTYPE_END);
                            rts |= GetRailTypeInfo(rvi.railtype).introduces_railtypes;
                        }
                    }
                }

                return AddDateIntroducedRailTypes(rts, _date);
            }

            /**
             * Get the rail type for a given label.
             * @param label the railtype label.
             * @param allow_alternate_labels Search in the alternate label lists as well.
             * @return the railtype.
             */
            RailType GetRailTypeByLabel(RailTypeLabel label, bool allow_alternate_labels)
            {
                /* Loop through each rail type until the label is found */
                for (var r = RailType.RAILTYPE_BEGIN; r != RailType.RAILTYPE_END; r++)
                {
                    RailtypeInfo rti = GetRailTypeInfo(r);
                    if (rti.label == label) return r;
                }

                if (allow_alternate_labels)
                {
                    /* Test if any rail type defines the label as an alternate. */
                    for (var r = RailType.RAILTYPE_BEGIN; r != RailType.RAILTYPE_END; r++)
                    {
                        RailtypeInfo rti = GetRailTypeInfo(r);
                        if (rti.alternate_labels.Contains(label)) return r;
                    }
                }

                /* No matching label was found, so it is invalid */
                return RailType.INVALID_RAILTYPE;
            }



            public static RailType _sorted_railtypes = new RailType[(int) RailType.RAILTYPE_END];
            public static byte _sorted_railtypes_size;

/**
 * Loop header for iterating over railtypes, sorted by sortorder.
 * @param var Railtype.
 */
            public static IEnumerable<RailType> FOR_ALL_SORTED_RAILTYPES()
            {
                for (var i = 0; i < _sorted_railtypes_size; i++)
                {
                    yield return _sorted_railtypes[i];
                }
            }
        }

    }