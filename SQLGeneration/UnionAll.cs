﻿using System;

namespace SQLGeneration
{
    /// <summary>
    /// Adds all of the items from the first query to the second.
    /// </summary>
    public class UnionAll : SelectCombiner
    {
        /// <summary>
        /// Initializes a new instance of a UnionAll.
        /// </summary>
        public UnionAll()
        {
        }

        /// <summary>
        /// Retrieves the text used to combine two queries.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        /// <returns>The text used to combine two queries.</returns>
        protected override string GetCombinationString(BuilderContext context)
        {
            return "UNION ALL";
        }
    }
}
