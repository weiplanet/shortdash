﻿using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using ShortDash.Server.Data;
using ShortDash.Server.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ShortDash.Server.Components
{
    public partial class AddDashboardDialog : ComponentBase
    {
        private SelectedItem Selected { get; } = new SelectedItem();

        public static Task<ModalResult> ShowAsync(IModalService modalService)
        {
            var modal = modalService.Show<AddDashboardDialog>("New Dashboard");
            return modal.Result;
        }

        private class SelectedItem
        {
            public string Value { get; set; }
        }
    }
}