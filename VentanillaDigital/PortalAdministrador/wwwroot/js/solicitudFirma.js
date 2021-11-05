var m_btns; // The array of buttons that we are emulating.
var m_clickBtn = -1;
var intf;
var formDiv;
var protocol;
var m_usbDevices;
var tablet;
var m_capability;
var m_inkThreshold;
var m_encodingMode;
var modalBackground;
var formDiv;
var m_penData;
var lastPoint;
var isDown;
var compareciente = { identificacion: "", nombre: "" }
var m_firmando;
var serviceHelper;
var p = new WacomGSS.STU.Protocol();

function setHelper(h) {
    console.log('sethelper', h);
    serviceHelper = h;
}

async function checkArbitratorConnection() {
    // Establishing a connection to SigCaptX Web Service can take a few seconds, 
    // particularly if the browser itself is still loading/initialising 
    // or on a slower machine. 
    if (WacomGSS.STU.isServiceReady()) {
        return WacomGSS.STU.isDCAReady()
            .then(function (message) {
                if (message) {
                    return connect()
                        .then(function (is_connected) {
                            if (is_connected) {

                                return disconnect()
                                    .then(function (message) {
                                        return 4;
                                    });
                            }
                            return 3;
                        });
                }
                return 2;
            });
    }
    return 0;
}

function onLoad() {
    portCheck();

    window.addEventListener("beforeunload", function (e) {
        var confirmationMessage = "";
        WacomGSS.STU.close();
        (e || window.event).returnValue = confirmationMessage; // Gecko + IE 
        return confirmationMessage; // Webkit, Safari, Chrome 
    });
}

function portCheck() {
    WacomGSS.STU = new WacomGSS.STUConstructor();

}

function connect() {

    return WacomGSS.STU.isDCAReady()
        .then(function (message) {
            if (!message) {
                throw new DCANotReady();
            }
            // Set handler for Device Control App timeout
            WacomGSS.STU.onDCAtimeout = onDCAtimeout;

            return WacomGSS.STU.getUsbDevices();
        }).then(function (message) {
            if (message == null || message.length == 0) {
                throw new Error("No STU devices found");
            }
            //console.log("received: " + JSON.stringify(message));
            m_usbDevices = message;
            return WacomGSS.STU.isSupportedUsbDevice(m_usbDevices[0].idVendor, m_usbDevices[0].idProduct);
        }).then(function (message) {
            intf = new WacomGSS.STU.UsbInterface();
            return intf.Constructor();
        }).then(function (message) {
            return intf.connect(m_usbDevices[0], true);
        }).then(function (message) {
            if (0 != message.value) {
                throw new Error("can't connect");
            }
            tablet = new WacomGSS.STU.Tablet();
            return tablet.Constructor(intf);
        }).then(function (message) {
            intf = null;
            return tablet.getInkThreshold();
        }).then(function (message) {
            m_inkThreshold = message;
            return tablet.getCapability();
        }).then(function (message) {
            m_capability = message;
            return tablet.getProductId();
        }).then(function (message) {
            return WacomGSS.STU.ProtocolHelper.simulateEncodingFlag(message, m_capability.encodingFlag);
        }).then(function (message) {
            var encodingFlag = message;
            if ((encodingFlag & p.EncodingFlag.EncodingFlag_24bit) != 0) {
                return tablet.supportsWrite()
                    .then(function (message) {
                        m_encodingMode = message ? p.EncodingMode.EncodingMode_24bit_Bulk : p.EncodingMode.EncodingMode_24bit;
                    });
            } else if ((encodingFlag & p.EncodingFlag.EncodingFlag_16bit) != 0) {
                return tablet.supportsWrite()
                    .then(function (message) {
                        m_encodingMode = message ? p.EncodingMode.EncodingMode_16bit_Bulk : p.EncodingMode.EncodingMode_16bit;
                    });
            } else { // assumes 1bit is available
                m_encodingMode = p.EncodingMode.EncodingMode_1bit;
            }
        }).then(function (message) {
            return serviceHelper.invokeMethodAsync('ActualizarEstadoServicio', true);
        }).then(function (message) {
            return true;
        }).fail(function (ex) {
            console.error(ex);

            if (ex instanceof DCANotReady) {
                // Device Control App not detected 
                // Reinitialize and re-try
                WacomGSS.STU.Reinitialize();
            }
            else {
                // Some other error - Inform the user and closedown 
                //alert("connect failed:\n" + ex);
                setTimeout(disconnect(), 0);
            }
            return false;
        });
}

