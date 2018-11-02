// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Diagnostics;

namespace ImageHuntBotBuilder
{
    /// <summary>
    /// state for the conversation.
    /// Stored in <see cref="Microsoft.Bot.Builder.ConversationState"/> and
    /// backed by <see cref="Microsoft.Bot.Builder.MemoryStorage"/>.
    /// </summary>
    public class ImageHuntState
    {
        public Status Status { get; set; }
    }

    public enum Status  
    {
    }
}
