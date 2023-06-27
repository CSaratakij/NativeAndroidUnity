using System;
using UnityEngine;

namespace LuviKunG.Android
{
    /// <summary>
    /// Native Android that contains various native call or dialog usage.
    /// </summary>
    public static class NativeAndroid
    {
        public delegate void AndroidAlertDialogCallback(NativeAndroid.AlertDialogResponseType type);
        public delegate void AndroidDateTimeCallback(DateTime date);
        public delegate void AndroidTimeSpanCallback(TimeSpan time);

        /// <summary>
        /// Response type of alert dialogue.
        /// This is used for <see cref="AndroidAlertDialogCallback"/> event.
        /// </summary>
        public enum AlertDialogResponseType : int
        {
            /// <summary>
            /// Positive button.
            /// </summary>
            Positive = 0,
            /// <summary>
            /// Negative button.
            /// </summary>
            Negative = 1,
            /// <summary>
            /// Neutral button.
            /// </summary>
            Neutral = 2,
        }

        /// <summary>
        /// Response type of native prompt.
        /// </summary>
        public enum NativeResponse : int
        {
            /// <summary>
            /// Internal Error.
            /// Such as cannot retrive service to check status or unknown error.
            /// </summary>
            InternalError = -2,
            /// <summary>
            /// Unsupport device.
            /// Such as android device version isn't support the prompt.
            /// </summary>
            Unsupported = -1,
            /// <summary>
            /// Negative.
            /// </summary>
            Negative = 0,
            /// <summary>
            /// Positive.
            /// </summary>
            Positive = 1,
        }

        /// <summary>
        /// This is <see cref="AndroidJavaProxy"/> callback for usage in <see cref="NativeAndroid"/>.
        /// </summary>
        private sealed class NativeAndroidCallbackProxy : AndroidJavaProxy
        {
            public AndroidAlertDialogCallback onAlertDialog;
            public AndroidDateTimeCallback onPickDate;
            public AndroidTimeSpanCallback onPickTime;

            public NativeAndroidCallbackProxy(string identifier) : base(identifier)
            {

            }

            public void OnAlertDialog(int type)
            {
                NativeAndroid.AlertDialogResponseType responseType = (NativeAndroid.AlertDialogResponseType)type;
                onAlertDialog?.Invoke(responseType);
            }

            public void OnPickDate(int year, int month, int day)
            {
                onPickDate?.Invoke(new DateTime(year, month + 1, day));
            }

            public void OnPickTime(int hour, int minute)
            {
                onPickTime?.Invoke(new TimeSpan(hour, minute, 0));
            }
        }

        private const string BUNDLE_UNITY_PLAYER_ACTIVITY = "com.unity3d.player.UnityPlayer";
        private const string BUNDLE_PLUGINS = "com.luvikung.nativeandroid";
        private const string BUNDLE_CLASS_INSTANCE = BUNDLE_PLUGINS + ".PluginInstance";
        private const string BUNDLE_CLASS_CALLBACK = BUNDLE_PLUGINS + ".PluginCallback";

#if UNITY_ANDROID
        private static AndroidJavaClass m_unityClass;
        private static AndroidJavaObject m_unityActivity;
        private static AndroidJavaObject m_pluginInstance;
        private static NativeAndroidCallbackProxy m_pluginCallback;
#endif

        /// <summary>
        /// Default constructor.
        /// </summary>
        static NativeAndroid()
        {
            Debug.Log($"{nameof(NativeAndroid)}");
#if UNITY_EDITOR
            Debug.LogWarning("Please note that Native Android is not supported on Editor.");
#elif UNITY_ANDROID
            m_unityClass = new AndroidJavaClass(BUNDLE_UNITY_PLAYER_ACTIVITY);
            m_unityActivity = m_unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            m_pluginInstance = new AndroidJavaObject(BUNDLE_CLASS_INSTANCE);
            m_pluginInstance.Call("setActivity", m_unityActivity);
            
            m_pluginCallback = new NativeAndroidCallbackProxy(BUNDLE_CLASS_CALLBACK);
            m_pluginInstance.Call("setPluginCallback", m_pluginCallback);
            int version = m_pluginInstance.Call<int>("getPluginVersion");
            Debug.Log($"{nameof(NativeAndroid)} version {version} was initialized successfully.");
#endif
        }

