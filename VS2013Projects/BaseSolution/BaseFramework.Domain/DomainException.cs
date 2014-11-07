using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseFramework.Domain
{
    /// <summary>
    /// Bussiness exceptions occured within the domain
    /// </summary>
    public class DomainException : ApplicationException
    {
        /// <summary>
        /// Exception code
        /// </summary>
        public string Code { get; set; }


        public object[] Args { get; set; }

        /// <summary>
        /// User data, for keeping key-value information based on system basic type
        /// </summary>
        private IDictionary<string, object> _data = new Dictionary<string, object>();
        public new IDictionary<string, object> Data
        {
            get { return _data; }
            set { _data = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/>  class.
        /// </summary>
        public DomainException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public DomainException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class with
        /// a specified code and error message.
        /// 
        /// Note:if the code is just a format, then initialize with DomainException(string format, params object[] args)
        /// </summary>
        /// <param name="code">exception code</param>
        /// <param name="message">message</param>
        public DomainException(string code, string message)
            : this(IsFormat(code) ? string.Format(code, message) : message)
        {
            if (!IsFormat(code))
                Code = code;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class with
        /// a specified error message and user data.
        /// </summary>
        /// <param name="data">User data</param>
        /// <param name="message">A message that describes the error.</param>
        public DomainException(string message, IDictionary<string, object> data)
            : base(message)
        {
            Data = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class with
        /// a specified code,error message and user data.
        /// </summary>
        /// <param name="code">exception code</param>
        /// <param name="message">message</param>
        /// <param name="data">user data</param>
        public DomainException(string code, string message, IDictionary<string, object> data)
            : this(message, data)
        {
            Code = code;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class with
        /// a specified message format and arguments.
        /// </summary>
        /// <param name="format">message format</param>
        /// <param name="args">message arguments</param>
        public DomainException(string format, params object[] args)
            : this(string.Format(format, args))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class with
        /// a specified code,message format and arguments.
        /// 
        /// Note:if the code is just a format, then initialize with DomainException(string format, params object[] args)
        /// </summary>
        /// <param name="code">exception code</param>
        /// <param name="format">message format</param>
        /// <param name="args">message format arguments</param>
        public DomainException(string code, string format, params object[] args)
            : this(IsFormat(code) ?
                string.Format(code, (new object[] { format }.Concat(args)).ToArray())
                : string.Format(format, args))
        {
            if (!IsFormat(code))
                Code = code;

            if (args != null && args.Length > 0)
                Args = args.ToArray();
        }

        /// <summary>
        /// Indicate if the text is the string format or not 
        /// </summary>
        /// <param name="text">text</param>
        /// <returns>if the text contains '{0}' return true; otherwise return false</returns>
        static bool IsFormat(string text)
        {
            return !string.IsNullOrWhiteSpace(text) && text.Contains("{0}");
        }

    }
}
