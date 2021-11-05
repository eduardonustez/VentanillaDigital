function focusInput(elementId, previous, next) {
  var value = document.getElementById(elementId).value;
  if (value.length == 0) {
    document.getElementById(previous).focus();
  } else {
    document.getElementById(next).focus();
  }
}

function focusInput(id) {
  document.getElementById(id).focus();
}

function pdf() {
  var mynode = document.getElementById("pdfObject");
  PDFObject.embed("manual-firma-correo-2020.pdf", mynode);
}

function mostrarMenu() {
  $("#sidebar").toggleClass("hide");
  $("#content").toggleClass("with-sidebar");
}

function mostrarFiltros(mostrarOverlay) {
  let filtros = document.getElementById("filtros");
  let overlay = document.getElementById("overlay");
  filtros.classList.toggle("mostrar");
  overlay.classList.toggle("mostrar");
}

function logKey(e) {
  console.log(`${e.code}`);
}

function addFunctionPin(inputs) {
  if (inputs != null) {
    inputs.querySelectorAll(".pin-firma").forEach(function (item, index, arr) {
      item.onkeyup = function () {
        if (
          item.value.length === parseInt(item.attributes["maxlength"].value)
        ) {
          if (index < arr.length - 1) {
            arr[index + 1].focus();
          }
        } else if (item.value.length === 0) {
          if (index > 0) {
            arr[index - 1].focus();
          }
        }
      };
    });
  }
}

function skipToNextInputRechazar() {
  var inputs = document.getElementById("pin-rechazar-tramite");
  addFunctionPin(inputs);
}

function skipToNextInput() {
  var inputs = document.getElementById("pin-autorizar");
  addFunctionPin(inputs);
}

function seleccionarTodos() {
  var input = $(".checkbox-table");
  var seleccionarTodosCheck = $("#seleccionarTodosCheck");
  if (seleccionarTodosCheck.prop("checked")) input.prop("checked", true);
  else input.prop("checked", false);
  verBotones();
}

function deseleccionarTodos() {
  $(".checkbox-table").prop("checked", false);
  verBotones();
}

function habilitarBotones() {
  $("#boton-rechazar").prop("disabled", false);
  $("#boton-autorizar").prop("disabled", false);
}

function deshabilitarBotones() {
  $("#boton-rechazar").prop("disabled", true);
  $("#boton-autorizar").prop("disabled", true);
}

function verBotones() {
  if ($("#mi-tabla input:checkbox:checked").length) {
    habilitarBotones();
  } else {
    deshabilitarBotones();
  }
}

function quitarFilas(arr) {
  deseleccionarTodos();
  arr.forEach(function (value) {
    const target = document.getElementById(value);
    target.classList.add("tabla-tramites-en-proceso");
  });
}

function habilitarFilas() {
  $("#mi-tabla > tbody > tr").removeClass("tabla-tramites-en-proceso");
}

function quitarLoadScreen() {
  $(".load-screen").remove();
}

function focusPrimerInput() {
  $("#modalForPinFirma").on("shown.bs.modal", function () {
    $("#inputUno").focus();
  });
}

function cerrarModal(modalACerrar) {
  $(modalACerrar).modal("toggle");
}

function expandirNav() {
  $(".contenido_principal").toggleClass("contenido_principal-abierto");
  $(".main-menu").toggleClass("expanded");
}
