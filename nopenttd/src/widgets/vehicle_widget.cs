/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file vehicle_widget.h Types related to the vehicle widgets. */

namespace Nopenttd.Widgets
{
    /** Widgets of the #VehicleViewWindow class. */
    public enum VehicleViewWidgets
    {
// Caption of window.
        WID_VV_CAPTION, // Viewport widget.
        WID_VV_VIEWPORT, // Start or stop this vehicle, and show information about the current state.
        WID_VV_START_STOP, // Center the main view on this vehicle.
        WID_VV_CENTER_MAIN_VIEW, // Order this vehicle to go to the depot.
        WID_VV_GOTO_DEPOT, // Open the refit window.
        WID_VV_REFIT, // Show the orders of this vehicle.
        WID_VV_SHOW_ORDERS, // Show details of this vehicle.
        WID_VV_SHOW_DETAILS, // Clone this vehicle.
        WID_VV_CLONE, // Selection widget between 'goto depot', and 'clone vehicle' buttons.
        WID_VV_SELECT_DEPOT_CLONE, // Selection widget between 'refit' and 'turn around' buttons.
        WID_VV_SELECT_REFIT_TURN, // Turn this vehicle around.
        WID_VV_TURN_AROUND, // Force this vehicle to pass a signal at danger.
        WID_VV_FORCE_PROCEED,
    }

/** Widgets of the #RefitWindow class. */
    public enum VehicleRefitWidgets
    {
// Caption of window.
        WID_VR_CAPTION, // Display with a representation of the vehicle to refit.
        WID_VR_VEHICLE_PANEL_DISPLAY, // Selection widget for the horizontal scrollbar.
        WID_VR_SHOW_HSCROLLBAR, // Horizontal scrollbar or the vehicle display.
        WID_VR_HSCROLLBAR, // Header with question about the cargo to carry.
        WID_VR_SELECT_HEADER, // Options to refit to.
        WID_VR_MATRIX, // Scrollbar for the refit options.
        WID_VR_SCROLLBAR, // Information about the currently selected refit option.
        WID_VR_INFO, // Perform the refit.
        WID_VR_REFIT,
    }

/** Widgets of the #VehicleDetailsWindow class. */
    public enum VehicleDetailsWidgets
    {
// Caption of window.
        WID_VD_CAPTION, // Rename this vehicle.
        WID_VD_RENAME_VEHICLE, // Panel with generic details.
        WID_VD_TOP_DETAILS, // Increase the servicing interval.
        WID_VD_INCREASE_SERVICING_INTERVAL, // Decrease the servicing interval.
        WID_VD_DECREASE_SERVICING_INTERVAL, // Dropdown to select default/days/percent service interval.
        WID_VD_SERVICE_INTERVAL_DROPDOWN, // Information about the servicing interval.
        WID_VD_SERVICING_INTERVAL, // Details for non-trains.
        WID_VD_MIDDLE_DETAILS, // List of details for trains.
        WID_VD_MATRIX, // Scrollbar for train details.
        WID_VD_SCROLLBAR, // Show carried cargo per part of the train.
        WID_VD_DETAILS_CARGO_CARRIED, // Show all parts of the train with their description.
        WID_VD_DETAILS_TRAIN_VEHICLES, // Show the capacity of all train parts.
        WID_VD_DETAILS_CAPACITY_OF_EACH, // Show the capacity and carried cargo amounts aggregated per cargo of the train.
        WID_VD_DETAILS_TOTAL_CARGO,
    }

/** Widgets of the #VehicleListWindow class. */
    public enum VehicleListWidgets
    {
// Caption of window.
        WID_VL_CAPTION, // Sort order.
        WID_VL_SORT_ORDER, // Sort by dropdown list.
        WID_VL_SORT_BY_PULLDOWN, // List of the vehicles.
        WID_VL_LIST, // Scrollbar for the list.
        WID_VL_SCROLLBAR, // Selection to hide the buttons.
        WID_VL_HIDE_BUTTONS, // Available vehicles.
        WID_VL_AVAILABLE_VEHICLES, // Manage vehicles dropdown list.
        WID_VL_MANAGE_VEHICLES_DROPDOWN, // Stop all button.
        WID_VL_STOP_ALL, // Start all button.
        WID_VL_START_ALL,
    }
}