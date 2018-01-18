using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Socona.ImVehicle.Core;

namespace Socona.ImVehicle.Infrastructure.Authorization
{
  
        public static class VehicleOperations
        {
            public static OperationAuthorizationRequirement Create =
              new OperationAuthorizationRequirement { Name = Constants.CreateOperationName };
            public static OperationAuthorizationRequirement Read =
              new OperationAuthorizationRequirement { Name = Constants.ReadOperationName };
            public static OperationAuthorizationRequirement Update =
              new OperationAuthorizationRequirement { Name = Constants.UpdateOperationName };
            public static OperationAuthorizationRequirement Delete =
              new OperationAuthorizationRequirement { Name = Constants.DeleteOperationName };
            public static OperationAuthorizationRequirement Approve =
              new OperationAuthorizationRequirement { Name = Constants.ApproveOperationName };
            public static OperationAuthorizationRequirement Reject =
              new OperationAuthorizationRequirement { Name = Constants.RejectOperationName };
        public static OperationAuthorizationRequirement UploadUserFile =
             new OperationAuthorizationRequirement { Name = Constants.UploadUserFileOperationName };
    }

       
    
}
