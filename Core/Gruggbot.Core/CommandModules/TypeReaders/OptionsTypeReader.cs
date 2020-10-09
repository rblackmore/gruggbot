// <copyright file="OptionsTypeReader.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.CommandModules.TypeReaders
{
    using System;
    using System.Threading.Tasks;

    using Discord.Commands;

    public class OptionsTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            if (string.IsNullOrEmpty(input))
                return Task.FromResult(TypeReaderResult.FromSuccess(TestOption.None));

            // Try parse input as Options. Ignore case.
            if (Enum.TryParse<TestOption>(input, true, out TestOption result))
                return Task.FromResult(TypeReaderResult.FromSuccess(result));

            var errorReason = string.Format("Failed to parse `{0}` as Options value", input);

            return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, errorReason));
        }
    }
}
