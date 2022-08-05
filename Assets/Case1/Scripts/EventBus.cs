using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    public static Action<Bounds,float> ViewBoundsChanged;
    public static Action<Bounds> ViewBoundsChange;
}
