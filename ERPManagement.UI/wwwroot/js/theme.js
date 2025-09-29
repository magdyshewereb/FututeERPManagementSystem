export function setTheme(isDark) {
    if (isDark) {
        document.body.classList.add("dark-mode");
    } else {
        document.body.classList.remove("dark-mode");
    }
}

const toggleBtn = document.getElementById("toggle-theme");
const body = document.body;

// نتأكد إذا فيه مود متخزن قبل كده
if (localStorage.getItem("theme") === "dark") {
    body.classList.add("dark-mode");
    toggleBtn.textContent = "☀️"; // يبين انه في دارك مود
}

// حدث عند الضغط
toggleBtn.addEventListener("click", () => {
    body.classList.toggle("dark-mode");

    // لو اتفعل نخزنه
    if (body.classList.contains("dark-mode")) {
        localStorage.setItem("theme", "dark");
        toggleBtn.textContent = "☀️";
    } else {
        localStorage.setItem("theme", "light");
        toggleBtn.textContent = "🌙";
    }
});
