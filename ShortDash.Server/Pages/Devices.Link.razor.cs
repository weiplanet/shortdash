﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OtpNet;
using ShortDash.Core.Interfaces;
using ShortDash.Server.Components;
using ShortDash.Server.Data;
using ShortDash.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace ShortDash.Server.Pages
{
    public sealed partial class Devices_Link : PageBase, IDisposable
    {
        private DeviceClaims DeviceClaims { get; set; } = new DeviceClaims();

        private string DeviceLinkCode { get; set; }

        private string DeviceLinkSecureUrl { get; set; }

        private string DeviceLinkUrl { get; set; }

        private bool Linking { get; set; }

        public void Dispose()
        {
            StopLinking();
        }

        protected override Task OnParametersSetAsync()
        {
            DeviceLinkUrl = NavigationManager.ToAbsoluteUri("/").ToString();
            Linking = false;
            return base.OnParametersSetAsync();
        }

        private void Cancel()
        {
            if (Linking)
            {
                StopLinking();
                StateHasChanged();
            }
            else
            {
                NavigationManager.NavigateTo("/devices");
            }
        }

        private void DeviceLinked(string deviceLinkCode, string deviceId)
        {
            if (!deviceLinkCode.Equals(DeviceLinkCode))
            {
                return;
            }
            StopLinking();
            ToastService.ShowSuccess("The device has been linked!", "LINKED");
            NavigationManager.NavigateTo("/devices/" + HttpUtility.UrlEncode(deviceId));
        }

        private void DeviceLinkedEvent(object sender, DeviceLinkedEventArgs eventArgs)
        {
            InvokeAsync(() => DeviceLinked(eventArgs.DeviceLinkCode, eventArgs.DeviceId));
        }

        private string GenerateDeviceLinkCode()
        {
            var otp = new Totp(KeyGeneration.GenerateRandomKey(10));
            return otp.ComputeTotp();
        }

        private async void StartLinking()
        {
            if (!await SecureContext.ValidateUserAsync())
            {
                return;
            }
            DeviceLinkCode = GenerateDeviceLinkCode();
            DeviceLinkSecureUrl = NavigationManager.ToAbsoluteUri("/?c=" + HttpUtility.UrlEncode(DeviceLinkCode)).ToString();

            DeviceLinkService.OnDeviceLinked += DeviceLinkedEvent;

            var request = new LinkDeviceRequest { DeviceLinkCode = DeviceLinkCode };
            request.DeviceClaims.AddRange(DeviceClaims);
            DeviceLinkService.AddRequest(request);

            Linking = true;
        }

        private void StopLinking()
        {
            if (!Linking)
            {
                return;
            }
            Linking = false;
            DeviceLinkService.OnDeviceLinked -= DeviceLinkedEvent;
        }
    }
}
