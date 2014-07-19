using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RunProcessAsTask;

namespace Shields.Graphviz
{
    public class GraphvizService : IGraphvizService
    {
        private readonly string graphvizBin;

        public GraphvizService(string graphvizBin)
        {
            if (graphvizBin == null)
            {
                throw new ArgumentNullException("graphvizBin");
            }

            this.graphvizBin = graphvizBin;
        }

        private static string Escape(string s)
        {
            const string quote = "\"";
            const string backslash = "\\";
            return quote + s.Replace(quote, backslash + quote) + quote;
        }

        public async Task DotAsync(string inputPath, string outputPath, GraphvizOutputFormat outputFormat, CancellationToken cancellationToken)
        {
            var fileName = Path.Combine(graphvizBin, "dot.exe");
            var arguments = string.Format("-T{0} {1} -o {2}", outputFormat.ToString(), Escape(inputPath), Escape(outputPath));
            var startInfo = new ProcessStartInfo(fileName, arguments);
            var results = await ProcessEx.RunAsync(startInfo, cancellationToken);
        }
    }
}
