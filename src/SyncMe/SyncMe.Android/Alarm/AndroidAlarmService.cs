﻿using Android.App;
using Android.Content;
using Android.Icu.Util;
using Android.OS;
using Android.Widget;
using Microsoft.Extensions.Logging;
using SyncMe.Droid.Extensions;
using SyncMe.Lib.Extensions;
using SyncMe.Models;
using AndroidApp = Android.App.Application;

namespace SyncMe.Droid.Alarm;

internal class AndroidAlarmService : IAlarmService
{
    private readonly ILogger<AndroidAlarmService> _logger;

    public AndroidAlarmService(ILogger<AndroidAlarmService> logger)
    {
        _logger = logger;
    }

    public void SetAlarmForEvent(SyncEvent syncEvent)
    {
        if (syncEvent.TryGetNearestAlarm(out var syncAlarm))
        {
            SetAlarm(syncAlarm);
        }
    }

    private void SetAlarm(SyncAlarm syncAlarm)
    {
        var triggerAtMs = GetTriggerAtMs(syncAlarm.AlarmTime);
        var alarmIntent = GetAlarmIntent(syncAlarm, AndroidApp.Context);

        SetAlarm(triggerAtMs, alarmIntent, AndroidApp.Context);
        string text = $"{syncAlarm.Title} Scheduled on {syncAlarm.AlarmTime}";
        Toast.MakeText(AndroidApp.Context, text, ToastLength.Long).Show();
        _logger.LogInformation(text);
    }

    private static long GetTriggerAtMs(DateTime alarmTime)
    {
        var calendarItem = Calendar.Instance;
        calendarItem.Set(alarmTime.Year, alarmTime.Month - 1, alarmTime.Day, alarmTime.Hour, alarmTime.Minute, alarmTime.Second);
        return calendarItem.TimeInMillis;
    }

    private void SetAlarm(long triggerAtMs, PendingIntent alarmIntent, Context context)
    {
        var am = context.GetSystemService(Context.AlarmService) as AlarmManager;
        if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            am.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, triggerAtMs, alarmIntent);
        else
            am.SetExact(AlarmType.RtcWakeup, triggerAtMs, alarmIntent);
    }

    private PendingIntent GetAlarmIntent(SyncAlarm syncAlarm, Context context)
    {
        var intent = new Intent(context, typeof(AlarmReceiver))
            .PutExtra(MessageKeys.ActionKey, MessageKeys.ProcessAlarmAction)
            .PutExtra(syncAlarm)
            .AddFlags(ActivityFlags.IncludeStoppedPackages)
            .AddFlags(ActivityFlags.ReceiverForeground);

        int uniqueId = Guid.NewGuid().GetHashCode();
        return PendingIntent.GetBroadcast(context, uniqueId, intent, PendingIntentFlags.Immutable);
    }
}
