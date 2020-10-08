// <copyright file="HiddenAttribute.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.CommandModules
{
    using System;
    using System.Threading.Tasks;

    using Discord.Commands;

    public class HiddenAttribute : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            return Task.FromResult(PreconditionResult.FromSuccess());
        }
    }
}
