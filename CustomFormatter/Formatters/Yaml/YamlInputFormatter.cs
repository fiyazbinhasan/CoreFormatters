using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Threading.Tasks;
using System.Text;
using YamlDotNet.Serialization;
using CustomFormatter.Formatters.Internal;

namespace CustomFormatter.Formatters.Yaml
{
    public class YamlInputFormatter : TextInputFormatter
    {
        private readonly Deserializer _deserializer;

        public YamlInputFormatter(Deserializer deserializer)
        {
            _deserializer = deserializer;

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationYaml);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.TextYaml);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            var request = context.HttpContext.Request;

            using (var streamReader = context.ReaderFactory(request.Body, encoding))
            {
                var type = context.ModelType;

                try
                {
                    var content = await streamReader.ReadToEndAsync();
                    var model = _deserializer.Deserialize(content, type);
                    return await InputFormatterResult.SuccessAsync(model);
                }
                catch (Exception)
                {
                    return await InputFormatterResult.FailureAsync();
                }
            }
        }
    }
}
