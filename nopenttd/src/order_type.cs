/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file order_type.h Types related to orders. */

namespace Nopenttd
{

    /// The index of an order within its current vehicle (not pool related)
    public struct VehicleOrderID
    {
        public byte Id { get; set; }

        public VehicleOrderID(byte id)
        {
            Id = id;
        }

        public static implicit operator byte(VehicleOrderID id)
        {
            return id.Id;
        }

        public static implicit operator VehicleOrderID(byte id)
        {
            return new VehicleOrderID(id);
        }
    }

    public struct OrderID
    {
        public ushort Id { get; set; }

        public OrderID(ushort id)
        {
            Id = id;
        }

        public static implicit operator ushort(OrderID id)
        {
            return id.Id;
        }

        public static implicit operator OrderID(ushort id)
        {
            return new OrderID(id);
        }
    }

    public struct OrderListID
    {
        public ushort Id { get; set; }

        public OrderListID(ushort id)
        {
            Id = id;
        }

        public static implicit operator ushort(OrderListID id)
        {
            return id.Id;
        }

        public static implicit operator OrderListID(ushort id)
        {
            return new OrderListID(id);
        }
    }

    public struct DestinationID
    {
        public ushort Id { get; set; }

        public DestinationID(ushort id)
        {
            Id = id;
        }

        public static implicit operator ushort(DestinationID id)
        {
            return id.Id;
        }

        public static implicit operator DestinationID(ushort id)
        {
            return new DestinationID(id);
        }
    }

    public class VehicleConstants
    {

/** Invalid vehicle order index (sentinel) */
        public static readonly VehicleOrderID INVALID_VEH_ORDER_ID = 0xFF;
        /** Last valid VehicleOrderID. */
        public static readonly VehicleOrderID MAX_VEH_ORDER_ID = (byte) (INVALID_VEH_ORDER_ID - 1);

        /** Invalid order (sentinel) */
        public static readonly OrderID INVALID_ORDER = 0xFFFF;

        /**
         * Maximum number of orders in implicit-only lists before we start searching
         * harder for duplicates.
         */
        public const uint IMPLICIT_ORDER_ONLY_CAP = 32;
    }

/** Order types */

    public enum OrderType
    {
        OT_BEGIN = 0,
        OT_NOTHING = 0,
        OT_GOTO_STATION = 1,
        OT_GOTO_DEPOT = 2,
        OT_LOADING = 3,
        OT_LEAVESTATION = 4,
        OT_DUMMY = 5,
        OT_GOTO_WAYPOINT = 6,
        OT_CONDITIONAL = 7,
        OT_IMPLICIT = 8,
        OT_END
    }

/** It needs to be 8bits, because we save and load it as such */
//typedef SimpleTinyEnumT<OrderType, byte> OrderTypeByte;


/**
 * Flags related to the unloading order.
 */

    public enum OrderUnloadFlags
    {
        /// Unload all cargo that the station accepts.	
        OUF_UNLOAD_IF_POSSIBLE = 0,

        /// Force unloading all cargo onto the platform, possibly not getting paid. 
        OUFB_UNLOAD = 1 << 0,

        /// Transfer all cargo onto the platform. 
        OUFB_TRANSFER = 1 << 1,

        /// Totally no unloading will be done. 
        OUFB_NO_UNLOAD = 1 << 2,
    }

/**
 * Flags related to the loading order.
 */

    public enum OrderLoadFlags
    {
        /// Load as long as there is cargo that fits in the train.
        OLF_LOAD_IF_POSSIBLE = 0,

        /// Full load all cargoes of the consist.
        OLFB_FULL_LOAD = 1 << 1,

        /// Full load a single cargo of the consist.
        OLF_FULL_LOAD_ANY = 3,

        /// Do not load anything.
        OLFB_NO_LOAD = 4,
    }

/**
 * Non-stop order flags.
 */

    public enum OrderNonStopFlags
    {
        /// The vehicle will stop at any station it passes and the destination.
        ONSF_STOP_EVERYWHERE = 0,

        /// The vehicle will not stop at any stations it passes except the destination. 
        ONSF_NO_STOP_AT_INTERMEDIATE_STATIONS = 1,

        /// The vehicle will stop at any station it passes except the destination. 
        ONSF_NO_STOP_AT_DESTINATION_STATION = 2,

        /// The vehicle will not stop at any stations it passes including the destination. 
        ONSF_NO_STOP_AT_ANY_STATION = 3,
        ONSF_END
    }

/**
 * Where to stop the trains.
 */

    public enum OrderStopLocation
    {
        /// Stop at the near end of the platform
        OSL_PLATFORM_NEAR_END = 0,