function disconnect() {

    WacomGSS.STU.onDCAtimeout = null;
    var deferred = Q.defer();
    if (!(undefined === tablet || null === tablet)) {
        tablet.setInkingMode(p.InkingMode.InkingMode_Off)
            .then(function (message) {
                return tablet.endCapture();
            })
            .then(function (message) {
                return tablet.disconnect();
            })
            .then(function (message) {
                tablet = null;
                return serviceHelper.invokeMethodAsync('ActualizarEstadoServicio', false);
            })
            .then(function (message) {
                deferred.resolve();
            })
            .fail(function (message) {
                console.error("disconnect error: " + message);
                deferred.resolve();
            })
    } else {
        deferred.resolve();
    }
    return deferred.promise;
}


function SendImgToSTU(data) {
    m_penData = new Array();
    return WacomGSS.STU.ProtocolHelper.resizeAndFlatten(data,
                0, 0, 0, 0,
                m_capability.screenWidth, m_capability.screenHeight,
                m_encodingMode,
                1, false, 0, true)
        .then(function (message) {
            return tablet.writeImage(m_encodingMode, message);
        }).fail(function (ex) {
            console.log(ex);

            setTimeout(disconnect(), 0);
        });
}


function ClearSTU() {
    m_penData = new Array();
    return tablet.setClearScreen()
        .fail(function (message) {
            setTimeout(disconnect(), 0);
        });
}

function SetInkingMode(mode) {
    if(tablet !== undefined){
        return tablet.setInkingMode(mode)
            .fail(function (message) {
                setTimeout(disconnect(), 0);
            });
    }
}


//============================================================================================================================



function onDCAtimeout() {
    // Device Control App has timed-out and shut down
    // For this sample, we just closedown tabletDemo (assumking it's running)
    console.log("DCA disconnected");
    setTimeout(disconnect, 0);
}

function Rectangle(x, y, width, height) {
    this.x = x;
    this.y = y;
    this.width = width;
    this.height = height;

    this.Contains = function (pt) {
        if (((pt.x >= this.x) && (pt.x <= (this.x + this.width))) &&
            ((pt.y >= this.y) && (pt.y <= (this.y + this.height)))) {
            return true;
        } else {
            return false;
        }
    }
}

// In order to simulate buttons, we have our own Button class that stores the bounds and event handler.
// Using an array of these makes it easy to add or remove buttons as desired.
//  delegate void ButtonClick();
function Button() {
    this.Bounds; // in Screen coordinates
    this.Text;
    this.Click;
};

function Point(x, y) {
    this.x = x;
    this.y = y;
}

// Error-derived object for Device Control App not ready exception
function DCANotReady() { }
DCANotReady.prototype = new Error();

