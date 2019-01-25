using System;

namespace Rcp.Utilities.Extensions
{
    public static class EventHandlerExtensions
    {
        public static void Raise(this EventHandler eventHandler, object sender, EventArgs args)
        {
            eventHandler?.Invoke(sender, args);
        }

        public static void Raise<T>(this EventHandler<T> eventHandler, object sender, T args)
        {
            eventHandler?.Invoke(sender, args);
        }
    }
}