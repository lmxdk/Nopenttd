/* $Id$ */

    /*
     * This file is part of OpenTTD.
     * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
     * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
     * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
     */

    /** @file company_widget.h Types related to the company widgets. */

namespace Nopenttd.Widgets
{

    /** Widgets of the #CompanyWindow class. */
    public enum CompanyWidgets
    {
// Caption of the window.
        WID_C_CAPTION,

// View of the face.
        WID_C_FACE, // Title for the face.
        WID_C_FACE_TITLE,

// Inauguration.
        WID_C_DESC_INAUGURATION, // Colour scheme.
        WID_C_DESC_COLOUR_SCHEME, // Colour scheme example.
        WID_C_DESC_COLOUR_SCHEME_EXAMPLE, // Vehicles.
        WID_C_DESC_VEHICLE, // Vehicle count.
        WID_C_DESC_VEHICLE_COUNTS, // Company value.
        WID_C_DESC_COMPANY_VALUE, // Infrastructure.
        WID_C_DESC_INFRASTRUCTURE, // Infrastructure count.
        WID_C_DESC_INFRASTRUCTURE_COUNTS,

// Owners.
        WID_C_SELECT_DESC_OWNERS, // Owner in Owners.
        WID_C_DESC_OWNERS,

// Selection widget for the button bar.
        WID_C_SELECT_BUTTONS, // Button to make new face.
        WID_C_NEW_FACE, // Button to change colour scheme.
        WID_C_COLOUR_SCHEME, // Button to change president name.
        WID_C_PRESIDENT_NAME, // Button to change company name.
        WID_C_COMPANY_NAME, // Button to buy a share.
        WID_C_BUY_SHARE, // Button to sell a share.
        WID_C_SELL_SHARE,

// Panel about HQ.
        WID_C_SELECT_VIEW_BUILD_HQ, // Button to view the HQ.
        WID_C_VIEW_HQ, // Button to build the HQ.
        WID_C_BUILD_HQ,

// Panel about 'Relocate HQ'.
        WID_C_SELECT_RELOCATE, // Button to relocate the HQ.
        WID_C_RELOCATE_HQ,

// Panel about infrastructure.
        WID_C_VIEW_INFRASTRUCTURE,

// Has company password lock.
        WID_C_HAS_PASSWORD, // Multiplayer selection panel.
        WID_C_SELECT_MULTIPLAYER, // Button to set company password.
        WID_C_COMPANY_PASSWORD, // Button to join company.
        WID_C_COMPANY_JOIN,
    }

/** Widgets of the #CompanyFinancesWindow class. */
    public enum CompanyFinancesWidgets
    {
// Caption of the window.
        WID_CF_CAPTION, // Toggle windows size.
        WID_CF_TOGGLE_SIZE, // Select panel or nothing.
        WID_CF_SEL_PANEL, // Column for expenses category strings.
        WID_CF_EXPS_CATEGORY, // Column for year Y-2 expenses.
        WID_CF_EXPS_PRICE1, // Column for year Y-1 expenses.
        WID_CF_EXPS_PRICE2, // Column for year Y expenses.
        WID_CF_EXPS_PRICE3, // Panel for totals.
        WID_CF_TOTAL_PANEL, // Selection of maxloan column.
        WID_CF_SEL_MAXLOAN, // Bank balance value.
        WID_CF_BALANCE_VALUE, // Loan.
        WID_CF_LOAN_VALUE, // Line for summing bank balance and loan.
        WID_CF_LOAN_LINE, // Total.
        WID_CF_TOTAL_VALUE, // Gap above max loan widget.
        WID_CF_MAXLOAN_GAP, // Max loan widget.
        WID_CF_MAXLOAN_VALUE, // Selection of buttons.
        WID_CF_SEL_BUTTONS, // Increase loan.
        WID_CF_INCREASE_LOAN, // Decrease loan..
        WID_CF_REPAY_LOAN, // View company infrastructure.
        WID_CF_INFRASTRUCTURE,
    }


/** Widgets of the #SelectCompanyLiveryWindow class. */
    public enum SelectCompanyLiveryWidgets
    {
// Caption of window.
        WID_SCL_CAPTION, // Class general.
        WID_SCL_CLASS_GENERAL, // Class rail.
        WID_SCL_CLASS_RAIL, // Class road.
        WID_SCL_CLASS_ROAD, // Class ship.
        WID_SCL_CLASS_SHIP, // Class aircraft.
        WID_SCL_CLASS_AIRCRAFT, // Spacer for dropdown.
        WID_SCL_SPACER_DROPDOWN, // Dropdown for primary colour.
        WID_SCL_PRI_COL_DROPDOWN, // Dropdown for secondary colour.
        WID_SCL_SEC_COL_DROPDOWN, // Matrix.
        WID_SCL_MATRIX,
    }


/**
 * Widgets of the #SelectCompanyManagerFaceWindow class.
 * Do not change the order of the widgets from WID_SCMF_HAS_MOUSTACHE_EARRING to WID_SCMF_GLASSES_R,
 * this order is needed for the WE_CLICK event of DrawFaceStringLabel().
 */
    public enum SelectCompanyManagerFaceWidgets
    {
// Caption of window.
        WID_SCMF_CAPTION, // Toggle for large or small.
        WID_SCMF_TOGGLE_LARGE_SMALL, // Select face.
        WID_SCMF_SELECT_FACE, // Cancel.
        WID_SCMF_CANCEL, // Accept.
        WID_SCMF_ACCEPT, // Male button in the simple view.
        WID_SCMF_MALE, // Female button in the simple view.
        WID_SCMF_FEMALE, // Male button in the advanced view.
        WID_SCMF_MALE2, // Female button in the advanced view.
        WID_SCMF_FEMALE2, // Selection to display the load/save/number buttons in the advanced view.
        WID_SCMF_SEL_LOADSAVE, // Selection to display the male/female buttons in the simple view.
        WID_SCMF_SEL_MALEFEMALE, // Selection to display the buttons for setting each part of the face in the advanced view.
        WID_SCMF_SEL_PARTS, // Create random new face.
        WID_SCMF_RANDOM_NEW_FACE, // Toggle for large or small.
        WID_SCMF_TOGGLE_LARGE_SMALL_BUTTON, // Current face.
        WID_SCMF_FACE, // Load face.
        WID_SCMF_LOAD, // Get the face code.
        WID_SCMF_FACECODE, // Save face.
        WID_SCMF_SAVE, // Text about moustache and earring.
        WID_SCMF_HAS_MOUSTACHE_EARRING_TEXT, // Text about tie and earring.
        WID_SCMF_TIE_EARRING_TEXT, // Text about lips and moustache.
        WID_SCMF_LIPS_MOUSTACHE_TEXT, // Text about glasses.
        WID_SCMF_HAS_GLASSES_TEXT, // Text about hair.
        WID_SCMF_HAIR_TEXT, // Text about eyebrows.
        WID_SCMF_EYEBROWS_TEXT, // Text about eyecolour.
        WID_SCMF_EYECOLOUR_TEXT, // Text about glasses.
        WID_SCMF_GLASSES_TEXT, // Text about nose.
        WID_SCMF_NOSE_TEXT, // Text about chin.
        WID_SCMF_CHIN_TEXT, // Text about jacket.
        WID_SCMF_JACKET_TEXT, // Text about collar.
        WID_SCMF_COLLAR_TEXT, // Text about ethnicity european.
        WID_SCMF_ETHNICITY_EUR, // Text about ethnicity african.
        WID_SCMF_ETHNICITY_AFR, // Has moustache or earring.
        WID_SCMF_HAS_MOUSTACHE_EARRING, // Has glasses.
        WID_SCMF_HAS_GLASSES, // Eyecolour left.
        WID_SCMF_EYECOLOUR_L, // Eyecolour.
        WID_SCMF_EYECOLOUR, // Eyecolour right.
        WID_SCMF_EYECOLOUR_R, // Chin left.
        WID_SCMF_CHIN_L, // Chin.
        WID_SCMF_CHIN, // Chin right.
        WID_SCMF_CHIN_R, // Eyebrows left.
        WID_SCMF_EYEBROWS_L, // Eyebrows.
        WID_SCMF_EYEBROWS, // Eyebrows right.
        WID_SCMF_EYEBROWS_R, // Lips / Moustache left.
        WID_SCMF_LIPS_MOUSTACHE_L, // Lips / Moustache.
        WID_SCMF_LIPS_MOUSTACHE, // Lips / Moustache right.
        WID_SCMF_LIPS_MOUSTACHE_R, // Nose left.
        WID_SCMF_NOSE_L, // Nose.
        WID_SCMF_NOSE, // Nose right.
        WID_SCMF_NOSE_R, // Hair left.
        WID_SCMF_HAIR_L, // Hair.
        WID_SCMF_HAIR, // Hair right.
        WID_SCMF_HAIR_R, // Jacket left.
        WID_SCMF_JACKET_L, // Jacket.
        WID_SCMF_JACKET, // Jacket right.
        WID_SCMF_JACKET_R, // Collar left.
        WID_SCMF_COLLAR_L, // Collar.
        WID_SCMF_COLLAR, // Collar right.
        WID_SCMF_COLLAR_R, // Tie / Earring left.
        WID_SCMF_TIE_EARRING_L, // Tie / Earring.
        WID_SCMF_TIE_EARRING, // Tie / Earring right.
        WID_SCMF_TIE_EARRING_R, // Glasses left.
        WID_SCMF_GLASSES_L, // Glasses.
        WID_SCMF_GLASSES, // Glasses right.
        WID_SCMF_GLASSES_R,
    }

/** Widgets of the #CompanyInfrastructureWindow class. */
    public enum CompanyInfrastructureWidgets
    {
// Caption of window.
        WID_CI_CAPTION, // Description of rail.
        WID_CI_RAIL_DESC, // Count of rail.
        WID_CI_RAIL_COUNT, // Description of road.
        WID_CI_ROAD_DESC, // Count of road.
        WID_CI_ROAD_COUNT, // Description of water.
        WID_CI_WATER_DESC, // Count of water.
        WID_CI_WATER_COUNT, // Description of station.
        WID_CI_STATION_DESC, // Count of station.
        WID_CI_STATION_COUNT, // Description of total.
        WID_CI_TOTAL_DESC, // Count of total.
        WID_CI_TOTAL,
    }

/** Widgets of the #BuyCompanyWindow class. */
    public enum BuyCompanyWidgets
    {
// Caption of window.
        WID_BC_CAPTION, // Face button.
        WID_BC_FACE, // Question text.
        WID_BC_QUESTION, // No button.
        WID_BC_NO, // Yes button.
        WID_BC_YES,
    }

}
