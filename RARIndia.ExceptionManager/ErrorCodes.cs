using System;

namespace Coditech.ExceptionManager
{
    public static class ErrorCodes
    {
        // Error codes for general exception
        public const Int32 NullModel = 1;
        public const Int32 AlreadyExist = 2;
        public const Int32 AtLeastSelectOne = 3;
        public const Int32 AssociationDeleteError = 4;
        public const Int32 InvalidData = 5;
        public const Int32 NotFound = 6;
        public const Int32 NotPermitted = 7;
        public const Int32 IdLessThanOne = 8;
        public const Int32 ContactAdministrator =9;
    }
}
