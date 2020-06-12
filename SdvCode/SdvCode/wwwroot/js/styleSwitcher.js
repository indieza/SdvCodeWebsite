function setSiteTheme() {
    let theme = localStorage.getItem("SdvCodeTheme");
    if (!theme) {
        localStorage.setItem("SdvCodeTheme", "/css/presets/preset1.css");
    }

    let sound = localStorage.getItem("SdvCodeSound");
    if (!sound) {
        localStorage.setItem("SdvCodeSound", "/NotificationSound1.mp3");
    }
    document.getElementById("style-switch").href = localStorage.getItem("SdvCodeTheme");
    document.getElementById("targetAudio").src = localStorage.getItem("SdvCodeSound");
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
document.getElementById("sound1").addEventListener("click", function (e) {
    e.preventDefault();
    localStorage.setItem("SdvCodeSound", "/NotificationSound1.mp3");
    document.getElementById("targetAudio").src = "/NotificationSound1.mp3";
});
document.getElementById("sound2").addEventListener("click", function (e) {
    e.preventDefault();
    localStorage.setItem("SdvCodeSound", "/NotificationSound2.mp3");
    document.getElementById("targetAudio").src = "/NotificationSound2.mp3";
});
document.getElementById("sound3").addEventListener("click", function (e) {
    e.preventDefault();
    localStorage.setItem("SdvCodeSound", "/NotificationSound3.mp3");
    document.getElementById("targetAudio").src = "/NotificationSound3.mp3";
});
document.getElementById("sound4").addEventListener("click", function (e) {
    e.preventDefault();
    localStorage.setItem("SdvCodeSound", "/NotificationSound4.mp3");
    document.getElementById("targetAudio").src = "/NotificationSound4.mp3";
});
document.getElementById("sound5").addEventListener("click", function (e) {
    e.preventDefault();
    localStorage.setItem("SdvCodeSound", "/NotificationSound5.mp3");
    document.getElementById("targetAudio").src = "/NotificationSound5.mp3";
});
document.getElementById("sound6").addEventListener("click", function (e) {
    e.preventDefault();
    localStorage.setItem("SdvCodeSound", "/NotificationSound6.mp3");
    document.getElementById("targetAudio").src = "/NotificationSound6.mp3";
});