function SetListeningMode(botones, firmando, canvas) { 
    
    m_firmando = firmando;

    return tablet.isSupported(p.ReportId.ReportId_PenDataOptionMode)
        .then(function (message) {
            if (message) {
                return tablet.getProductId()
                    .then(function (message) {
                        var penDataOptionMode = p.PenDataOptionMode.PenDataOptionMode_None;
                        switch (message) {
                            case WacomGSS.STU.ProductId.ProductId_520A:
                                penDataOptionMode = p.PenDataOptionMode.PenDataOptionMode_TimeCount;
                                break;
                            case WacomGSS.STU.ProductId.ProductId_430:
                            case WacomGSS.STU.ProductId.ProductId_530:
                                penDataOptionMode = p.PenDataOptionMode.PenDataOptionMode_TimeCountSequence;
                                break;
                            default:
                                //console.log("Unknown tablet supporting PenDataOptionMode, setting to None.");
                        };
                        return tablet.setPenDataOptionMode(penDataOptionMode);
                    });
            }
            else {
                m_encodingMode = p.EncodingMode.EncodingMode_1bit;
                return m_encodingMode;
            }
        })
        .then(function (message) {
            //console.log("received: " + JSON.stringify(message));
            addButtons(botones);
            return message;
        })
        .then(function (message) {
            //console.log("received: " + JSON.stringify(message));
            var reportHandler = new WacomGSS.STU.ProtocolHelper.ReportHandler();
            lastPoint = { "x": 0, "y": 0 };
            isDown = false;

            var penData = function (report) {
                //console.log("report: " + JSON.stringify(report));
                //m_penData.push(report);
                processButtons(report, canvas); //permite que los botones funciones en la tableta
                if (m_clickBtn == -1) {
                    m_penData.push(report);
                }
                //processPoint(report, canvas, ctx);
            }
            var penDataEncryptedOption = function (report) {
                //console.log("reportOp: " + JSON.stringify(report));
                //m_penData.push(report.penData[0], report.penData[1]);
                processButtons(report.penData[0], canvas);
                //processPoint(report.penData[0], canvas, ctx);
                processButtons(report.penData[1], canvas);
                if (m_clickBtn == -1) {
                    m_penData.push(report);
                }
                //processPoint(report.penData[1], canvas, ctx);
            }

            var log = function (report) {
                //console.log("report: " + JSON.stringify(report));
            }

            var decrypted = function (report) {
                //console.log("decrypted: " + JSON.stringify(report));
            }
            m_penData = new Array();
            reportHandler.onReportPenData = penData;
            reportHandler.onReportPenDataOption = penData;
            reportHandler.onReportPenDataTimeCountSequence = penData;
            reportHandler.onReportPenDataEncrypted = penDataEncryptedOption;
            reportHandler.onReportPenDataEncryptedOption = penDataEncryptedOption;
            reportHandler.onReportPenDataTimeCountSequenceEncrypted = penData;
            reportHandler.onReportDevicePublicKey = log;
            reportHandler.onReportEncryptionStatus = log;
            reportHandler.decrypt = decrypted;
            return reportHandler.startReporting(tablet, true);
        }).then(function (message) {
            return tablet.setInkingMode(0x01);
        })
        .fail(function (ex) {
            console.error(ex);

            if (ex instanceof DCANotReady) {
                // Device Control App not detected 
                // Reinitialize and re-try
                WacomGSS.STU.Reinitialize();
                setTimeout(tabletDemo, 1000);
            }
            else {
                setTimeout(disconnect(), 0);
            }
        });
}

function addButtons(botones) {
    if (botones == null)
        return;
    m_btns = new Array(botones.length);

    for (var i = 0; i < botones.length; ++i) {
        m_btns[i] = new Button();


        m_btns[i].Bounds = new Rectangle(botones[i].x, botones[i].y,
            botones[i].width, botones[i].height);
        var x = i;
        m_btns[i].Click = clickDelegate(i);
    }
}

function clickDelegate(x) {
    return () => btn_Click(x);
}


function distance(a, b) {
    return Math.pow(a.x - b.x, 2) + Math.pow(a.y - b.y, 2);
}

function clearCanvas(in_canvas, in_ctx) {
    in_ctx.save();
    in_ctx.setTransform(1, 0, 0, 1, 0, 0);
    in_ctx.fillStyle = "white";
    in_ctx.fillRect(0, 0, in_canvas.width, in_canvas.height);

    //in_ctx.rect(canvas.width / 2 - 300, canvas.height / 2 - 50, 600, 150);
    //in_ctx.fillStyle = "white";
    //in_ctx.fill();
    //in_ctx.lineWidth = 5;
    in_ctx.strokeStyle = "#8ED6FF";
    in_ctx.stroke();
    in_ctx.restore();
}

function btn_Click(btn) {
    serviceHelper.invokeMethodAsync('ActivarBoton', btn)
}

