using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCompToHtml
{
    //Copied from aspnetcore repo
    public readonly struct ComponentRenderedText
    {
        public ComponentRenderedText(int componentId, IEnumerable<string> tokens)
        {
            ComponentId = componentId;
            Tokens = tokens;
        }

        public int ComponentId { get; }

        public IEnumerable<string> Tokens { get; }
    }
}
