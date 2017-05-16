    /*
     * This file is part of OpenTTD.
     * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
     * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
     * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
     */

    /** @file station_widget.h Types related to the station widgets. */

namespace Nopenttd.Widgets
{

    /** Widgets of the #StationViewWindow class. */
    public enum StationViewWidgets
    {
// Caption of the window.
        WID_SV_CAPTION, // 'Sort order' button
        WID_SV_SORT_ORDER, // 'Sort by' button
        WID_SV_SORT_BY, // label for "group by"
        WID_SV_GROUP, // 'Group by' button
        WID_SV_GROUP_BY, // List of waiting cargo.
        WID_SV_WAITING, // Scrollbar.
        WID_SV_SCROLLBAR, // List of accepted cargoes / rating of cargoes.
        WID_SV_ACCEPT_RATING_LIST, // 'Location' button.
        WID_SV_LOCATION, // 'Accepts' / 'Ratings' button.
        WID_SV_ACCEPTS_RATINGS, // 'Rename' button.
        WID_SV_RENAME, // 'Close airport' button.
        WID_SV_CLOSE_AIRPORT, // List of scheduled trains button.
        WID_SV_TRAINS, // List of scheduled road vehs button.
        WID_SV_ROADVEHS, // List of scheduled ships button.
        WID_SV_SHIPS, // List of scheduled planes button.
        WID_SV_PLANES,
    }

/** Widgets of the #CompanyStationsWindow class. */
    public enum StationListWidgets
    {
        /* Name starts with ST instead of S, because of collision with SaveLoadWidgets */ // Caption of the window.
        WID_STL_CAPTION, // The main panel, list of stations.
        WID_STL_LIST, // Scrollbar next to the main panel.
        WID_STL_SCROLLBAR,

        /* Vehicletypes need to be in order of StationFacility due to bit magic */ // 'TRAIN' button - list only facilities where is a railroad station.
        WID_STL_TRAIN, // 'TRUCK' button - list only facilities where is a truck stop.
        WID_STL_TRUCK, // 'BUS' button - list only facilities where is a bus stop.
        WID_STL_BUS, // 'AIRPLANE' button - list only facilities where is an airport.
        WID_STL_AIRPLANE, // 'SHIP' button - list only facilities where is a dock.
        WID_STL_SHIP, // 'ALL' button - list all facilities.
        WID_STL_FACILALL,

// 'NO' button - list stations where no cargo is waiting.
        WID_STL_NOCARGOWAITING, // 'ALL' button - list all stations.
        WID_STL_CARGOALL,

// 'Sort by' button - reverse sort direction.
        WID_STL_SORTBY, // Dropdown button.
        WID_STL_SORTDROPBTN,

// Widget numbers used for list of cargo types (not present in _company_stations_widgets).
        WID_STL_CARGOSTART,
    }

/** Widgets of the #SelectStationWindow class. */
    public enum JoinStationWidgets
    {
        WID_JS_CAPTION, // Caption of the window.
        WID_JS_PANEL, // Main panel.
        WID_JS_SCROLLBAR, // Scrollbar of the panel.
    }
}
