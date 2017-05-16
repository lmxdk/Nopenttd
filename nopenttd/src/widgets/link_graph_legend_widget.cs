
    /*
     * This file is part of OpenTTD.
     * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
     * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
     * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
     */

    /** @file link_graph_legend_widget.h Types related to the linkgraph_legend widgets. */

namespace Nopenttd.Widgets
{

    /** Widgets of the WC_LINKGRAPH_LEGEND. */

    public enum LinkGraphLegendWidgets
    {
// Caption widget.
        WID_LGL_CAPTION, // Saturation legend.
        WID_LGL_SATURATION,
        WID_LGL_SATURATION_FIRST,
        WID_LGL_SATURATION_LAST = WID_LGL_SATURATION_FIRST + 11, // Company selection widget.
        WID_LGL_COMPANIES,
        WID_LGL_COMPANY_FIRST,
        WID_LGL_COMPANY_LAST = WID_LGL_COMPANY_FIRST + Owner.MAX_COMPANIES - 1,
        WID_LGL_COMPANIES_ALL,
        WID_LGL_COMPANIES_NONE, // Cargo selection widget.
        WID_LGL_CARGOES,
        WID_LGL_CARGO_FIRST,
        WID_LGL_CARGO_LAST = WID_LGL_CARGO_FIRST + CargoTypes.NUM_CARGO - 1,
        WID_LGL_CARGOES_ALL,
        WID_LGL_CARGOES_NONE,
    }
}
