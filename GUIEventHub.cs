using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DeftLib
{
    public interface GUIEventListener
    {
        void OnGUIEvent();
        Rectangle Bounds { get; }
    }

    public static class GUIEventHub
    {
        private static List<GUIEventListener> _listeners = new List<GUIEventListener>();
        private static GUIEventListener _activeListener;

        public static GUIEventListener ActiveListener
        {
            get { return _activeListener; }
        }

        public static void Subscribe(GUIEventListener newListener)
            => _listeners.Add(newListener);

        public static void Unsubscribe(GUIEventListener newListener)
            => _listeners.RemoveAll(x => x == newListener); // Make sure extra aliases aren't left around.

        public static void OnGUIEvent()
        {
            foreach (var listener in _listeners)
                listener.OnGUIEvent();

            // Select new active gadget on left mouse press
            if (Input.LeftMousePressed())
            {
                foreach (GUIEventListener g in _listeners)
                {
                    if (g.Bounds.Contains(Input.MousePos))
                    {
                        _activeListener = g;
                        break;
                    }
                }
            }

            // Increment active gadget on tab press
            if (Input.KeyTyped(Keys.Tab))
            {
                if (_activeListener != null)
                {
                    int index = _listeners.IndexOf(_activeListener);
                    ++index;

                    if (index >= _listeners.Count - 1)
                        index = 0;

                    _activeListener = _listeners[index];
                }
            }
        }
    }
}
