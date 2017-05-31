﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HA4IoT.Extensions
{
    public class AbsenceService
    {
        private IDayOffProvider _dayOffProvider;
        private List<DayDescription> _absences;

        public AbsenceService(IDayOffProvider dayOffProvider)
        {
            _dayOffProvider = dayOffProvider;
        }

        public AbsenceService IncludeDay()
        {
            return this;
        }

        public AbsenceService IncludeWorkingDays()
        {
            return this;
        }

        public AbsenceService ExcludeDay()
        {
            return this;
        }

        //public HourActivity WorkingDaysAbsenceHours;
    }

    public struct DayDescription
    {
        public DateTime Date { get; set; }
        public HourActivity HourActivity { get; set; }
        public DayCyclic Cyclic { get; set; }
    }

    public enum DayCyclic
    {
        None,
        WorkingDay,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    public class HourActivity
    {
        public byte[] _hours { get; private set; } = new byte[24];
    }

    public class PolishDayOffProvider : IDayOffProvider
    {
        public bool IsDayOff(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday) return true;
            if (date.DayOfWeek == DayOfWeek.Sunday) return true;
            if (date.Month == 01 && date.Day == 01) return true; // Nowy Rok
            if (date.Month == 01 && date.Day == 01 && (date.Year >= 1952 && date.Year <= 1960)) return true; // Trzech Króli if (date.Month == 05 && date.Day == 01) return true; // 1 maja if (date.Month == 05 && date.Day == 03 && (date.Year >= 1918 && date.Year <= 1950 || date.Year >= 1990)) return true; // 3 maja
            if (date.Month == 07 && date.Day == 22 && (date.Year >= 1945 && date.Year <= 1989)) return true; // Narodowe Święto Odrodzenia Polski
            if (date.Month == 08 && date.Day == 15 && (date.Year <= 1960 || date.Year >= 1989)) return true; // Wniebowzięcie Najświętszej Marii Panny, Święto Wojska Polskiego (rocznica “cudu nad Wisłą”)
            if (date.Month == 11 && date.Day == 01) return true; // Dzień Wszystkich Świętych
            if (date.Month == 11 && date.Day == 11 && (date.Year == 1937 || date.Year == 1938 || date.Year >= 1990)) return true; // Dzień Niepodległości 
            if (date.Month == 12 && date.Day == 25) return true; // Boże Narodzenie
            if (date.Month == 12 && date.Day == 26) return true; // Boże Narodzenie
            int a = date.Year % 19;
            int b = date.Year % 4;
            int c = date.Year % 7;
            int d = (a * 19 + 24) % 30;
            int e = (2 * b + 4 * c + 6 * d + 5) % 7;
            if (d == 29 && e == 6) d -= 7;
            if (d == 28 && e == 6 && a > 10) d -= 7;
            DateTime Easter = new DateTime(date.Year, 3, 22).AddDays(d + e);
            if (date.AddDays(-1) == Easter) return true; // Wielkanoc (poniedziałek)
            if (date.AddDays(-60) == Easter) return true; // Boże Ciało
            return false;
        }
    }

    public interface IDayOffProvider
    {
        bool IsDayOff(DateTime date);
    }


}
