function invokeServiceWorkerUpdateFlow(registration) {
  let label = document.createElement("span");
  label.textContent = "Hay una nueva version disponible. ";

  let updateButton = document.createElement("a");
  updateButton.id = "check";
  updateButton.textContent = "Actualizar ahora";
  updateButton.addEventListener("click", () => {
    console.log("Updating to new version");
    registration.waiting.postMessage("SKIP_WAITING");
  });

  let wrapper = document.createElement("div");
  wrapper.className = "wrapper";
  wrapper.appendChild(label);
  wrapper.appendChild(updateButton);

  let newVersionToast = document.createElement("div");
  newVersionToast.className = "new-version-toast";
  newVersionToast.appendChild(wrapper);

  document.getElementsByTagName("body")[0].prepend(newVersionToast);
}

// Chequea si el navegador soporta service worker
if ("serviceWorker" in navigator) {
  // Espera hasta la carga de la página
  window.addEventListener("load", async () => {
    // Registra el service worker desde el archivo especificado
    const registration = await navigator.serviceWorker.register(
      "/service-worker.js"
    );

    // Asegura que si en el caso de que al momento de encontrar una actualizacion y el evento se pierda,
    // pueda ser invocado cuando el service worker esté en estado waitingr
    if (registration.waiting) {
      invokeServiceWorkerUpdateFlow(registration);
    }

    // Detecta si existe una actualización disponible y espera por la descarga de los archivos para iniciar su instalacion
    registration.addEventListener("updatefound", () => {
      if (registration.installing) {
        // Espera hasta que el nuevo service worker este listo para ser instalado
        registration.installing.addEventListener("statechange", () => {
          if (registration.waiting) {
            // Si existe un nuevo service worker, muestre el mensaje para actualizar
            if (navigator.serviceWorker.controller) {
              invokeServiceWorkerUpdateFlow(registration);
            } else {
              // En otro caso, si no hay ningun nuevo controlador, no haga nada.
              console.log("Service Worker initialized for the first time");
            }
          }
        });
      }
    });

    let refreshing = false;

    // Detecta el cambio de controlador y refresca la pagina
    navigator.serviceWorker.addEventListener("controllerchange", () => {
      if (!refreshing) {
        window.location.reload();
        refreshing = true;
      }
    });
  });
}

// Crea una notificacion usando un broadcastChannel
const bc = new BroadcastChannel("sw-channel");
bc.onmessage = function (message) {
  if (message && message.data == "downloading-files") {
    customMessage("La actualización está siendo descargada...");
  } else if (message && message.data == "all-downloaded") {
    customMessage("¡La aplicación está en su última versión!");
  } else if (message && message.data == "error-cache-sw") {
    errorUpdating("Error en la descarga de actualización.");
  }
};

function customMessage(textOnLabel) {
  let myId = "myswmessage";
  let container = document.getElementById(myId);
  if (container) {
    document.getElementById("myspansw").textContent = textOnLabel;
    // Remueve el elemento luego de cinco segundos
    setTimeout(function(){ container.remove(); }, 5000);
  } else {
    let label = document.createElement("span");
    label.id = "myspansw";
    label.textContent = textOnLabel;
    let wrapper = document.createElement("div");
    wrapper.className = "wrapper";
    wrapper.appendChild(label);

    let newVersionToast = document.createElement("div");
    newVersionToast.className = "status-toast";
    newVersionToast.id = myId;
    newVersionToast.appendChild(wrapper);

    document.getElementsByTagName("body")[0].prepend(newVersionToast);
  }
}

function errorUpdating(message) {
  let label = document.createElement("span");
  label.textContent = message;

  let updateButton = document.createElement("a");
  updateButton.id = "errorsw";
  updateButton.textContent = "Reintentar";
  updateButton.addEventListener("click", () => {
    // Eliminar service worker
    if ("serviceWorker" in navigator) {
      navigator.serviceWorker.getRegistrations().then(function (registrations) {
        for (let registration of registrations) {
          registration.unregister();
        }
      });
    }

    // Borrar cookies
    deleteAllCookies();

    // Recarga la pagina
    location.reload();
  });

  let wrapper = document.createElement("div");
  wrapper.className = "wrapper";
  wrapper.appendChild(label);
  wrapper.appendChild(updateButton);

  let newVersionToast = document.createElement("div");
  newVersionToast.className = "error-toast";
  newVersionToast.appendChild(wrapper);

  document.getElementsByTagName("body")[0].prepend(newVersionToast);
}

function deleteAllCookies() {
  var cookies = document.cookie.split(";");

  for (var i = 0; i < cookies.length; i++) {
    var cookie = cookies[i];
    var eqPos = cookie.indexOf("=");
    var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
    document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
  }
}
