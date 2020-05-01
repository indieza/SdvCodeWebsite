function setSiteTheme() {
    let theme = localStorage.getItem("SdvCodeTheme");
    if (!theme) {
        localStorage.setItem("SdvCodeTheme", "/css/presets/preset1.css");
    }
    document.getElementById("style-switch").href = localStorage.getItem("SdvCodeTheme");
}

document.getElementById("preset1").addEventListener("click", function (e) {
    e.preventDefault();
    localStorage.setItem("SdvCodeTheme", "/css/presets/preset1.css");
    document.getElementById("style-switch").href = "/css/presets/preset1.css";
});
document.getElementById("preset2").addEventListener("click", function (e) {
    e.preventDefault();
    localStorage.setItem("SdvCodeTheme", "/css/presets/preset2.css");
    document.getElementById("style-switch").href = "/css/presets/preset2.css";
});
document.getElementById("preset3").addEventListener("click", function (e) {
    e.preventDefault();
    localStorage.setItem("SdvCodeTheme", "/css/presets/preset3.css");
    document.getElementById("style-switch").href = "/css/presets/preset3.css";
});
document.getElementById("preset4").addEventListener("click", function (e) {
    e.preventDefault();
    localStorage.setItem("SdvCodeTheme", "/css/presets/preset4.css");
    document.getElementById("style-switch").href = "/css/presets/preset4.css";
});
document.getElementById("preset5").addEventListener("click", function (e) {
    e.preventDefault();
    localStorage.setItem("SdvCodeTheme", "/css/presets/preset5.css");
    document.getElementById("style-switch").href = "/css/presets/preset5.css";
});
document.getElementById("preset6").addEventListener("click", function (e) {
    e.preventDefault();
    localStorage.setItem("SdvCodeTheme", "/css/presets/preset6.css");
    document.getElementById("style-switch").href = "/css/presets/preset6.css";
});