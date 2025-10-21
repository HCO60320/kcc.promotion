namespace KCC.PROMOTION.Model
{
    using System;
    using System.Data;

    /// <summary>
    /// Public Class Json Response
    /// </summary>
    public class JsonResponse
    {
        /// <summary>
        /// Gets or sets Response Status
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets Response Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the Response DataTable
        /// </summary>
        public DataTable Data { get; set; }
    }
}