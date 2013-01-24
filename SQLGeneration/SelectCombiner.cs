﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SQLGeneration.Properties;

namespace SQLGeneration
{
    /// <summary>
    /// Performs a set operation on the results of two queries.
    /// </summary>
    public abstract class SelectCombiner : ISelectCombiner
    {
        private readonly List<ISelectBuilder> _queries;

        /// <summary>
        /// Initializes a new instance of a QueryCombiner.
        /// </summary>
        protected SelectCombiner()
        {
            _queries = new List<ISelectBuilder>();
        }

        /// <summary>
        /// Creates a new column under the table.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>The column.</returns>
        public Column CreateColumn(string columnName)
        {
            return new Column(this, columnName);
        }

        IColumn IColumnSource.CreateColumn(string columnName)
        {
            return CreateColumn(columnName);
        }

        /// <summary>
        /// Creates a new column under the multi-select.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="alias">The alias to give the column.</param>
        /// <returns>The column.</returns>
        public Column CreateColumn(string columnName, string alias)
        {
            return new Column(this, columnName);
        }

        IColumn IColumnSource.CreateColumn(string columnName, string alias)
        {
            return CreateColumn(columnName, alias);
        }

        /// <summary>
        /// Gets the queries that are to be combined.
        /// </summary>
        public IEnumerable<ISelectBuilder> Queries
        {
            get { return new ReadOnlyCollection<ISelectBuilder>(_queries); }
        }

        /// <summary>
        /// Adds the query to the combination.
        /// </summary>
        /// <param name="query">The query to add.</param>
        public void AddQuery(ISelectBuilder query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            _queries.Add(query);
        }

        /// <summary>
        /// Removes the query from the combination.
        /// </summary>
        /// <param name="query">The query to remove.</param>
        /// <returns>True if the query is removed; otherwise, false.</returns>
        public bool RemoveQuery(ISelectBuilder query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return _queries.Remove(query);
        }

        /// <summary>
        /// Retrieves the text used to combine two queries.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        /// <returns>The text used to combine two queries.</returns>
        protected abstract string GetCombinationString(BuilderContext context);

        /// <summary>
        /// Gets the command text.
        /// </summary>
        public string GetCommandText()
        {
            return GetCommandText(new BuilderContext());
        }

        /// <summary>
        /// Gets the command text.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        public string GetCommandText(BuilderContext context)
        {
            if (_queries.Count == 0)
            {
                throw new SQLGenerationException(Resources.NoQueries);
            }
            string combinationString = " " + GetCombinationString(context) + " ";
            return String.Join(combinationString, from query in _queries select "(" + query.GetCommandText() + ")");
        }

        /// <summary>
        /// Gets or sets an alias for the query results.
        /// </summary>
        public string Alias
        {
            get;
            set;
        }

        string IJoinItem.GetDeclaration(BuilderContext context, IFilterGroup where)
        {
            StringBuilder result = new StringBuilder();
            if (_queries.Count > 1)
            {
                result.Append("(");
                result.Append(GetCommandText());
                result.Append(")");
            }
            else
            {
                result.Append(GetCommandText());
            }
            if (!String.IsNullOrWhiteSpace(Alias))
            {
                result.Append(' ');
                if (context.Options.AliasJoinItemsUsingAs)
                {
                    result.Append("AS ");
                }
                result.Append(Alias);
            }
            return result.ToString();
        }

        string IColumnSource.GetReference(BuilderContext context)
        {
            if (String.IsNullOrWhiteSpace(Alias))
            {
                throw new SQLGenerationException(Resources.ReferencedQueryCombinerWithoutAlias);
            }
            return Alias;
        }

        string IProjectionItem.GetFullText(BuilderContext context)
        {
            return '(' + GetCommandText() + ')';
        }

        string IFilterItem.GetFilterItemText(BuilderContext context)
        {
            return '(' + GetCommandText() + ')';
        }

        bool IValueProvider.IsQuery
        {
            get { return true; }
        }
    }
}