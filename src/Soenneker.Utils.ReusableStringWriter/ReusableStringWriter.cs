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

    public ReusableStringWriter() : base(new StringBuilder(256))
    {
        _sb = GetStringBuilder();
    }

    /// <summary>
    /// Clears the logical contents while retaining the internal buffer for reuse. Existing characters are not securely erased.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reset() => _sb.Clear();

    /// <summary>
    /// Allocates and returns the accumulated string while leaving the internal buffer and logical contents intact.
    /// </summary>
    /// <returns>The current contents of the internal buffer as a <see cref="string"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Finish() => _sb.ToString();
}
