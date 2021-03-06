﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppInsights.EventHub.Sink.NetFramework.Telemetry.Batching;
using AppInsights.EventHub.Sink.NetFramework.Telemetry.Configuration;
using Microsoft.ApplicationInsights.Channel;

namespace AppInsights.EventHub.Sink.NetFramework.Telemetry.Sink
{
    public abstract class EventHubSinkPublisher : BatchingPublisher
    {
        private readonly EventHubSinkSettings _eventHubSettings;

        protected EventHubSinkPublisher(EventHubSinkSettings eventHubSettings)
            : base(eventHubSettings.BatchBufferSettings)
        {
            _eventHubSettings = eventHubSettings;
        }

        protected override async Task Publish(ICollection<ITelemetry> telemetryEvents)
        {
            if (telemetryEvents == null)
                return;

            try
            {
                await PublishToEventHub(telemetryEvents);
            }
            catch (Exception e)
            {
                // do exception logging
            }
        }

        protected async Task PublishToEventHub(ICollection<ITelemetry> events)
        {
            try
            {
                var transmission = new EventHubTransmission(_eventHubSettings, events);

                await transmission.SendAsync();
            }
            catch (Exception e)
            {
                // do some logging
            }
        }
    }
}