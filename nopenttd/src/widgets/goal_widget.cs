
/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file goal_widget.h Types related to the goal widgets. */
namespace Nopenttd.Widgets
{
    /** Widgets of the #GoalListWindow class. */
    public enum GoalListWidgets
    {
// Caption of the window.
        WID_GOAL_CAPTION, // Goal list.
        WID_GOAL_LIST, // Scrollbar of the goal list.
        WID_GOAL_SCROLLBAR,
    }

/** Widgets of the #GoalQuestionWindow class. */
    public enum GoalQuestionWidgets
    {
// Caption of the window.
        WID_GQ_CAPTION, // Question text.
        WID_GQ_QUESTION, // Buttons selection (between 1, 2 or 3).
        WID_GQ_BUTTONS, // First button.
        WID_GQ_BUTTON_1, // Second button.
        WID_GQ_BUTTON_2, // Third button.
        WID_GQ_BUTTON_3,
    }

}
