using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeftLib
{
    public interface GUIEventListener
    {
        void OnGUIEvent();
    }

    public static class GUIEventHub
    {
        private static List<GUIEventListener> _listeners = new List<GUIEventListener>();

        public static void Subscribe(GUIEventListener newListener)
            => _listeners.Add(newListener);

        public static void Unsubscribe(GUIEventListener newListener)
            => _listeners.RemoveAll(x => x == newListener); // Make sure extra aliases aren't left around.

        public static void OnGUIEvent()
        {
            foreach (var listener in _listeners)
                listener.OnGUIEvent();
        }
    }
}
