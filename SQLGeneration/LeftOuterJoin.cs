﻿using System;
using System.Text;

namespace SQLGeneration
{
    /// <summary>
    /// Represents an left-outer join in a select statement.
    /// </summary>
    public class LeftOuterJoin : FilteredJoin
    {
        /// <summary>
        /// Initializes a new instance of a LeftOuterJoin.
        /// </summary>
        /// <param name="leftHand">The left hand item in the join.</param>
        /// <param name="rightHand">The right hand item in the join.</param>
        public LeftOuterJoin(IJoinItem leftHand, IJoinItem rightHand)
            : base(leftHand, rightHand, new IFilter[0])
        {
        }

        /// <summary>
        /// Initializes a new instance of a LeftOuterJoin.
        /// </summary>
        /// <param name="leftHand">The left hand item in the join.</param>
        /// <param name="rightHand">The right hand item in the join.</param>
        /// <param name="filters">The filters to join to the join items on.</param>
        public LeftOuterJoin(IJoinItem leftHand, IJoinItem rightHand, params IFilter[] filters)
            : base(leftHand, rightHand, filters)
        {
        }

        /// <summary>
        /// Gets the name of the join type.
        /// </summary>
        /// <param name="context">The configuration to use when building the command.</param>
        /// <returns>The name of the join type.</returns>
        protected override string GetJoinName(BuilderContext context)
        {
            StringBuilder result = new StringBuilder("LEFT ");
            if (context.Options.VerboseOuterJoin)
            {
                result.Append("OUTER ");
            }
            result.Append("JOIN");
            return result.ToString();
        }
    }
}