        /// <summary>
        /// Set an Android device's wake lock to keep the screen on.
        /// This requires the "android.permission.WAKE_LOCK" android permission.
        /// </summary>
        /// <param name="isLock">Activate or deactivate the wake lock.</param>
        /// <param name="millisecond">The android screen's wake lock time is. (default is 10 minutes)</param>
        public static void SetWakeLock(bool isLock, int millisecond = 0)
        {
            Debug.Log($"{nameof(NativeAndroid)}|{nameof(SetWakeLock)}");
#if UNITY_EDITOR
            Debug.LogWarning($"Please note that this isn't work anything in Unity Editor. Calling any static function will return default behaviour.");
#elif UNITY_ANDROID
            if (m_pluginInstance == null)
                return;
            m_pluginInstance.Call("wakeLock", isLock, millisecond);
#endif
        }

        /// <summary>
        /// Navigate to this app's Android application settings.
        /// </summary>
        public static void OpenApplicationSettings()
        {
            Debug.Log($"{nameof(NativeAndroid)}|{nameof(OpenApplicationSettings)}");
#if UNITY_EDITOR
            Debug.LogWarning($"Please note that this isn't work anything in Unity Editor.");
#elif UNITY_ANDROID
            if (m_pluginInstance == null)
                return;
            m_pluginInstance.Call("openApplicationSettings");
#endif
        }

        /// <summary>
        /// Navigate to Google Play Account.
        /// </summary>
        public static void OpenGooglePlayAccount()
        {
            Debug.Log($"{nameof(NativeAndroid)}|{nameof(OpenGooglePlayAccount)}");
#if UNITY_EDITOR
            Debug.LogWarning($"Please note that this isn't work anything in Unity Editor.");
#elif UNITY_ANDROID
            if (m_pluginInstance == null)
                return;
            m_pluginInstance.Call("openGooglePlayAccount");
#endif
        }

        /// <summary>
        /// Navigate to Google Play Account Subscription.
        /// </summary>
        public static void OpenGooglePlayAccountSubscription()
        {
            Debug.Log($"{nameof(NativeAndroid)}|{nameof(OpenGooglePlayAccountSubscription)}");
#if UNITY_EDITOR
            Debug.LogWarning($"Please note that this isn't work anything in Unity Editor.");
#elif UNITY_ANDROID
            if (m_pluginInstance == null)
                return;
            m_pluginInstance.Call("openGooglePlayAccountSubscription");
#endif
        }

        /// <summary>
        /// Check are there notification permission or enable.
        /// </summary>
        /// <returns>Returns <see cref="NativeResponse"/> of notification permission.</returns>
        public static NativeResponse AreNotificationsEnabled()
        {
            Debug.Log($"{nameof(NativeAndroid)}|{nameof(AreNotificationsEnabled)}");
#if UNITY_EDITOR
            Debug.LogWarning($"Please note that this isn't work anything in Unity Editor.");
            return NativeResponse.Unsupported;
#elif UNITY_ANDROID
            if (m_pluginInstance == null)
                return NativeResponse.InternalError;
            return (NativeResponse)m_pluginInstance.Call<int>("areNotificationsEnabled");
#endif
        }

        /// <summary>
        /// Check are there notification permission or paused.
        /// </summary>
        /// <returns>Returns <see cref="NativeResponse"/> of notification permission.</returns>
        public static NativeResponse AreNotificationsPaused()
        {
            Debug.Log($"{nameof(NativeAndroid)}|{nameof(AreNotificationsPaused)}");
#if UNITY_EDITOR
            Debug.LogWarning($"Please note that this isn't work anything in Unity Editor.");
            return NativeResponse.Unsupported;
#elif UNITY_ANDROID
            if (m_pluginInstance == null)
                return NativeResponse.InternalError;
            return (NativeResponse)m_pluginInstance.Call<int>("areNotificationsPaused");
#endif
        }

