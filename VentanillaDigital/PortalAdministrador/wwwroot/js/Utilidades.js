var timer;
var timerNotification;
var datosConfig;
var timeToAlert = 0;
var maxTime = 60000;

function timerInactivo(dotnetHelper, SessionTimeMinutes) {
    //document.onmousemove = resetTimer;
    document.onclick = resetTimer;
    document.onkeypress = resetTimer;
    document.onfocus = resetTimer;
    resetTimer();
    function resetTimer() {
        if (SessionTimeMinutes >= 3) {
            timeToAlert = (SessionTimeMinutes * maxTime) - 120000;
        } else if (SessionTimeMinutes >= 1 && SessionTimeMinutes < 3) {
            timeToAlert = (SessionTimeMinutes * maxTime) - 30000;
        } else {
            timeToAlert = 0;
        }
        if (timeToAlert > 0) {
            clearTimeout(timerNotification);
            timerNotification = setTimeout(notificationLogout, timeToAlert);
        }

        clearTimeout(timer);
        timer = setTimeout(logout, (SessionTimeMinutes * maxTime));
    }

    function logout() {
        $('.modal').modal('hide');
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

//var timer;
//var datosConfig;

//function timerInactivo(dotnetHelper, SessionTimeMinutes) {

//    //document.onmousemove = resetTimer;
//    document.onclick = resetTimer;
//    document.onkeypress = resetTimer;
//    document.onfocus = resetTimer;

//    function resetTimer() {
//        clearTimeout(timer);
//        timer = setTimeout(logout, (SessionTimeMinutes * 60000));

//    }

//    function logout() {
//        dotnetHelper.invokeMethodAsync("Logout");
//    }

//}

//function FocusElement(element) {
//    if (element instanceof HTMLElement) {
//        element.focus();
//    }
//}

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

function cerrarModal(modal) {
    $(`#${modal}`).modal('toggle')
}