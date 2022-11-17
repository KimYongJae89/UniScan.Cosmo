using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP.Base.Services;

namespace UWP.SEMCNS.Services
{
    public class InspectService
    {
        public async Task Inspect()
        {
            await Task.Run(() =>
            {
                foreach (var grabber in DeviceService.Grabbers)
                    grabber.GrabOnce();
            });

            foreach (var grabber in DeviceService.Grabbers)
                grabber.GrabOnce();
        }
    }
}
