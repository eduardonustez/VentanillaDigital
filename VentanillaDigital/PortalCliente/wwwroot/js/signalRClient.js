"use strict";
var wacomHubProxy;
var serviceHelperWacomAgenteService;

function SR_SetHelper(h) {
  serviceHelperWacomAgenteService = h;
}

function SR_ClientListening() {
  wacomHubProxy.client.onButtonPressed = function (btn) {
    serviceHelperWacomAgenteService.invokeMethodAsync("SR_ActivarBoton", btn);
  };
  wacomHubProxy.client.onErrorOcurred = function (msgError) {
    console.error("Error Conexión Wacom con SignalR:", msgError);
  };
}

function SR_DisconnectWacom() {
  if (SR_IsConnected() && window.hubReady && serviceHelperWacomAgenteService) {
    window.hubReady.done(function () {
      wacomHubProxy.server.disconnectWacom();
      serviceHelperWacomAgenteService.invokeMethodAsync(
        "SR_ActualizarEstadoServicio",
        false
      );
    }).fail(function (e) {
      if (e.source === "HubException") {
          console.error(e.message);
      }
  });
  }
}
function SR_IsConnectedWacom() {
  if (SR_IsConnected() && window.hubReady) {
    window.hubReady.done(function () {
      return wacomHubProxy.server.isConnectedWacom();
      //return true;
    })
    .fail(function (e) {
      if (e.source === "HubException") {
          console.error(e.message);
      }
  });
  } else {
    return false;
  }
}

//function SR_SendImgToSTU(dataJson) {
//  var dataArray = JSON.parse(dataJson).Result;
//  if (SR_IsConnected() && window.hubReady) {
    
//    window.hubReady.done(function () {
//      for (var i = 0; i < dataArray.length; i++) {
//        wacomHubProxy.server.writeImage(dataArray[i]);
//      }
//    }).fail(function (e) {
//      if (e.source === "HubException") {
//          console.error(e.message);
//      }
//  });
//  }
//}
function SR_SendImgToSTU(keyImage,force) {
    if (SR_IsConnected() && window.hubReady) {
        window.hubReady.done(function () {
            wacomHubProxy.server.writeImageFromCache(keyImage,force);
        }).fail(function (e) {
            if (e.source === "HubException") {
                console.error(e.message);
            }
        });
    }
}
function SR_AddImageToCache(keyImage,data) {
    if (SR_IsConnected() && window.hubReady) {
        window.hubReady.done(function () {
            wacomHubProxy.server.addImage(keyImage,data);
        }).fail(function (e) {
            if (e.source === "HubException") {
                console.error(e.message);
            }
        });
    }
}
function SR_ImageExist(keyImage) {
  let exist=false;
    if (SR_IsConnected() && window.hubReady) {
        window.hubReady.done(function () {
            exist = wacomHubProxy.server.imageExist(keyImage);
        }).fail(function (e) {
            if (e.source === "HubException") {
                console.error(e.message);
            }
        });
    }
  return exist;
}
function SR_SetInkingMode(mode) {
  if (SR_IsConnected() && window.hubReady) {
    
    window.hubReady.done(function () {
      wacomHubProxy.server.setInkingMode(mode);
    }).fail(function (e) {
      if (e.source === "HubException") {
          console.error(e.message);
      }
  });
  }
}

function SR_AddButtons(botones, habilitarFirma) {
  if (botones == null) return;
  if (SR_IsConnected() && window.hubReady) {
    
    window.hubReady.done(function () {
      wacomHubProxy.server.addButtons(botones,habilitarFirma);
    });
  }
}
function SR_SetCapturedImageSize(height, width, lineWidth) {
  let signatureCaptured = null;
  if (SR_IsConnected() && window.hubReady) {
    
    window.hubReady.done(function () {
      signatureCaptured = wacomHubProxy.server.setCapturedImageSize(height,width,lineWidth);
    });
  }
  return signatureCaptured;
}

function SR_GenerateImage() {
  let signatureCaptured = null;
  if (SR_IsConnected() && window.hubReady) {
    
    window.hubReady.done(function () {
      signatureCaptured = wacomHubProxy.server.requireSignature();
    });
  }
  return signatureCaptured;
}
