function VisualizarReporte(data) {
    try {
        if (data !== "") {
            var models = window["powerbi-client"].models;
            var reportContainer = $("#report-container").get(0);

            powerbi.bootstrap(reportContainer, { type: "report" });

            embedData = $.parseJSON(data);
            reportLoadConfig = {
                type: "report",
                tokenType: models.TokenType.Embed,
                accessToken: embedData.EmbedToken.Token,
                embedUrl: embedData.EmbedReport[0].EmbedUrl,
                //settings: {
                //    background: models.BackgroundType.Transparent
                //}
            };

            tokenExpiry = embedData.EmbedToken.Expiration;

            var report = powerbi.embed(reportContainer, reportLoadConfig);

            const filter = {
                $schema: "http://powerbi.com/product/schema#basic",
                target: {
                    table: "GuidNotarias",
                    column: "Guid"
                },
                operator: 'In',
                values: [embedData.Filter],
                filterType: 1,
                requireSingleSelection: false
            };

            report.off("loaded");
            report.on("loaded", function () {
                //console.log("Report load successful");
                setFilterReport(report, filter).then(x => { console.log(x) })
                    .catch(z => console.log(z));
            });
            report.off("rendered");

            report.on("rendered", function () {
                //console.log("Report render successful");
            });

            report.off("error");

            report.on("error", function (event) {
                var errorMsg = event.detail;
                console.error(errorMsg);
                return;
            });
        } else {
            var errorContainer = $(".error-container");
            $(".embed-container").hide();
            errorContainer.show();
            var errMessageHtml = "<strong> Detalle Error: </strong> <br/>" + "Error al obtener el reporte, por favor refresque de nuevo la página, si el error persiste por favor comunicarse con el administrador";
            errMessageHtml = errMessageHtml.split("\n").join("<br/>");

            errorContainer.append(errMessageHtml);
        }

    } catch (e) {
        var errorContainer = $(".error-container");
        $(".embed-container").hide();
        errorContainer.show();
        var errMessageHtml = "<strong> Detalle Error: </strong> <br/>" + e.message;
        errMessageHtml = errMessageHtml.split("\n").join("<br/>");

        errorContainer.append(errMessageHtml);
    }
}

const setFilterReport = async (report, filter) => {
    return await report.setFilters([filter]);
    //return await report.getFilters();
}

// Localización

function showError(error) {
    console.log('showError: ', error);
    var mensaje = "";
    switch (error.code) {
        case error.PERMISSION_DENIED:
            mensaje = "Se ha negado el permiso a la geolocalización";
            break;
        case error.POSITION_UNAVAILABLE:
            mensaje = "La información de geolocalización no está disponible";
            break;
        case error.TIMEOUT:
            mensaje =
                "Se agotó el tiempo de espera de la solicitud para obtener la ubicación del usuario";
            break;
        default:
            mensaje = error.message;
            break;
    }
    return mensaje;
}

function obtenerGeolocalizacion() {
    if (!navigator.geolocation) {
        return undefined;
    }

    return new Promise((resolve, reject) => {
        navigator.geolocation.getCurrentPosition(
            (resp) => {
                resolve({
                    lng: resp.coords.longitude,
                    lat: resp.coords.latitude,
                });
            },
            (err) => {
                reject(this.showError(err));
            }
        );
    });
}