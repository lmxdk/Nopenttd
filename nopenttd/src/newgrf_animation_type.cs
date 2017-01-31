/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file newgrf_animation_type.h Definitions related to NewGRF animation. */

namespace Nopenttd
{
    public class AnimationConstants
    {
        /// Animation is not looping.
        public const byte ANIM_STATUS_NON_LOOPING = 0x00;

        /// Animation is looping.
        public const byte ANIM_STATUS_LOOPING = 0x01;

        /// There is no animation.
        public const byte ANIM_STATUS_NO_ANIMATION = 0xFF;
    }

/** Information about animation. */

    struct AnimationInfo
    {
        /// The number of frames.
        byte frames;

        /// Status; 0: no looping, 1: looping, 0xFF: no animation.
        byte status;

        /// The speed, i.e. the amount of time between frames.
        byte speed;

        /// The triggers that trigger animation.
        ushort triggers;
    };

/** Animation triggers for station. */

    enum StationAnimationTrigger
    {
        /// Trigger tile when built.
        SAT_BUILT,

        /// Trigger station on new cargo arrival.
        SAT_NEW_CARGO,

        /// Trigger station when cargo is completely taken.
        SAT_CARGO_TAKEN,

        /// Trigger platform when train arrives.
        SAT_TRAIN_ARRIVES,

        /// Trigger platform when train leaves.
        SAT_TRAIN_DEPARTS,

        /// Trigger platform when train loads/unloads.
        SAT_TRAIN_LOADS,

        /// Trigger station every 250 ticks.
        SAT_250_TICKS,
    }

/** Animation triggers of the industries. */

    enum IndustryAnimationTrigger
    {
        /// Trigger whenever the construction state changes.
        IAT_CONSTRUCTION_STATE_CHANGE,

        /// Trigger in the periodic tile loop.  
        IAT_TILELOOP,

        /// Trigger every tick.  
        IAT_INDUSTRY_TICK,

        /// Trigger when cargo is received .  
        IAT_INDUSTRY_RECEIVED_CARGO,

        /// Trigger when cargo is distributed.  
        IAT_INDUSTRY_DISTRIBUTES_CARGO,
    }

/** Animation triggers for airport tiles */

    enum AirpAnimationTrigger
    {
        /// Triggered when the airport is built (for all tiles at the same time).
        AAT_BUILT,

        /// Triggered in the periodic tile loop. 
        AAT_TILELOOP,

        /// Triggered when new cargo arrives at the station (for all tiles at the same time). 
        AAT_STATION_NEW_CARGO,

        /// Triggered when a cargo type is completely removed from the station (for all tiles at the same time). 
        AAT_STATION_CARGO_TAKEN,

        /// Triggered every 250 ticks (for all tiles at the same time). 
        AAT_STATION_250_TICKS,
    }

/** Animation triggers for objects. */

    enum ObjectAnimationTrigger
    {
        /// Triggered when the object is built (for all tiles at the same time).
        OAT_BUILT,

        /// Triggered in the periodic tile loop.
        OAT_TILELOOP,

        /// Triggered every 256 ticks (for all tiles at the same time).
        OAT_256_TICKS,
    }
}