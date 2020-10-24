﻿using System;

namespace ShortDash.Core.Models
{
    public class ExecuteActionParameters
    {
        public string ActionTypeName { get; set; }
        public string Parameters { get; set; }
        public Guid RequestId { get; set; }
        public bool ToggleState { get; set; }
    }
}
