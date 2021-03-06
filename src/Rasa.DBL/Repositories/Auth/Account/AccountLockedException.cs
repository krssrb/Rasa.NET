﻿using System;

namespace Rasa.Repositories.Auth.Account
{
    using Structures;
    using Structures.Auth;

    public class AccountLockedException : Exception
    {
        public AccountLockedException(AuthAccountEntry entry)
            : base($"User ({entry.Username}, {entry.Id}) tried to log in, but he/she is locked.")
        {
        }
    }
}