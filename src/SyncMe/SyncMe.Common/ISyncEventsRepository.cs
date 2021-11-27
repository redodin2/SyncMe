﻿using SyncMe.Models;

namespace SyncMe;

public interface ISyncEventsRepository
{
    IReadOnlyCollection<SyncEvent> GetAllSyncEvents();
    bool TryGetSyncEvent(Guid id, out SyncEvent syncEvent);
    Guid AddSyncEvent(SyncEvent syncEvent);
    void RemoveEvents(Func<SyncEvent, bool> predicate);
}
