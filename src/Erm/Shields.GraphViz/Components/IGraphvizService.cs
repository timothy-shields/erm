using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shields.Graphviz
{
    /// <summary>
    /// Provides access to Graphviz applications.
    /// </summary>
    public interface IGraphvizService
    {
        /// <summary>
        /// Runs Graphviz dot.
        /// </summary>
        /// <param name="inputPath">The input path. This should be an existing dot file.</param>
        /// <param name="outputPath">The output path. This will be a render of the input file with the specified output format.</param>
        /// <param name="outputFormat">The output format.</param>
        /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
        /// <returns>The task that signals completion.</returns>
        Task DotAsync(string inputPath, string outputPath, GraphvizOutputFormat outputFormat, CancellationToken cancellationToken);
    }
}
