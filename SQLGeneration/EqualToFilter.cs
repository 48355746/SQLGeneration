﻿using System;

namespace SQLGeneration
{
    /// <summary>
    /// Represents a comparison between two items for equality.
    /// </summary>
    public class EqualToFilter : ComparisonFilter
    {
        /// <summary>
        /// Initializes a new instance of a EqualToFilter.
        /// </summary>
        /// <param name="leftHand">The left hand item.</param>
        /// <param name="rightHand">The right hand item.</param>
        public EqualToFilter(IFilterItem leftHand, IFilterItem rightHand)
            : base(leftHand, rightHand)
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
            return leftHand + " = " + rightHand;
        }
    }
}
