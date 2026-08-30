[![](https://img.shields.io/nuget/v/soenneker.utils.reusablestringwriter.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.utils.reusablestringwriter/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.utils.reusablestringwriter/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.utils.reusablestringwriter/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.utils.reusablestringwriter.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.utils.reusablestringwriter/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.utils.reusablestringwriter/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.utils.reusablestringwriter/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Utils.ReusableStringWriter
A high-performance, reusable StringWriter that avoids unnecessary allocations by clearing and reusing its internal StringBuilder instance.

## Installation

```bash
dotnet add package Soenneker.Utils.ReusableStringWriter
```

## Quick start

```csharp
using Soenneker.Utils.ReusableStringWriter;
```

Create one writer for a bounded synchronous scope and reset it between payloads:

```csharp
using var writer = new ReusableStringWriter();

foreach (Order order in orders)
{
    writer.Write("Order: ");
    writer.Write(order.Id);

    string text = writer.Finish();
    await destination.WriteAsync(text, cancellationToken);

    writer.Reset();
}
```

`Finish` calls `StringBuilder.ToString()`: it allocates an independent string and leaves all
characters in the writer. Call `Reset` explicitly before writing the next value. Reset changes the
logical length to zero but retains the builder's capacity, which is where the allocation savings
come from.

The writer starts with capacity 256 and grows as needed. A single unusually large payload can
therefore leave a large retained buffer for the rest of that writer's lifetime. Choose a bounded
reuse lifetime when payload sizes vary significantly.

## Lifecycle and inherited behavior

- Reuse the writer before disposing it. Dispose once when the whole reuse scope ends; disposal is not a reset operation.
- The type is not thread-safe. Do not write, finish, or reset it concurrently.
- `Finish` can be called repeatedly, but every call creates another string.
- `Reset` does not securely erase the underlying character array. Do not use the writer as secure storage for secrets.
- `Encoding`, `NewLine`, format-provider behavior, and the synchronous/async `TextWriter` APIs come from `StringWriter`. In particular, consumers such as XML serializers can observe `StringWriter.Encoding` when emitting an encoding declaration.

There is no dependency-injection registration; instantiate the writer where its ownership and
reuse boundary are clear.
