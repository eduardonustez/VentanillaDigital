function navegador() {
    var agente = window.navigator.userAgent;
    var navegadores = ["Chrome", "Firefox", "Safari", "Opera", "Trident", "MSIE", "Edg"];
    for (var i in navegadores) {
        if (agente.indexOf(navegadores[i]) != -1) {
            return navegadores[i];
        }
    }
}

function FuntionResize() {
    var widthBrowser = window.outerWidth;
    //var heightBrowser = window.outerHeight;
    return widthBrowser;
}
function detectOS() {
    const platform = navigator.platform.toLowerCase(),
        iosPlatforms = ['iphone', 'ipad', 'ipod', 'ipod touch'];

    if (platform.includes('mac')) return 'MacOS';
    if (iosPlatforms.includes(platform)) return 'iOS';
    if (platform.includes('win')) return 'Windows';
    if (/android/.test(navigator.userAgent.toLowerCase())) return 'Android';
    if (/linux/.test(platform)) return 'Linux';

    return 'unknown';
}
