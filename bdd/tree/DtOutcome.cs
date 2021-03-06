﻿using System;
using System.Diagnostics;

namespace Modd
{
    [DebuggerDisplay("{ToDebuggerString()}")]
    public class DtOutcome : BaseDtVertexType
    {
        public DtOutcome(string outcome) : base()
        {
            if (outcome == null)
            {
                throw new ArgumentNullException(nameof(outcome));
            }
            this.OutcomeValue = outcome;
        }
        public string OutcomeValue { get; set; }

        public string ToDebuggerString()
        {
            return $"O:{OutcomeValue}";
        }
        
    }
}