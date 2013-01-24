﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SQLGeneration
{
    /// <summary>
    /// Builds a string of an insert statement.
    /// </summary>
    public class InsertBuilder : IInsertBuilder
    {
        private readonly ITable _table;
        private readonly List<IColumn> _columns;
        private readonly IValueProvider _values;

        /// <summary>
        /// Initializes a new instance of a InsertBuilder.
        /// </summary>
        /// <param name="table">The table being inserted into.</param>
        /// <param name="values">The values to insert into the table.</param>
        public InsertBuilder(ITable table, IValueProvider values)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            _table = table;
            _columns = new List<IColumn>();
            _values = values;
        }

        /// <summary>
        /// Gets the table that is being inserted into.
        /// </summary>
        public ITable Table
        {
            get { return _table; }
        }

        /// <summary>
        /// Gets the columns being inserted into.
        /// </summary>
        public IEnumerable<IColumn> Columns
        {
            get { return new ReadOnlyCollection<IColumn>(_columns); }
        }

        /// <summary>
        /// Adds the column to the insert statement.
        /// </summary>
        /// <param name="column">The column to add.</param>
        public void AddColumn(IColumn column)
        {
            if (column == null)
            {
                throw new ArgumentNullException("column");
            }
            _columns.Add(column);
        }

        /// <summary>
        /// Removes the column from the insert statement.
        /// </summary>
        /// <param name="column">The column to remove.</param>
        /// <returns>True if the column was removed; otherwise, false.</returns>
        public bool RemoveColumn(IColumn column)
        {
            if (column == null)
            {
                throw new ArgumentNullException("column");
            }
            return _columns.Remove(column);
        }

        /// <summary>
        /// Gets the list of values or select statement that populates the insert.
        /// </summary>
        public IValueProvider Values
        {
            get { return _values; }
        }

        /// <summary>
        /// Gets the SQL for the insert statement.
        /// </summary>
        public string GetCommandText()
        {
            return getCommandText(new BuilderContext());
        }

        /// <summary>
        /// Gets the SQL for the insert statement.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        public string GetCommandText(BuilderContext context)
        {
            context = context.Clone();
            context.IsSelect = false;
            context.IsInsert = true;
            context.IsUpdate = false;
            context.IsDelete = false;

            return getCommandText(context);
        }

        private string getCommandText(BuilderContext context)
        {
            StringBuilder result = new StringBuilder("INSERT INTO ");
            result.Append(_table.GetDeclaration(context, null));
            if (context.Options.OneClausePerLine)
            {
                result.AppendLine();
            }
            else
            {
                result.Append(' ');
            }
            if (_columns.Count > 0)
            {
                result.Append('(');
                StringBuilder separatorBuilder = new StringBuilder(",");
                if (context.Options.OneInsertColumnPerLine)
                {
                    result.AppendLine();
                    separatorBuilder.AppendLine();
                }
                else
                {
                    separatorBuilder.Append(' ');
                }
                ProjectionItemFormatter columnFormatter = new ProjectionItemFormatter(context);
                IEnumerable<string> columns = _columns.Select(column => columnFormatter.GetUnaliasedReference(column));
                if (context.Options.OneInsertColumnPerLine && context.Options.IndentInsertColumns)
                {
                    string indentationText = context.Indent().GetIndentationText();
                    columns = columns.Select(column => indentationText + column);
                }
                string separated = String.Join(separatorBuilder.ToString(), columns);
                result.Append(separated);
                if (context.Options.OneInsertColumnPerLine)
                {
                    result.AppendLine();
                }
                result.Append(')');
            }
            if (context.Options.OneClausePerLine)
            {
                result.AppendLine();
            }
            else
            {
                result.Append(' ');
            }
            if (!_values.IsQuery)
            {
                result.Append("VALUES");
                if (context.Options.OneClausePerLine)
                {
                    result.AppendLine();
                }
                else
                {
                    result.Append(' ');
                }
            }
            result.Append(_values.GetFilterItemText(context));
            return result.ToString();
        }
    }
}