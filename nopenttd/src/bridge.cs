/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file bridge.h Header file for bridges */

using System;
using System.Diagnostics;
using Nopenttd.Slopes;

namespace Nopenttd
{



/**
 * This enum is related to the definition of bridge pieces,
 * which is used to determine the proper sprite table to use
 * while drawing a given bridge part.
 */
    public enum BridgePieces
    {
        BRIDGE_PIECE_NORTH = 0,
        BRIDGE_PIECE_SOUTH,
        BRIDGE_PIECE_INNER_NORTH,
        BRIDGE_PIECE_INNER_SOUTH,
        BRIDGE_PIECE_MIDDLE_ODD,
        BRIDGE_PIECE_MIDDLE_EVEN,
        BRIDGE_PIECE_HEAD,
        BRIDGE_PIECE_INVALID,
    };
//DECLARE_POSTFIX_INCREMENT(BridgePieces)

    public static class Bridges
    {
        /// Maximal number of available bridge specs.
        public const uint MAX_BRIDGES = 13;

        public static BridgeSpec[] _bridge = new BridgeSpec[ MAX_BRIDGES];


        Foundation GetBridgeFoundation(Slope tileh, Axis axis) => throw new NotImplementedException();
        bool HasBridgeFlatRamp(Slope tileh, Axis axis) => throw new NotImplementedException();

        /**
         * Get the specification of a bridge type.
         * @param i The type of bridge to get the specification for.
         * @return The specification.
         */
        //inline
        public static BridgeSpec GetBridgeSpec(BridgeType i)
        {
            Debug.Assert(i<_bridge.Length);
            return _bridge[i];
        }

        void DrawBridgeMiddle(const TileInfo* ti) => throw new NotImplementedException();

        CommandCost CheckBridgeAvailability(BridgeType bridge_type, uint bridge_len, DoCommandFlag flags = DC_NONE) => throw new NotImplementedException();
        int CalcBridgeLenCostFactor(int x) => throw new NotImplementedException();

        void ResetBridges() => throw new NotImplementedException();
    }

    /// Bridge spec number.

    public struct BridgeType
    {
        public uint Id { get; set; }

        public BridgeType(uint id)
        {
            Id = id;
        }

        public static implicit operator uint(BridgeType id)
        {
            return id.Id;
        }

        public static implicit operator BridgeType(uint id)
        {
            return new BridgeType(id);
        }
    }

/**
 * Struct containing information about a single bridge type
 */
    public class BridgeSpec
    {
        Year avail_year;
        byte min_length;
        ushort max_length;
        ushort price;
        ushort speed;
        SpriteID sprite;
        PaletteID pal;
        StringID material;
        StringID transport_name[2];
        PalSpriteID** sprite_table;
        byte flags;
    };
}