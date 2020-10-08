// <copyright file="LookupTypeReader.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.CommandModules
{
    using System;
    using System.Threading.Tasks;

    using Discord.Commands;

    internal class LookupTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            if (string.IsNullOrEmpty(input))
                return Task.FromResult(TypeReaderResult.FromSuccess(LookupType.None));

            // Try parse input as LookupType. Ignore case.
            if (Enum.TryParse<LookupType>(input, true, out LookupType result))
                return Task.FromResult(TypeReaderResult.FromSuccess(result));

            var errorReason = string.Format("Failed to parse `{0}` as LookupType value", input);

            return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, errorReason));
        }
    }
}
