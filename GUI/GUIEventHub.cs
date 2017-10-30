﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DeftLib
{
    public static class GUIEventHub
    {
        private static List<Gadget> _gadgets = new List<Gadget>();
        private static Gadget _activeGadget;

        public static int GadgetCount
        {
            get { return _gadgets.Count; }
        }

        private static Dictionary<Type, Type> _gadgetTypeMap =
           new Dictionary<Type, Type>();

        static GUIEventHub()
        {
            AssociateGadgetType<int, IntBox>();
            AssociateGadgetType<float, FloatBox>();
            AssociateGadgetType<string, StringBox>();
            AssociateGadgetType<Vector2, Vector2Panel>();


            //gadgetTypeMap[typeof(int)] = typeof(IntBox);
            //gadgetTypeMap[typeof(float)] = typeof(FloatBox);
            //gadgetTypeMap[typeof(Color)] = typeof(ColorPanel);
            //gadgetTypeMap[typeof(Vector2)] = typeof(VectorPanel);
            //gadgetTypeMap[typeof(Rectangle)] = typeof(RectanglePanel);
        }

        public static void AssociateGadgetType<DataType, GadgetType>() where GadgetType : Gadget
        {
            _gadgetTypeMap[typeof(DataType)] = typeof(GadgetType);
        }    
        
        public static Type GetGadgetType<DataType>()
        {
            Type dataType = typeof(DataType);

            if (_gadgetTypeMap.ContainsKey(dataType))
                return _gadgetTypeMap[dataType];
            return null;
        }

        public static Type GetGadgetType(Type dataType)
        {
            if (_gadgetTypeMap.ContainsKey(dataType))
                return _gadgetTypeMap[dataType];
            return null;
        }


        public static void Subscribe(Gadget g)
        {
            if (!_gadgets.Contains(g)) // Prevent aliasing.
                _gadgets.Add(g);
        }

        public static void Unsubscribe(Gadget g)
        {
            // When unsubscribing panels, recursively call until all gadgets are unsubscribed
            Panel p = g as Panel;
            //if (p != null)
              //  p.ClearGadgets();

            _gadgets.RemoveAll(gadget => gadget == g); // Prevent aliasing.
        }        

        public static void OnGUIEvent()
        {
            // Select new active gadget on left mouse press
            if (Input.LeftMousePressed())
            {
                _activeGadget = null;

                foreach (var g in _gadgets)
                {
                    if (g.Bounds.Contains(Input.MousePos))
                    {
                        _activeGadget = g;
                        break;
                    }
                }                
            }

            // GUI events are only processed for the active gadget.
            if (_activeGadget != null)
                _activeGadget.OnGUIEvent();
        }
    }
}
