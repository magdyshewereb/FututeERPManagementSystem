
var TreeView = TreeView || {}; //it's for declaring a "namespace" object
//This means that if TreeView doesn't have value (undefined, null) then use {}. It is a way of assigning a default value to a variable in JavaScript

var oldIDs;
//clear the active class from current active
TreeView.setHighlightDefault = function () {

    //const collection = document.getElementsByClassName("unSelect");
    for (let i = 0; i < oldIDs.length; i++) {
        document.getElementById(oldIDs[i]).style.backgroundColor = "inherit";
        document.getElementById(oldIDs[i]).style.color = "inherit";
    
    }
}
TreeView.highlightSelected = function (IDs) {
    oldIDs = IDs;
    for (let i = 0; i < IDs.length; i++) {
        document.getElementById(IDs[i]).style.color = "White";
        document.getElementById(IDs[i]).style.backgroundColor = "#3068EB";
    }
}
//////////////////////////////////////////////////////////////////////////////
var CheckBoxTree = CheckBoxTree || {};
//UnChecked 
CheckBoxTree.setChecked = function (IDs) {
    for (let i = 0; i < IDs.length; i++) {
        document.getElementById(IDs[i]).checked = true;
    }
}
CheckBoxTree.setIndeterminate = function (IDs) {
    for (let i = 0; i < IDs.length; i++) {
        document.getElementById(IDs[i]).indeterminate = true;
    }
}

CheckBoxTree.setUnchecked = function (ID) {
    
        document.getElementById(ID).indeterminate = false;
        document.getElementById(ID).checked = false;

}

TreeView.afterChangeData = function (id) {
    //document.getElementById(id).style.color = "White";
    //document.getElementById(id).style.backgroundColor = "Red";
    //document.getElementById(id).style.display= "block";
    document.getElementById(id).innerText = "@ToggleParents(" + id + ")";
}
TreeView.inputMaxLength = function (id, length) {
    document.getElementById(id).style.inputMaxLength = length;
}


// sript.js
var CustomAlert = CustomAlert || {};
CustomAlert.customAlert = function (message) {
        //const message =
        //    "Are you sure you want to perform this action?";
        document.getElementById("alert-message").
            innerText = message;
        document.getElementById("custom-alert").
            style.visibility = "visible";
        document.body.
            style.backgroundColor = "rgb(49 45 45 / 45%)";
    }

CustomAlert.confirmAction= function () {
    document.getElementById("custom-alert").
        style.visibility = "hidden";
    document.body.
        style.backgroundColor = "#fff"
    return true;
}

CustomAlert.cancelAction=function () {
    document.getElementById("custom-alert").
        style.visibility = "hidden";
    document.body.
        style.backgroundColor = "#fff"
    return false;
}


