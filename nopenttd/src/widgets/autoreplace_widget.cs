	/* $Id$ */

	/*
	* This file is part of OpenTTD.
	* OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
	* OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
	* See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
	*/

	/** @file autoreplace_widget.h Types related to the autoreplace widgets. */

namespace Nopenttd.Widgets
{

	/** Widgets of the #ReplaceVehicleWindow class. */
	public enum ReplaceVehicleWidgets {// Caption of the window.
		WID_RV_CAPTION,

		/* Sort dropdown at the right. */
		// Ascending/descending sort order button.
		WID_RV_SORT_ASCENDING_DESCENDING, // Toggle whether to display the hidden vehicles.
		WID_RV_SHOW_HIDDEN_ENGINES,       // Dropdown for the sort criteria.
		WID_RV_SORT_DROPDOWN,

		/* Left and right matrix + details. */
		// The matrix on the left.
		WID_RV_LEFT_MATRIX,              // The scrollbar for the matrix on the left.
		WID_RV_LEFT_SCROLLBAR,           // The matrix on the right.
		WID_RV_RIGHT_MATRIX,             // The scrollbar for the matrix on the right.
		WID_RV_RIGHT_SCROLLBAR,          // Details of the entry on the left.
		WID_RV_LEFT_DETAILS,             // Details of the entry on the right.
		WID_RV_RIGHT_DETAILS,

		/* Button row. */
		// Start Replacing button.
		WID_RV_START_REPLACE,            // Info tab.
		WID_RV_INFO_TAB,                 // Stop Replacing button.
		WID_RV_STOP_REPLACE,

		/* Train only widgets. */
		// Dropdown to select engines and/or wagons.
		WID_RV_TRAIN_ENGINEWAGON_DROPDOWN, // Dropdown menu about the railtype.
		WID_RV_TRAIN_RAILTYPE_DROPDOWN,  // Button to toggle removing wagons.
		WID_RV_TRAIN_WAGONREMOVE_TOGGLE,
	};

}