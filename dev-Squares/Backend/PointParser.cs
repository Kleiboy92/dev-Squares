using System;
using System.Collections.Generic;

namespace dev_Squares.Backend
{

    public static class PointParser
    {
        public static IEnumerable<Point> ParseFile(string file, Action<ParsingInfo> infoOutput)
        {
            var lines = file.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            var importedPointCount = 0;
            var lineNumber = 0;
            foreach (var line in lines)
            {
                Point? generatedPoint = null;
                try
                {
                    generatedPoint = Point.Parse(line.Trim());
                }
                catch (Exception e)
                {
                    infoOutput(new ParsingInfo(lineNumber, e.Message, ParserType.danger));
                }

                if (generatedPoint.HasValue)
                {
                    importedPointCount++;
                    yield return generatedPoint.Value;
                }

                lineNumber++;
            }

            infoOutput(new ParsingInfo(lineNumber, "file parsed, points parsed: " + importedPointCount, ParserType.success));
        }
    }
}