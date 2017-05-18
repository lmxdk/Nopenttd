
    /*
     * This file is part of OpenTTD.
     * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
     * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
     * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
     */

    /** @file network_widget.h Types related to the network widgets. */

namespace Nopenttd.Widgets
{

    /** Widgets of the #NetworkGameWindow class. */
    public enum NetworkGameWidgets
    {
// Main panel.
        WID_NG_MAIN,

// Label in front of connection droplist.
        WID_NG_CONNECTION, // 'Connection' droplist button.
        WID_NG_CONN_BTN, // Label in front of client name edit box.
        WID_NG_CLIENT_LABEL, // Panel with editbox to set client name.
        WID_NG_CLIENT, // Label in front of the filter/search edit box.
        WID_NG_FILTER_LABEL, // Panel with the edit box to enter the search text.
        WID_NG_FILTER,

// Header container of the matrix.
        WID_NG_HEADER, // 'Name' button.
        WID_NG_NAME, // 'Clients' button.
        WID_NG_CLIENTS, // 'Map size' button.
        WID_NG_MAPSIZE, // 'Date' button.
        WID_NG_DATE, // 'Years' button.
        WID_NG_YEARS, // Third button in the game list panel.
        WID_NG_INFO,

// Panel with list of games.
        WID_NG_MATRIX, // Scrollbar of matrix.
        WID_NG_SCROLLBAR,

// Label "Last joined server:".
        WID_NG_LASTJOINED_LABEL, // Info about the last joined server.
        WID_NG_LASTJOINED, // Spacer after last joined server panel.
        WID_NG_LASTJOINED_SPACER,

// Panel with game details.
        WID_NG_DETAILS, // Spacer for game actual details.
        WID_NG_DETAILS_SPACER, // 'Join game' button.
        WID_NG_JOIN, // 'Refresh server' button.
        WID_NG_REFRESH, // 'NewGRF Settings' button.
        WID_NG_NEWGRF, // Selection 'widget' to hide the NewGRF settings.
        WID_NG_NEWGRF_SEL, // 'Find missing NewGRF online' button.
        WID_NG_NEWGRF_MISSING, // Selection widget for the above button.
        WID_NG_NEWGRF_MISSING_SEL,

// 'Find server' button.
        WID_NG_FIND, // 'Add server' button.
        WID_NG_ADD, // 'Start server' button.
        WID_NG_START, // 'Cancel' button.
        WID_NG_CANCEL,
    }

/** Widgets of the #NetworkStartServerWindow class. */
    public enum NetworkStartServerWidgets
    {
// Background of the window.
        WID_NSS_BACKGROUND, // Label for the game name.
        WID_NSS_GAMENAME_LABEL, // Background for editbox to set game name.
        WID_NSS_GAMENAME, // 'Set password' button.
        WID_NSS_SETPWD, // Label for 'connection type'.
        WID_NSS_CONNTYPE_LABEL, // 'Connection type' droplist button.
        WID_NSS_CONNTYPE_BTN, // Label for 'max clients'.
        WID_NSS_CLIENTS_LABEL, // 'Max clients' downarrow.
        WID_NSS_CLIENTS_BTND, // 'Max clients' text.
        WID_NSS_CLIENTS_TXT, // 'Max clients' uparrow.
        WID_NSS_CLIENTS_BTNU, // Label for 'max companies'.
        WID_NSS_COMPANIES_LABEL, // 'Max companies' downarrow.
        WID_NSS_COMPANIES_BTND, // 'Max companies' text.
        WID_NSS_COMPANIES_TXT, // 'Max companies' uparrow.
        WID_NSS_COMPANIES_BTNU, // Label for 'max spectators'.
        WID_NSS_SPECTATORS_LABEL, // 'Max spectators' downarrow.
        WID_NSS_SPECTATORS_BTND, // 'Max spectators' text.
        WID_NSS_SPECTATORS_TXT, // 'Max spectators' uparrow.
        WID_NSS_SPECTATORS_BTNU,

// Label for 'language spoken'.
        WID_NSS_LANGUAGE_LABEL, // 'Language spoken' droplist button.
        WID_NSS_LANGUAGE_BTN,

// New game button.
        WID_NSS_GENERATE_GAME, // Load game button.
        WID_NSS_LOAD_GAME, // Play scenario button.
        WID_NSS_PLAY_SCENARIO, // Play heightmap button.
        WID_NSS_PLAY_HEIGHTMAP,

// 'Cancel' button.
        WID_NSS_CANCEL,
    }

/** Widgets of the #NetworkLobbyWindow class. */
    public enum NetworkLobbyWidgets
    {
// Background of the window.
        WID_NL_BACKGROUND, // Heading text.
        WID_NL_TEXT, // Header above list of companies.
        WID_NL_HEADER, // List of companies.
        WID_NL_MATRIX, // Scroll bar.
        WID_NL_SCROLLBAR, // Company details.
        WID_NL_DETAILS, // 'Join company' button.
        WID_NL_JOIN, // 'New company' button.
        WID_NL_NEW, // 'Spectate game' button.
        WID_NL_SPECTATE, // 'Refresh server' button.
        WID_NL_REFRESH, // 'Cancel' button.
        WID_NL_CANCEL,
    }

/** Widgets of the #NetworkClientListWindow class. */
    public enum ClientListWidgets
    {
// Panel of the window.
        WID_CL_PANEL,
    }

/** Widgets of the #NetworkClientListPopupWindow class. */
    public enum ClientListPopupWidgets
    {
// Panel of the window.
        WID_CLP_PANEL,
    }

/** Widgets of the #NetworkJoinStatusWindow class. */
    public enum NetworkJoinStatusWidgets
    {
// Background of the window.
        WID_NJS_BACKGROUND, // Cancel / OK button.
        WID_NJS_CANCELOK,
    }

/** Widgets of the #NetworkCompanyPasswordWindow class. */
    public enum NetworkCompanyPasswordWidgets
    {
// Background of the window.
        WID_NCP_BACKGROUND, // Label in front of the password field.
        WID_NCP_LABEL, // Input field for the password.
        WID_NCP_PASSWORD, // Toggle 'button' for saving the current password as default password.
        WID_NCP_SAVE_AS_DEFAULT_PASSWORD, // Close the window without changing anything.
        WID_NCP_CANCEL, // Safe the password etc.
        WID_NCP_OK,
    }
}