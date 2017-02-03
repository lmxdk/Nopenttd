/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file sound_type.h Types related to sounds. */

namespace Nopenttd
{

    public struct SoundEntry
    {
        byte file_slot;
        char file_offset; //TODO was size_t????
        char file_size; //TODO was size_t????
        ushort rate;
        byte bits_per_sample;
        byte channels;
        byte volume;
        byte priority;
        byte grf_container_ver; ///< NewGRF container version if the sound is from a NewGRF.
    };

/**
 * Sound effects from baseset.
 *
 * This enum contains the sound effects from the sound baseset.
 * For hysterical raisins the order of sound effects in the baseset
 * is different to the order they are referenced in TTD/NewGRF.
 *  - The first two sound effects from the baseset are inserted at position 39.
 *    (see translation table _sound_idx)
 *  - The order in the enum is the order using in TTD/NewGRF.
 *  - The naming of the enum values includes the position in the baseset.
 * That is, for sound effects 0x02 to 0x28 the naming is off-by-two.
 */

    public enum SoundFx
    {
        SND_BEGIN = 0,

        /// Water construction.
        SND_02_SPLAT_WATER = 0,
        SND_03_FACTORY_WHISTLE,
        SND_04_TRAIN,
        SND_05_TRAIN_THROUGH_TUNNEL,
        SND_06_SHIP_HORN,
        SND_07_FERRY_HORN,
        SND_08_PLANE_TAKE_OFF,
        SND_09_JET,
        SND_0A_TRAIN_HORN,
        SND_0B_MINING_MACHINERY,
        SND_0C_ELECTRIC_SPARK,
        SND_0D_STEAM,
        SND_0E_LEVEL_CROSSING,
        SND_0F_VEHICLE_BREAKDOWN,
        SND_10_TRAIN_BREAKDOWN,
        SND_11_CRASH, // 16 == 0x10
        SND_12_EXPLOSION,
        SND_13_BIG_CRASH,
        SND_14_CASHTILL, // 19 == 0x13
        SND_15_BEEP, // 20 == 0x14
        SND_16_MORSE,
        SND_17_SKID_PLANE,
        SND_18_HELICOPTER,
        SND_19_BUS_START_PULL_AWAY,
        SND_1A_BUS_START_PULL_AWAY_WITH_HORN,
        SND_1B_TRUCK_START,
        SND_1C_TRUCK_START_2,
        SND_1D_APPLAUSE,
        SND_1E_OOOOH,

        /// Non-water non-rail construction.
        SND_1F_SPLAT_OTHER,

        /// Rail construction.
        SND_20_SPLAT_RAIL,
        SND_21_JACKHAMMER,
        SND_22_CAR_HORN,
        SND_23_CAR_HORN_2,
        SND_24_SHEEP,
        SND_25_COW,
        SND_26_HORSE,
        SND_27_BLACKSMITH_ANVIL, // 38 == 0x26 !
        SND_28_SAWMILL, // 39 == 0x27 !
        SND_00_GOOD_YEAR, // 40 == 0x28 !
        SND_01_BAD_YEAR, // 41 == 0x29 !
        SND_29_RIP,
        SND_2A_EXTRACT_AND_POP,
        SND_2B_COMEDY_HIT,
        SND_2C_MACHINERY,
        SND_2D_RIP_2,
        SND_2E_EXTRACT_AND_POP,
        SND_2F_POP,
        SND_30_CARTOON_SOUND,
        SND_31_EXTRACT,
        SND_32_POP_2,
        SND_33_PLASTIC_MINE,
        SND_34_WIND,
        SND_35_COMEDY_BREAKDOWN,
        SND_36_CARTOON_CRASH,
        SND_37_BALLOON_SQUEAK,
        SND_38_CHAINSAW,
        SND_39_HEAVY_WIND,
        SND_3A_COMEDY_BREAKDOWN_2,
        SND_3B_JET_OVERHEAD,
        SND_3C_COMEDY_CAR,
        SND_3D_ANOTHER_JET_OVERHEAD,
        SND_3E_COMEDY_CAR_2,
        SND_3F_COMEDY_CAR_3,
        SND_40_COMEDY_CAR_START_AND_PULL_AWAY,
        SND_41_MAGLEV,
        SND_42_LOON_BIRD,
        SND_43_LION,
        SND_44_MONKEYS,
        SND_45_PLANE_CRASHING,
        SND_46_PLANE_ENGINE_SPUTTERING,
        SND_47_MAGLEV_2, // 72 == 0x48
        SND_48_DISTANT_BIRD,
        SND_END
    }

    public class SoundConstants
    {

/** The number of sounds in the original sample.cat */
        public const uint ORIGINAL_SAMPLE_COUNT = 73;

        public static readonly SoundID INVALID_SOUND = 0xFFFF;
    }


    public struct SoundID
    {
        public ushort Id { get; set; }

        public SoundID(ushort id)
        {
            Id = id;
        }

        public static implicit operator ushort(SoundID id)
        {
            return id.Id;
        }

        public static implicit operator SoundID(ushort id)
        {
            return new SoundID(id);
        }
    }
}