using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fiddler.LocalRedirect.Model
{
    public static class EventBusManager
    {
        private static readonly IEventBus eventBus = new EventBus();
        public static IEventBus Get()
        {
            return eventBus;
        }
    }
}
