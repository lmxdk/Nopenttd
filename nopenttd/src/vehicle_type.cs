/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file vehicle_type.h Types related to vehicles. */

namespace Nopenttd
{

/** The type all our vehicle IDs have. */

    public struct VehicleID
    {
        public int Id { get; set; }

        public VehicleID(int id)
        {
            Id = id;
        }

        public static implicit operator int(VehicleID id)
        {
            return id.Id;
        }

        public static implicit operator VehicleID(int id)
        {
            return new VehicleID(id);
        }
    }

    /** Available vehicle types. */

    public enum VehicleType
    {
        VEH_BEGIN,

        /// %Train vehicle type.
        VEH_TRAIN = VEH_BEGIN,

        /// Road vehicle type.
        VEH_ROAD,

        /// %Ship vehicle type.
        VEH_SHIP,

        /// %Aircraft vehicle type.
        VEH_AIRCRAFT,

        /// Last company-ownable type.
        VEH_COMPANY_END,

        /// Effect vehicle type (smoke, explosions, sparks, bubbles)
        VEH_EFFECT = VEH_COMPANY_END,

        /// Disaster vehicle type.
        VEH_DISASTER,

        VEH_END,

        /// Non-existing type of vehicle.
        VEH_INVALID = 0xFF,
    }

//DECLARE_POSTFIX_INCREMENT(VehicleType)
/** Helper information for extract tool. */
//template <> struct EnumPropsT<VehicleType> : MakeEnumPropsT<VehicleType, byte, VEH_TRAIN, VEH_END, VEH_INVALID, 3> {};
/** It needs to be 8bits, because we save and load it as such */
//typedef SimpleTinyEnumT<VehicleType, byte> VehicleTypeByte;

/** Base vehicle class. */

    internal struct BaseVehicle
    {
        /// Type of vehicle
        private VehicleType type;
    }

    public class VehicleConstants
    {
        /// Constant representing a non-existing vehicle.
        public static readonly VehicleID INVALID_VEHICLE = 0xFFFFF;

        /// The maximum length of a vehicle name in characters including '\0'
        public const uint MAX_LENGTH_VEHICLE_NAME_CHARS = 32;

        /** The length of a vehicle in tile units. */
        public const uint VEHICLE_LENGTH = 8;

    }

    /** Pathfinding option states */

    public enum VehiclePathFinders
    {
        /// The Original PathFinder (only for ships)
        VPF_OPF = 0,

        /// New PathFinder
        VPF_NPF = 1,

        /// Yet Another PathFinder
        VPF_YAPF = 2,
    }

/** Flags to add to p1 for goto depot commands. */

    public enum DepotCommand
    {
        /// The vehicle will leave the depot right after arrival (serivce only)
        DEPOT_SERVICE = (1 << 28),

        /// Tells that it's a mass send to depot command (type in VLW flag)
        DEPOT_MASS_SEND = (1 << 29),

        /// Don't cancel current goto depot command if any
        DEPOT_DONT_CANCEL = (1 << 30),

        /// Find another airport if the target one lacks a hangar
        DEPOT_LOCATE_HANGAR = (1 << 31),
        DEPOT_COMMAND_MASK = 0xF << 28,
    }


/** Vehicle acceleration models. */

    public enum AccelerationModel
    {
        AM_ORIGINAL,
        AM_REALISTIC,
    }

/** Visualisation contexts of vehicles and engines. */

    public enum EngineImageType
    {
        /// Vehicle drawn in viewport.
        EIT_ON_MAP = 0x00,

        /// Vehicle drawn in depot.  
        EIT_IN_DEPOT = 0x10,

        /// Vehicle drawn in vehicle details, refit window, ...  
        EIT_IN_DETAILS = 0x11,

        /// Vehicle drawn in vehicle list, group list, ...  
        EIT_IN_LIST = 0x12,

        /// Vehicle drawn in purchase list, autoreplace gui, ...  
        EIT_PURCHASE = 0x20,

        /// Vehicle drawn in preview window, news, ...  
        EIT_PREVIEW = 0x21,
    }
}