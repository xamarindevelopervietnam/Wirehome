﻿using System.Linq;
using System.Xml.Linq;
using System.IO;
using Wirehome.Extensions.Devices;
using Wirehome.Contracts.Components.States;
using Wirehome.Extensions.Extensions;

namespace Wirehome.Extensions.Messaging.DenonMessages
{
    public class DenonStatusLightMessage : HttpMessage
    {
        public string Zone { get; set; } = "1";

        public DenonStatusLightMessage()
        {
            RequestType = "GET";
        }

        public override string MessageAddress()
        {
            if(Zone == "1")
            {
                return $"http://{Address}/goform/formMainZone_MainZoneXmlStatusLite.xml";
            }
            return $"http://{Address}/goform/formZone{Zone}_Zone{Zone}XmlStatusLite.xml";
        }

        public override object ParseResult(string responseData)
        {
            using (var reader = new StringReader(responseData))
            {
                var xml = XDocument.Parse(responseData);
                
                return new DenonStatus
                {
                    ActiveInput = xml.Descendants("InputFuncSelect").FirstOrDefault()?.Value?.Trim(),
                    PowerStatus = xml.Descendants("Power").FirstOrDefault()?.Value?.Trim().ToLower() == "on" ? PowerStateValue.On : PowerStateValue.Off,
                    MasterVolume = NormalizeVolume(xml.Descendants("MasterVolume").FirstOrDefault()?.Value?.Trim().ToFloat()),
                    Mute = xml.Descendants("Mute").FirstOrDefault()?.Value?.Trim().ToLower() == "on"
                };
            }
        }

        public float? NormalizeVolume(float? volume)
        {
            return volume == null ? null : volume + 80.0f;
        }
    }
}