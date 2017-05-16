
/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file graph_widget.h Types related to the graph widgets. */

namespace Nopenttd.Widgets
{
    /** Widgets of the #GraphLegendWindow class. */
    public enum GraphLegendWidgets
    {
// Background of the window.
        WID_GL_BACKGROUND,

// First company in the legend.
        WID_GL_FIRST_COMPANY, // Last company in the legend.
        WID_GL_LAST_COMPANY = WID_GL_FIRST_COMPANY + Owner.MAX_COMPANIES - 1,
    }

/** Widgets of the #OperatingProfitGraphWindow class, #IncomeGraphWindow class, #DeliveredCargoGraphWindow class, and #CompanyValueGraphWindow class. */
    public enum CompanyValueWidgets
    {
// Key button.
        WID_CV_KEY_BUTTON, // Background of the window.
        WID_CV_BACKGROUND, // Graph itself.
        WID_CV_GRAPH, // Resize button.
        WID_CV_RESIZE,
    }

/** Widget of the #PerformanceHistoryGraphWindow class. */
    public enum PerformanceHistoryGraphWidgets
    {
// Key button.
        WID_PHG_KEY, // Detailed performance.
        WID_PHG_DETAILED_PERFORMANCE, // Background of the window.
        WID_PHG_BACKGROUND, // Graph itself.
        WID_PHG_GRAPH, // Resize button.
        WID_PHG_RESIZE,
    }

/** Widget of the #PaymentRatesGraphWindow class. */
    public enum CargoPaymentRatesWidgets
    {
// Background of the window.
        WID_CPR_BACKGROUND, // Header.
        WID_CPR_HEADER, // Graph itself.
        WID_CPR_GRAPH, // Resize button.
        WID_CPR_RESIZE, // Footer.
        WID_CPR_FOOTER, // Enable cargoes button.
        WID_CPR_ENABLE_CARGOES, // Disable cargoes button.
        WID_CPR_DISABLE_CARGOES, // First cargo in the list.
        WID_CPR_CARGO_FIRST,
    }

/** Widget of the #CompanyLeagueWindow class. */
    public enum CompanyLeagueWidgets
    {
// Background of the window.
        WID_CL_BACKGROUND,
    }

/** Widget of the #PerformanceRatingDetailWindow class. */
    public enum PerformanceRatingDetailsWidgets
    {
// First entry in the score list.
        WID_PRD_SCORE_FIRST, // Last entry in the score list.
        WID_PRD_SCORE_LAST = WID_PRD_SCORE_FIRST + (ScoreID.SCORE_END - ScoreID.SCORE_BEGIN) - 1,

// First company.
        WID_PRD_COMPANY_FIRST, // Last company.
        WID_PRD_COMPANY_LAST = WID_PRD_COMPANY_FIRST + Owner.MAX_COMPANIES - 1,
    }

}
