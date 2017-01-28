using CustomFormatter.Formatters.Internal;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CustomFormatter.Formatters.Pdf
{
    public class PdfOutputFormatter : OutputFormatter
    {
        private readonly PdfPCell _cell;
        private readonly Font _font;

        private PdfPTable _table;
        private Document _document;

        public PdfOutputFormatter()
        {
            /* Make this fields configurable from startup if you will */
            _font = FontFactory.GetFont("Arial", 10, Font.NORMAL);
            _cell = new PdfPCell()
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_CENTER
            };

            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationPdf);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = context.HttpContext.Response;

            using (var stream = new MemoryStream())
            {
                var type = context.Object.GetType().GetGenericArguments().Length > 0 ? context.Object.GetType().GenericTypeArguments.First() : context.Object.GetType();

                _document = new Document(PageSize.A4, 20, 20, 20, 20);
                _table = new PdfPTable(type.GetProperties().Length);

                PdfWriter.GetInstance(_document, stream);

                _document.Open();

                if (typeof(IEnumerable).IsAssignableFrom(context.ObjectType) &&
                    context.ObjectType.IsConstructedGenericType)
                {

                    WriteObject(_document, context.Object, type, true);
                }
                else
                {
                    WriteObject(_document, context.Object, type, false);
                }

                _document.Close();

                await response.Body.WriteAsync(stream.ToArray(), 0, stream.ToArray().Length);
                await stream.FlushAsync();
            }
        }

        private void WriteObject(Document document, object contextObject, Type type, bool isGenericEnumerable)
        {
            foreach (var property in type.GetProperties())
            {
                _cell.Phrase = new Phrase(property.Name, _font);

                _table.AddCell(_cell);
            }

            _table.CompleteRow();

            if (isGenericEnumerable)
            {
                foreach (var obj in (IEnumerable<object>)contextObject)
                {
                    var values = type.GetProperties().Select(p => p.GetValue(obj));

                    AddValuesInCell(values);

                    _table.CompleteRow();
                }
            }
            else
            {
                var values = contextObject.GetType().GetProperties().Select(p => p.GetValue(contextObject));

                AddValuesInCell(values);

                _table.CompleteRow();
            }

            document.Add(_table);
        }

        private void AddValuesInCell(IEnumerable<object> values)
        {
            foreach (var value in values)
            {
                _cell.Phrase = new Phrase(value.ToString(), _font);
                _table.AddCell(_cell);
            }
        }
    }
}
