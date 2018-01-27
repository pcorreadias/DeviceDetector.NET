using System;
using System.Collections.Generic;
using DeviceDetector.NET.Class.Client;
using DeviceDetector.NET.Results;
using DeviceDetector.NET.Results.Client;

namespace DeviceDetector.NET.Parser.Client.Browser
{
    public class EngineParser : ClientParserAbstract<List<BrowserEngine>, ClientMatchResult>
    {
        /// <summary>
        /// Known browser engines mapped to their internal short codes
        /// </summary>
        protected static string[] AvailableEngines = {
            "WebKit",
            "Blink",
            "Trident",
            "Text-based",
            "Dillo",
            "iCab",
            "Elektra",
            "Presto",
            "Gecko",
            "KHTML",
            "NetFront",
            "Edge"
        };

        public EngineParser()
        {
            FixtureFile = "regexes/client/browser_engine.yml";
            ParserName = "browserengine";
            regexList = GetRegexes();
        }

        /// <summary>
        /// Returns list of all available browser engines
        /// </summary>
        /// <returns></returns>
        public static string[] GetAvailableEngines()
        {
            return AvailableEngines;
        }

        /// <summary>
        /// Returns list of all available browser engines
        /// </summary>
        /// <returns></returns>
        public override ParseResult<ClientMatchResult> Parse()
        {
            var result = new ParseResult<ClientMatchResult>();
            BrowserEngine localEngine = null;
            string[] localMatches = null;
            foreach (var engine in regexList)
            {
                var matches = MatchUserAgent(engine.Regex);
                if (matches.Length <= 0) continue;
                localEngine = engine;
                localMatches = matches;
                break;
            }
            if (localMatches == null) return result;
            var name = BuildByMatch(localEngine.Name, localMatches);
            foreach (var engineName in AvailableEngines)
            {
                if (name.ToLower().Equals(engineName.ToLower()))
                {
                    result.Add(new ClientMatchResult{Name = name});
                }
            }
            return result;
            // This Exception should never be thrown. If so a defined browser name is missing in $availableEngines
            throw new Exception("Detected browser engine was not found in AvailableEngines");

        }
    }
}