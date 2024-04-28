using System;
using System.Collections.Generic;

namespace Coditech.ExceptionManager
{
    public class CoditechException : Exception
    {
        public int? ErrorCode { get; private set; }
        public string ErrorMessage { get; private set; }
        public Dictionary<string, string> ErrorDetailList { get; private set; }

        /// <summary>
        /// Creates a new RARIndiaException.
        /// </summary>
        public CoditechException()
            : base("RARIndia Exception")
        {
        }

        /// <summary>
        /// Creates a new RARIndiaException.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="errorMessage">The error message.</param>
        public CoditechException(int? errorCode, string errorMessage)
            : base(errorMessage ?? "RARIndiaException with errorCode" + errorCode.GetValueOrDefault().ToString())
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Creates a new RARIndiaException.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorDetailList">The error details.</param>
        public CoditechException(int? errorCode, string errorMessage, Dictionary<string, string> errorDetailList)
            : base(errorMessage ?? "RARIndiaException with errorCode" + errorCode.GetValueOrDefault().ToString())
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            ErrorDetailList = errorDetailList;
        }
    }
}
