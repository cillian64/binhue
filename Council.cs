using System;
using System.Threading.Tasks;
using Q42.HueApi.ColorConverters;  // for RGBColor

namespace binhue
{
    // This describes a particular waste bin
    struct Bin
    {
        public string contents;  // Human readable, for display only
        public RGBColor color;
    }

    // This is an abstraction of council waste collection services
    // The basic interface is a function which tells us what bin is
    // being collected tomorrow, if any.
    abstract class Council
    {
        public class ParseException : Exception
        {
            public ParseException(string message) : base(message) {}
        };

        abstract public Task<Bin> getTomorrowBin();
    }
}
