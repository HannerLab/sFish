using System;

namespace HannerLabApp.Utils
{
    public static class MsgEvents
    {
        /// <summary>
        /// Events that trigger message bus
        /// </summary>
        public enum Event
        {
            Addition,
            Deletion,
            Update
        }

        /// <summary>
        /// Generates a message string for the specific model being updated.
        /// </summary>
        /// <param name="modelType"></param>
        /// <param name="e"></param>
        /// <returns>The message string</returns>
        public static string GetModel(Type modelType, Event e)
        {
            var t = e.ToString() + "_" + modelType.Name;
            return t.ToLower();
        }
    }
}
