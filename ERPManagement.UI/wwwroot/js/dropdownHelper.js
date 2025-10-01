export function registerOutsideClick(dropdownElement, dotnetHelper) {
    function onClick(event) {
        if (dropdownElement && !dropdownElement.contains(event.target)) {
            dotnetHelper.invokeMethodAsync("CloseDropdown");
        }
    }
    document.addEventListener("mousedown", onClick);

    return {
        dispose: () => document.removeEventListener("mousedown", onClick)
    };
}
