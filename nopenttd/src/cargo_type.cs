/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file cargo_type.h Types related to cargoes... */

using System;
using System.Linq;

namespace Nopenttd
{

/**
 * Cargo slots to indicate a cargo type within a game.
 * Numbers are re-used between different climates.
 * @see CargoTypes
 */

    public struct CargoID
    {
        public byte Id { get; set; }

        public CargoID(byte id)
        {
            Id = id;
        }

        public static implicit operator byte(CargoID id)
        {
            return id.Id;
        }

        public static implicit operator CargoID(byte id)
        {
            return new CargoID(id);
        }
    }


    /** Available types of cargo */

    public enum CargoTypes
    {
        /* Temperate */
        CT_PASSENGERS = 0,
        CT_COAL = 1,
        CT_MAIL = 2,
        CT_OIL = 3,
        CT_LIVESTOCK = 4,
        CT_GOODS = 5,
        CT_GRAIN = 6,
        CT_WOOD = 7,
        CT_IRON_ORE = 8,
        CT_STEEL = 9,
        CT_VALUABLES = 10,

        /* Arctic */
        CT_WHEAT = 6,
        CT_HILLY_UNUSED = 8,
        CT_PAPER = 9,
        CT_GOLD = 10,
        CT_FOOD = 11,

        /* Tropic */
        CT_RUBBER = 1,
        CT_FRUIT = 4,
        CT_MAIZE = 6,
        CT_COPPER_ORE = 8,
        CT_WATER = 9,
        CT_DIAMONDS = 10,

        /* Toyland */
        CT_SUGAR = 1,
        CT_TOYS = 3,
        CT_BATTERIES = 4,
        CT_CANDY = 5,
        CT_TOFFEE = 6,
        CT_COLA = 7,
        CT_COTTON_CANDY = 8,
        CT_BUBBLES = 9,
        CT_PLASTIC = 10,
        CT_FIZZY_DRINKS = 11,

        /// Maximal number of cargo types in a game.
        NUM_CARGO = 32,

        /// Automatically choose cargo type when doing auto refitting.
        CT_AUTO_REFIT = 0xFD,

        /// Do not refit cargo of a vehicle (used in vehicle orders and auto-replace/auto-new).
        CT_NO_REFIT = 0xFE,

        /// Invalid cargo type.
        CT_INVALID = 0xFF,
    }

/** Class for storing amounts of cargo */
//struct
    public class CargoArray
    {
        /// Amount of each type of cargo.
        private uint[] amount = new uint[(int) CargoTypes.NUM_CARGO];

        /** Reset all entries. */
        //inline 
        public void Clear()
        {
            Array.Clear(amount, 0, amount.Length);
        }

        /**
         * Read/write access to an amount of a specific cargo type.
         * @param cargo Cargo type to access.
         */
        //inline 
        public uint this[CargoID cargo]
        {
            get { return amount[cargo]; }
            set { amount[cargo] = value; }
        }

        /**
         * Get the sum of all cargo amounts.
         * @return The sum.
         */
        //inline 
        public long GetSum() => amount.Sum(a => a);

/**
 * Get the amount of cargos that have an amount.
 * @return The amount.
 */
//inline 
        public byte GetCount() => (byte) amount.Count(a => a != 0);
    };


/** Types of cargo source and destination */

    enum SourceType
    {
        /// Source/destination is an industry
        ST_INDUSTRY,

        /// Source/destination is a town 
        ST_TOWN,

        /// Source/destination are company headquarters 
        ST_HEADQUARTERS,
    }

    /// The SourceType packed into a byte for savegame purposes.
    //typedef SimpleTinyEnumT<SourceType, byte> SourceTypeByte;



    /// Invalid/unknown index of source
    public static class SourceConstants
    {
        public static readonly SourceID INVALID_SOURCE = 0xFFFF;
    }


    /// Contains either industry ID, town ID or company ID (or INVALID_SOURCE)
    public struct SourceID
    {
        public ushort Id { get; set; }

        public SourceID(ushort id)
        {
            Id = id;
        }

        public static implicit operator ushort(SourceID id)
        {
            return id.Id;
        }

        public static implicit operator SourceID(ushort id)
        {
            return new SourceID(id);
        }
    }
}

