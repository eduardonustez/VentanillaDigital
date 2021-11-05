"use strict";

var SCANNER_IMPL = {};
SCANNER_IMPL.Escaners = null;
SCANNER_IMPL.ServiceScannerHelper = null;
SCANNER_IMPL.ConfigScannerHelper = null;
SCANNER_IMPL.ScannerHub = null;
SCANNER_IMPL.SignalRServerUrl = null;

function setSignalR2URL(url) {
    SCANNER_IMPL.SignalRServerUrl = url;
}

SCANNER_IMPL.SR_SetHelper = function (h) {
    if (SCANNER_IMPL.ServiceScannerHelper == null) {
        SCANNER_IMPL.ServiceScannerHelper = h;
    }
};

SCANNER_IMPL.SetConfigScannerHelper = function (h) {
    if (SCANNER_IMPL.ConfigScannerHelper == null) {
        SCANNER_IMPL.ConfigScannerHelper = h;
    }
};

function SR_IsConnected() {
    disconnect();
    if (
        $.connection.hub && $.connection.hub.state === $.signalR.connectionState.disconnected
    ) {
        return signalRv2_IsConnected();
    }
    return SCANNER_IMPL.ScannerHub != null;
}

$(function () {
    SR_IsConnected();
    handleDisconnected();
    handleError();
});

function signalRv2_IsConnected() {
    $("#callScanner").prop("disabled", true);
    $.getScript(SCANNER_IMPL.SignalRServerUrl)
        .done(() => {
            $.connection.hub.url = SCANNER_IMPL.SignalRServerUrl;
            SCANNER_IMPL.ScannerHub = $.connection.scanner;
            wacomHubProxy = $.connection.wacom;
            metodosClienteSignalR();
            SR_ClientListening();
            window.hubReady = $.connection.hub.start();
            window.hubReady.done(function () {
            // $.connection.hub
            //     .start()
            //     .done(function () {
                    console.log("📇 SignalR Inicializado.");
                    wacomHubProxy.server.connectWacom();
                    SCANNER_IMPL.ScannerHub.server
                        .obtenerScanners()
                        .done(() => {
                            console.log("📇 Obteniendo scanner...");
                        })
                        .fail(function (e) {
                            if (e.source === "HubException") {
                                $("#mensaje-escaner").show();
                                $(".input-container-escaner").hide();
                            }
                        });
                    return true;
                })
                .fail(function (reason) {
                    console.log("📇 SignalR conexión fallida: " + reason);
                    return false;
                });
        })
        .fail(() => {
            $("#mensaje-escaner").show();
            $(".input-container-escaner").hide();
            return false;
        });
    return false;
}

function metodosClienteSignalR() {
    recuperarDocumento();
    obtenerScannerConectados();
    twain_exception();
}

function enviarAEscanear(opciones) {
    if (SR_IsConnected() && SCANNER_IMPL.ScannerHub) {
        $("#callScanner").prop("disabled", true);
        SCANNER_IMPL.ScannerHub.server.enviarComando(opciones).fail(function (e) {
            $("#callScanner").prop("disabled", false);
            if (e.source === "HubException") {
                if (e.message.includes("La secuencia no contiene elementos")) {
                    modalScannerInfo("Información del dispositivo", "Compruebe que el escáner esté encendido y conectado al equipo");
                } else if (e.message.includes("HRESULT")) {
                    modalScannerInfo("Información del dispositivo", "Ha ocurrido un error al escanear el documento. Verifique que el documento esté en la bandeja de entrada de forma correcta");
                }
                else {
                    modalScannerInfo("Información del dispositivo", e.message);
                }
            }
        });
    }
}

function obtenerDispositivosTwain() {
    if (SR_IsConnected()) {
        if (SCANNER_IMPL.ScannerHub) {
            Scanner_ObtenerDispositivos();
        }
    }
}

function Scanner_ObtenerDispositivos() {
    SCANNER_IMPL.ScannerHub.server
        .obtenerScanners()
        .done(() => {
            console.log("📇 Obteniendo scanner...");
        })
        .fail(function (e) {
            if (e.source === "HubException") {
                $("#mensaje-escaner").show();
                $(".input-container-escaner").hide();
            }
        });
}

