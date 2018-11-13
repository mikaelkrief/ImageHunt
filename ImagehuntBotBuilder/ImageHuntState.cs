﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Bot.Schema;

namespace ImageHuntBotBuilder
{
    /// <summary>
    /// state for the conversation.
    /// Stored in <see cref="Microsoft.Bot.Builder.ConversationState"/> and
    /// backed by <see cref="Microsoft.Bot.Builder.MemoryStorage"/>.
    /// </summary>
    public class ImageHuntState
    {
        public string ConversationId { get; set; }
        public Status Status { get; set; }
        public int? GameId { get; set; }
        public int? TeamId { get; set; }
        public GeoCoordinates CurrentLocation { get; set; }
        public string[] GroupAdmins { get; set; }
        public GameResponse Game { get; set; }
        public TeamResponse Team { get; set; }
        public NodeResponse CurrentNode { get; set; }
        public int? CurrentNodeId { get; set; }
    }

    public enum Status  
    {
        None,
        Initialized,
        Started,
        Ended
    }
}