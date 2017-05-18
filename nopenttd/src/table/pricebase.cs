/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file pricebase.h Price Bases */
namespace Nopenttd.Tables
{
  public static class PriceBase{

	public static readonly PriceBaseSpec[] _price_base_specs = {/// PR_STATION_VALUE
  new PriceBaseSpec(    100, PriceCategory.PCAT_NONE,         GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_BUILD_RAIL
  new PriceBaseSpec(    100, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_BUILD_ROAD
  new PriceBaseSpec(     95, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_BUILD_SIGNALS
  new PriceBaseSpec(     65, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_BUILD_BRIDGE
  new PriceBaseSpec(    275, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_BUILD_DEPOT_TRAIN
  new PriceBaseSpec(    600, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_BUILD_DEPOT_ROAD
  new PriceBaseSpec(    500, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_BUILD_DEPOT_SHIP
  new PriceBaseSpec(    700, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_BUILD_TUNNEL
  new PriceBaseSpec(    450, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_BUILD_STATION_RAIL
  new PriceBaseSpec(    200, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_BUILD_STATION_RAIL_LENGTH
  new PriceBaseSpec(    180, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_BUILD_STATION_AIRPORT
  new PriceBaseSpec(    600, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_BUILD_STATION_BUS
  new PriceBaseSpec(    200, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_BUILD_STATION_TRUCK
  new PriceBaseSpec(    200, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_BUILD_STATION_DOCK
  new PriceBaseSpec(    350, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_BUILD_VEHICLE_TRAIN
  new PriceBaseSpec( 400000, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_TRAINS,       Price.INVALID_PRICE         ), /// PR_BUILD_VEHICLE_WAGON
  new PriceBaseSpec(   2000, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_TRAINS,       Price.INVALID_PRICE         ), /// PR_BUILD_VEHICLE_AIRCRAFT
  new PriceBaseSpec( 700000, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_AIRCRAFT,     Price.INVALID_PRICE         ), /// PR_BUILD_VEHICLE_ROAD
  new PriceBaseSpec(  14000, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_ROADVEHICLES, Price.INVALID_PRICE         ), /// PR_BUILD_VEHICLE_SHIP
  new PriceBaseSpec(  65000, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_SHIPS,        Price.INVALID_PRICE         ), /// PR_BUILD_TREES
  new PriceBaseSpec(     20, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_TERRAFORM
  new PriceBaseSpec(    250, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_CLEAR_GRASS
  new PriceBaseSpec(     20, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_CLEAR_ROUGH
  new PriceBaseSpec(     40, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_CLEAR_ROCKS
  new PriceBaseSpec(    200, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_CLEAR_FIELDS
  new PriceBaseSpec(    500, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_CLEAR_TREES
  new PriceBaseSpec(     20, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_CLEAR_RAIL
  new PriceBaseSpec(    -70, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_CLEAR_SIGNALS
  new PriceBaseSpec(     10, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_CLEAR_BRIDGE
  new PriceBaseSpec(     50, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_CLEAR_DEPOT_TRAIN
  new PriceBaseSpec(     80, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_CLEAR_DEPOT_ROAD
  new PriceBaseSpec(     80, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_CLEAR_DEPOT_SHIP
  new PriceBaseSpec(     90, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_CLEAR_TUNNEL
  new PriceBaseSpec(     30, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_CLEAR_WATER
  new PriceBaseSpec(  10000, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_CLEAR_STATION_RAIL
  new PriceBaseSpec(     50, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_CLEAR_STATION_AIRPORT
  new PriceBaseSpec(     30, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_CLEAR_STATION_BUS
  new PriceBaseSpec(     50, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_CLEAR_STATION_TRUCK
  new PriceBaseSpec(     50, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_CLEAR_STATION_DOCK
  new PriceBaseSpec(     55, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_CLEAR_HOUSE
  new PriceBaseSpec(   1600, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_CLEAR_ROAD
  new PriceBaseSpec(     40, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_RUNNING_TRAIN_STEAM
  new PriceBaseSpec(   5600, PriceCategory.PCAT_RUNNING,      GrfSpecFeature.GSF_TRAINS,       Price.INVALID_PRICE         ), /// PR_RUNNING_TRAIN_DIESEL
  new PriceBaseSpec(   5200, PriceCategory.PCAT_RUNNING,      GrfSpecFeature.GSF_TRAINS,       Price.INVALID_PRICE         ), /// PR_RUNNING_TRAIN_ELECTRIC
  new PriceBaseSpec(   4800, PriceCategory.PCAT_RUNNING,      GrfSpecFeature.GSF_TRAINS,       Price.INVALID_PRICE         ), /// PR_RUNNING_AIRCRAFT
  new PriceBaseSpec(   9600, PriceCategory.PCAT_RUNNING,      GrfSpecFeature.GSF_AIRCRAFT,     Price.INVALID_PRICE         ), /// PR_RUNNING_ROADVEH
  new PriceBaseSpec(   1600, PriceCategory.PCAT_RUNNING,      GrfSpecFeature.GSF_ROADVEHICLES, Price.INVALID_PRICE         ), /// PR_RUNNING_SHIP
  new PriceBaseSpec(   5600, PriceCategory.PCAT_RUNNING,      GrfSpecFeature.GSF_SHIPS,        Price.INVALID_PRICE         ), /// PR_BUILD_INDUSTRY
  new PriceBaseSpec(1000000, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.INVALID_PRICE         ), /// PR_CLEAR_INDUSTRY
  new PriceBaseSpec(   1600, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.PR_CLEAR_HOUSE        ), /// PR_BUILD_OBJECT
  new PriceBaseSpec(     40, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_OBJECTS,      Price.PR_CLEAR_ROUGH        ), /// PR_CLEAR_OBJECT
  new PriceBaseSpec(     40, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_OBJECTS,      Price.PR_CLEAR_ROUGH        ), /// PR_BUILD_WAYPOINT_RAIL
  new PriceBaseSpec(    600, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.PR_BUILD_DEPOT_TRAIN  ), /// PR_CLEAR_WAYPOINT_RAIL
  new PriceBaseSpec(     80, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.PR_CLEAR_DEPOT_TRAIN  ), /// PR_BUILD_WAYPOINT_BUOY
  new PriceBaseSpec(    350, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.PR_BUILD_STATION_DOCK ), /// PR_CLEAR_WAYPOINT_BUOY
  new PriceBaseSpec(     50, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.PR_CLEAR_STATION_TRUCK), /// PR_TOWN_ACTION
  new PriceBaseSpec(1000000, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.PR_BUILD_INDUSTRY     ), /// PR_BUILD_FOUNDATION
  new PriceBaseSpec(    250, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.PR_TERRAFORM          ), /// PR_BUILD_INDUSTRY_RAW
  new PriceBaseSpec(8000000, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.PR_BUILD_INDUSTRY     ), /// PR_BUILD_TOWN
  new PriceBaseSpec(1000000, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.PR_BUILD_INDUSTRY     ), /// PR_BUILD_CANAL
  new PriceBaseSpec(   5000, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.PR_CLEAR_WATER        ), /// PR_CLEAR_CANAL
  new PriceBaseSpec(   5000, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.PR_CLEAR_WATER        ), /// PR_BUILD_AQUEDUCT
  new PriceBaseSpec(  10000, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.PR_CLEAR_WATER        ), /// PR_CLEAR_AQUEDUCT
  new PriceBaseSpec(   2000, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.PR_CLEAR_BRIDGE       ), /// PR_BUILD_LOCK
  new PriceBaseSpec(   7500, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.PR_CLEAR_WATER        ), /// PR_CLEAR_LOCK
  new PriceBaseSpec(   2000, PriceCategory.PCAT_CONSTRUCTION, GrfSpecFeature.GSF_END,          Price.PR_CLEAR_WATER        ), /// PR_INFRASTRUCTURE_RAIL
  new PriceBaseSpec(     10, PriceCategory.PCAT_RUNNING,      GrfSpecFeature.GSF_END,          Price.PR_BUILD_RAIL         ), /// PR_INFRASTRUCTURE_ROAD
  new PriceBaseSpec(     10, PriceCategory.PCAT_RUNNING,      GrfSpecFeature.GSF_END,          Price.PR_BUILD_ROAD         ), /// PR_INFRASTRUCTURE_WATER
  new PriceBaseSpec(      8, PriceCategory.PCAT_RUNNING,      GrfSpecFeature.GSF_END,          Price.PR_BUILD_CANAL        ), /// PR_INFRASTRUCTURE_STATION
  new PriceBaseSpec(    100, PriceCategory.PCAT_RUNNING,      GrfSpecFeature.GSF_END,          Price.PR_STATION_VALUE      ), /// PR_INFRASTRUCTURE_AIRPORT
  new PriceBaseSpec(   5000, PriceCategory.PCAT_RUNNING,      GrfSpecFeature.GSF_END,          Price.PR_BUILD_STATION_AIRPORT), 
	};
// assert_compile(lengthof(_price_base_specs) == PR_END);
  }
}

