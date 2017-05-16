/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file misc_widget.h Types related to the misc widgets. */

namespace Nopenttd.Widgets
{

    /** Widgets of the #LandInfoWindow class. */
    public enum LandInfoWidgets
    {
// Background of the window.
        WID_LI_BACKGROUND,
    }

/** Widgets of the #TooltipsWindow class. */
    public enum ToolTipsWidgets
    {
// Background of the window.
        WID_TT_BACKGROUND,
    }

/** Widgets of the #AboutWindow class. */
    public enum AboutWidgets
    {
// The actually scrolling text.
        WID_A_SCROLLING_TEXT, // URL of OpenTTD website.
        WID_A_WEBSITE,
    }

/** Widgets of the #QueryStringWindow class. */
    public enum QueryStringWidgets
    {
// Caption of the window.
        WID_QS_CAPTION, // Text of the query.
        WID_QS_TEXT, // Default button.
        WID_QS_DEFAULT, // Cancel button.
        WID_QS_CANCEL, // OK button.
        WID_QS_OK,
    }

/** Widgets of the #QueryWindow class. */
    public enum QueryWidgets
    {
// Caption of the window.
        WID_Q_CAPTION, // Text of the query.
        WID_Q_TEXT, // Yes button.
        WID_Q_NO, // No button.
        WID_Q_YES,
    }

/** Widgets of the #TextfileWindow class. */
    public enum TextfileWidgets
    {
// The caption of the window.
        WID_TF_CAPTION, // Whether or not to wrap the text.
        WID_TF_WRAPTEXT, // Panel to draw the textfile on.
        WID_TF_BACKGROUND, // Vertical scrollbar to scroll through the textfile up-and-down.
        WID_TF_VSCROLLBAR, // Horizontal scrollbar to scroll through the textfile left-to-right.
        WID_TF_HSCROLLBAR,
    }
}
