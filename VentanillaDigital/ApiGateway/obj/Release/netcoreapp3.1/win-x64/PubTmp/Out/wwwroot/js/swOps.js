const bc = new BroadcastChannel("blazor-channel");
bc.onmessage = function (message) {
  if (message && message.data == "new-version-found") {
    let label = document.createElement("span");
    label.textContent = "Hay una nueva version";

    let updateButton = document.createElement("button");
    updateButton.textContent = "Actualizar ahora";
    updateButton.addEventListener("click", () => {
      //console.log("Updating to new version");
      bc.postMessage("skip-waiting");
    });

    let newVersionToast = document.createElement("div");
    newVersionToast.className = "new-version-toast";
    newVersionToast.appendChild(label);
    newVersionToast.appendChild(updateButton);

    document.getElementsByTagName("body")[0].appendChild(newVersionToast);
  } else if (message && message.data == "reload-page") {
    console.info("Reloading page");
    window.location.href = window.location.href;
  }
};

navigator.serviceWorker.register("service-worker.js");
