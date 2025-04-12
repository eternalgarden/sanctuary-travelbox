/* 
      |\      _,,,---,,_
ZZZzz /,`.-'`'    -.  ;-;;,_
     |,4-  ) )-,_. ,\ (  `'-'
    '---''(_/--'  `-'\_)
*/

using System;
using System.Text;

namespace TravelBox.Extensions
{
    [Flags]
    public enum TimeSpanFormatting
    {
        Default = 1 << 0,
        Hours = 1 << 1,
        Minutes = 1 << 2,
        Seconds = 1 << 3,
        Named = 1 << 4,
        FilledIn = 1 << 5,
        // Dense = 1 << 1,
        Separated = 1 << 6,
        ColonSeparated = 1 << 7,
        NewlineSeparated = 1 << 8,
        ZeroHourRemoved = 1 << 9,
    }

    public static class TimeSpanExtensions
    {
        // -------------

        private const TimeSpanFormatting DefaultTimeSpanFormatting =
            TimeSpanFormatting.Hours |
            TimeSpanFormatting.Minutes |
            TimeSpanFormatting.Named |
            TimeSpanFormatting.Separated;

        public static string GetPrintableFormat(this TimeSpan timeSpan, TimeSpanFormatting formatting = TimeSpanFormatting.Default)
        {

            if (formatting == TimeSpanFormatting.Default)
            {
                formatting = DefaultTimeSpanFormatting;
            }

            StringBuilder outputString = new StringBuilder("xsysz", 20);

            int iHours = timeSpan.Hours;
            int iMinutes = timeSpan.Minutes;
            int iSeconds = timeSpan.Seconds;

            if (iHours == 0 && formatting.HasFlag(TimeSpanFormatting.ZeroHourRemoved))
            {
                outputString.Replace("xs", "\0");
            }

            string hours, minutes, seconds;

            if (formatting.HasFlag(TimeSpanFormatting.FilledIn))
            {
                hours = iHours < 10 ? $"0{iHours}" : iHours.ToString();
                minutes = iMinutes < 10 ? $"0{iMinutes}" : iMinutes.ToString();
                seconds = iSeconds < 10 ? $"0{iSeconds}" : iSeconds.ToString();
            }
            else
            {
                hours = iHours.ToString();
                minutes = iMinutes.ToString();
                seconds = iSeconds.ToString();
            }

            if (formatting.HasFlag(TimeSpanFormatting.Separated))
            {
                if (formatting.HasFlag(TimeSpanFormatting.ColonSeparated))
                {
                    outputString.Replace((string)"s", ":");
                }
                else if (formatting.HasFlag(TimeSpanFormatting.NewlineSeparated))
                {
                    outputString.Replace((string)"s", "\n");
                }
                else
                {
                    outputString.Replace((string)"s", " ");
                }
            }
            else
            {
                outputString.Replace('s', '\0'); // replace with null character
            }

            if (formatting.HasFlag(TimeSpanFormatting.Named))
            {
                outputString.Replace((string)"x", "xh");
                outputString.Replace((string)"y", "ym");
                outputString.Replace((string)"z", "zs");
            }

            if (formatting.HasFlag(TimeSpanFormatting.Hours))
            {
                outputString.Replace("x", hours);
            }
            else
            {
                outputString.Replace("x", "\0");
            }

            if (formatting.HasFlag(TimeSpanFormatting.Minutes))
            {
                outputString.Replace("y", minutes);
            }
            else
            {
                outputString.Replace("y", "\0");
            }

            if (formatting.HasFlag(TimeSpanFormatting.Seconds))
            {
                outputString.Replace("z", seconds);
            }
            else
            {
                outputString.Replace("z", "\0");
            }

            return outputString.ToString();
        }

        // -------------
    }
}
/* maria aurelia at 06 April 2022 🌊 */
/* dreamy guardian ASCII kitty by Felix Lee, found at asciiart.eu 🐱‍👤 */