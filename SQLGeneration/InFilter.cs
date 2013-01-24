﻿using System;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a filter where the values on the left hand must be in the values on the right hand.
    /// </summary>
    public class InFilter : ComparisonFilter
    {
        /// <summary>
        /// Initializes a new instance of a InFilter.
        /// </summary>
        /// <param name="leftHand">The left hand value that must exist in the list of values.</param>
        /// <param name="values">The list of values the left hand must exist in.</param>
        public InFilter(IFilterItem leftHand, IValueProvider values)
            : base(leftHand, values)
        {
        }

        /// <summary>
        /// Gets the filter text without parentheses or a not.
        /// </summary>
        /// <param name="leftHand">The left hand side of the comparison.</param>
        /// <param name="rightHand">The right hand side of the comparison.</param>
        /// <param name="context">The configuration to use when building the command.</param>
        /// <returns>A string representing the filter.</returns>
        protected override string Combine(BuilderContext context, string leftHand, string rightHand)
        {
            return leftHand + " IN " + rightHand;
        }
    }
}
