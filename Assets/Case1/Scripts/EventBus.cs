using System;
using UnityEngine;

namespace Case1
{
    public static class EventBus
    {
        public static Action<Bounds,float> ViewBoundsChanged;
        public static Action<Bounds> ViewBoundsChange;
    }
}
