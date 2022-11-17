using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Standard.DynMvp.Base
{
    public class GrabFailException : ApplicationException
    {
        string defaultMessage = StringManager.GetString("GrabFailException", "Grab Fail");

        public GrabFailException()
        {
            LogHelper.Error(LoggerType.Error, defaultMessage);
        }
        public GrabFailException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
        public GrabFailException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
    }

    // Vision Object의 유효성에 문제가 있을 때 발생하는 Exception
    public class InvalidObjectException : ApplicationException
    {
        string defaultMessage = StringManager.GetString("InvalidObjectException", "Invalid Object");

        public InvalidObjectException()
            : base()
        {
            LogHelper.Error(LoggerType.Error, defaultMessage);
        }
        public InvalidObjectException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
        public InvalidObjectException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class AllocFailedException : ApplicationException
    {
        string defaultMessage = StringManager.GetString("AllocFailedException", "Allocation Failed");

        public AllocFailedException()
            : base()
        {
            LogHelper.Error(LoggerType.Error, defaultMessage);
        }
        public AllocFailedException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
        public AllocFailedException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class InvalidModelNameException : ApplicationException
    {
        string defaultMessage = StringManager.GetString("InvalidModelNameException", "Invalid Model Name");

        public InvalidModelNameException()
            : base()
        {
            LogHelper.Error(LoggerType.Error, defaultMessage);
        }
        public InvalidModelNameException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
        public InvalidModelNameException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class InvalidSourceException : ApplicationException
    {
        string defaultMessage = StringManager.GetString("InvalidSourceException", "Invalid Source");

        public InvalidSourceException()
            : base()
        {
            LogHelper.Error(LoggerType.Error, defaultMessage);
        }
        public InvalidSourceException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
        public InvalidSourceException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class InvalidTargetException : ApplicationException
    {
        string defaultMessage = StringManager.GetString("InvalidTargetException", "Invalid Source");

        public InvalidTargetException()
            : base()
        {
            LogHelper.Error(LoggerType.Error, defaultMessage);
        }
        public InvalidTargetException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
        public InvalidTargetException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class InvalidDataException : ApplicationException
    {
        string defaultMessage = StringManager.GetString("InvalidDataException", "Data format is invalid");

        public InvalidDataException()
        {
            LogHelper.Error(LoggerType.Error, defaultMessage);
        }
        public InvalidDataException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
        public InvalidDataException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class TooManyItemsException : ApplicationException
    {
        string defaultMessage = StringManager.GetString("TooManyItemsException", "Excess number of items");

        public TooManyItemsException()
        {
            LogHelper.Error(LoggerType.Error, defaultMessage);
        }
        public TooManyItemsException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
        public TooManyItemsException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class InvalidImageFormatException : ApplicationException
    {
        string defaultMessage = StringManager.GetString("InvalidImageFormatException", "Invalid Image");

        public InvalidImageFormatException()
        {
            LogHelper.Error(LoggerType.Error, defaultMessage);
        }
        public InvalidImageFormatException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
        public InvalidImageFormatException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class InvalidTypeException : ApplicationException
    {
        string defaultMessage = StringManager.GetString("InvalidTypeException", "Invalid Type");

        public InvalidTypeException()
        {
            LogHelper.Error(LoggerType.Error, defaultMessage);
        }
        public InvalidTypeException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
        public InvalidTypeException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class DepthScannerInitializeFailException : ApplicationException
    {
        string defaultMessage = StringManager.GetString("DepthScannerInitializeFailException", "Depth Scanner Initialization is Failed");

        public DepthScannerInitializeFailException()
        {
            LogHelper.Error(LoggerType.Error, defaultMessage);
        }
        public DepthScannerInitializeFailException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
        public DepthScannerInitializeFailException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class CameraInitializeFailException : ApplicationException
    {
        string defaultMessage = "Camera Initialization is Failed";

        public CameraInitializeFailException()
        {
            LogHelper.Error(LoggerType.Error, defaultMessage);
        }
        public CameraInitializeFailException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
        public CameraInitializeFailException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class InvalidResourceException : ApplicationException
    {
        public InvalidResourceException()
            :base("Camera Initialization is Failed")
        {
            LogHelper.Error(LoggerType.Error, this.Message);
        }
        public InvalidResourceException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, message);
        }
        public InvalidResourceException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, message);
        }
    }
}
