using ERPManagement.UI.GeneralClasses;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using static ERPManagement.UI.Components.Global.ModalDialog;

namespace ERPManagement.UI.Components.Base
{
    public partial class Buttons : ComponentBase
    {
        [Parameter]
        public HiddenButtonsConfig hiddenButtons { get; set; }
      
        [Parameter]
        public Func<Task<List<string>>> ValidateDataFun { get; set; }
        [Parameter]
        public Func<string> BtnAddClickFun { get; set; }
        [Parameter]
        public Func<string> BtnAddRootClickFun { get; set; }
        [Parameter]
        public Func<string> BtnUpdateClickFun { get; set; }

        [Parameter]
        public Func<string> BtnDeleteClickFun { get; set; }
        [Parameter]
        public Func <Task<bool>> AddFun { get; set; }
        [Parameter]
        public Func <Task<bool>> UpdateFun { get; set; }
        [Parameter]
        public EventCallback OnDeleteCallback { get; set; }

        [Parameter]
        public EventCallback OnCancelCallback { get; set; }

        [Parameter]
        public EventCallback OnClearControlsCallback { get; set; }

        [Parameter]
        public EventCallback OnSearchCallback { get; set; }
        [Parameter]
        public EventCallback OnPreviousCallback { get; set; }
        [Parameter]
        public EventCallback OnNextCallback { get; set; }

        [Parameter]
        public EventCallback OnCopyCallback { get; set; }
        [Parameter]
        public EventCallback<GlobalVariables.States> OnChangeStateCallback { get; set; }

        [Parameter]
        public EventCallback<bool> OnConfirmCallback { get; set; }

        [Parameter]
        public string SearchCode { get; set; } = "";

        [Parameter]
        public string formName { get; set; } = "";

        string Message = "";
        bool isModalVisible = false;
        bool isNavMode = true;
        string Title = "";
        //private bool isQMBModalVisible;
        //string QMBMessage = "";
        public ModalDialogType DialogType { get; set; }
        GlobalVariables.States currentState = GlobalVariables.States.NavMode;
        private void DeleteClick()
        {
            DialogType = ModalDialogType.DeleteCancel;
            isModalVisible = true;
            Title = localizer["Delete"];
            Message = localizer["DeleteMsg"];
        }
        private async Task OnClose(bool action)
        {
            if (DialogType == ModalDialogType.DeleteCancel && action)
            {
                string deleteCheckMsg = BtnDeleteClickFun();
                if (BtnDeleteClickFun is null || deleteCheckMsg == "")
                {
                    await OnDeleteCallback.InvokeAsync();
                }
                else
                {
                    ShowAlert(deleteCheckMsg);
                    return;
                }
            }
            else if (DialogType == ModalDialogType.OkCancel)
            {
                if (Title == localizer["Cancel"])
                {
                    if (action)
                    {
                        await CancelForm();
                    }
                    isModalVisible = false;
                    return;
                }
                else if (action)
                {
                    await OnConfirmCallback.InvokeAsync(true);
                    await PreSavePressed();
                    return;
                }
                else
                {
                    await OnConfirmCallback.InvokeAsync(false);
                    await PreSavePressed();
                    return;
                }
            }
            isModalVisible = false;
        }
        //private async Task CancelAction()
        //{
        //    if (QMBTitle == localizer["Confirm"])
        //    {
        //        await OnConfirmCallback.InvokeAsync(false);
        //        await PreSavePressed();
        //    }           
        //    //isDeleteModalVisible = true;
        //}

        private async Task OnAddRootCallback()
        {
            string checkBeforeAddRootMsg = "";
            if (BtnAddRootClickFun is not null)
                checkBeforeAddRootMsg = BtnAddRootClickFun.Invoke();
            if (BtnAddRootClickFun is null || checkBeforeAddRootMsg == "")
            {
                currentState = GlobalVariables.States.Adding;
                isNavMode = false;
                await OnChangeStateCallback.InvokeAsync(currentState);
                await OnClearControlsCallback.InvokeAsync();
            }
            else { ShowAlert(checkBeforeAddRootMsg); }
        }
        private void OnPrintCallback()
        {

        }
        private void Close()
        {
            NavigationManager.NavigateTo("/index");
        }
        private void ShowAlert(string alertMessage)
        {
            DialogType = ModalDialogType.Ok;
            Message = alertMessage;
            isModalVisible = true;
        }
        private async Task AddButtonPressed()
        {
            string checkBeforeAddMsg = "";
            if (BtnAddClickFun is not null)
                checkBeforeAddMsg = BtnAddClickFun.Invoke();
            if (BtnAddClickFun is null || checkBeforeAddMsg == "")
            {
                currentState = GlobalVariables.States.Adding; 
                isNavMode = false;
                await OnChangeStateCallback.InvokeAsync(currentState);
                await OnClearControlsCallback.InvokeAsync();
            }
            else { ShowAlert(checkBeforeAddMsg); }
        }
        private async Task UpdateButtonPressed()
        {
            string checkBeforeUpdateMsg = "";
            if (BtnUpdateClickFun is not null)
                checkBeforeUpdateMsg = BtnUpdateClickFun.Invoke();
            if (BtnUpdateClickFun is null || checkBeforeUpdateMsg == "")
            {
                currentState = GlobalVariables.States.Updating;
                isNavMode = false;
                await OnChangeStateCallback.InvokeAsync(currentState);
            }
            else { ShowAlert(checkBeforeUpdateMsg); }
        }
        private async Task PreSavePressed()
        {
            List<string> lstMsg = await ValidateDataFun.Invoke();
            if (ValidateDataFun is null || lstMsg[0] != "")
            {
                ShowAlert(lstMsg[0]);
            }
            else if (lstMsg[1] != "")
            {
                DialogType = ModalDialogType.OkCancel;
                isModalVisible = true;
                Title = localizer["Confirm"];
                Message = lstMsg[1];
            }
            else 
            {
                await SaveButtonPressed();
            }
        }
        private async Task SaveButtonPressed()
        {
            if (currentState == GlobalVariables.States.Adding)
            {
                bool IsAdded = await AddFun.Invoke();
                if (IsAdded)
                {
                    await OnClearControlsCallback.InvokeAsync();
                }
            }
            else if (currentState == GlobalVariables.States.Updating)
            {
                bool IsUpdated = await UpdateFun.Invoke();
                if (IsUpdated)
                {
                    currentState = GlobalVariables.States.NavMode;
                    isNavMode = true;
                    await OnChangeStateCallback.InvokeAsync(currentState);
                }
            }
            isModalVisible = false;
        }
        private async Task SaveAndCloseButtonPressed()
        {
            await PreSavePressed();
            Close();
        }
        private async Task btnCancel_Click()
        {
            DialogType = ModalDialogType.OkCancel;
            isModalVisible = true;
            Title = localizer["Cancel"];
            Message = localizer["CancelMsg"];
        }
        private async Task CancelForm()
        {
            await OnCancelCallback.InvokeAsync();
            currentState = GlobalVariables.States.NavMode;
            isNavMode = true;
            await OnChangeStateCallback.InvokeAsync(currentState);

        }

    }
}
