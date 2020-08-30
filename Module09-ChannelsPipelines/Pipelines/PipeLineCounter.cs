using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pipelines
{
    public class PipeLineCounter
    {
        public async Task<int> CountLines(Uri uri)
        {
            using var client = new HttpClient();
            await using var stream = await client.GetStreamAsync(uri);

            int counter = 0;
            var reader = PipeReader.Create(stream);
            while (true)
            {
                var res = await reader.ReadAsync();
                var buffer = res.Buffer;
                SequencePosition? pos;

                do
                {
                    pos = buffer.PositionOf((byte) '\n');
                    if (pos != null)
                    {
                        counter++;
                        buffer = buffer.Slice(buffer.GetPosition(1, pos.Value));
                    }
                } while (pos != null);

                reader.AdvanceTo(buffer.End, buffer.End);

                if (res.IsCompleted) break;
            }

            await reader.CompleteAsync();
            return counter;
        }
    }
}