function handleError() {
    if ($.connection.hub.url) {
        $.connection.hub.error(function (error) {
            $("#callScanner").prop("disabled", true);
            console.log("📇 SignalR error: " + error);
            SignalRv2_Disconnect();
            setTimeout(function () {
                SR_IsConnected();
            }, 5000);
        });
    }
}

function handleDisconnected() {
    if ($.connection.hub.url) {
        $.connection.hub.disconnected(function () {
            SignalRv2_Disconnect();
            setTimeout(function () {
                //SR_IsConnected();
                signalRv2_IsConnected();
                //window.hubReady = $.connection.hub.start();
            }, 5000);
            $("#callScanner").prop("disabled", true);
        });
    }
}

function recuperarDocumento() {
    let nombreDocumento;
    let apellidoDocumento;
    if (SCANNER_IMPL.ScannerHub) {
        SCANNER_IMPL.ScannerHub.client.documentMessage = function (documento) {
            if (documento != null) {
                nombreDocumento = `${documento.PrimerNombre} ${documento.SegundoNombre}`;
                apellidoDocumento = `${documento.PrimerApellido} ${documento.SegundoApellido}`;
                SCANNER_IMPL.ServiceScannerHelper.invokeMethodAsync(
                    "AgregarValoresACampos",
                    nombreDocumento,
                    apellidoDocumento,
                    documento.Cedula.toString()
                );
            } else {
                modalScannerInfo("Mensaje de escáner", "No ha obtenido información, verifique que el documento este en la bandeja de alimentación correctamente o en la configuración cambie la densidad de pixeles");
                SCANNER_IMPL.ServiceScannerHelper.invokeMethodAsync(
                    "AgregarValoresACampos",
                    "",
                    "",
                    ""
                );
            }
            $("#callScanner").prop("disabled", false);
        };
    }
}

function obtenerScannerConectados() {
    if (SCANNER_IMPL.ScannerHub) {
        SCANNER_IMPL.ScannerHub.client.dispositivosConectados = function (
            dispositivosConectados
        ) {
            if (dispositivosConectados.length > 0) {
                agregarDispositivosAConfiguracion(dispositivosConectados);
            } else {
                $("#mensaje-escaner").show();
                $(".input-container-escaner").hide();
            }
        };
    }
}

function obtenerEscanerVariable() {
    return SCANNER_IMPL.Escaners;
}

function agregarDispositivosAConfiguracion(dispositivosConectados) {
    SCANNER_IMPL.Escaners = dispositivosConectados;
    if (SCANNER_IMPL.ConfigScannerHelper) {
        SCANNER_IMPL.ConfigScannerHelper.invokeMethodAsync(
            "RecuperarScanners",
            SCANNER_IMPL.Escaners
        );
    }
}

function twain_exception() {
    if (SCANNER_IMPL.ScannerHub) {
        SCANNER_IMPL.ScannerHub.client.excepcionCapturada = function (mensaje) {
            console.log("👳🏼‍♀️Test", mensaje);
            if (mensaje.includes("La secuencia no contiene elementos")) {
                modalScannerInfo("Mensaje de escáner", "Compruebe que el escáner esté encendido y conectado al equipo");
            } else {
                modalScannerInfo("Mensaje de escáner", mensaje);
            }
        };
    }
}

function asignarValorDefectoEnSelector(valor) {
    $("#escaner-detectados").val(valor).change();
}

function SignalRv2_Disconnect() {
    SCANNER_IMPL = {};
    SCANNER_IMPL.ServiceScannerHelper = null;
    SCANNER_IMPL.ConfigScannerHelper = null;
    if (SCANNER_IMPL.ScannerHub != null) {
        $.connection.hub.stop().done(function () {
            SCANNER_IMPL.ScannerHub = null;
            console.log("📇 Disconnected.");
        });
    }
}

function modalScannerInfo(title, message) {
    $("#scannerModalCenter").modal("toggle");
    $("#scanner-title").text(title);
    $("#scanner-message").text(message);
}
