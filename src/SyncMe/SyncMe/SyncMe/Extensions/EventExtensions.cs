﻿using SyncMe.Models;
using Microsoft.Graph;


namespace SyncMe.Extensions
{
    internal static class EventExtensions
    {
        public static SyncEvent ToSyncEvent(this Event e, string username = null) => new SyncEvent {
                                                                                                     ExternalId = e.Id,
                                                                                                     ExternalEmail = username,
                                                                                                     Title = e.Subject,
                                                                                                     Description = e.Body.Content,
                                                                                                     Namespace = new Namespace { Title = "", Id = Guid.Empty },
                                                                                                     Schedule = new SyncSchedule { Repeat = SyncRepeat.None },
                                                                                                     Alert = new SyncAlert { Reminders = new[] { SyncReminder.AtEventTime } },
                                                                                                     Status = SyncStatus.Active,
                                                                                                     Start = DateTime.Parse(e.Start.DateTime),
                                                                                                     End = DateTime.Parse(e.End.DateTime) 
                                                                                                   };
    }
}