function processButtons(point, in_canvas) {
    var nextPoint = {};
    nextPoint.x = Math.round(in_canvas.width * point.x / m_capability.tabletMaxX);
    nextPoint.y = Math.round(in_canvas.height * point.y / m_capability.tabletMaxY);
    var isDown2 = (isDown ? !(point.pressure <= m_inkThreshold.offPressureMark) : (point.pressure > m_inkThreshold.onPressureMark));

    var btn = -1;
    for (var i = 0; i < m_btns.length; ++i) {
        if (m_btns[i].Bounds.Contains(nextPoint)) {
            btn = i;
            break;
        }
    }

    if (isDown && !isDown2) {
        if (btn != -1 && m_clickBtn === btn) {
            m_btns[btn].Click();
        }
        m_clickBtn = -1;
    }
    else if (btn != -1 && !isDown && isDown2) {
        m_clickBtn = btn;
    }
    isDown = isDown2;
    return (btn == -1);
}

function processPoint(point, in_canvas, in_ctx) {
    var nextPoint = {};
    nextPoint.x = Math.round(in_canvas.width * point.x / m_capability.tabletMaxX);
    nextPoint.y = Math.round(in_canvas.height * point.y / m_capability.tabletMaxY);
    var isDown2 = (isDown ? !(point.pressure <= m_inkThreshold.offPressureMark) : (point.pressure > m_inkThreshold.onPressureMark));

    if (isDown2) {
        if (m_actArea == undefined) {
            m_actArea = new Rectangle(nextPoint.x, nextPoint.y,
                0, 0);
        }
        if (nextPoint.x < m_actArea.x) {
            m_actArea.width += m_actArea.x - nextPoint.x;
            m_actArea.x = nextPoint.x;
        }
        if (nextPoint.y < m_actArea.y) {
            m_actArea.height += m_actArea.y - nextPoint.y;
            m_actArea.y = nextPoint.y;
        }
        if (nextPoint.x > m_actArea.x + m_actArea.width) {
            m_actArea.width = nextPoint.x - m_actArea.x;
        }
        if (nextPoint.y > m_actArea.y + m_actArea.height) {
            m_actArea.height = nextPoint.y - m_actArea.y;
        }
    }

    if (!isDown && isDown2) {
        lastPoint = nextPoint;
    }

    if ((isDown2 && 10 < distance(lastPoint, nextPoint)) || (isDown && !isDown2)) {
        in_ctx.beginPath();
        in_ctx.moveTo(lastPoint.x, lastPoint.y);
        in_ctx.lineTo(nextPoint.x, nextPoint.y);
        in_ctx.stroke();
        in_ctx.closePath();
        lastPoint = nextPoint;
    }

    isDown = isDown2;
}

function generateImage(height, width, area) {
    var signatureCanvas = document.createElement("canvas");
    signatureCanvas.id = "signatureCanvas";
    signatureCanvas.height = height;
    signatureCanvas.width = width;

    var signatureCtx = signatureCanvas.getContext("2d");

    signatureCtx.strokeStyle = "#8ED6FF";
    signatureCtx.stroke();
    signatureCtx.restore();


    signatureCtx.lineWidth = 4;
    signatureCtx.strokeStyle = 'black';
    lastPoint = { "x": 0, "y": 0 };
    isDown = false;
    m_actArea = undefined;

    for (var i = 0; i < m_penData.length; i++) {
        processPoint(m_penData[i], signatureCanvas, signatureCtx);
    }
    //return signatureCanvas.toDataURL("image/png");

    return createImageBitmap(signatureCanvas, m_actArea.x, m_actArea.y, m_actArea.width, m_actArea.height)
        .then(function (img) {

            var returnCanvas = document.createElement("canvas");
            returnCanvas.height = m_actArea.height;
            returnCanvas.width = m_actArea.width;

            var returnCtx = returnCanvas.getContext("2d");

            returnCtx.drawImage(img, 0, 0);

            return returnCanvas.toDataURL("image/png");
        });
}

function onCanvasClick(event) {
    // Enable the mouse to click on the simulated buttons that we have displayed.

    // Note that this can add some tricky logic into processing pen data
    // if the pen was down at the time of this click, especially if the pen was logically
    // also 'pressing' a button! This demo however ignores any that.

    var posX = event.pageX - formDiv.offsetLeft;
    var posY = event.pageY - formDiv.offsetTop;

    for (var i = 0; i < m_btns.length; i++) {
        if (m_btns[i].Bounds.Contains(new Point(posX, posY))) {
            m_btns[i].Click();
            break;
        }
    }
}