        /// Stop at the middle of the platform
        OSL_PLATFORM_MIDDLE = 1,

        /// Stop at the far end of the platform
        OSL_PLATFORM_FAR_END = 2,
        OSL_END
    }

/**
 * Reasons that could cause us to go to the depot.
 */

    public enum OrderDepotTypeFlags
    {
        /// Manually initiated order.
        ODTF_MANUAL = 0,

        /// This depot order is because of the servicing limit. 
        ODTFB_SERVICE = 1 << 0,

        /// This depot order is because of a regular order. 
        ODTFB_PART_OF_ORDERS = 1 << 1,
    }

/**
 * Actions that can be performed when the vehicle enters the depot.
 */

    public enum OrderDepotActionFlags
    {
        /// Only service the vehicle.
        ODATF_SERVICE_ONLY = 0,

        /// Service the vehicle and then halt it.
        ODATFB_HALT = 1 << 0,

        /// Send the vehicle to the nearest depot.
        ODATFB_NEAREST_DEPOT = 1 << 1,
    }

//DECLARE_ENUM_AS_BIT_SET(OrderDepotActionFlags)

/**
 * Variables (of a vehicle) to 'cause' skipping on.
 */

    public enum OrderConditionVariable
    {
        /// Skip based on the amount of load
        OCV_LOAD_PERCENTAGE,

        /// Skip based on the reliability
        OCV_RELIABILITY,

        /// Skip based on the maximum speed
        OCV_MAX_SPEED,

        /// Skip based on the age
        OCV_AGE,

        /// Skip when the vehicle requires service
        OCV_REQUIRES_SERVICE,

        /// Always skip
        OCV_UNCONDITIONALLY,

        /// Skip based on the remaining lifetime
        OCV_REMAINING_LIFETIME,
        OCV_END
    }

/**
 * Comparator for the skip reasoning.
 */

    public enum OrderConditionComparator
    {
        /// Skip if both values are equal
        OCC_EQUALS,

        /// Skip if both values are not equal
        OCC_NOT_EQUALS,

        /// Skip if the value is less than the limit
        OCC_LESS_THAN,

        /// Skip if the value is less or equal to the limit
        OCC_LESS_EQUALS,

        /// Skip if the value is more than the limit
        OCC_MORE_THAN,

        /// Skip if the value is more or equal to the limit
        OCC_MORE_EQUALS,

        /// Skip if the variable is true
        OCC_IS_TRUE,

        /// Skip if the variable is false
        OCC_IS_FALSE,
        OCC_END
    }


/**
 * public enumeration for the data to set in #CmdModifyOrder.
 */

    public enum ModifyOrderFlags
    {
        /// Passes an OrderNonStopFlags.
        MOF_NON_STOP,

        /// Passes an OrderStopLocation.
        MOF_STOP_LOCATION,

        /// Passes an OrderUnloadType.
        MOF_UNLOAD,

        /// Passes an OrderLoadType
        MOF_LOAD,

        /// Selects the OrderDepotAction
        MOF_DEPOT_ACTION,

        /// A conditional variable changes.
        MOF_COND_VARIABLE,

        /// A comparator changes.
        MOF_COND_COMPARATOR,

        /// The value to set the condition to.
        MOF_COND_VALUE,

        /// Change the destination of a conditional order.
        MOF_COND_DESTINATION,
        MOF_END
    }

//template <> struct EnumPropsT<ModifyOrderFlags> : MakeEnumPropsT<ModifyOrderFlags, byte, MOF_NON_STOP, MOF_END, MOF_END, 4> {};

/**
 * Depot action to switch to when doing a #MOF_DEPOT_ACTION.
 */

    public enum OrderDepotAction
    {
        DA_ALWAYS_GO,

        /// Always go to the depot
        DA_SERVICE,

        /// Service only if needed
        DA_STOP,

        /// Go to the depot and stop there
        DA_END
    }

/**
 * Enumeration for the data to set in #CmdChangeTimetable.
 */

    public enum ModifyTimetableFlags
    {
        /// Set wait time.
        MTF_WAIT_TIME,

        /// Set travel time.
        MTF_TRAVEL_TIME,

        /// Set max travel speed.
        MTF_TRAVEL_SPEED,
        MTF_END
    }

//template <> struct EnumPropsT<ModifyTimetableFlags> : MakeEnumPropsT<ModifyTimetableFlags, byte, MTF_WAIT_TIME, MTF_END, MTF_END, 2> {};


/** Clone actions. */

    public enum CloneOptions
    {
        CO_SHARE = 0,
        CO_COPY = 1,
        CO_UNSHARE = 2
    }

}