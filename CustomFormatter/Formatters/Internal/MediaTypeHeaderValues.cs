using Microsoft.Net.Http.Headers;

namespace CustomFormatter.Formatters.Internal
{
    internal class MediaTypeHeaderValues
    {
        public static readonly MediaTypeHeaderValue ApplicationYaml
            = MediaTypeHeaderValue.Parse("application/x-yaml").CopyAsReadOnly();

        public static readonly MediaTypeHeaderValue TextYaml
            = MediaTypeHeaderValue.Parse("text/yaml").CopyAsReadOnly();

        public static readonly MediaTypeHeaderValue ApplicationPdf
            = MediaTypeHeaderValue.Parse("application/pdf").CopyAsReadOnly();


        public static readonly MediaTypeHeaderValue ApplicationVendorMsExcel
            = MediaTypeHeaderValue.Parse("application/vnd.ms-excel").CopyAsReadOnly();


        public static readonly MediaTypeHeaderValue ApplicationXMsExcel
            = MediaTypeHeaderValue.Parse("application/x-msexcel").CopyAsReadOnly();


        public static readonly MediaTypeHeaderValue ApplicationMsExcel
            = MediaTypeHeaderValue.Parse("application/ms-excel").CopyAsReadOnly();


        public static readonly MediaTypeHeaderValue ApplicationXExel
            = MediaTypeHeaderValue.Parse("application/x-excel").CopyAsReadOnly();


        public static readonly MediaTypeHeaderValue ApplicationVendorSpredSheet
            = MediaTypeHeaderValue.Parse("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet").CopyAsReadOnly();

    }
}
