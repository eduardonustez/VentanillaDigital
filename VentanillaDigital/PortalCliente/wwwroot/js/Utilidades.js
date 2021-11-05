var timer;
var timerNotification;
var datosConfig;
var timeToAlert = 0;

function cambiarTheme() {
    var tabFijas = document.getElementById("color-a");
    var tabDigital = document.getElementById("color-b");

    if (window.location.href.includes("virtual") || window.location.href.includes("ciudadano")) {
        cambiarThemeAAzul();
    }

    if (tabFijas)
        tabFijas.onclick = () => {
            cambiarThemeARojo();
        };

    if (tabDigital)
        tabDigital.onclick = () => {
            if (!window.location.href.includes("/tramite/"))
                cambiarThemeAAzul();
        };
}

function getCurrentURL() {
    return window.location.href
}

function cambiarThemeARojo() {
    var root = document.documentElement;
    var tabFijas = document.getElementById("color-a");
    var tabDigital = document.getElementById("color-b");
    var menuLateral = document.querySelector(".left-sidebar");
    var linkTramitesVirtuales = document.getElementById("tramites-virtuales-link");
    if (tabFijas) tabFijas.classList.add("active-fijas");
    if (tabDigital) tabDigital.classList.remove("active-ciudadano");
    if (menuLateral) menuLateral.style.display = "block";
    if (linkTramitesVirtuales) linkTramitesVirtuales.style.display = "block";
    root.style.setProperty("--rojo-900", "hsl(0, 100%, 10%)");
    root.style.setProperty("--rojo-800", "hsl(0, 90%, 20%)");
    root.style.setProperty("--rojo-700", "hsl(2, 80%, 30%)");
    root.style.setProperty("--rojo-600", "hsl(2, 60%, 40%)");
    root.style.setProperty("--rojo-500", "hsl(2, 60%, 53%)");
    root.style.setProperty("--rojo-400", "hsl(2, 60%, 63%)");
    root.style.setProperty("--rojo-300", "hsl(2, 76%, 73%)");
    root.style.setProperty("--rojo-200", "hsl(0, 81%, 84%)");
    root.style.setProperty("--rojo-100", "hsl(0, 100%, 92%)");
    root.style.setProperty("--rojo-primario", "hsl(2, 100%, 38%)");
}

function cambiarThemeAAzul() {
    var root = document.documentElement;
    var tabFijas = document.getElementById("color-a");
    var tabDigital = document.getElementById("color-b");
    var menuLateral = document.querySelector(".left-sidebar");
    var linkTramitesVirtuales = document.getElementById("tramites-virtuales-link");
    if (tabFijas) tabFijas.classList.remove("active-fijas");
    if (tabDigital) tabDigital.classList.add("active-ciudadano");
    if (menuLateral) menuLateral.style.display = "none";
    if (linkTramitesVirtuales) linkTramitesVirtuales.style.display = "none";
    root.style.setProperty("--rojo-900", "hsl(223, 100%, 10%)");
    root.style.setProperty("--rojo-800", "hsl(223, 93%, 20%)");
    root.style.setProperty("--rojo-700", "hsl(220, 82%, 33%)");
    root.style.setProperty("--rojo-600", "hsl(220, 67%, 42%)");
    root.style.setProperty("--rojo-500", "hsl(220, 57%, 53%)");
    root.style.setProperty("--rojo-400", "hsl(220, 65%, 64%)");
    root.style.setProperty("--rojo-300", "hsl(220, 77%, 74%)");
    root.style.setProperty("--rojo-200", "hsl(220, 84%, 86%)");
    root.style.setProperty("--rojo-100", "hsl(220, 100%, 95%)");
    root.style.setProperty("--rojo-primario", "hsl(220, 100%, 38%)");
}

function standardModalRecover(idClassModal, displayValue) {
    var modal = document.getElementById(idClassModal);
    if (modal) {
        modal.style.display = displayValue;
    }
}

function abrirNativeModal(idClassModal) {
    standardModalRecover(idClassModal, "flex");
}

function cerrarNativeModal(idClassModal) {
    standardModalRecover(idClassModal, "none");
}

window.onload = function () {
    window.onpopstate = function () {
        console.info(window.location.href);
        window.history.forward();
        window.history.pushState(null, window.document.title, null);
    }
};

function timerInactivo(dotnetHelper, SessionTimeMinutes) {
    //document.onmousemove = resetTimer;
    document.onclick = resetTimer;
    document.onkeypress = resetTimer;
    document.onfocus = resetTimer;
    resetTimer();
    function resetTimer() {
        if (SessionTimeMinutes >= 3) {
            timeToAlert = (SessionTimeMinutes * 60000) - 120000;
        } else if (SessionTimeMinutes >= 1 && SessionTimeMinutes < 3) {
            timeToAlert = (SessionTimeMinutes * 60000) - 30000;
        } else {
            timeToAlert = 0;
        }
        if (timeToAlert > 0) {
            clearTimeout(timerNotification);
            timerNotification = setTimeout(notificationLogout, timeToAlert);
        }
        clearTimeout(timer);
        timer = setTimeout(logout, (SessionTimeMinutes * 60000));
    }
    function logout() {
        dotnetHelper.invokeMethodAsync("Logout");
    }
    function notificationLogout() {
        dotnetHelper.invokeMethodAsync("notificationLogout");
    }
}

function FocusElement(element) {
    if (element instanceof HTMLElement) {
        element.focus();
    }
}

function saveAsFile(filename, bytesBase64) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/octet-stream;base64," + bytesBase64;
    document.body.appendChild(link); // Needed for Firefox
    link.click();
    document.body.removeChild(link);
}

window.saveFile = function (bytesBase64, mimeType, fileName) {
    var fileUrl = "data:" + mimeType + ";base64," + bytesBase64;
    fetch(fileUrl)
        .then(response => response.blob())
        .then(blob => {
            var link = window.document.createElement("a");
            link.href = window.URL.createObjectURL(blob, { type: mimeType });
            link.download = fileName;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        });
}
