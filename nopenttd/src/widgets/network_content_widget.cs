/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file network_content_widget.h Types related to the network content widgets. */

namespace Nopenttd.Widgets
{

    /** Widgets of the #NetworkContentDownloadStatusWindow class. */
    public enum NetworkContentDownloadStatusWidgets
    {
// Background of the window.
        WID_NCDS_BACKGROUND, // (Optional) Cancel/OK button.
        WID_NCDS_CANCELOK,
    }

/** Widgets of the #NetworkContentListWindow class. */
    public enum NetworkContentListWidgets
    {
// Resize button.
        WID_NCL_BACKGROUND,

// Caption for the filter editbox.
        WID_NCL_FILTER_CAPT, // Filter editbox.
        WID_NCL_FILTER,

// Button above checkboxes.
        WID_NCL_CHECKBOX, // 'Type' button.
        WID_NCL_TYPE, // 'Name' button.
        WID_NCL_NAME,

// Panel with list of content.
        WID_NCL_MATRIX, // Scrollbar of matrix.
        WID_NCL_SCROLLBAR,

// Panel with content details.
        WID_NCL_DETAILS, // Open readme, changelog (+1) or license (+2) of a file in the content window.
        WID_NCL_TEXTFILE,

// 'Select all' button.
        WID_NCL_SELECT_ALL = WID_NCL_TEXTFILE + TFT_END, // 'Select updates' button.
        WID_NCL_SELECT_UPDATE, // 'Unselect all' button.
        WID_NCL_UNSELECT, // 'Open url' button.
        WID_NCL_OPEN_URL, // 'Cancel' button.
        WID_NCL_CANCEL, // 'Download' button.
        WID_NCL_DOWNLOAD,

// #NWID_SELECTION widget for select all/update buttons..
        WID_NCL_SEL_ALL_UPDATE, // Search external sites for missing NewGRF.
        WID_NCL_SEARCH_EXTERNAL,
    }

}