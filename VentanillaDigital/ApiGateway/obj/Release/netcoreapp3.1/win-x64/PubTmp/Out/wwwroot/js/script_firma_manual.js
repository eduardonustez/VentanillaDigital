var fm_canvas;
var fm_ctx;
var fm_stop;

var drawing = false;
var mousePos = { x: 0, y: 0 };
var lastPos = mousePos;

function initialize_fm() {
  fm_canvas = document.getElementById("sig-canvas");
  fm_canvas.width = 400;
  fm_canvas.height = 240;
  fm_ctx = fm_canvas.getContext("2d");
  fm_ctx.strokeStyle = "#222222";
  fm_ctx.lineWith = 2;

  fm_canvas.addEventListener(
    "mousedown",
    function (e) {
      e.preventDefault();
      drawing = true;
      lastPos = getMousePos(fm_canvas, e);
      var button = document.getElementById("aceptar-firma-movil");
      button.removeAttribute("disabled");
    },
    false
  );
  fm_canvas.addEventListener(
    "mouseup",
    function (e) {
      e.preventDefault();
      drawing = false;
    },
    false
  );
  fm_canvas.addEventListener(
    "mousemove",
    function (e) {
      e.preventDefault();
      mousePos = getMousePos(fm_canvas, e);
    },
    false
  );

  fm_canvas.addEventListener(
    "touchstart",
    function (e) {
      e.preventDefault();
      var button = document.getElementById("aceptar-firma-movil");
      button.removeAttribute("disabled");
      mousePos = getTouchPos(fm_canvas, e);
      var touch = e.touches[0];
      var mouseEvent = new MouseEvent("mousedown", {
        clientX: touch.clientX,
        clientY: touch.clientY,
      });
      fm_canvas.dispatchEvent(mouseEvent);
    },
    false
  );
  fm_canvas.addEventListener(
    "touchend",
    function (e) {
      e.preventDefault();
      var mouseEvent = new MouseEvent("mouseup", {});
      fm_canvas.dispatchEvent(mouseEvent);
    },
    false
  );
  fm_canvas.addEventListener(
    "touchmove",
    function (e) {
      e.preventDefault();
      var touch = e.touches[0];
      var mouseEvent = new MouseEvent("mousemove", {
        clientX: touch.clientX,
        clientY: touch.clientY,
      });
      fm_canvas.dispatchEvent(mouseEvent);
    },
    false
  );

  document.body.addEventListener(
    "touchstart",
    function (e) {
      if (e.target == fm_canvas) {
        e.preventDefault();
        e.stopPropagation();
      }
    },
    { passive: false }
  );
  document.body.addEventListener(
    "touchend",
    function (e) {
      if (e.target == fm_canvas) {
        e.preventDefault();
        e.stopPropagation();
      }
    },
    { passive: false }
  );
  document.body.addEventListener(
    "touchmove",
    function (e) {
      if (e.target == fm_canvas) {
        e.preventDefault();
        e.stopPropagation();
      }
    },
    { passive: false }
  );

  window.requestAnimFrame = (function (callback) {
    return (
      window.requestAnimationFrame ||
      window.webkitRequestAnimationFrame ||
      window.mozRequestAnimationFrame ||
      window.oRequestAnimationFrame ||
      window.msRequestAnimaitonFrame ||
      function (callback) {
        window.setTimeout(callback, 1000 / 60);
      }
    );
  })();

  fm_stop = false;
  drawLoop_fm();
}

function getMousePos(canvasDom, mouseEvent) {
  var rect = canvasDom.getBoundingClientRect();
  return {
    x: mouseEvent.clientX - rect.left,
    y: mouseEvent.clientY - rect.top,
  };
}

function getTouchPos(canvasDom, touchEvent) {
  var rect = canvasDom.getBoundingClientRect();
  return {
    x: touchEvent.touches[0].clientX - rect.left,
    y: touchEvent.touches[0].clientY - rect.top,
  };
}

function renderCanvas() {
  if (drawing) {
    fm_ctx.moveTo(lastPos.x, lastPos.y);
    fm_ctx.lineTo(mousePos.x, mousePos.y);
    fm_ctx.stroke();
    lastPos = mousePos;
  }
}

function clearCanvas_fm() {
  startRender_fm();
  var test = fm_canvas.width;
  fm_canvas.width = test;
  var button = document.getElementById("aceptar-firma-movil");
  button.setAttribute("disabled", "");
}

function getImage_fm() {
  fm_stop = true;
  return fm_canvas.toDataURL("image/png");
}

function drawLoop_fm() {
  if (!fm_stop) {
    requestAnimFrame(drawLoop_fm);
    renderCanvas();
  }
}

function startRender_fm() {
  if (fm_stop) {
    fm_stop = false;
    drawLoop_fm();
  }
}
