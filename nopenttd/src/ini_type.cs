/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file ini_type.h Types related to reading/writing '*.ini' files. */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Nopenttd
{


/** Types of groups */

    public enum IniGroupType
    {
        /// Values of the form "landscape = hilly".
        IGT_VARIABLES = 0,

        /// A list of values, separated by \n and terminated by the next group block.
        IGT_LIST = 1,

        /// A list of uninterpreted lines, terminated by the next group block.
        IGT_SEQUENCE = 2,
    }

/** A single "line" in an ini file. */

    public class IniItem
    {
        /// The next item in this group
        public IniItem next;

        /// The name of this item
        public string name;

        /// The value of this item
        public string value;

        /// The comment associated with this item
        public string comment;


        /**
         * Construct a new in-memory item of an Ini file.
         * @param parent the group we belong to
         * @param name   the name of the item
         * @param last   the last element of the name of the item
         */
        public IniItem(IniGroup parent, string name) //, string last = null)
        {
            next = null;
            value = null;
            comment = null;
	
            this.name = name;
            //str_validate(this.name, this.name + strlen(this.name));

            parent.items.Add(name, this);
        }

        /**
         * Replace the current value with another value.
         * @param value the value to replace with.
         */
        public void SetValue(string value)
		{
			this.value = value;
		}

}

/** A group within an ini file. */

public class IniGroup
    {
        /// the next group within this file
        /// type of group 
        public IniGroupType type;

        /// the first item in the group 
        public Dictionary<string, IniItem> items;

        /// the last item in the group 

        /// name of group 
        readonly string name;

        /// comment for group 
        public string comment;

        /**
* Construct a new in-memory group of an Ini file.
* @param parent the file we belong to
* @param name   the name of the group
* @param last   the last element of the name of the group
*/

        public IniGroup(IniLoadFile parent, string name)
        {
            this.name = name;
            //str_validate(this.name, this.name + strlen(this.name));

			parent.groups.Add(name, this);

            if (parent.list_group_names?.Contains(this.name) ?? false)
            { 
                this.type = IniGroupType.IGT_LIST;
                return;              
            }
            if (parent.seq_group_names?.Contains(this.name) ?? false) {
                this.type = IniGroupType.IGT_SEQUENCE;
                return;
            }
        }


        /**
 * Get the item with the given name, and if it doesn't exist
 * and create is true it creates a new item.
 * @param name   name of the item to find.
 * @param create whether to create an item when not found or not.
 * @return the requested item or NULL if not found.
 */
        public IniItem GetItem(string name, bool create)
        {
			if (items.TryGetValue(name, out var item))
			{
				return item;
			}

            if (!create)
            {
                return null;
            }

			/* otherwise make a new one */
			return new IniItem(this, name);
		}

/**
 * Clear all items in the group
 */
public void Clear()
{
	items.Clear();
}
    }

/** Ini file that only supports loading. */

    public abstract class IniLoadFile
    {
        /// the first group in the ini
        public Dictionary<string, IniGroup> groups;


        /// last comment in file
        public string comment;

        /// NULL terminated list with group names that are lists
        public List<string> list_group_names;

        /// NULL terminated list with group names that are sequences.
        public List<string> seq_group_names;

        /**
 * Construct a new in-memory Ini file representation.
 * @param list_group_names A \c NULL terminated list with group names that should be loaded as lists instead of variables. @see IGT_LIST
 * @param seq_group_names  A \c NULL terminated list with group names that should be loaded as lists of names. @see IGT_SEQUENCE
 */
        public IniLoadFile(List<string> list_group_names = null, List<string> seq_group_names = null)
{
            groups = new Dictionary<string, IniGroup>();
    comment = null;
		this.list_group_names = list_group_names;
    this.seq_group_names = seq_group_names;
}
		
/**
 * Get the group with the given name. If it doesn't exist
 * and \a create_new is \c true create a new group.
 * @param name name of the group to find.
 * @param len  the maximum length of said name (\c 0 means length of the string).
 * @param create_new Allow creation of group if it does not exist.
 * @return The requested group if it exists or was created, else \c NULL.
 */
IniGroup GetGroup(string name, int len, bool create_new)
{
    /* does it exist already? */
    if (groups.TryGetValue(name, out var group))
    {
        return group;
    }

	
	if (!create_new) return null;

	/* otherwise make a new one */
    return new IniGroup(this, name) {comment = "\n"};
}

        /**
         * Remove the group with the given name.
         * @param name name of the group to remove.
         */

        public void RemoveGroup(string name)
        {
            groups.Remove(name);
}

/**
 * Load the Ini file's data from the disk.
 * @param filename the file to load.
 * @param subdir the sub directory to load the file from.
 * @pre nothing has been loaded yet.
 */
public void LoadFromDisk(string filename, Subdirectory subdir)
{
			Debug.Assert(groups.Any() == false);

            //FILE *in = this.OpenFile(filename, subdir, &end);
            //if (in == NULL) return;
            var path = filename; //TODO create
    IniGroup group = null;
			
    
    var commentBuilder = new StringBuilder();
    var hasComment = false;
            
			/* for each line in the file */
            foreach (var l in File.ReadLines(path, Encoding.UTF8))
	{
	    var line = l.Trim();
	    if (line.Any() == false)
	    {
	        continue;
	    }
	    var firstChar = line.First();
        
        /* Skip comments and empty lines outside IGT_SEQUENCE groups. */
        if ((group == null || group.type != IniGroupType.IGT_SEQUENCE) && (firstChar == '#' || firstChar == ';' || firstChar == '\0'))
        {
            commentBuilder.AppendLine(line); // comment newline
            hasComment = true;
            continue;
        }

        /* it's a group? */
        if (firstChar == '[')
        {
            var lineLength = line.Length;
            if (line.Last() != ']')
            {
                this.ReportFileError("ini: invalid group name '", line, "'");
            }
            else
            {
                lineLength--;
            }
            group = new IniGroup(this, line.Substring(1,lineLength-1)); // skip [
            if (hasComment)
            {
                group.comment = commentBuilder.ToString();
                commentBuilder.Clear();
                hasComment = false;
            }
        }
        else if (group != null)
        {
            if (group.type == IniGroupType.IGT_SEQUENCE)
            {
                /* A sequence group, use the line as item name without further interpretation. */
                var itemInner = new IniItem(group, line);
                if (hasComment)
                {
                    itemInner.comment = commentBuilder.ToString();
                    commentBuilder.Clear();
                    hasComment = false;
                }
                continue;
            }

            var index = 0;
            string name = null;
            /* find end of keyname */
            if (firstChar == '\"')
            {
                index++;
                var length = line.IndexOf("\"", index);
                name = line.Substring(index, length);
                index += length+1;
            }
            else
            {
                for (var i = 0; i < line.Length; i++)
                {
                    var c = line[i];
                    if (Char.IsWhiteSpace(c) || c == '=')
                    {
                        name = line.Substring(index, i);
                        index = i+1;
                        break;
                    }
                }
                if (index == 0)
                {
                    name = line;

                }
            }
			

            /* it's an item in an existing group */
            var item = new IniItem(group, name);
            if (hasComment)
            {
                item.comment = commentBuilder.ToString();
                commentBuilder.Clear();
                hasComment = false;
            }

            /* find start of parameter */
            for (; index < line.Length; index++)
            {
                var c = line[index];
                if (Char.IsWhiteSpace(c) == false || c == '=')
                {
                    break;
                }
            }

            bool quoted = (line[index] == '\"');
            /* remove starting quotation marks */
            if (quoted) index++;
            /* remove ending quotation marks */
            var value = (string)null;
            if (index < line.Length - 1)
            {
                value = line.EndsWith("\"")
                    ? line.Substring(index, line.Length - index - 1)
                    : line.Substring(index);
            }

            /* If the value was not quoted and empty, it must be NULL */
            item.value = value;
            //if (item.value != null) str_validate(item.value, item.value + strlen(item.value));
        }
        else
        {
            /* it's an orphan item */
            this.ReportFileError("ini: '", line, "' outside of group");
        }
    }

    if (hasComment)
    {
        this.comment = commentBuilder.ToString();
        commentBuilder.Clear();
        hasComment = false;
    }
}

/**
 * Open the INI file.
 * @param filename Name of the INI file.
 * @param subdir The subdir to load the file from.
 * @param size [out] Size of the opened file.
 * @return File handle of the opened file, or \c NULL.
 */
abstract FILE* OpenFile(string filename, Subdirectory subdir, int* size);

        /**
         * Report an error about the file contents.
         * @param pre    Prefix text of the \a buffer part.
         * @param buffer Part of the file with the error.
         * @param post   Suffix text of the \a buffer part.
         */
        abstract void ReportFileError(string pre, string buffer, string post);
    };

/** Ini file that supports both loading and saving. */

    public class IniFile : IniLoadFile
    {
        /**
 * Create a new ini file with given group names.
 * @param list_group_names A \c NULL terminated list with group names that should be loaded as lists instead of variables. @see IGT_LIST
 */

        public IniFile(List<string> list_group_names = null) : base(list_group_names)
        {
            
        }
        virtual FILE* OpenFile(string filename, Subdirectory subdir, size_t* size);
        virtual void ReportFileError(string pre, string buffer, string post);


		
        /**
         * Save the Ini file's data to the disk.
         * @param filename the file to save to.
         * @return true if saving succeeded.
         */
        public bool SaveToDisk(string filename)
{
    /*
	 * First write the configuration to a (temporary) file and then rename
	 * that file. This to prevent that when OpenTTD crashes during the save
	 * you end up with a truncated configuration file.
	 */
    var file_new = filename + ".new";

            //FILE* f = fopen(file_new, "w");
            //if (f == NULL) return false;

    using (var stream = File.OpenWrite(file_new))
    {
        using (var writer = new StreamWriter(stream, Encoding.UTF8))
        {


            foreach (var group in groups.Values)
            {
                if (group.comment != null)
                {
                    writer.Write(group.comment);
                }

                writer.WriteLine(group.name);
                foreach (var item in group.items.Values)
                {
                    if (group.comment != null)
                    {
                        writer.Write(group.comment);
                    }

                    /* protect item.name with quotes if needed */
                    if (item.name.Contains(' ') || item.name[0] == '[')
                    {

                        writer.Write($"\"{item.name}\"");
                    }
                    else
                    {
                        writer.Write(item.name);
                    }

                    writer.WriteLine($" = {item.value}");
                }
            }
            if (this.comment != null)
            {
                writer.Write(this.comment);
            }

        }
    }

	var newFile = new FileInfo(file_new);
    newFile.CopyTo(filename, true);
	newFile.Delete();

	return true;
}

/* virtual */
FILE* IniFile::OpenFile(const char* filename, Subdirectory subdir, size_t* size)
{
    /* Open the text file in binary mode to prevent end-of-line translations
	 * done by ftell() and friends, as defined by K&R. */
    return FioFOpenFile(filename, "rb", subdir, size);
}

/* virtual */
void IniFile::ReportFileError(const char* const pre, const char* const buffer, const char* const post)
{
    ShowInfoF("%s%s%s", pre, buffer, post);
}

    }

}