        /// <summary>
        /// Quick check if the device has notification permission and isn't pause it.
        /// This will use <see cref="AreNotificationsEnabled"/> and <see cref="AreNotificationsPaused"/> to check.
        /// </summary>
        /// <returns>Is notification allowed and didn't paused.</returns>
        public static bool IsNotificationAllowed()
        {
            Debug.Log($"{nameof(NativeAndroid)}|{nameof(IsNotificationAllowed)}");
#if UNITY_EDITOR
            Debug.LogWarning($"Please note that this isn't work anything in Unity Editor.");
            return false;
#elif UNITY_ANDROID
            bool isEnabled = AreNotificationsEnabled() == NativeResponse.Positive;
            bool isPaused = AreNotificationsPaused() == NativeResponse.Positive;
            return isEnabled && !isPaused;
#endif
        }

        /// <summary>
        /// Ask the app if it can show a permission rationale for the given permission.
        /// </summary>
        /// <param name="permission">Android permission string.</param>
        /// <returns>Is should show permission.</returns>
        public static bool ShouldShowRequestPermission(string permission)
        {
            Debug.Log($"{nameof(NativeAndroid)}|{nameof(ShouldShowRequestPermission)}");
#if UNITY_EDITOR
            Debug.LogWarning($"Please note that this isn't work anything in Unity Editor.");
            return false;
#elif UNITY_ANDROID
            if (m_pluginInstance == null)
                return false;
            return m_pluginInstance.Call<bool>("shouldShowRequestPermission", permission);
#endif
        }

        /// <summary>
        /// Ask the app if it can show a permission notification permission.
        /// </summary>
        /// <returns>Is should show permission.</returns>
        public static bool ShouldShowRequestPermissionNotification()
        {
            Debug.Log($"{nameof(NativeAndroid)}|{nameof(ShouldShowRequestPermissionNotification)}");
#if UNITY_EDITOR
            Debug.LogWarning($"Please note that this isn't work anything in Unity Editor.");
            return false;
#elif UNITY_ANDROID
            return ShouldShowRequestPermission("android.permission.ACCESS_NOTIFICATION_POLICY");
#endif
        }

        /// <summary>
        /// Request android permission.
        /// </summary>
        /// <param name="permission">Android permission string.</param>
        public static void RequestPermission(string permission)
        {
            Debug.Log($"{nameof(NativeAndroid)}|{nameof(RequestPermission)}");
#if UNITY_EDITOR
            Debug.LogWarning($"Please note that this isn't work anything in Unity Editor.");
#elif UNITY_ANDROID
            if (m_pluginInstance == null)
                return;
            m_pluginInstance.Call("requestPermission", permission);
#endif
        }

        /// <summary>
        /// Show alert dialog.
        /// </summary>
        /// <param name="title">Title of this alert dialog. Can be null</param>
        /// <param name="message">Message of this alert dialog. Cannot be null.</param>
        /// <param name="isCancelable">Is this alert dialog cancelable? Alert dialog will be canceled when touching outside the dialog or press the back button.</param>
        /// <param name="positiveButton">Message of the positive button. Cannot be null.</param>
        /// <param name="negativeButton">Message of the negative button. Can be null.</param>
        /// <param name="neutralButton">Message of the neutral button. Can be null.</param>
        /// <param name="onDone">Callback when this alert dialog has receive input from the user. If this alert dialog are cancelable, this callback will not be called.</param>
        public static void ShowAlertDialog(string title, string message, bool isCancelable, string positiveButton, string negativeButton, string neutralButton, AndroidAlertDialogCallback onDone)
        {
            Debug.Log($"{nameof(NativeAndroid)}|{nameof(ShowAlertDialog)}");
#if UNITY_EDITOR
            Debug.LogWarning($"Please note that this isn't work anything in Unity Editor. This will callback as positive as default.");
            onDone?.Invoke(AlertDialogResponseType.Positive);
#elif UNITY_ANDROID
            if (m_pluginInstance == null)
                return;
            m_pluginCallback.onAlertDialog = onDone;
            m_pluginInstance.Call("alertDialog", title, message, isCancelable, positiveButton, negativeButton, neutralButton);
#endif
        }

