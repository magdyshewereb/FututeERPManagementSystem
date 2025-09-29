using ERPManagement.Application.Shared.Constants;

namespace ERPManagement.Application.Shared.Services
{
    public static class MessageDataSource
    {
        public static Dictionary<string, Message> LoadMessages()
        {

            return new Dictionary<string, Message>
        {
           #region ValidationMessages
           { ValidationMessages.RequitedCode, new Message("برجاء ادخال الكود", "Please enter the code") },
           { ValidationMessages.RequiredNameAr, new Message("برجاء ادخال الاسم العربى", "Please enter the Arabic name.") },
           { ValidationMessages.RequiredNameEn, new Message("برجاء ادخال الاسم الانجليزى", "Please enter the English name.") },
           #endregion     
           #region AuthMessages        
           { AuthMessages.InvalidLogin, new Message("اسم المستخدم أو كلمة المرور غير صحيحة", "Invalid username or password.") },
           { AuthMessages.ConfirmEmail, new Message("برجاء تأكيد الإيميل قبل تسجيل الدخول", "Please confirm your email before logging in.") },
           { AuthMessages.InvalidRoleID, new Message("معرف الدور غير صالح.", "Invalid role ID.") },
           { AuthMessages.InvalidRoleData, new Message("تنسيق البيانات غير صالح.", "Invalid data format.") },
           { AuthMessages.InvalidpermissionID, new Message("معرف الإذن غير صالح.", "Invalid permission ID.") },
            #endregion       
           #region GenericMessages   
           { GenericMessages.Success, new Message("تم الحفظ بنجاح", "Data Saved Successfully") },
           { GenericMessages.Failed, new Message("خطأ فى الحفظ", "Error On Saving Data") },
           { GenericMessages.DataRequited, new Message("برجاء ادخال البيانات", "Data Is Requited") },
           #endregion

        };
        }
    }

}
