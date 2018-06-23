using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CustomFormatter.Formatters.Internal;
using Microsoft.AspNetCore.Mvc.Formatters;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace CustomFormatter.Formatters.Xls
{
    public class XlsxOutputFormatter : OutputFormatter
    {
        public XlsxOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationXExel);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = context.HttpContext.Response;

            var package = new ExcelPackage();

            var type = context.Object.GetType().GetGenericArguments().Length > 0
                ? context.Object.GetType().GenericTypeArguments.First()
                : context.Object.GetType();

            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            if (typeof(IEnumerable).IsAssignableFrom(context.ObjectType) &&
                context.ObjectType.IsConstructedGenericType)
            {
                WriteObjectList(worksheet, context.Object, type);
            }
            else
            {
                WriteObject(worksheet, context.Object, type);
            }

            var bytes = package.GetAsByteArray();

            await response.Body.WriteAsync(bytes, 0, bytes.Length);
            await package.Stream.FlushAsync();
        }

        private void WriteObject(ExcelWorksheet worksheet, object contextObject, Type type)
        {
            var properties = type.GetProperties();

            int i = 1;

            foreach (var property in properties)
            {
                worksheet.Cells[i, 1].Value = property.Name;
                worksheet.Cells[i, 2].Value = property.GetValue(contextObject).ToString();

                i++;
            }

        }

        private void WriteObjectList(ExcelWorksheet worksheet, object contextObject, Type type)
        {
            var memberInfo = type.GetProperties().Select(pi => (MemberInfo) pi).ToArray();

            worksheet.Cells.LoadFromCollection(
                    (IEnumerable<object>) contextObject
                    , true
                    , TableStyles.None
                    , BindingFlags.Public | BindingFlags.Instance
                    , memberInfo);
        }
    }
}
