/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file industry_widget.h Types related to the industry widgets. */

namespace Nopenttd.Widgets
{
    /** Widgets of the #BuildIndustryWindow class. */
    public enum DynamicPlaceIndustriesWidgets
    {
// Matrix of the industries.
        WID_DPI_MATRIX_WIDGET, // Scrollbar of the matrix.
        WID_DPI_SCROLLBAR, // Info panel about the industry.
        WID_DPI_INFOPANEL, // Display chain button.
        WID_DPI_DISPLAY_WIDGET, // Fund button.
        WID_DPI_FUND_WIDGET,
    }

/** Widgets of the #IndustryViewWindow class. */
    public enum IndustryViewWidgets
    {
// Caption of the window.
        WID_IV_CAPTION, // Viewport of the industry.
        WID_IV_VIEWPORT, // Info of the industry.
        WID_IV_INFO, // Goto button.
        WID_IV_GOTO, // Display chain button.
        WID_IV_DISPLAY,
    }

/** Widgets of the #IndustryDirectoryWindow class. */
    public enum IndustryDirectoryWidgets
    {
// Dropdown for the order of the sort.
        WID_ID_DROPDOWN_ORDER, // Dropdown for the criteria of the sort.
        WID_ID_DROPDOWN_CRITERIA, // Industry list.
        WID_ID_INDUSTRY_LIST, // Scrollbar of the list.
        WID_ID_SCROLLBAR,
    }

/** Widgets of the #IndustryCargoesWindow class */
    public enum IndustryCargoesWidgets
    {
// Caption of the window.
        WID_IC_CAPTION, // Row of buttons at the bottom.
        WID_IC_NOTIFY, // Panel that shows the chain.
        WID_IC_PANEL, // Scrollbar of the panel.
        WID_IC_SCROLLBAR, // Select cargo dropdown.
        WID_IC_CARGO_DROPDOWN, // Select industry dropdown.
        WID_IC_IND_DROPDOWN,
    }

}