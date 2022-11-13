﻿using FluentValidation.Results;

namespace NerdStore.Core.Messages
{
    public abstract class Command : Message
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; set; }

        public Command()
        {
            Timestamp = DateTime.Now;
        }
        
        public abstract bool IsValid();
        
    }
}
