/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file road.cpp Generic road related functions. */

using Nopenttd.Core;
using Nopenttd.src;
using Nopenttd.src.Settings;
using Nopenttd.Slopes;
using Nopenttd.Tiles;

namespace Nopenttd
{
    public class Road
    {
/**
 * Return if the tile is a valid tile for a crossing.
 *
 * @param tile the current tile
 * @param ax the axis of the road over the rail
 * @return true if it is a valid tile
 */
        static bool IsPossibleCrossing(TileIndex tile, Axis ax)
        {
            return (TileMap.IsTileType(tile, TileType.MP_RAILWAY) &&
                    GetRailTileType(tile) == RAIL_TILE_NORMAL &&
                    GetTrackBits(tile) == (ax == Axis.AXIS_X ? TrackBits.TRACK_BIT_Y : TrackBits.TRACK_BIT_X) &&
                    GetFoundationSlope(tile) == Slope.SLOPE_FLAT);
        }

/**
 * Clean up unnecessary RoadBits of a planed tile.
 * @param tile current tile
 * @param org_rb planed RoadBits
 * @return optimised RoadBits
 */
        RoadBits CleanUpRoadBits(TileIndex tile, RoadBits org_rb)
        {
            if (!TileMap.IsValidTile(tile)) return RoadBits.ROAD_NONE;
            for (DiagDirection dir = DiagDirection.DIAGDIR_BEGIN; dir < DiagDirection.DIAGDIR_END; dir++)
            {
                TileIndex neighbor_tile = TileAddByDiagDir(tile, dir);

                /* Get the Roadbit pointing to the neighbor_tile */
                RoadBits target_rb = DiagDirToRoadBits(dir);

                /* If the roadbit is in the current plan */
                if (org_rb & target_rb)
                {
                    bool connective = false;
                    const RoadBits mirrored_rb = MirrorRoadBits(target_rb);

                    if (TileMap.IsValidTile(neighbor_tile))
                    {
                        switch (TileMap.GetTileType(neighbor_tile))
                        {
                            /* Always connective ones */
                            case TileType.MP_CLEAR:
                            case TileType.MP_TREES:
                                connective = true;
                                break;

                            /* The conditionally connective ones */
                            case TileType.MP_TUNNELBRIDGE:
                            case TileType.MP_STATION:
                            case TileType.MP_ROAD:
                                if (IsNormalRoadTile(neighbor_tile))
                                {
                                    /* Always connective */
                                    connective = true;
                                }
                                else
                                {
                                    const RoadBits neighbor_rb =
                                        GetAnyRoadBits(neighbor_tile, RoadType.ROADTYPE_ROAD) |
                                        GetAnyRoadBits(neighbor_tile, RoadType.ROADTYPE_TRAM);

                                    /* Accept only connective tiles */
                                    connective = (neighbor_rb & mirrored_rb) != RoadBits.ROAD_NONE;
                                }
                                break;

                            case TileType.MP_RAILWAY:
                                connective = IsPossibleCrossing(neighbor_tile, DiagDirToAxis(dir));
                                break;

                            case TileType.MP_WATER:
                                /* Check for real water tile */
                                connective = !WaterMap.IsWater(neighbor_tile);
                                break;

                            /* The definitely not connective ones */
                            default: break;
                        }
                    }

                    /* If the neighbor tile is inconnective, remove the planed road connection to it */
                    if (!connective) org_rb ^= target_rb;
                }
            }

            return org_rb;
        }

/**
 * Finds out, whether given company has all given RoadTypes available
 * @param company ID of company
 * @param rts RoadTypes to test
 * @return true if company has all requested RoadTypes available
 */
        public static bool HasRoadTypesAvail(CompanyID company, RoadTypes rts)
        {
            RoadTypes avail_roadtypes;

            if (company == OWNER_DEITY || company == OWNER_TOWN || _game_mode == GM_EDITOR || _generating_world)
            {
                avail_roadtypes = ROADTYPES_ROAD;
            }
            else
            {
                Company c = Company.GetIfValid(company);
                if (c == null) return false;
                avail_roadtypes =
                    (RoadTypes) c.avail_roadtypes |
                    RoadTypes.ROADTYPES_ROAD; // road is available for always for everybody
            }
            return (rts & ~avail_roadtypes) == 0;
        }

        /**
         * Validate functions for rail building.
         * @param rt road type to check.
         * @return true if the current company may build the road.
         */
        public static bool ValParamRoadType(RoadType rt)
        {
            return HasRoadTypesAvail(_current_company, RoadTypeToRoadTypes(rt));
        }

        /**
         * Get the road types the given company can build.
         * @param company the company to get the roadtypes for.
         * @return the road types.
         */
        public static RoadTypes GetCompanyRoadtypes(CompanyID company)
        {
            RoadTypes rt = RoadTypes.ROADTYPES_NONE;

            Engine e;
            FOR_ALL_ENGINES_OF_TYPE(e, VEH_ROAD) {
                EngineInfo ei = e.info;

                if (BitMath.HasBit(ei.climates, _settings_game.game_creation.landscape) &&
                    (BitMath.HasBit(e.company_avail, company) ||
                     DateConstants._date >= e.intro_date + DateConstants.DAYS_IN_YEAR))
                {
                    BitMath.SetBit(rt, BitMath.HasBit(ei.misc_flags, EF_ROAD_TRAM) ? ROADTYPE_TRAM : ROADTYPE_ROAD);
                }
            }

            return rt;

        }
    }
}
