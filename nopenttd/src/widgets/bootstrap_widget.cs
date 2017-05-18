/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file bootstrap_widget.h Types related to the bootstrap widgets. */

namespace Nopenttd.Widgets
{

    /** Widgets of the #BootstrapBackground class. */
    public enum BootstrapBackgroundWidgets
    {// Background of the window.
        WID_BB_BACKGROUND,
    }

    /** Widgets of the #BootstrapContentDownloadStatusWindow class. */
    public enum BootstrapAskForDownloadWidgets
    {// The question whether to download.
        WID_BAFD_QUESTION, // An affirmative answer to the question.
        WID_BAFD_YES,      // An negative answer to the question.
        WID_BAFD_NO,
    }

}