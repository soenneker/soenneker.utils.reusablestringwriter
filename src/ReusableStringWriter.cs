using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Soenneker.Utils.ReusableStringWriter;

/// <summary>
/// A high-performance, reusable <see cref="StringWriter"/> that avoids unnecessary allocations
/// by clearing and reusing its internal <see cref="StringBuilder"/> instance.
/// </summary>
public sealed class ReusableStringWriter : StringWriter
{
    private readonly StringBuilder _sb;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReusableStringWriter"/> class
    /// with an initial buffer size of 256 characters.
    /// </summary>
    public ReusableStringWriter() : base(new StringBuilder(256))
    {
        _sb = GetStringBuilder();
    }

    /// <summary>
    /// Clears the internal buffer to prepare the writer for reuse.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reset() => _sb.Clear();

    /// <summary>
    /// Returns the accumulated string and leaves the internal buffer intact.
    /// </summary>
    /// <returns>The current contents of the internal buffer as a <see cref="string"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Finish() => _sb.ToString();
}