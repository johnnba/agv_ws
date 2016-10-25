using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGV_WS.src.protocol
{
    public static class Protocol
    {
        public const int ID_HEARTBEAT_NTC = 0x00;
        public const int ID_SENDPLAN_NTC = 0xFF;
        public const int ID_GUIDE_START_NTC = 0x01;
        public const int ID_GUIDE_FINISH_NTC = 0x02;
        public const int ID_STATION_INFO_NTC = 1019;
        public const int ID_USONIC_INFO_NTC = 1020;
        public const int ID_SPEED_INFO_NTC = 1021;
        public const int ID_LOAD_INFO_NTC = 1022;
        public const int ID_VOLTAGE_INFO_NTC = 1023;
        public const int ID_WIFI_INFO_NTC = 1024;
    }
}
