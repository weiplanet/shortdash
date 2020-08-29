﻿using Microsoft.AspNetCore.Components;
using ShortDash.Core.Plugins;
using System;
using System.Text.Json;

namespace ShortDash.Server.Actions
{
    public class DashLinkAction : IShortDashAction
    {
        private readonly NavigationManager navigationManager;

        public DashLinkAction(NavigationManager navigationManager)
        {
            this.navigationManager = navigationManager;
        }

        public bool Execute(string parameters)
        {
            var dashLinkParameters = JsonSerializer.Deserialize<DashLinkProcessParameters>(parameters);
            Console.WriteLine($"Clicked dash link button ID {dashLinkParameters.DashboardId}");
            navigationManager.NavigateTo($"/dashboard/{dashLinkParameters.DashboardId}");
            return true;
        }

        private class DashLinkProcessParameters
        {
            public int DashboardId { get; set; }
        }
    }
}