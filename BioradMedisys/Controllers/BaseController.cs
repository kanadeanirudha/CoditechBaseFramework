using Microsoft.Web.Mvc;

using Newtonsoft.Json;

using Coditech.Model;
using Coditech.Model.Model;
using Coditech.Utilities.Constant;
using Coditech.Utilities.Helper;

using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Coditech.Controllers
{
    public class BaseController : Controller
    {
        #region Notification
        /// <summary>
        /// Set notification message.
        /// </summary>
        /// <param name="notificationMessage">Message to set.</param>
        protected void SetNotificationMessage(string notificationMessage)
            => TempData[CoditechConstant.Notifications] = notificationMessage;

        /// <summary>
        /// Get the success notification message.
        /// </summary>
        /// <param name="successMessage">success message.</param>
        /// <returns>Returns serialize MessageBoxModel with notification set to success.</returns>
        protected string GetSuccessNotificationMessage(string successMessage)
            => GenerateNotificationMessages(successMessage, NotificationType.success);

        /// <summary>
        /// Get the error notification message.
        /// </summary>
        /// <param name="errorMessage">error message.</param>
        /// <returns>Returns serialize MessageBoxModel with notification set to error.</returns>
        protected string GetErrorNotificationMessage(string errorMessage)
            => GenerateNotificationMessages(errorMessage, NotificationType.error);

        /// <summary>
        /// Get the information notification message.
        /// </summary>
        /// <param name="infoMessage">information message.</param>
        /// <returns>Returns serialize MessageBoxModel with notification set to info.</returns>
        protected string GetInfoNotificationMessage(string infoMessage)
            => GenerateNotificationMessages(infoMessage, NotificationType.info);

        /// <summary>
        /// To show Notification message 
        /// </summary>
        /// <param name="message">string message to show on page</param>
        /// <param name="type">enum type of message</param>
        /// <param name="isFadeOut">bool isFadeOut true/false</param>
        /// <param name="fadeOutMilliSeconds">int fadeOutMilliSeconds</param>
        /// <returns>string Json format of message box</returns>
        protected string GenerateNotificationMessages(string message, NotificationType type)
        {
            MessageBoxModel msgObj = new MessageBoxModel();
            msgObj.Message = message;
            msgObj.Type = type.ToString();
            msgObj.IsFadeOut = CheckIsFadeOut();
            return JsonConvert.SerializeObject(msgObj);
        }
        /// <summary>
        /// To get IsFadeOut status from web config file, 
        /// if NotificationMessagesIsFadeOut key not found in config then it will returns false 
        /// </summary>
        /// <returns>return true/false</returns>
        private bool CheckIsFadeOut()
        {
            bool isFadeOut = false;
            if (!string.IsNullOrEmpty(CoditechSetting.NotificationMessagesIsFadeOut))
            {
                isFadeOut = Convert.ToBoolean(CoditechSetting.NotificationMessagesIsFadeOut);
            }
            else
            {
                //To do : need to log this in log file after common log functionality is ready.
                //ZnodeLogging.LogMessage(ZnodeResources.ConfigKeyNotificationMessagesIsFadeOutMissing);
            }
            return isFadeOut;
        }
        #endregion

        #region ActionView
        public virtual ActionResult ActionView()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            return View();
        }

        public virtual ActionResult ActionView(IView view)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            return View(view);
        }

        public virtual ActionResult ActionView(object model)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            return View(model);
        }

        public virtual ActionResult ActionView(string viewName)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView(viewName);
            }
            return View(viewName);
        }

        public virtual ActionResult ActionView(IView view, object model)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            return View(view, model);
        }

        public virtual ActionResult ActionView(string viewName, object model)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView(viewName, model);
            }
            return View(viewName, model);
        }

        public virtual ActionResult ActionView(string viewName, string masterName)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            return View(viewName, masterName);
        }

        public virtual ActionResult ActionView(string viewName, string masterName, object model)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            return View(viewName, masterName, model);
        }
        #endregion
        /// <summary>
        /// Strongly Type Redirect To Action
        /// </summary>
        /// <typeparam name="TController">Controller Name</typeparam>
        /// <param name="action">Action Name</param>
        /// <returns>Strongly Type Action Result</returns>
        /// <example>
        /// If your controller name is "Dashboard" and Action Mehtod name is "Dashboard"
        /// Then we can redirect to action method using strongly type as
        /// <code>
        /// RedirectToAction<DashboardController>(o => o.Index())
        /// </code>
        /// </example>
        protected ActionResult RedirectToAction<TController>(
                Expression<Action<TController>> action)
                where TController : Controller
        {
            return ControllerExtensions.RedirectToAction(this, action);
        }

        protected DataTableModel CreateActionDataTable(string centreCode = null, int selectedDepartmentID = 0)
        {
            return new DataTableModel()
            {
                SortByColumn = SortKeys.CreatedDate,
                SortBy = CoditechConstant.DESCKey,
                SelectedCentreCode = centreCode,
                SelectedDepartmentID = selectedDepartmentID
            };
        }

        protected DataTableModel UpdateActionDataTable(string centreCode = null, int selectedDepartmentID = 0)
        {
            return new DataTableModel()
            {
                SortByColumn = SortKeys.ModifiedDate,
                SortBy = CoditechConstant.DESCKey,
                SelectedCentreCode = centreCode,
                SelectedDepartmentID = selectedDepartmentID
            };
        }
    }
}