        /// <summary>
        /// Show date picker dialog.
        /// </summary>
        /// <param name="dateTime">Default date that will show in the picker.</param>
        /// <param name="onDone">Callback when this date picker dialog has receive input from the user.</param>
        public static void ShowPickDate(DateTime dateTime, AndroidDateTimeCallback onDone)
        {
            Debug.Log($"{nameof(NativeAndroid)}|{nameof(ShowPickDate)}");
#if UNITY_EDITOR
            Debug.LogWarning($"Please note that this isn't work anything in Unity Editor. This will callback as current date as default.");
            onDone?.Invoke(DateTime.UtcNow);
#elif UNITY_ANDROID
            if (m_pluginInstance == null)
                return;
            m_pluginCallback.onPickDate = onDone;
            m_pluginInstance.Call("pickDate", dateTime.Year, dateTime.Month - 1, dateTime.Day);
#endif
        }

        /// <summary>
        /// Show date picker dialog.
        /// This will show current date as default.
        /// </summary>
        /// <param name="onDone">Callback when this date picker dialog has receive input from the user.</param>
        public static void ShowPickDate(AndroidDateTimeCallback onDone)
        {
            Debug.Log($"{nameof(NativeAndroid)}|{nameof(ShowPickDate)}");
#if UNITY_EDITOR
            Debug.LogWarning($"Please note that this isn't work anything in Unity Editor. This will callback as current date as default.");
            onDone?.Invoke(DateTime.UtcNow);
#elif UNITY_ANDROID
            if (m_pluginInstance == null)
                return;
            m_pluginCallback.onPickDate = onDone;
            m_pluginInstance.Call("pickDate");
#endif
        }

        /// <summary>
        /// Show time picker dialog.
        /// </summary>
        /// <param name="time">Default time that will show in the picker.</param>
        /// <param name="onDone">Callback when this time picker dialog has receive input from the user.</param>
        public static void ShowPickTime(bool is24HourView, TimeSpan time, AndroidTimeSpanCallback onDone)
        {
            Debug.Log($"{nameof(NativeAndroid)}|{nameof(ShowPickTime)}");
#if UNITY_EDITOR
            Debug.LogWarning($"Please note that this isn't work anything in Unity Editor. This will callback as current time as default.");
            onDone?.Invoke(DateTime.UtcNow.TimeOfDay);
#elif UNITY_ANDROID
            if (m_pluginInstance == null)
                return;
            m_pluginCallback.onPickTime = onDone;
            m_pluginInstance.Call("pickTime", is24HourView, time.Hours, time.Minutes);
#endif
        }

        /// <summary>
        /// Show time picker dialog.
        /// This will show current time as default.
        /// </summary>
        /// <param name="onDone">Callback when this time picker dialog has receive input from the user.</param>
        public static void ShowPickTime(bool is24HourView, AndroidTimeSpanCallback onDone)
        {
            Debug.Log($"{nameof(NativeAndroid)}|{nameof(ShowPickTime)}");
#if UNITY_EDITOR
            Debug.LogWarning($"Please note that this isn't work anything in Unity Editor. This will callback as current time as default.");
            onDone?.Invoke(DateTime.UtcNow.TimeOfDay);
#elif UNITY_ANDROID
            if (m_pluginInstance == null)
                return;
            m_pluginCallback.onPickTime = onDone;
            m_pluginInstance.Call("pickTime", is24HourView);
#endif
        }

        /// <summary>
        /// Show toast message.
        /// </summary>
        /// <param name="message">Message to show in toast.</param>
        public static void ShowToastMessage(string message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#elif UNITY_ANDROID
            if (m_pluginInstance == null)
                return;
            m_pluginInstance.Call("toast", message);
#endif
        }
    }
}