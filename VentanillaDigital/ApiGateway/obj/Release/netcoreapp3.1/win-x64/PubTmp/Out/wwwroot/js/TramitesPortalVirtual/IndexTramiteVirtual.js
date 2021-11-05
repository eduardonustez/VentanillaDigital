function VisualizarModelTramiteVirtual(info) {
    try {
        console.info(info);
        if (info != null) {
            const elNotario = 'El Notario';

            localStorage.setItem('correo', info.loginConvenio);
            localStorage.setItem('personaNombre', elNotario);
            obtenerIframe(info);
        } else {
            console.error(`Error al obtener datos ${info}`);
        }
    } catch (e) {
        console.error(`Error al obtener datos ${e}`);
    }
}

function obtenerIframe(info) {
    localStorage.setItem('loaded', 'false');
    var myframe = document.createElement('iframe');
    myframe.classList.add("global");
    myframe.onload = function () {
        if (localStorage.getItem('loaded') == 'false') {
            const iframe = myframe.contentWindow;
            let authObj = {
                token: info.token,
                personaNombre: localStorage.getItem('personaNombre'),
                correo: localStorage.getItem('correo'),
                phoneUser: localStorage.getItem('phoneUser')
            }
            configObj = info;

            if (!configObj) {
                console.warn(!!!"Error al obtener datos MiFirma");
                return;
            }

            Object.keys(authObj).forEach(key => {
                iframe.postMessage({
                    action: 'save',
                    key: key,
                    value: authObj[key]
                }, '*');
            });

            //Object.keys(configObj).forEach(key => {
            //    iframe.postMessage({
            //        action: 'save',
            //        key: key,
            //        value: configObj[key]
            //    }, '*');
            //});

            iframe.postMessage({
                action: 'save',
                key: 'CUANDI',
                value: configObj.cuandi
            }, '*');

            iframe.postMessage({
                action: 'save',
                key: 'documentTitle',
                value: configObj.titulo
            }, '*');

            iframe.postMessage({
                action: 'save',
                key: 'configurationGuid',
                value: configObj.configurationGuid
            }, '*');

            if (configObj.comparecientes.length > 0) {
                let contador = 0;
                for (var i = 0; i < configObj.comparecientes.length; i++) {
                    contador++;
                    iframe.postMessage({
                        action: 'save',
                        key: `nombre${contador}`,
                        value: configObj.comparecientes[i].nombre
                    }, '*');

                    iframe.postMessage({
                        action: 'save',
                        key: `correo${contador}`,
                        value: configObj.comparecientes[i].correo
                    }, '*');
                }
            }

            iframe.postMessage({
                action: 'save',
                key: 'firmantes',
                value: configObj.comparecientes.length
            }, '*');


            iframe.postMessage({
                action: 'save',
                key: 'refresh',
                value: 'refresh'
            }, '*');
            localStorage.setItem('loaded', 'true');
        }
    }
    myframe.src = info.myFrame;
    //myframe.src = 'http://olsrvdeswtce01:6581/main/documentos';
    document.getElementById('iframeTramiteVirtual').appendChild(myframe);
    console.warn("Ready");
}


