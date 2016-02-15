﻿using System;
using HA4IoT.Contracts.Configuration;
using HA4IoT.Contracts.Core;

namespace HA4IoT.Core
{
    public static class ControllerExtensions
    {
        public static IArea CreateArea(this IController controller, Enum id)
        {
            var area = new Area(AreaId.From(id), controller);
            controller.AddArea(area);

            return area;
        }
    }
}
